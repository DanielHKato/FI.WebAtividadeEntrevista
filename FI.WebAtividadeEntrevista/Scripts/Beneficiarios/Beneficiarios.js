function CriarBeneficiariosModal() {
    if (beneficiariosConfig && !beneficiariosConfig.isLoaded) {
        $.ajax({
            type: 'GET',
            url: beneficiariosConfig.form.Url,
            success: function (data) {
                $(beneficiariosConfig.mainContainer).html(data);

                LimparFormulario();
                DefinirEventoOnClickBtnCancelar();
                DefinirMascaras();
                CriarTabelaBeneficiarios();
                DefinirFormSubmitBeneficiarios();

                beneficiariosConfig.isLoaded = true;
            },
            error: function (r) {
                MostrarMensagemErro(r);
            },
        });
    }

    $(beneficiariosConfig.mainContainer).modal('show');
}

function DefinirEventoOnClickBtnCancelar() {
    $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.buttons.btnCancelar)
        .click(function (e) {
            e.preventDefault();
            LimparFormulario();
        });
}

function DefinirMascaras() {
    $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.fields.CPF)
        .inputmask("999.999.999-99");
}

function CriarTabelaBeneficiarios() {
    var grid = $(beneficiariosConfig.gridTable.mainContainer);

    grid.jtable({
        title: beneficiariosConfig.gui.tituloGrid,
        paging: true,
        pageSize: 5,
        sorting: true,
        defaultSorting: 'Nome ASC',
        actions: {
            listAction: beneficiariosConfig.gridTable.listUrl,
        },
        ajaxSettings: {
            data: {
                idCliente: beneficiariosConfig.ClientId
            }
        },
        fields: {
            CPF: {
                title: 'CPF',
                width: '30%',
                display: function (data) {
                    /*
                    para formatar como CPF:
                    return data.record.CPF.substring(0, 3) + '.' +
                        data.record.CPF.substring(3, 3) + '.' + 
                        data.record.CPF.substring(6, 3) + '-' +
                        data.record.CPF.substring(9, 2);
                    */
                    return data.record.CPF;
                }
            },
            Nome: {
                title: 'Nome',
                width: '40%'
            },
            Alterar: {
                title: '',
                width: '30%',
                display: function (data) {
                    return '<button onclick="Consultar(' + data.record.Id + ')" class="btn btn-primary btn-sm">' + beneficiariosConfig.gui.labelBotaoAlterar + '</button>&nbsp;<button onclick="Excluir(' + data.record.Id + ')" class="btn btn-primary btn-sm">' + beneficiariosConfig.gui.labelBotaoExcluir + '</button>';
                }
            }
        }
    });

    grid.jtable('load');
}

function RecarregarTabela() {
    var grid = $(beneficiariosConfig.gridTable.mainContainer);

    if (grid) {
        grid.jtable('load');
    }
}

function DefinirFormSubmitBeneficiarios() {
    $(beneficiariosConfig.form.id).submit(function (e) {
        e.preventDefault();

        var data = GerarCorpoRequisicaoSubmit();
        var submitUrl = beneficiariosConfig.form.incluirUrl;

        if (parseInt(data.Id) > 0) {
            submitUrl = beneficiariosConfig.form.alterarUrl;
        }

        $.ajax({
            url: submitUrl,
            method: "POST",
            data: data,
            error:
                function (r) {
                    MostrarMensagemErro(r);
                },
            success:
                function (r) {
                    ModalDialog(beneficiariosConfig.mensagens.requisicaoSucesso, r);
                    LimparFormulario();
                    RecarregarTabela();
                }
        });
    })
}

function Consultar(beneficiarioId) {
    $.ajax({
        type: 'POST',
        url: beneficiariosConfig.form.consultarUrl,
        data: {
            id: beneficiarioId
        },
        success: function (data) {
            LimparFormulario();
            DefinirValoresNosCampos(data.Id, data.CPF, data.Nome, beneficiariosConfig.gui.labelBotaoSalvarEdicao);
        },
        error: function (r) {
            MostrarMensagemErro(r);
        },
    });
}

function Excluir(beneficiarioId) {
    $.ajax({
        type: 'POST',
        url: beneficiariosConfig.form.excluirUrl,
        data: {
            id: beneficiarioId
        },
        success: function (data) {
            ModalDialog(beneficiariosConfig.mensagens.requisicaoSucesso, data);
            LimparFormulario();
            RecarregarTabela();
        },
        error: function (r) {
            MostrarMensagemErro(r);
        },
    });
}

function LimparFormulario() {
    DefinirValoresNosCampos(0, '', '', beneficiariosConfig.gui.labelBotaoSalvarCadastro);
}

function MostrarMensagemErro(r) {
    if (r.status == 400)
        ModalDialog(beneficiariosConfig.mensagens.erro.titulo, r.responseJSON);
    else if (r.status == 500)
        ModalDialog(beneficiariosConfig.mensagens.erro.titulo, beneficiariosConfig.mensagens.erro.erroServidor);
    else if (r.ResponseText)
        ModalDialog(beneficiariosConfig.mensagens.erro.titulo, r.ResponseText);
    else 
        ModalDialog(beneficiariosConfig.mensagens.erro.titulo, beneficiariosConfig.mensagens.erro.erroDesconhecido);
}

function DefinirValoresNosCampos(id, cpf, nome, btnSalvarLabel) {
    $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.fields.Id).val(id);
    $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.fields.CPF).val(cpf);
    $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.fields.Nome).val(nome);
    $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.buttons.btnSalvar).html(btnSalvarLabel);
}

function GerarCorpoRequisicaoSubmit() {
    return {
        "Nome": $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.fields.Nome).val(),
        "CPF": $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.fields.CPF).val(),
        "Id": $(beneficiariosConfig.form.id + ' ' + beneficiariosConfig.form.fields.Id).val(),
        "ClienteId": beneficiariosConfig.ClientId
    };
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}