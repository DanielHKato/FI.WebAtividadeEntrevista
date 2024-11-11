using FI.AtividadeEntrevista.Enums;
using FI.WebAtividadeEntrevista.DataValidation;

namespace FI.WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Essa classe é utilizada para a alteração de um cliente já existente. Ela aplica validações específicas para a edição (Validação de CPF, que muda um pouco da validação de CPF no cadastro).
    /// </summary>
    public class AlterarClienteModel : ClienteModel
    {
        [ValidarCPF(
            tipoArquivoResources: typeof(Resources.Messages),
            mensagemDeErroParaDigitosInvalidosResourceName: "ValidarCPF_DigitoInvalido",
            mensagemDeErroParaFormatoDeCPFInvalidoResourceName: "ValidarCPF_FormatoInvalido",
            mensagemDeErroParaCPFJaCadastradoResourceName: "ValidarCPF_JaExisteNaBase",
            tipoVerificacaoExistencia: EnumTipoVerificacaoExistencia.Clientes
        )]
        public override string CPF { get; set; }
    }
}