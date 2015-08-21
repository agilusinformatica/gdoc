using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GDoc.Models;
using System.Text;
using GDoc.ViewModels;

namespace GDoc.Controllers
{
    public class UsuarioController : Controller
    {
        private Entities db = new Entities();

        // GET: /Usuario/
        public ActionResult Index()
        {
            Seguranca.ValidaAcesso(0);
            var usuarios = db.Usuarios.Include(u => u.Filial).Include(u => u.Perfil);
            return View(usuarios.ToList());
        }

        // GET: /Usuario/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: /Usuario/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(1);
            ViewBag.IdFilial = new SelectList(db.Filiais, "Id", "Nome");
            ViewBag.IdPerfil = new SelectList(db.Perfis, "Id", "Nome");
            ViewBag.IdTipoTelefone = new SelectList(db.TiposTelefone, "Id", "Nome");
            ViewBag.TiposTelefone = db.TiposTelefone;

            var u = new UsuarioViewModel();
            return View(u);
        }

        private void MontaListaTelefones(HttpRequestBase request, UsuarioViewModel usu)
        {
            int contTelefone = 1;

            string telefone = request.Form["telefone" + contTelefone];
            while (! String.IsNullOrEmpty(telefone))
            {
                telefone = telefone.Replace("_"," ");
                string tipoTelefone = request.Form["idtipotelefone" + contTelefone];
                if (! String.IsNullOrEmpty(tipoTelefone))
                {
                    usu.Telefones.Add(new ItemTelefone { Telefone = telefone, IdTipoTelefone = Convert.ToInt16(tipoTelefone) });
                }
                else
                {
                    usu.Telefones.Add(new ItemTelefone { Telefone = telefone });
                }
                    
                contTelefone++;
                telefone = Request.Form["telefone" + contTelefone];
            }
        }

        private void TransferePropriedades( UsuarioViewModel usuOrigem, Usuario usuDestino)
        {
            usuDestino.Id = usuOrigem.Id;
            usuDestino.Login = usuOrigem.Login;
            usuDestino.Nome = usuOrigem.Nome;
            usuDestino.IdPerfil = usuOrigem.IdPerfil;
            usuDestino.IdFilial = usuOrigem.IdFilial;
            usuDestino.Email = usuOrigem.Email;
            usuDestino.Empresa = usuOrigem.Empresa;
            usuDestino.Cpf = usuOrigem.Cpf;
            usuDestino.Rg = usuOrigem.Rg;
            usuDestino.Endereco = usuOrigem.Endereco;
            usuDestino.NumeroEndereco = usuOrigem.NumeroEndereco;
            usuDestino.Bairro = usuOrigem.Bairro;
            usuDestino.Cidade = usuOrigem.Cidade;
            usuDestino.Uf = usuOrigem.Uf;
            usuDestino.Cep = usuOrigem.Cep;
            usuDestino.Ativo = usuOrigem.Ativo;
            usuDestino.TrocarSenha = usuOrigem.TrocarSenha;
            usuDestino.Complemento = usuOrigem.Complemento;
        }

