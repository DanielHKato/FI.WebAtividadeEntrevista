using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.WebAtividadeEntrevista.Models;
using FI.AtividadeEntrevista.Interfaces;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private IBllCRUDBasico<Cliente> _bo;

        public ClienteController(IBllCRUDBasico<Cliente> bo)
        {
            _bo = bo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(InserirClienteModel model)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                
                model.Id = _bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

           
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(AlterarClienteModel model)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;

                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                _bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });
                               
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            Cliente cliente = _bo.Consultar(id);
            AlterarClienteModel model = null;

            if (cliente != null)
            {
                model = new AlterarClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            int qtd = 0;
            var campo = string.Empty;
            var crescente = string.Empty;
            var ordemCrescente = true;

            if (jtSorting != null)
            {
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                {
                    campo = array[0].ToUpper();

                    if (array.Length > 1)
                    {
                        crescente = array[1];

                        ordemCrescente = crescente.Trim().Equals("ASC", StringComparison.InvariantCultureIgnoreCase);
                    }
                }
            }

            try
            {
                var clientes = _bo.Pesquisa(
                    jtStartIndex,
                    jtPageSize,
                    campo,
                    ordemCrescente,
                    out qtd,
                    new Dictionary<string, string>() 
                );

                return Json(new
                {
                    Result = "OK",
                    Records = clientes,
                    TotalRecordCount = qtd
                });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}