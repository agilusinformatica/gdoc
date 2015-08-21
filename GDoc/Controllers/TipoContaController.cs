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
    public class TipoContaController : Controller
    {
        private Entities db = new Entities();

        // GET: /TipoConta/
        public ActionResult Index()
        {
            Seguranca.ValidaAcesso(26);
            return View(db.TiposConta.ToList());
        }

        // GET: /TipoConta/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(26);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoConta tipoconta = db.TiposConta.Find(id);
            if (tipoconta == null)
            {
                return HttpNotFound();
            }
            return View(tipoconta);
        }

        // GET: /TipoConta/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(27);
            return View();
        }

        // POST: /TipoConta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Nome")] TipoConta tipoconta)
        {
            Seguranca.ValidaAcesso(27);
            if (ModelState.IsValid)
            {
                db.TiposConta.Add(tipoconta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoconta);
        }

        // GET: /TipoConta/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(28);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoConta tipoconta = db.TiposConta.Find(id);
            if (tipoconta == null)
            {
                return HttpNotFound();
            }
            return View(tipoconta);
        }

        // POST: /TipoConta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Nome")] TipoConta tipoconta)
        {
            Seguranca.ValidaAcesso(28);
            if (ModelState.IsValid)
            {
                db.Entry(tipoconta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoconta);
        }

        // GET: /TipoConta/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(29);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoConta tipoconta = db.TiposConta.Find(id);
            if (tipoconta == null)
            {
                return HttpNotFound();
            }
            return View(tipoconta);
        }

        // POST: /TipoConta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(29);
            TipoConta tipoconta = db.TiposConta.Find(id);
            db.TiposConta.Remove(tipoconta);
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
