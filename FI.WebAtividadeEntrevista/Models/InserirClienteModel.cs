using FI.AtividadeEntrevista.Enums;
using FI.WebAtividadeEntrevista.DataValidation;

namespace FI.WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Essa classe é utilizada para o cadastro de novos clientes. Ela aplica validações específicas para novos cadastros (CPF).
    /// </summary>
    public class InserirClienteModel : ClienteModel
    {
        /// <summary>
        /// CPF
        /// </summary>
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