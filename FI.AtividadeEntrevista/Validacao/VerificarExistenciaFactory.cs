using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.Enums;
using FI.AtividadeEntrevista.Interfaces;

namespace FI.AtividadeEntrevista.Validacao
{
    /// <summary>
    /// Fornece uma instância de IBllVerificarExistencia, que permite que a base de dados dos CPFs possa ser acessada para a verificação
    /// </summary>
    public class VerificarExistenciaFactory : IVerificarExistenciaFactory
    {
        /// <summary>
        /// Retorna uma instância que permite que a base de dados dos CPFs possa ser acessada, conforme o tipo de base necessária (Ex: Clientes e Beneficiarios).
        /// </summary>
        /// <param name="tipoVerificacaoExistencia">Tipo de verificação</param>
        /// <returns>instância de IBllVerificarExistencia</returns>
        public IBllVerificarExistencia ObterVerificarExistencia(EnumTipoVerificacaoExistencia tipoVerificacaoExistencia = EnumTipoVerificacaoExistencia.Clientes)
        {
            switch (tipoVerificacaoExistencia)
            {
                case EnumTipoVerificacaoExistencia.Beneficiarios:
                    return new BoBeneficiario();

                case EnumTipoVerificacaoExistencia.Clientes:
                default:
                    return new BoCliente();
            }
        }
    }
}
