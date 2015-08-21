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
    public class ConvenioController : Controller
    {
        private Entities db = new Entities();

        // GET: /Convenio/
        public ActionResult Index()
        {
            Seguranca.ValidaAcesso(18);
            return View(db.Convenios.ToList());
        }

        // GET: /Convenio/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(18);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenio convenio = db.Convenios.Find(id);
            if (convenio == null)
            {
                return HttpNotFound();
            }
            return View(convenio);
        }

        // GET: /Convenio/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(19);
            return View();
        }

        // POST: /Convenio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Nome")] Convenio convenio)
        {
            Seguranca.ValidaAcesso(19);
            if (ModelState.IsValid)
            {
                db.Convenios.Add(convenio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(convenio);
        }

        // GET: /Convenio/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(20);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenio convenio = db.Convenios.Find(id);
            if (convenio == null)
            {
                return HttpNotFound();
            }
            return View(convenio);
        }

        // POST: /Convenio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Nome")] Convenio convenio)
        {
            Seguranca.ValidaAcesso(20);
            if (ModelState.IsValid)
            {
                db.Entry(convenio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(convenio);
        }

        // GET: /Convenio/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(21);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenio convenio = db.Convenios.Find(id);
            if (convenio == null)
            {
                return HttpNotFound();
            }
            return View(convenio);
        }

        // POST: /Convenio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(21);
            Convenio convenio = db.Convenios.Find(id);
            db.Convenios.Remove(convenio);
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
