using FI.AtividadeEntrevista.Enums;
using FI.AtividadeEntrevista.Interfaces;
using FI.AtividadeEntrevista.Validacao;
using FI.WebAtividadeEntrevista.Models;
using FI.WebAtividadeEntrevista.Util;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;

namespace FI.WebAtividadeEntrevista.DataValidation
{
    /// <summary>
    /// Está ValidationAttribute valida o CPF informado.
    /// </summary>
    /// <remarks>
    /// O comportamento desta validação pode mudar se for uma operação de edição de Cliente ou Beneficiário, por exemplo.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidarCPF : ValidationAttribute
    {
        private readonly string _mensagemDeErroParaFormatoDeCPFInvalido = Resources.Messages.ValidarCPF_FormatoInvalido;
        private readonly string _mensagemDeErroParaDigitosInvalidos;
        private readonly string _mensagemDeErroParaCPFJaCadastrado;
        private readonly EnumTipoVerificacaoExistencia _tipoVerificacaoExistencia;
        private readonly bool _verificarSeCPFEstaCadastradoNaBase;
        private readonly bool _executarValidacaoFormatoCPF;

        /// <summary>
        /// Define as mensagens de erro das validações e o tipo de verificação a ser efetuada (Ex: CPF na base de Clientes, CPF na base de Beneficiários, ...).
        /// </summary>
        /// <param name="mensagemDeErroParaDigitosInvalidos">Mensagem de erro caso os dígitos do CPF forem inválidos.</param>
        /// <param name="mensagemDeErroParaFormatoDeCPFInvalido">[OPCIONAL] Mensagem de erro caso o CPF não esteja no formato 000.000.000-00, ou ser nulo/string vazia.</param>
        /// <param name="mensagemDeErroParaCPFJaCadastrado">[OPCIONAL] Mensagem de erro caso o CPF já esteja cadastrado na base.</param>
        /// <param name="tipoVerificacaoExistencia">[OPCIONAL] Origem da base de dados dos CPFs (Cliente, Beneficiários,...).</param>
        /// <param name="executarValidacaoFormatoCPF">[OPCIONAL] Habilita a validação do CPF no formato 000.000.000-00</param>
        /// <param name="verificarSeCPFEstaCadastradoNaBase">[OPCIONAL] Habilita a validação de CPF já cadastrado na base de dados. O valor padrão é sim (true).</param>
        public ValidarCPF(
            string mensagemDeErroParaDigitosInvalidos,
            string mensagemDeErroParaFormatoDeCPFInvalido = "",
            string mensagemDeErroParaCPFJaCadastrado = "",
            EnumTipoVerificacaoExistencia tipoVerificacaoExistencia = EnumTipoVerificacaoExistencia.Clientes,
            bool executarValidacaoFormatoCPF = false,
            bool verificarSeCPFEstaCadastradoNaBase = true
            )
        {
            _mensagemDeErroParaFormatoDeCPFInvalido = mensagemDeErroParaFormatoDeCPFInvalido;
            _mensagemDeErroParaDigitosInvalidos = mensagemDeErroParaDigitosInvalidos;
            _mensagemDeErroParaCPFJaCadastrado = mensagemDeErroParaCPFJaCadastrado;
            _tipoVerificacaoExistencia = tipoVerificacaoExistencia;
            _executarValidacaoFormatoCPF = executarValidacaoFormatoCPF;
            _verificarSeCPFEstaCadastradoNaBase = verificarSeCPFEstaCadastradoNaBase;
        }

        /// <summary>
        /// Define as mensagens de erro das validações e o tipo de verificação a ser efetuada (Ex: CPF na base de Clientes, CPF na base de Beneficiários, ...).
        /// </summary>
        /// <param name="tipoArquivoResources">Tipo do Resource (typeof(ResorceType))</param>
        /// <param name="mensagemDeErroParaDigitosInvalidosResourceName">Nome da chave da mensagem de erro caso os dígitos do CPF forem inválidos.</param>
        /// <param name="mensagemDeErroParaFormatoDeCPFInvalidoResourceName">[OPCIONAL] Nome da chave da mensagem de erro caso o CPF não esteja no formato 000.000.000-00, ou ser nulo/string vazia.</param>
        /// <param name="mensagemDeErroParaCPFJaCadastradoResourceName">[OPCIONAL] Nome da chave da mensagem de erro caso o CPF já esteja cadastrado na base.</param>
        /// <param name="tipoVerificacaoExistencia">[OPCIONAL] Origem da base de dados dos CPFs (Cliente, Beneficiários,...).</param>
        /// <param name="executarValidacaoFormatoCPF">[OPCIONAL] Habilita a validação do CPF no formato 000.000.000-00</param>
        /// <param name="verificarSeCPFEstaCadastradoNaBase">[OPCIONAL] Habilita a validação de CPF já cadastrado na base de dados. O valor padrão é sim (true).</param>
        public ValidarCPF(
            Type tipoArquivoResources, 
            string mensagemDeErroParaDigitosInvalidosResourceName,
            string mensagemDeErroParaFormatoDeCPFInvalidoResourceName = "",
            string mensagemDeErroParaCPFJaCadastradoResourceName = "",
            EnumTipoVerificacaoExistencia tipoVerificacaoExistencia = EnumTipoVerificacaoExistencia.Clientes,
            bool executarValidacaoFormatoCPF = false,
            bool verificarSeCPFEstaCadastradoNaBase = true
            )
        {
            _tipoVerificacaoExistencia = tipoVerificacaoExistencia;
            _executarValidacaoFormatoCPF = executarValidacaoFormatoCPF;
            _verificarSeCPFEstaCadastradoNaBase = verificarSeCPFEstaCadastradoNaBase;

            try
            {
                ResourceManager mensagens = new ResourceManager(tipoArquivoResources);

                _mensagemDeErroParaDigitosInvalidos = mensagens.GetString(mensagemDeErroParaDigitosInvalidosResourceName);

                if (_executarValidacaoFormatoCPF && !mensagemDeErroParaFormatoDeCPFInvalidoResourceName.IsNullOrWhiteSpace())
                {
                    _mensagemDeErroParaFormatoDeCPFInvalido = mensagens.GetString(mensagemDeErroParaFormatoDeCPFInvalidoResourceName);
                }

                if (_verificarSeCPFEstaCadastradoNaBase && !mensagemDeErroParaCPFJaCadastradoResourceName.IsNullOrWhiteSpace())
                {
                    _mensagemDeErroParaCPFJaCadastrado = mensagens.GetString(mensagemDeErroParaCPFJaCadastradoResourceName);
                }
            }
            catch
            {
                _mensagemDeErroParaFormatoDeCPFInvalido = Resources.Messages.ValidarCPF_FormatoInvalido;
                _mensagemDeErroParaDigitosInvalidos = Resources.Messages.ValidarCPF_DigitoInvalido;
                _mensagemDeErroParaCPFJaCadastrado = Resources.Messages.ValidarCPF_JaExisteNaBase;
            }
        }

        /// <summary>
        /// Executa as validações no CPF:
        /// 1. Valida o formato do CPF (REGEX);
        /// 2. Valida os dígitos verificadores;
        /// 3. Se habilitada, valida se o CPF já não existe na base.
        /// </summary>
        /// <param name="value">Valor a ser validado</param>
        /// <param name="context">Contexto da validação</param>
        /// <returns>
        /// ValidationResult contendo as mensagens de erro ou [ValidationResult.Success], em caso de sucesso na validação
        /// </returns>
        /// <remarks>
        /// O método [IsValid] foi projetado para operar como short-circuit (se uma validação falhar, toda a validação ainda restante não é mais executada e o resultado é um retorno com o erro de validação encontrado).
        /// </remarks>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            ValidationResult result = ValidationResult.Success;
            var parametrosAdicionais = RecuperarParametrosAdicionaisValidacaoCPFCadastrado(context.ObjectInstance, _tipoVerificacaoExistencia);
            var formatoValido = !_executarValidacaoFormatoCPF || (ValidarFormatoCPF(value) || ValidationUtil.GerarErrorResult(_mensagemDeErroParaFormatoDeCPFInvalido, out result));
            var digitoValido = formatoValido && (ValidarDigitoVerificadorCPF(value) || ValidationUtil.GerarErrorResult(_mensagemDeErroParaDigitosInvalidos, out result));
            var cpfNaoCadastradoNaBase = digitoValido && 
                (!_verificarSeCPFEstaCadastradoNaBase || (ValidarCPFNaoCadastradoNaBaseDeDados(value, _tipoVerificacaoExistencia, parametrosAdicionais) || ValidationUtil.GerarErrorResult(_mensagemDeErroParaCPFJaCadastrado, out result)));

            return result;
        }

