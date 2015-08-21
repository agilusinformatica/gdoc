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
    public class TipoDocumentoController : Controller
    {
        private Entities db = new Entities();

        // GET: /TipoDocumento/
        public ActionResult Index()
        {
            Seguranca.ValidaAcesso(30);
            return View(db.TiposDocumento.ToList());
        }

        // GET: /TipoDocumento/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(30);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumento tipodocumento = db.TiposDocumento.Find(id);
            if (tipodocumento == null)
            {
                return HttpNotFound();
            }
            return View(tipodocumento);
        }

        // GET: /TipoDocumento/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(31);
            return View();
        }

        // POST: /TipoDocumento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Nome")] TipoDocumento tipodocumento)
        {
            Seguranca.ValidaAcesso(31);
            if (ModelState.IsValid)
            {
                db.TiposDocumento.Add(tipodocumento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipodocumento);
        }

        // GET: /TipoDocumento/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(32);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumento tipodocumento = db.TiposDocumento.Find(id);
            if (tipodocumento == null)
            {
                return HttpNotFound();
            }
            return View(tipodocumento);
        }

        // POST: /TipoDocumento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Nome")] TipoDocumento tipodocumento)
        {
            Seguranca.ValidaAcesso(32);
            if (ModelState.IsValid)
            {
                db.Entry(tipodocumento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipodocumento);
        }

        // GET: /TipoDocumento/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(33);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumento tipodocumento = db.TiposDocumento.Find(id);
            if (tipodocumento == null)
            {
                return HttpNotFound();
            }
            return View(tipodocumento);
        }

        // POST: /TipoDocumento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(33);
            TipoDocumento tipodocumento = db.TiposDocumento.Find(id);
            db.TiposDocumento.Remove(tipodocumento);
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
