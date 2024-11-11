using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DAL.Beneficiarios;
using FI.AtividadeEntrevista.Enums;
using FI.AtividadeEntrevista.Interfaces;

namespace FI.AtividadeEntrevista.Validacao
{
    /// <summary>
    /// Fornece uma instância de IBllVerificarExistencia, que permite que a base de dados dos CPFs possa ser acessada para a verificação
    /// </summary>
    /// <remarks>
    /// Importante quando a injeção de dependência não conseguir modificar o ValidationContext do ValidationAttribute
    /// </remarks>
    public class VerificarExistenciaFactory : IVerificarExistenciaFactory
    {
        /// <summary>
        /// Retorna uma instância que permite que a base de dados dos CPFs possa ser acessada, conforme o tipo de base necessária (Ex: Clientes e Beneficiarios).
        /// </summary>
        /// <param name="tipoVerificacaoExistencia">Tipo de verificação</param>
        /// <returns>instância de IBllVerificarExistencia</returns>
        public IBllVerificarExistencia ObterVerificarExistencia(EnumTipoVerificacaoExistencia tipoVerificacaoExistencia)
        {
            switch (tipoVerificacaoExistencia)
            {
                case EnumTipoVerificacaoExistencia.Beneficiarios:
                    var daoBeneficiario = new DaoBeneficiario();

                    return new BoBeneficiario(daoBeneficiario, daoBeneficiario);
                case EnumTipoVerificacaoExistencia.Clientes:
                default:
                    var daoCliente = new DaoCliente();

                    return new BoCliente(daoCliente, daoCliente);
            }
        }
    }
}