        #region Etapas da validação

        /// <summary>
        /// Valida se o CPF está no formato definido: 000.000.000-00
        /// </summary>
        /// <param name="value">CPF a ser verificado</param>
        /// <returns>CPF está no formato (true) ou não (false)</returns>
        public static bool ValidarFormatoCPF(object value)
        {
            var regex = new Regex(@"^\d{3}.\d{3}.\d{3}-\d{2}$");
            var cpf = ValidationUtil.ConverterParaString(value);

            return regex.Match(cpf).Success;
        }

        /// <summary>
        /// Valida o dígito verificador do CPF, utilizando a regra de pesos. Está validação não aborda a validação de regiões do 9º dígito.
        /// </summary>
        /// <param name="value">CPF a ser validado</param>
        /// <returns>Dígito válido (true) ou não (false)</returns>
        /// <remarks>
        /// Em caso de CPF inválido (como CPF incompleto, ou não numérico, por exemplo), a validação vai retornar falso.
        /// </remarks>
        public static bool ValidarDigitoVerificadorCPF(object value)
        {
            var cpf = ValidationUtil.ConverterParaString(value);
            var somenteNumerosCPF = ValidationUtil.RemoverCaracteresNaoNumericosDoCPF(cpf);
            var cpfFoiInformado = !string.IsNullOrWhiteSpace(somenteNumerosCPF);
            var cpfTemExatamenteOnzeDigitos = cpfFoiInformado && somenteNumerosCPF.Length == 11;
            var cpfENumerico = cpfTemExatamenteOnzeDigitos && ValidarCPFNumerico(somenteNumerosCPF);

            return cpfENumerico && ValidarDigitosVerificadoresCPF(somenteNumerosCPF);
        }

