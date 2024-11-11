using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FI.WebAtividadeEntrevista.Util
{
    public static class ValidationUtil
    {
        /// <summary>
        /// Converte um tipo object para string, ou retorna string.Empty, caso o object seja nulo.
        /// </summary>
        /// <param name="value">objeto a ser convertido para string</param>
        /// <returns>objeto convertido para string, ou string.Empty</returns>
        public static string ConverterParaString(object value)
        {
            return (value ?? string.Empty).ToString();
        }

        /// <summary>
        /// Remove alguns caracteres não numéricos do CPF, como '-' e '.'.
        /// </summary>
        /// <param name="cpf">CPF a ser filtrado</param>
        /// <returns>CPF somente com números</returns>
        public static string RemoverCaracteresNaoNumericosDoCPF(string cpf)
        {
            return cpf.Trim().Replace("-", "").Replace(".", "").Replace(",", "");
        }

        /// <summary>
        /// Define o result com o status de falha na validação e adiciona a mensagem de erro
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro</param>
        /// <param name="result">ValidationResult que será gerada</param>
        /// <returns>O retorno sempre será false (para indicar que a validação reprovou o item validado)</returns>
        /// <remarks>Método criado para branchless programming</remarks>
        public static bool GerarErrorResult(string errorMessage, out ValidationResult result)
        {
            result = new ValidationResult(errorMessage);

            return false;
        }

        /// <summary>
        /// Define um iterador para retornar cada dígito da string informada, devidamente convertido para int.
        /// Por exemplo, com "123456789", ele retorna 1, 2, 3, 4, 5, 6, 7, 8 e 9.
        /// </summary>
        /// <param name="stringASerDividida">
        /// String a ser dividida. Insira somente strings composta de números, como "837", "3638204", ...
        /// </param>
        /// <param name="quantidadeDeDigitosParaRetornar">
        /// Quantidade de dígitos do inteiro a ser retornado (valor padrão: 1 dígito).
        /// </param>
        /// <returns>Um dígito da string, convertido para int.</returns>
        /// <example>
        /// ObterCadaDigitoDe("987654321").forEach(digito => ...)
        /// </example>
        /// <exception cref="InvalidCastException">
        /// Se a stringASerDividida tiver algum caracter não numérico.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o quantidadeDeDigitosParaRetornar for muito grande, na última iteração do loop.
        /// </exception>
        public static IEnumerable<int> ObterCadaDigitoDe(string stringASerDividida, int quantidadeDeDigitosParaRetornar = 1)
        {
            for (int indiceDoCaractere = 0; indiceDoCaractere < stringASerDividida.Length; indiceDoCaractere++)
            {
                yield return Convert.ToInt32(stringASerDividida.Substring(indiceDoCaractere, quantidadeDeDigitosParaRetornar));
            }
        }

        /// <summary>
        /// Método para adicionar uma KeyValuePair fixa [<"Id", Id informado no parâmetro>] no dicionário parametros. 
        /// </summary>
        /// <param name="id">Id a ser inserido no dicionário</param>
        /// <param name="parametros">Dicionário que vai receber o Id</param>
        /// <returns>True</returns>
        /// <remarks>Método criado para branchless programming</remarks>
        public static bool AdicionarParametroId(long id, Dictionary<string, string> parametros)
        {
            parametros.Add("ID", id.ToString());

            return true;
        }
    }
}