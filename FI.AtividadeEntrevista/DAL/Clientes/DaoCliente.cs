using FI.AtividadeEntrevista.DAL.Padrao;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Cliente
    /// </summary>
    public class DaoCliente :
        DaoCRUDBasico<Cliente>,
        IDalVerificarExistencia,
        IDalVerificarExistenciaCliente
    {
        private readonly string _nomeProcedureVerificarExistencia = string.Empty;

        public DaoCliente() : base(
            ConfigurationManager.AppSettings["ProcedureName_Cliente_Deletar"],
            ConfigurationManager.AppSettings["ProcedureName_Cliente_Alterar"],
            ConfigurationManager.AppSettings["ProcedureName_Cliente_Incluir"],
            ConfigurationManager.AppSettings["ProcedureName_Cliente_Consultar"],
            ConfigurationManager.AppSettings["ProcedureName_Cliente_Listar"],
            ConfigurationManager.AppSettings["ProcedureName_Cliente_Pesquisar"]
        )
        {
            _nomeProcedureVerificarExistencia = ConfigurationManager.AppSettings["ProcedureName_Cliente_Verificar"];
        }

        public DaoCliente(
            string nomeProcedureExcluir,
            string nomeProcedureAlterar,
            string nomeProcedureInserir,
            string nomeProcedureRecuperarPorId,
            string nomeProcedureListar,
            string nomeProcedurePesquisar,
            string nomeProcedureVerificarExistencia) : base(nomeProcedureExcluir,
                   nomeProcedureAlterar,
                   nomeProcedureInserir,
                   nomeProcedureRecuperarPorId,
                   nomeProcedureListar,
                   nomeProcedurePesquisar
            )
        {
            _nomeProcedureVerificarExistencia = nomeProcedureVerificarExistencia;
        }

        /// <summary>
        /// Método que retorna se o CPF já se encontra cadastrado em outro registro.
        /// </summary>
        /// <param name="parametrosPesquisa">Parâmetros adicionais de pesquisa</param>
        /// <returns>CPF já cadastrado (true) ou não (false)</returns>
        public bool VerificarExistencia(Dictionary<string, string> parametrosPesquisa)
        {
            var parametros = parametrosPesquisa.Select(param => new SqlParameter(param.Key, param.Value)).ToList();
            var ds = base.Consultar(_nomeProcedureVerificarExistencia, parametros);

            return ds.Tables[0].Rows.Count > 0;
        }
    }
}
