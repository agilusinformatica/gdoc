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
    public class BancoController : Controller
    {
        private Entities db = new Entities();

        // GET: /Banco/
        public ActionResult Index()
        {
            Seguranca.ValidaAcesso(14);
            return View(db.Bancos.ToList());
        }

        // GET: /Banco/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(14);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banco banco = db.Bancos.Find(id);
            if (banco == null)
            {
                return HttpNotFound();
            }
            return View(banco);
        }

        // GET: /Banco/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(15);
            var bancoVM = new BancoViewModel();

            var tiposDocumento = db.TiposDocumento.ToList();
            foreach (var d in tiposDocumento)
            {
                bancoVM.Documentos.Add(new ViewModels.ItemDocumento() { Id = d.Id, Nome = d.Nome, Obrigatorio = false, Selecionado = false });
            }
            return View(bancoVM);
        }

        // POST: /Banco/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome")] BancoViewModel bancoVM)
        {
            Seguranca.ValidaAcesso(15);
            if (ModelState.IsValid)
            {
                var banco = new Banco();
                banco.Id = bancoVM.Id;
                banco.Nome = bancoVM.Nome;
                db.Bancos.Add(banco);

                var docs = db.TiposDocumento.ToList();
                foreach (var d in docs)
                {
                    if (Request.Form["s" + d.Id] == "on")
                    {
                        bool obrigatorio = (Request.Form["o" + d.Id] == "on");
                        var docBanco = new BancoDocumento();
                        docBanco.IdBanco = banco.Id;
                        docBanco.IdTipoDocumento = d.Id;
                        docBanco.Obrigatorio = obrigatorio;
                        db.BancoDocumentos.Add(docBanco);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bancoVM);
        }

        // GET: /Banco/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(16);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banco banco = db.Bancos.Find(id);
            if (banco == null)
            {
                return HttpNotFound();
            }

            var bancoVM = new BancoViewModel();
            bancoVM.Id = banco.Id;
            bancoVM.Nome = banco.Nome;

            var tiposDocumento = db.TiposDocumento.ToList();
            foreach (var td in tiposDocumento)
            {
                var doc = banco.BancoDocumentos.Where(d => d.TipoDocumento.Id == td.Id).FirstOrDefault();

                bool selecionado = false;
                bool obrigatorio = false;
                if (doc != null)
                {
                    selecionado = true;
                    obrigatorio = doc.Obrigatorio;
                }
                bancoVM.Documentos.Add(new ViewModels.ItemDocumento() { Id = td.Id, Nome = td.Nome, Obrigatorio = obrigatorio, Selecionado = selecionado });
            }

            return View(bancoVM);
        }

        // POST: /Banco/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome")] BancoViewModel bancoVM)
        {
            Seguranca.ValidaAcesso(16);
            if (ModelState.IsValid)
            {
                var banco = db.Bancos.Find(bancoVM.Id);
                if (banco == null)
                {
                    return HttpNotFound();
                }
                banco.Nome = bancoVM.Nome;
                db.Entry(banco).State = EntityState.Modified;

                // monta o bancoVM conforme o post do Form
                var docs = db.TiposDocumento.ToList();
                foreach (var d in docs)
                {
                    bool selecionado = false;
                    bool obrigatorio = false;
                    if (Request.Form["s" + d.Id] == "on")
                    {
                        selecionado = true;
                        obrigatorio = (Request.Form["o" + d.Id] == "on");
                    }
                    var doc = new ItemDocumento();
                    doc.Id = d.Id;
                    doc.Selecionado = selecionado;
                    doc.Obrigatorio = obrigatorio;
                    bancoVM.Documentos.Add(doc);
                }

                int? sequencia = db.ObtemSequencia().FirstOrDefault();

                foreach (var item in bancoVM.Documentos.Where(d => d.Selecionado))
                {
                    db.Database.ExecuteSqlCommand("insert into AuxBancoDocumento (Seq, IdBanco, IdTipoDocumento, Obrigatorio) values({0}, {1}, {2}, {3})",
                        sequencia, banco.Id, item.Id, item.Obrigatorio);
                }

                db.GravaBancoDocumentos(sequencia, banco.Id);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bancoVM);
        }

        // GET: /Banco/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(17);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banco banco = db.Bancos.Find(id);
            if (banco == null)
            {
                return HttpNotFound();
            }
            return View(banco);
        }

        // POST: /Banco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(17);
            Banco banco = db.Bancos.Find(id);
            db.Bancos.Remove(banco);
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
