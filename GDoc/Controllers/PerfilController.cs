using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GDoc.Models;
using GDoc.ViewModels;

namespace GDoc.Controllers
{
    public class PerfilController : Controller
    {
        private Entities db = new Entities();

        // GET: /Perfil/
        public ActionResult Index()
        {
            Seguranca.ValidaAcesso(10);
            return View(db.Perfis.ToList());
        }

        // GET: /Perfil/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(10);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfis.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        // GET: /Perfil/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(11);
            var funcoes = db.Funcoes;

            var perfilVM = new ViewModels.PerfilViewModel();
            foreach (var f in funcoes)
            {
                perfilVM.Acessos.Add(new ViewModels.ItemAcesso
                {
                    IdFuncao = f.Id,
                    Nome = f.Nome,
                    Selecionado = false,
                    IdFuncaoSuperior = f.IdFuncaoSuperior
                });

            }

            return View(perfilVM);
        }

        // POST: /Perfil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Ativo,LimitaPropostaUsuarioLogado")] PerfilViewModel perfilVM)
        {
            Seguranca.ValidaAcesso(11);
            if (ModelState.IsValid)
            {
                var perfil = new Perfil();
                perfil.Nome = perfilVM.Nome;
                perfil.Ativo = perfilVM.Ativo;
                perfil.LimitaPropostaUsuarioLogado = perfilVM.LimitaPropostaUsuarioLogado;
                db.Perfis.Add(perfil);
                db.SaveChanges();

                //perfil.Id = db.Perfis.Max(i => i.Id);

                var funcoes = db.Funcoes;
                foreach (var f in funcoes)
                {
                    if (Request.Form["s" + f.Id] == "on")
                    {
                        var acesso = new Acesso();
                        acesso.IdFuncao = f.Id;
                        acesso.IdPerfil = perfil.Id;
                        db.Acessos.Add(acesso);
                    }
                }
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            return View(perfilVM);
        }

        // GET: /Perfil/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(12);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfis.Find(id);

            if (perfil == null)
            {
                return HttpNotFound();
            }

            var perfilVM = new ViewModels.PerfilViewModel();
            perfilVM.Id = perfil.Id;
            perfilVM.Nome = perfil.Nome;
            perfilVM.Ativo = perfil.Ativo;
            perfilVM.LimitaPropostaUsuarioLogado = perfil.LimitaPropostaUsuarioLogado;

            var funcoes = db.Funcoes.ToList();

            foreach (var f in funcoes)
            {
                var ac = perfil.Acessos.Where(d => d.Funcao.Id == f.Id).FirstOrDefault();

                bool selecionado = ac != null;
                perfilVM.Acessos.Add(new ViewModels.ItemAcesso() { IdFuncao = f.Id, Nome = f.Nome, Selecionado = selecionado, IdFuncaoSuperior = f.IdFuncaoSuperior });
            }

            return View(perfilVM);
        }

        // POST: /Perfil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Ativo,LimitaPropostaUsuarioLogado")] PerfilViewModel perfilVM)
        {
            Seguranca.ValidaAcesso(12);
            if (ModelState.IsValid)
            {
                var perfil = db.Perfis.Find(perfilVM.Id);
                if (perfil == null)
                {
                    return HttpNotFound();
                }
                perfil.Nome = perfilVM.Nome;
                perfil.Ativo = perfilVM.Ativo;
                perfil.LimitaPropostaUsuarioLogado = perfilVM.LimitaPropostaUsuarioLogado;
                db.Entry(perfil).State = EntityState.Modified;
                db.SaveChanges();

                // monta o perfilVM conforme o post do Form
                var funcoes = db.Funcoes.ToList();
                foreach (var f in funcoes)
                {
                    bool selecionado = (Request.Form["s" + f.Id] == "on");
                    var acesso = new ItemAcesso();
                    acesso.IdFuncao = f.Id;
                    acesso.Selecionado = selecionado;
                    perfilVM.Acessos.Add(acesso);
                }


                int? sequencia = db.ObtemSequencia().FirstOrDefault();

                foreach (var item in perfilVM.Acessos.Where(a => a.Selecionado))
                {
                    db.Database.ExecuteSqlCommand("insert into AuxAcesso (Seq, IdPerfil, IdFuncao) values({0}, {1}, {2})",
                        sequencia, perfil.Id, item.IdFuncao);
                }

                db.GravaAcessos(sequencia, perfil.Id);

                return RedirectToAction("Index");
            }
            return View(perfilVM);
        }

        // GET: /Perfil/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(13);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfis.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        // POST: /Perfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(13);
            Perfil perfil = db.Perfis.Find(id);
            db.Perfis.Remove(perfil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
