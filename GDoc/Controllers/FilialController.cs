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
    public class FilialController : Controller
    {
        private Entities db = new Entities();

        // GET: /Filial/
        public ActionResult Index()
        {
            Seguranca.ValidaAcesso(22);
            return View(db.Filiais.ToList());
        }

        // GET: /Filial/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(22);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filial filial = db.Filiais.Find(id);
            if (filial == null)
            {
                return HttpNotFound();
            }
            return View(filial);
        }

        // GET: /Filial/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(23);
            return View();
        }

        // POST: /Filial/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Nome,Ativa")] Filial filial)
        {
            Seguranca.ValidaAcesso(23);
            if (ModelState.IsValid)
            {
                db.Filiais.Add(filial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(filial);
        }

        // GET: /Filial/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(24);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filial filial = db.Filiais.Find(id);
            if (filial == null)
            {
                return HttpNotFound();
            }
            return View(filial);
        }

        // POST: /Filial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Nome,Ativa")] Filial filial)
        {
            Seguranca.ValidaAcesso(24);
            if (ModelState.IsValid)
            {
                db.Entry(filial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(filial);
        }

        // GET: /Filial/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(25);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filial filial = db.Filiais.Find(id);
            if (filial == null)
            {
                return HttpNotFound();
            }
            return View(filial);
        }

        // POST: /Filial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(25);
            Filial filial = db.Filiais.Find(id);
            db.Filiais.Remove(filial);
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