        // POST: /Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Login,Nome,Senha,IdPerfil,IdFilial,Email,Empresa,Cpf,Rg,Endereco,NumeroEndereco,Bairro,Cidade,Uf,Cep,Ativo,TrocarSenha,Complemento")] UsuarioViewModel usuarioVM)
        {
            Seguranca.ValidaAcesso(1);
            MontaListaTelefones(Request, usuarioVM);
            
            if (ModelState.IsValid)
            {
                usuarioVM.Cpf = usuarioVM.Cpf.Replace(".","").Replace("-","");
                if (usuarioVM.Cep != null) 
                    usuarioVM.Cep = usuarioVM.Cep.Replace("-","");

                var usuario = new Usuario();
                TransferePropriedades(usuarioVM, usuario);
                db.Usuarios.Add(usuario);

                try
                {
                    db.SaveChanges();

                    foreach (var t in usuarioVM.Telefones)
                    {
                        db.TelefonesUsuario.Add(new TelefoneUsuario { IdUsuario = usuario.Id, Telefone = t.Telefone, IdTipoTelefone = t.IdTipoTelefone });                        
                    }

                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", 
                                                        eve.Entry.Entity.GetType().Name,
                                                        eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                        ve.PropertyName,
                                                        ve.ErrorMessage));
                        }
                    }
                    throw new DbEntityValidationException(sb.ToString(), e);
                }
                
                return RedirectToAction("Index");
            }

            ViewBag.IdFilial = new SelectList(db.Filiais, "Id", "Nome", usuarioVM.IdFilial);
            ViewBag.IdPerfil = new SelectList(db.Perfis, "Id", "Nome", usuarioVM.IdPerfil);
            ViewBag.IdTipoTelefone = new SelectList(db.TiposTelefone, "Id", "Nome");
            ViewBag.TiposTelefone = db.TiposTelefone;
            return View(usuarioVM);
        }

        // GET: /Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(2);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            var usuarioVM = new UsuarioViewModel();
            usuarioVM.Id = usuario.Id;
            usuarioVM.Login = usuario.Login;
            usuarioVM.Nome = usuario.Nome;
            usuarioVM.IdPerfil = usuario.IdPerfil;
            usuarioVM.IdFilial = usuario.IdFilial;
            usuarioVM.Email = usuario.Email;
            usuarioVM.Empresa = usuario.Empresa;
            usuarioVM.Cpf = usuario.Cpf;
            usuarioVM.Rg = usuario.Rg;
            usuarioVM.Endereco = usuario.Endereco;
            usuarioVM.NumeroEndereco = usuario.NumeroEndereco;
            usuarioVM.Bairro = usuario.Bairro;
            usuarioVM.Cidade = usuario.Cidade;
            usuarioVM.Uf = usuario.Uf;
            usuarioVM.Cep = usuario.Cep;
            usuarioVM.Ativo = usuario.Ativo;
            usuarioVM.TrocarSenha = usuario.TrocarSenha;
            usuarioVM.Complemento = usuario.Complemento;

            var telefones = db.TelefonesUsuario.Where(t => t.IdUsuario == id).ToList();
            foreach (var t in telefones)
            {
                usuarioVM.Telefones.Add(new ItemTelefone { Telefone = t.Telefone, IdTipoTelefone = t.IdTipoTelefone });
            }

            ViewBag.IdFilial = new SelectList(db.Filiais, "Id", "Nome", usuarioVM.IdFilial);
            ViewBag.IdPerfil = new SelectList(db.Perfis, "Id", "Nome", usuarioVM.IdPerfil);
            ViewBag.IdTipoTelefone = new SelectList(db.TiposTelefone, "Id", "Nome");
            ViewBag.TiposTelefone = db.TiposTelefone;

            return View(usuarioVM);
        }

        // POST: /Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Login,Nome,Senha,IdPerfil,IdFilial,Email,Empresa,Cpf,Rg,Endereco,NumeroEndereco,Bairro,Cidade,Uf,Cep,Ativo,TrocarSenha,Complemento")] UsuarioViewModel usuarioVM)
        {
            Seguranca.ValidaAcesso(2);
            MontaListaTelefones(Request, usuarioVM);

            if (ModelState.IsValid)
            {
                usuarioVM.Cpf = usuarioVM.Cpf.Replace(".","").Replace("-","");
                if (usuarioVM.Cep != null) 
                    usuarioVM.Cep = usuarioVM.Cep.Replace("-","");

                var usuario = new Usuario();
                TransferePropriedades(usuarioVM, usuario);

                db.Entry(usuario).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();

                    int? sequencia = db.ObtemSequencia().FirstOrDefault();

                    foreach (var item in usuarioVM.Telefones)
                    {
                        db.Database.ExecuteSqlCommand("insert into AuxTelefoneUsuario (Seq, IdUsuario, IdTipoTelefone, Telefone) values({0}, {1}, {2}, {3})",
                            sequencia, usuario.Id, item.IdTipoTelefone, item.Telefone);
                    }
                    db.GravaTelefonesUsuario(sequencia, usuario.Id);
                }
                catch (DbEntityValidationException e)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", 
                                                        eve.Entry.Entity.GetType().Name,
                                                        eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                        ve.PropertyName,
                                                        ve.ErrorMessage));
                        }
                    }
                    throw new DbEntityValidationException(sb.ToString(), e);
                }
                return RedirectToAction("Index");
            }
            ViewBag.IdFilial = new SelectList(db.Filiais, "Id", "Nome", usuarioVM.IdFilial);
            ViewBag.IdPerfil = new SelectList(db.Perfis, "Id", "Nome", usuarioVM.IdPerfil);
            ViewBag.IdTipoTelefone = new SelectList(db.TiposTelefone, "Id", "Nome");
            ViewBag.TiposTelefone = db.TiposTelefone;
            return View(usuarioVM);
        }

        // GET: /Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(3);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: /Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(3);
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TrocarSenha(int id)
        {
            if (id != UsuarioLogado.IdUsuario)
            {
                Seguranca.ValidaAcesso(4);    
            }

            var s = new SenhaViewModel();
            s.Id = id;
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrocarSenha(SenhaViewModel senha)
        {
            if (senha.Id != UsuarioLogado.IdUsuario)
            {
                Seguranca.ValidaAcesso(4);    
            }
            /*if (senha.SenhaNova != Seguranca.Encode(senha.SenhaNovaRepeticao))
            {
                ModelState.AddModelError("SenhaNova", "Senha nova não confere com a senha repetida");
            }*/
            if (ModelState.IsValid)
            {
                var usuario = db.Usuarios.Where(u => u.Id == senha.Id).First();
                usuario.Senha = senha.SenhaNova;
                usuario.TrocarSenha = false;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(senha);
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
