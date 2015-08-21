using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GDoc.Models;

namespace GDoc.Controllers
{
    public class TipoTelefoneController : Controller
    {
        private Entities db = new Entities();

        // GET: /TipoTelefone/
        public ActionResult Index()
        {
            Seguranca.ValidaAcesso(34);
            return View(db.TiposTelefone.ToList());
        }

        // GET: /TipoTelefone/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(34);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTelefone tipotelefone = db.TiposTelefone.Find(id);
            if (tipotelefone == null)
            {
                return HttpNotFound();
            }
            return View(tipotelefone);
        }

        // GET: /TipoTelefone/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(35);
            return View();
        }

        // POST: /TipoTelefone/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Nome")] TipoTelefone tipotelefone)
        {
            Seguranca.ValidaAcesso(35);
            if (ModelState.IsValid)
            {
                db.TiposTelefone.Add(tipotelefone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipotelefone);
        }

        // GET: /TipoTelefone/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(36);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTelefone tipotelefone = db.TiposTelefone.Find(id);
            if (tipotelefone == null)
            {
                return HttpNotFound();
            }
            return View(tipotelefone);
        }

        // POST: /TipoTelefone/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Nome")] TipoTelefone tipotelefone)
        {
            Seguranca.ValidaAcesso(36);
            if (ModelState.IsValid)
            {
                db.Entry(tipotelefone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipotelefone);
        }

        // GET: /TipoTelefone/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(37);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTelefone tipotelefone = db.TiposTelefone.Find(id);
            if (tipotelefone == null)
            {
                return HttpNotFound();
            }
            return View(tipotelefone);
        }

        // POST: /TipoTelefone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(37);
            TipoTelefone tipotelefone = db.TiposTelefone.Find(id);
            db.TiposTelefone.Remove(tipotelefone);
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
