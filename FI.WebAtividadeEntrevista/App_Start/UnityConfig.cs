using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DAL.Beneficiarios;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Interfaces;
using FI.AtividadeEntrevista.Validacao;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace FI.WebAtividadeEntrevista
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IDalCRUDBasico<Beneficiario>, DaoBeneficiario>();
            container.RegisterType<IBllCRUDBasico<Beneficiario>, BoBeneficiario>();

            container.RegisterType<IDalCRUDBasico<Cliente>, DaoCliente>();
            container.RegisterType<IBllCRUDBasico<Cliente>, BoCliente>();

            container.RegisterType<IVerificarExistenciaFactory, VerificarExistenciaFactory>();

            container.RegisterType<IDalVerificarExistenciaBeneficiario, DaoBeneficiario>();
            container.RegisterType<IDalVerificarExistenciaCliente, DaoCliente>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}