        #region Métodos de suporte do ValidarDigitoVerificadorCPF

        /// <summary>
        /// Valida se o CPF é um número e é maior que zero.
        /// </summary>
        /// <param name="cpfParaValidar">CPF a ser validado</param>
        /// <returns>CPF numérico e maior que zero (true) ou não (false)</returns>
        private static bool ValidarCPFNumerico(string cpfParaValidar)
        {
            return Int64.TryParse(cpfParaValidar, out long cpfConvertido) && cpfConvertido >= 0;
        }

        /// <summary>
        /// Valida os dois dígitos verificadores do CPF.
        /// </summary>
        /// <param name="cpfParaValidar">CPF a ser validado</param>
        /// <returns>Dígitos válidos (true) ou não (false)</returns>
        /// <remarks>
        /// - O iterador ObterCadaDigitoDe é utilizado para facilitar o entendimento da lógica aplicada. 
        /// - Foi utilizado um único loop para melhor performance.
        /// - As variáveis com o nome [acumulador...] recebem a soma dos dígitos multiplicados pelos pesos.
        /// </remarks>
        private static bool ValidarDigitosVerificadoresCPF(string cpfParaValidar)
        {
            var ultimosDoisDigitosDoCPFInformado = cpfParaValidar.Substring(9, 2);
            var primeirosNoveDigitosDoCPFInformado = cpfParaValidar.Substring(0, 9);
            var pesoDecimoDigito = 1;
            var pesoDecimoPrimeiroDigito = 0;
            var acumuladorDecimoDigito = 0;
            var acumuladorDecimoPrimeiroDigito = 0;
            var decimoDigitoCalculado = 0;
            var decimoPrimeiroDigitoCalculado = 0;

            foreach (var digit in ValidationUtil.ObterCadaDigitoDe(primeirosNoveDigitosDoCPFInformado))
            {
                acumuladorDecimoDigito += digit * pesoDecimoDigito++;
                acumuladorDecimoPrimeiroDigito += digit * pesoDecimoPrimeiroDigito++;
            }

            decimoDigitoCalculado = CalcularDigitoVerificador(acumuladorDecimoDigito);
            acumuladorDecimoPrimeiroDigito += decimoDigitoCalculado * pesoDecimoPrimeiroDigito;
            decimoPrimeiroDigitoCalculado = CalcularDigitoVerificador(acumuladorDecimoPrimeiroDigito);

            return ultimosDoisDigitosDoCPFInformado.Equals(decimoDigitoCalculado.ToString() + decimoPrimeiroDigitoCalculado.ToString());
        }

