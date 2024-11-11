using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Interfaces;
using FI.WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        private IBllCRUDBasico<Beneficiario> _bo;

        public BeneficiarioController(IBllCRUDBasico<Beneficiario> boBeneficiario)
        {
            _bo = boBeneficiario;
        }

        // GET: Beneficiario
        public ActionResult Index()
        {
            return PartialView("Form");
        }

        [HttpPost]
        public JsonResult Consultar(long? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                Response.StatusCode = 400;

                return Json(Resources.Messages.BeneficiarioController_Erro_IdInvalido);
            } 
            else
            {
                try
                {
                    var beneficiario = _bo.Consultar(id.Value);

                    if (beneficiario == null || beneficiario.ID == 0)
                    {
                        Response.StatusCode = 400;

                        return Json(Resources.Messages.BeneficiarioController_Erro_NaoEncontrado);
                    }
                    else
                    {
                        return Json(new BeneficiarioModel()
                        {
                            ClienteId = beneficiario.IDCLIENTE,
                            CPF = beneficiario.CPF,
                            Id = beneficiario.ID,
                            Nome = beneficiario.NOME
                        });
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;

                    return Json($"{Resources.Messages.BeneficiarioController_Erro}: {ex.Message}");
                }
            }
        }

        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel beneficiario)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    _bo.Incluir(new Beneficiario()
                    {
                        CPF = beneficiario.CPF,
                        NOME = beneficiario.Nome,
                        IDCLIENTE = beneficiario.ClienteId
                    });

                    return Json(Resources.Messages.BeneficiarioController_BeneficiarioCadastrado);
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;

                    return Json($"{Resources.Messages.BeneficiarioController_Erro}: {ex.Message}");
                }
            }
            else
            {
                List<string> erros =  (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;

                return Json(string.Join("<br />", erros));
            }
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel beneficiario)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    _bo.Alterar(new Beneficiario()
                    {
                        CPF = beneficiario.CPF,
                        NOME = beneficiario.Nome,
                        IDCLIENTE = beneficiario.ClienteId,
                        ID = beneficiario.Id
                    });

                    return Json(Resources.Messages.BeneficiarioController_BeneficiarioAlterado);
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;

                    return Json($"{Resources.Messages.BeneficiarioController_Erro}: {ex.Message}");
                }
            }
            else
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;

                return Json(string.Join("<br />", erros));
            }
        }

        [HttpPost]
        public JsonResult Excluir(long? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                Response.StatusCode = 400;

                return Json(Resources.Messages.BeneficiarioController_Erro_IdInvalido);
            }
            else
            {
                try
                {
                    _bo.Excluir(id.Value);

                    return Json(Resources.Messages.BeneficiarioController_BeneficiarioRemovido);
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;

                    return Json($"{Resources.Messages.BeneficiarioController_Erro}: {ex.Message}");
                }
            }
        }

        [HttpPost]
        public JsonResult BeneficiarioList(int idCliente, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
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
                var beneficiarios = _bo.Pesquisa(
                    jtStartIndex, 
                    jtPageSize, 
                    campo,
                    ordemCrescente, 
                    out qtd,
                    new Dictionary<string, string>() { { "IDCLIENTE", idCliente.ToString() } }
                );

                return Json(new { 
                    Result = "OK", 
                    Records = beneficiarios.Select(b => 
                        new BeneficiarioModel() {
                            Id = b.ID,
                            Nome = b.NOME,
                            CPF = b.CPF,
                            ClienteId = b.IDCLIENTE
                        }).ToList(), 
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