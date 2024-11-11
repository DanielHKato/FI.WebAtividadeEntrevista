using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Interfaces;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente : BoCRUDBasico<Cliente>, IBllVerificarExistencia
    {
        protected IDalVerificarExistencia _daoVerificarExistencia;

        public BoCliente(IDalCRUDBasico<Cliente> dao, IDalVerificarExistenciaCliente daoVerificarExistencia) : base()
        {
            _dao = dao;
            _daoVerificarExistencia = daoVerificarExistencia;
        }

        /// <summary>
        /// Invoca o método VerificarExistencia na base de dados
        /// </summary>
        /// <param name="parametrosPesquisa">Parâmetros adicionais da pesquisa</param>
        /// <returns>Dado existe na base (true) ou não (false)</returns>
        public bool VerificarExistencia(Dictionary<string, string> parametrosPesquisa)
        {
            return _daoVerificarExistencia.VerificarExistencia(parametrosPesquisa);
        }
    }
}
