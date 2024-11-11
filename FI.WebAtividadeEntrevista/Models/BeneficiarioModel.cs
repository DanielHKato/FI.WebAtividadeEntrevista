using FI.AtividadeEntrevista.Enums;
using FI.WebAtividadeEntrevista.DataValidation;
using FI.WebAtividadeEntrevista.Resources;
using Microsoft.Ajax.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using System.Web.UI.WebControls;

namespace FI.WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required(
            ErrorMessageResourceType = typeof(Resources.Messages), 
            ErrorMessageResourceName = "ValidarCPF_CampoObrigatorio")]
        [MaxLength(
            14, 
            ErrorMessageResourceType = typeof(Resources.Messages), 
            ErrorMessageResourceName = "ValidarCPF_TamanhoMaximo")]
        [ValidarCPF(
            tipoArquivoResources: typeof(Resources.Messages),
            mensagemDeErroParaDigitosInvalidosResourceName: "ValidarCPF_DigitoInvalido",
            mensagemDeErroParaFormatoDeCPFInvalidoResourceName: "ValidarCPF_FormatoInvalido",
            mensagemDeErroParaCPFJaCadastradoResourceName: "ValidarCPF_JaExisteNaBase",
            tipoVerificacaoExistencia: EnumTipoVerificacaoExistencia.Beneficiarios
        )]
        public string CPF { get; set; }

        /// <summary>
        /// Id do cliente
        /// </summary>
        [Required]
        public long ClienteId { get; set; }

        

    }
}