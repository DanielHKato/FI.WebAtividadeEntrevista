using FI.AtividadeEntrevista.DAL.Beneficiarios;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Interfaces;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario : BoCRUDBasico<Beneficiario>, IBllVerificarExistencia
    {
        protected IDalVerificarExistencia _daoCPF;
        public BoBeneficiario() : base()
        {
            _dao = new DaoBeneficiario(
                "FI_SP_DelBeneficiario", 
                "FI_SP_AltBeneficiario", 
                "FI_SP_IncBeneficiario", 
                "FI_SP_ConsBeneficiario",
                "FI_SP_ConsBeneficiario", 
                "FI_SP_PesqBeneficiarios", 
                "FI_SP_VerificaBeneficiario"
            );

            _daoCPF = (IDalVerificarExistencia) _dao;
        }

        public bool VerificarExistencia(Dictionary<string, string> parametrosPesquisa)
        {
            return _daoCPF.VerificarExistencia(parametrosPesquisa);
        }
    }
}
