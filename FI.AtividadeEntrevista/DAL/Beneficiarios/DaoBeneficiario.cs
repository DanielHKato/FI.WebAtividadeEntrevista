﻿using FI.AtividadeEntrevista.DAL.Padrao;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL.Beneficiarios
{
    internal class DaoBeneficiario : DaoCRUDBasico<Beneficiario>, IDalVerificarExistencia
    {
        private readonly string _nomeProcedureVerificarExistencia = string.Empty;

        public DaoBeneficiario(
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

        /// <summary>
        /// Método que retorna uma lista de beneficiários através do ID do Cliente.
        /// </summary>
        /// <param name="idCliente">ID do cliente.</param>
        /// <returns>Lista de beneficiários encontrados.</returns>
        public List<Beneficiario> ListarBeneficiariosDoCliente(long idCliente)
        {
            return this.ConverterDataSetParaList(
                base.Consultar(
                    _nomeProcedureRecuperarPorId,
                    new List<SqlParameter>() { new SqlParameter("IDCLIENTE", idCliente) }
                )
            );
        }
    }
}