﻿using AutoMapper;
using Maturidade_Online.Dominio;
using Maturidade_Online.Filter;
using Maturidade_Online.Models;
using Maturidade_Online.Repositorio;
using Maturidade_Online.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Maturidade_Online.Controllers
{
    public class PilarController : Controller
    {
        // GET: Pilar
        public ActionResult Manter(int? id)
        {
            var pilarViewModel = new PilarViewModel();
            using (var contexto = new ContextoDeDadosEF())
            {
                var pilarServico = ServicoDeDependencia.MontarPilarServico(contexto);

                if (id.HasValue && id.Value > 0)
                {
                    var pilar = new Pilar() { Id = id.Value };
                    var pilarDaBase = pilarServico.BuscarPorId(pilar);
                    pilarViewModel = Mapper.Map<Pilar, PilarViewModel>(pilarDaBase);

                    var subtopicoServico = ServicoDeDependencia.MontarSubtopicoServico(contexto);
                    pilarViewModel.Subtopicos = subtopicoServico.Listar(pilar);
                }

            }

            return View("Pilar", pilarViewModel);
        }

        public ActionResult Salvar(PilarViewModel pilarModel)
        {
            if (ModelState.IsValid)
            {
                using (var contexto = new ContextoDeDadosEF())
                {
                    //Adicionar no projeto
                    var pilar = new Pilar();
                    if (pilarModel.Id.HasValue)
                    {
                        pilar.Id = pilarModel.Id.Value;
                    }
                    pilar.Titulo = pilarModel.Titulo;

                    var pilarServico = ServicoDeDependencia.MontarPilarServico(contexto);

                    try
                    {
                        pilarServico.Persistir(pilar);
                    }
                    catch (UsuarioException e)
                    {
                        ModelState.AddModelError("", e.Message);
                    }

                    if (pilarModel.Id != null && pilarModel.Id.Value > 0)
                    {
                        TempData["MensagemSucesso"] = "Pilar alterado com sucesso.";
                    }
                    else
                    {
                        TempData["MensagemSucesso"] = "Pilar cadastrado com sucesso.";
                    }
                }

                return RedirectToAction("Manter");
            }

            return RedirectToAction("Manter");
        }

        public ActionResult ListarPilares()
        {
            using (var contexto = new ContextoDeDadosEF())
            {
                var pilarServico = ServicoDeDependencia.MontarPilarServico(contexto);
                var listaDePilares = pilarServico.Listar();


                return View("ListarPilares", listaDePilares);
            }

        }

        public ActionResult ExcluirPilar(int id)
        {
            using (var contexto = new ContextoDeDadosEF())
            {
                var usuarioAutenticado = new Usuario() { Email = ServicoDeAutenticacao.UsuarioLogado.Email };
                var usuarioServico = ServicoDeDependencia.MontarUsuarioServico(contexto).BuscarPorEmail(usuarioAutenticado);

                if (usuarioServico.Permissao.Equals(Permissao.ADMINISTRADOR))
                {

                    var pilarServico = ServicoDeDependencia.MontarPilarServico(contexto);
                    var pilar = new Pilar() { Id = id };
                    pilar = pilarServico.BuscarPorId(pilar);

                    pilarServico.Remover(pilar);
                }
            }

            return RedirectToAction("ListarPilares");
        }




        // Partial View
        //[Autorizador]
        public ActionResult PesquisarPilares(int[] id)
        {

            using (var contexto = new ContextoDeDadosEF())
            {
                var subtopicoServico = ServicoDeDependencia.MontarSubtopicoServico(contexto);



                var lista = subtopicoServico.ListarPorPilar(id.FirstOrDefault());
                var listaTotal = subtopicoServico.Listar();

                // TODO: consultar banco
                if (lista.Count < 1) return PartialView("_Subtopicos");

                return PartialView("_Pilares", lista);
            }


        }
    }
}