        /// <summary>
        /// Retorna o dígito verificador calculado da soma dos outros dígitos do CPF (multiplicados pelos seus pesos).
        /// </summary>
        /// <param name="acumulador">O valor da soma de todos os outros dígitos do CPF (multiplicados pelos seus pesos)</param>
        /// <returns>Dígito calculado, com, exatamente, tamanho 1.</returns>
        /// <remarks>
        /// Você obtém o módulo do [acumulator] por 11 e logo em seguida obtém o módulo deste resultado por 10. Isso é necessário para garantir que somente um dígito será retornado.
        /// </remarks>
        private static int CalcularDigitoVerificador(int acumulador)
        {
            return acumulador % 11 % 10;
        }

        #endregion

        /// <summary>
        /// Valida se o CPF não se encontra cadastrado na base de dados.
        /// </summary>
        /// <param name="value">CPF a ser verificado</param>
        /// <param name="tipoVerificacaoExistencia">Corresponde ao tipo da origem da base de dados para verificar o CPF</param>
        /// <param name="parametrosAdicionais">Informações adicionais para configurar a verificação, como por exemplo Id de Clientes</param>
        /// <returns>CPF não cadastrado (true) ou já cadastrado (false)</returns>
        private static bool ValidarCPFNaoCadastradoNaBaseDeDados(object value, EnumTipoVerificacaoExistencia tipoVerificacaoExistencia, Dictionary<string, string> parametrosAdicionais)
        {
            var validador = ObterVerificarExistencia(tipoVerificacaoExistencia);
            var cpf = ValidationUtil.ConverterParaString(value);
            var parametros = new Dictionary<string, string>() { { "CPF", cpf } };

            parametros = parametros
                .Union(parametrosAdicionais)
                .ToDictionary(val => val.Key, val => val.Value);

            return !validador.VerificarExistencia(
                parametros
            );
        }

        #region Métodos de suporte do ValidarCPFNaoCadastradoNaBaseDeDados

        /// <summary>
        /// Recupera de uma factory uma instância de um objeto que pode interagir com a camada de acesso à base de dados para verificar se o CPF existe ou não
        /// </summary>
        /// <param name="tipoVerificacaoExistencia">Enum que define o tipo de base de dados dos CPFs</param>
        /// <returns>instância de um validador de CPFs</returns>
        private static IBllVerificarExistencia ObterVerificarExistencia(EnumTipoVerificacaoExistencia tipoVerificacaoExistencia)
        {
            return new VerificarExistenciaFactory().ObterVerificarExistencia(tipoVerificacaoExistencia);
        }

        /// <summary>
        /// Adiciona alguns parâmetros de configuração na pesquisa de CPF já utilizado para cenários como:
        /// - Se for uma operação de edição, o CPF do usuário que está sendo editado precisa ser excluído da pesquisa.
        /// - Se for uma pesquisa de CPF na base de beneficiários, a mesma precisa ser limitada pelo Id do Cliente.
        /// </summary>
        /// <param name="contextObjectInstance">refere-se ao ValidationContext.ObjectInstance</param>
        /// <param name="tipoVerificacaoExistencia">Tipo de verificação de existência de CPF</param>
        /// <returns>Dictionary contendo os parâmetros adicionais para configurar a pesquisa</returns>
        private static Dictionary<string, string> RecuperarParametrosAdicionaisValidacaoCPFCadastrado(object contextObjectInstance, EnumTipoVerificacaoExistencia tipoVerificacaoExistencia)
        {
            var parametrosAdicionais = new Dictionary<string, string>();

            switch (tipoVerificacaoExistencia)
            {
                case EnumTipoVerificacaoExistencia.Beneficiarios:
                    var modeloBeneficiario = contextObjectInstance as BeneficiarioModel ?? new BeneficiarioModel();
                    var excluirBeneficiarioDaPesquisa = modeloBeneficiario.Id > 0 && 
                        ValidationUtil.AdicionarParametroId(modeloBeneficiario.Id, parametrosAdicionais);

                    parametrosAdicionais.Add("IDCLIENTE", modeloBeneficiario.ClienteId.ToString());

                    break;
                default:
                    var modeloCliente = contextObjectInstance as ClienteModel ?? new ClienteModel();
                    var excluirClienteDaPesquisa = modeloCliente.Id > 0 && 
                        ValidationUtil.AdicionarParametroId(modeloCliente.Id, parametrosAdicionais);

                    break;
            }

            return parametrosAdicionais;
        }

        #endregion

        #endregion
    }
}