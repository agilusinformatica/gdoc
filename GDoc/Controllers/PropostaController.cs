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
using System.Data.Entity.Validation;
using System.Text;
using System.IO;
using System.Globalization;

namespace GDoc.Controllers
{
    public class PropostaController : Controller
    {
        private Entities db = new Entities();

        // GET: /Proposta/
        public ActionResult Index(int? Status, string DataEntradaInicial, string DataEntradaFinal)
        {
            Seguranca.ValidaAcesso(5);
            DateTime dataInicial = DateTime.Today, dataFinal = DateTime.Today;
            if (String.IsNullOrEmpty(DataEntradaInicial) && String.IsNullOrEmpty(DataEntradaFinal))
            {
                DataEntradaInicial = DateTime.Today.ToShortDateString();
                DataEntradaFinal = DataEntradaInicial;
            }
            else
            {
                if (! String.IsNullOrEmpty(DataEntradaInicial))
                    dataInicial = DateTime.ParseExact(DataEntradaInicial, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (! String.IsNullOrEmpty(DataEntradaFinal))
                    dataFinal = DateTime.ParseExact(DataEntradaFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            dataFinal = dataFinal.AddDays(1);
    
            var propostas = (from pr in db.Propostas
                             where (Status == null || pr.StatusDocumentacao == Status)
                                && (String.IsNullOrEmpty(DataEntradaInicial) || pr.DataEntrada >= dataInicial)
                                && (String.IsNullOrEmpty(DataEntradaFinal) || pr.DataEntrada < dataFinal)
                                && (!UsuarioLogado.LimitaProposta || pr.Usuario.Id == UsuarioLogado.IdUsuario)
                             select pr).Include(p => p.Banco).Include(p => p.Convenio).Include(p => p.TipoConta).Include(p => p.Usuario).Include(p => p.UsuarioDigitacao);

            return View(propostas.ToList());
        }

        // GET: /Proposta/Details/5
        public ActionResult Details(int? id)
        {
            Seguranca.ValidaAcesso(5);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposta proposta = db.Propostas.Find(id);
            if (proposta == null)
            {
                return HttpNotFound();
            }
            return View(proposta);
        }

        // GET: /Proposta/Create
        public ActionResult Create()
        {
            Seguranca.ValidaAcesso(6);
            ViewBag.IdBancoConta = new SelectList(db.Bancos, "Id", "Nome");
            ViewBag.IdConvenio = new SelectList(db.Convenios, "Id", "Nome");
            ViewBag.IdBanco = new SelectList(db.Bancos, "Id", "Nome");
            ViewBag.IdTipoConta = new SelectList(db.TiposConta, "Id", "Nome");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Login");
            ViewBag.IdUsuarioDigitacao = new SelectList(db.Usuarios, "Id", "Login");
            ViewBag.IdTipoTelefone = new SelectList(db.TiposTelefone, "Id", "Nome");
            ViewBag.TiposTelefone = db.TiposTelefone;
            ViewBag.Bancos = db.Bancos;

            var p = new ViewModels.PropostaViewModel();
            return View(p);
        }

        private void MontaListaTelefones(HttpRequestBase request, PropostaViewModel usu)
        {
            /*int contTelefone = 1;

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
            }*/

            string[] telefones = request.Form["telefone"].Split(',');
            string[] idtipostelefone = request.Form["idtipotelefone"].Split(',');
            
            for (int i = 0; i < telefones.Length; i++)
            {
                usu.Telefones.Add(new ItemTelefone { Telefone = telefones[i], IdTipoTelefone = Convert.ToInt32(idtipostelefone[i]) });    
            }
        }

        private void MontaListaDividas(HttpRequestBase request, PropostaViewModel prop)
        {
            int contador = 1;

            string idBanco = request.Form["idbancodivida" + contador];
            while (! String.IsNullOrEmpty(idBanco))
            {
                string saldoDevedor = request.Form["saldodevedordivida" + contador];
                string valorParcela = request.Form["valorparceladivida" + contador];
                if (! String.IsNullOrEmpty(valorParcela))
                {
                    decimal saldo = Convert.ToDecimal(saldoDevedor);
                    prop.Dividas.Add(new ItemDivida { IdBanco = Convert.ToInt16(idBanco), SaldoDevedor = Convert.ToDecimal(saldoDevedor), ValorParcela = Convert.ToDecimal(valorParcela) });

                }
                else
                {
                    prop.Dividas.Add(new ItemDivida { IdBanco = Convert.ToInt16(idBanco), SaldoDevedor = Convert.ToDecimal(saldoDevedor) });
                }
                    
                contador++;
                idBanco = request.Form["idbancodivida" + contador];
            }
        }

        private void TransferePropriedades( PropostaViewModel origem, Proposta destino)
        {
            destino.Id = origem.Id;
            destino.IdUsuario = origem.IdUsuario;
            destino.DataEntrada = origem.DataEntrada;
            destino.Nome = origem.Nome;
            destino.CPF = origem.CPF;
            destino.DataNascimento = origem.DataNascimento;
            destino.Matricula = origem.Matricula;
            destino.SenhaConvenio = origem.SenhaConvenio;
            destino.IdBancoConta = origem.IdBancoConta;
            destino.Agencia = origem.Agencia;
            destino.ContaCorrente = origem.ContaCorrente;
            destino.IdTipoConta = origem.IdTipoConta;
            destino.IdUsuarioDigitacao = origem.IdUsuarioDigitacao;
            destino.DataDigitacao = origem.DataDigitacao;
            destino.IdBanco = origem.IdBanco;
            destino.IdConvenio = origem.IdConvenio;
            destino.ValorParcela = origem.ValorParcela;
            destino.MargemLivre = origem.MargemLivre;
            destino.MargemUtilizada = origem.MargemUtilizada;
            destino.Coeficiente = origem.Coeficiente;
            destino.QtdeParcelas = origem.QtdeParcelas;
            destino.ValorContrato = origem.ValorContrato;
            destino.Tabela = origem.Tabela;
            destino.DataCoeficiente = origem.DataCoeficiente;
            destino.Observacao = origem.Observacao;
            destino.Endereco = origem.Endereco;
            destino.NumeroEndereco = origem.NumeroEndereco;
            destino.Bairro = origem.Bairro;
            destino.Cidade = origem.Cidade;
            destino.Uf = origem.Uf;
            destino.Cep = origem.Cep;
            destino.Complemento = origem.Complemento;
        }

        // POST: /Proposta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,IdUsuario,DataEntrada,Nome,CPF,DataNascimento,Matricula,SenhaConvenio,IdBancoConta,Agencia,ContaCorrente,IdTipoConta,IdUsuarioDigitacao,DataDigitacao,IdBanco,IdConvenio,ValorParcela,MargemLivre,MargemUtilizada,Coeficiente,QtdeParcelas,ValorContrato,Tabela,DataCoeficiente,Observacao,Endereco,NumeroEndereco,Bairro,Cidade,Uf,Cep,Complemento")] PropostaViewModel propostaVM)
        {
            Seguranca.ValidaAcesso(6);
            MontaListaTelefones(Request, propostaVM);
            MontaListaDividas(Request, propostaVM);

            if (ModelState.IsValid)
            {

                propostaVM.CPF = propostaVM.CPF.Replace(".","").Replace("-","");
                if (propostaVM.Cep != null) 
                    propostaVM.Cep = propostaVM.Cep.Replace("-","");

                var proposta = new Proposta();
                TransferePropriedades(propostaVM, proposta);
                proposta.IdUsuarioDigitacao = UsuarioLogado.IdUsuario;
                if (UsuarioLogado.LimitaProposta)
                {
                    proposta.IdUsuario = UsuarioLogado.IdUsuario;
                }
                db.Propostas.Add(proposta);
                try
                {
                    db.SaveChanges();

                    foreach (var t in propostaVM.Telefones)
                    {
                        db.TelefonesProposta.Add(new TelefoneProposta { IdProposta = proposta.Id, Telefone = t.Telefone, IdTipoTelefone = t.IdTipoTelefone });                        
                    }

                    foreach (var d in propostaVM.Dividas)
                    {
                        db.Dividas.Add(new Divida { IdProposta = proposta.Id, IdBanco = d.IdBanco, SaldoDevedor = d.SaldoDevedor, ValorParcela = d.ValorParcela});                        
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

            ViewBag.IdBancoConta = new SelectList(db.Bancos, "Id", "Nome", propostaVM.IdBancoConta);
            ViewBag.IdConvenio = new SelectList(db.Convenios, "Id", "Nome", propostaVM.IdConvenio);
            ViewBag.IdBanco = new SelectList(db.Bancos, "Id", "Nome", propostaVM.IdBanco);
            ViewBag.IdTipoConta = new SelectList(db.TiposConta, "Id", "Nome", propostaVM.IdTipoConta);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Login", propostaVM.IdUsuario);
            ViewBag.IdUsuarioDigitacao = new SelectList(db.Usuarios, "Id", "Login", propostaVM.IdUsuarioDigitacao);
            ViewBag.IdTipoTelefone = new SelectList(db.TiposTelefone, "Id", "Nome");
            ViewBag.TiposTelefone = db.TiposTelefone;
            ViewBag.Bancos = db.Bancos;
            return View(propostaVM);
        }

        // GET: /Proposta/Edit/5
        public ActionResult Edit(int? id)
        {
            Seguranca.ValidaAcesso(7);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposta proposta = db.Propostas.Find(id);
            if (proposta == null)
            {
                return HttpNotFound();
            }

            if (UsuarioLogado.LimitaProposta && (proposta.IdUsuario != UsuarioLogado.IdUsuario))
            {
                throw new Exception("Acesso não autorizado");
            }

            var propostaVM = new PropostaViewModel();
            propostaVM.Id = proposta.Id;
            propostaVM.IdUsuario = proposta.IdUsuario;
            propostaVM.DataEntrada = proposta.DataEntrada;
            propostaVM.Nome = proposta.Nome;
            propostaVM.CPF = proposta.CPF;
            propostaVM.DataNascimento = proposta.DataNascimento;
            propostaVM.Matricula = proposta.Matricula;
            propostaVM.SenhaConvenio = proposta.SenhaConvenio;
            propostaVM.IdBancoConta = proposta.IdBancoConta;
            propostaVM.Agencia = proposta.Agencia;
            propostaVM.ContaCorrente = proposta.ContaCorrente;
            propostaVM.IdTipoConta = proposta.IdTipoConta;
            propostaVM.IdUsuarioDigitacao = proposta.IdUsuarioDigitacao;
            propostaVM.DataDigitacao = proposta.DataDigitacao;
            propostaVM.IdBanco = proposta.IdBanco;
            propostaVM.IdConvenio = proposta.IdConvenio;
            propostaVM.ValorParcela = proposta.ValorParcela;
            propostaVM.MargemLivre = proposta.MargemLivre;
            propostaVM.MargemUtilizada = proposta.MargemUtilizada;
            propostaVM.Coeficiente = proposta.Coeficiente;
            propostaVM.QtdeParcelas = proposta.QtdeParcelas;
            propostaVM.ValorContrato = proposta.ValorContrato;
            propostaVM.Tabela = proposta.Tabela;
            propostaVM.DataCoeficiente = proposta.DataCoeficiente;
            propostaVM.Observacao = proposta.Observacao;
            propostaVM.Endereco = proposta.Endereco;
            propostaVM.NumeroEndereco = proposta.NumeroEndereco;
            propostaVM.Bairro = proposta.Bairro;
            propostaVM.Cidade = proposta.Cidade;
            propostaVM.Uf = proposta.Uf;
            propostaVM.Cep = proposta.Cep;
            propostaVM.Complemento = proposta.Complemento;

            var telefones = db.TelefonesProposta.Where(t => t.IdProposta == id).ToList();
            foreach (var t in telefones)
            {
                propostaVM.Telefones.Add(new ItemTelefone { Telefone = t.Telefone, IdTipoTelefone = t.IdTipoTelefone });
            }

            var dividas = db.Dividas.Where(t => t.IdProposta == id).ToList();
            foreach (var d in dividas)
            {
                propostaVM.Dividas.Add(new ItemDivida { IdBanco = d.IdBanco, SaldoDevedor = d.SaldoDevedor, ValorParcela = d.ValorParcela });
            }

            ViewBag.IdBancoConta = new SelectList(db.Bancos, "Id", "Nome", proposta.IdBancoConta);
            ViewBag.IdConvenio = new SelectList(db.Convenios, "Id", "Nome", proposta.IdConvenio);
            ViewBag.IdBanco = new SelectList(db.Bancos, "Id", "Nome", proposta.IdBanco);
            ViewBag.IdTipoConta = new SelectList(db.TiposConta, "Id", "Nome", proposta.IdTipoConta);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Login", proposta.IdUsuario);
            ViewBag.IdUsuarioDigitacao = new SelectList(db.Usuarios, "Id", "Login", proposta.IdUsuarioDigitacao);
            ViewBag.IdTipoTelefone = new SelectList(db.TiposTelefone, "Id", "Nome");
            ViewBag.TiposTelefone = db.TiposTelefone;
            ViewBag.Bancos = db.Bancos;
            return View(propostaVM);
        }

        // POST: /Proposta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,IdUsuario,DataEntrada,Nome,CPF,DataNascimento,Matricula,SenhaConvenio,IdBancoConta,Agencia,ContaCorrente,IdTipoConta,IdUsuarioDigitacao,DataDigitacao,IdBanco,IdConvenio,ValorParcela,MargemLivre,MargemUtilizada,Coeficiente,QtdeParcelas,ValorContrato,Tabela,DataCoeficiente,Observacao,Endereco,NumeroEndereco,Bairro,Cidade,Uf,Cep,Complemento")] PropostaViewModel propostaVM)
        {
            Seguranca.ValidaAcesso(7);
            MontaListaTelefones(Request, propostaVM);
            MontaListaDividas(Request, propostaVM);

            if (ModelState.IsValid)
            {
                propostaVM.CPF = propostaVM.CPF.Replace(".","").Replace("-","");
                if (propostaVM.Cep != null) 
                    propostaVM.Cep = propostaVM.Cep.Replace("-","");

                var proposta = new Proposta();
                TransferePropriedades(propostaVM, proposta);

                if (UsuarioLogado.LimitaProposta)
                {
                    proposta.IdUsuario = UsuarioLogado.IdUsuario;
                }

                db.Entry(proposta).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();

                    int? sequencia = db.ObtemSequencia().FirstOrDefault();

                    foreach (var item in propostaVM.Telefones)
                    {
                        db.Database.ExecuteSqlCommand("insert into AuxTelefoneProposta (Seq, IdProposta, IdTipoTelefone, Telefone) values({0}, {1}, {2}, {3})",
                            sequencia, proposta.Id, item.IdTipoTelefone, item.Telefone);
                    }
                    db.GravaTelefonesProposta(sequencia, proposta.Id);

                    foreach (var item in propostaVM.Dividas)
                    {
                        db.Database.ExecuteSqlCommand("insert into AuxDividaProposta (Seq, IdProposta, IdBanco, SaldoDevedor, ValorParcela) values({0}, {1}, {2}, {3}, {4})",
                            sequencia, proposta.Id, item.IdBanco, item.SaldoDevedor, item.ValorParcela);
                    }
                    db.GravaDividasProposta(sequencia, proposta.Id);
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
            ViewBag.IdBancoConta = new SelectList(db.Bancos, "Id", "Nome", propostaVM.IdBancoConta);
            ViewBag.IdConvenio = new SelectList(db.Convenios, "Id", "Nome", propostaVM.IdConvenio);
            ViewBag.IdBanco = new SelectList(db.Bancos, "Id", "Nome", propostaVM.IdBanco);
            ViewBag.IdTipoConta = new SelectList(db.TiposConta, "Id", "Nome", propostaVM.IdTipoConta);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Login", propostaVM.IdUsuario);
            ViewBag.IdUsuarioDigitacao = new SelectList(db.Usuarios, "Id", "Login", propostaVM.IdUsuarioDigitacao);
            ViewBag.IdTipoTelefone = new SelectList(db.TiposTelefone, "Id", "Nome");
            ViewBag.TiposTelefone = db.TiposTelefone;
            ViewBag.Bancos = db.Bancos;
            return View(propostaVM);
        }

        // GET: /Proposta/Delete/5
        public ActionResult Delete(int? id)
        {
            Seguranca.ValidaAcesso(8);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposta proposta = db.Propostas.Find(id);
            if (proposta == null)
            {
                return HttpNotFound();
            }
            return View(proposta);
        }

        // POST: /Proposta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seguranca.ValidaAcesso(8);
            Proposta proposta = db.Propostas.Find(id);
            db.Propostas.Remove(proposta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Proposta/Anexos/5
        public ActionResult Anexos(int? id)
        {
            Seguranca.ValidaAcesso(9);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposta proposta = db.Propostas.Find(id);
            if (proposta == null)
            {
                return HttpNotFound();
            }


            var anexosVM = new AnexosViewModel();
            anexosVM.IdProposta = (int)id;
            anexosVM.Imagens = db.DocumentosProposta.Where(d => d.IdProposta == id).ToList();
            anexosVM.Docs = db.BancoDocumentos.Where(d => d.IdBanco == proposta.IdBanco).Include(d => d.TipoDocumento).ToList();

            Parametro parametro = db.Parametros.First();
            anexosVM.PastaRaiz = parametro.PastaRaiz;
            anexosVM.TaxaCompressao = parametro.TaxaCompressao;
            ViewBag.Modelo = anexosVM;

            return View();
        }

        [HttpPost]
        public ActionResult Anexos(Imagem imagemAnexada)
        {
            Seguranca.ValidaAcesso(9);
            var parametro = db.Parametros.FirstOrDefault();
            string idProposta = Request.Form["IdProposta"];
            string idTipoDocumento = Request.Form["IdTipoDocumentoSelecionado"];
            string idDocumentoPropostaExcluido = Request.Form["IdDocumentoPropostaExcluido"];
            string pasta = Server.MapPath(Path.Combine(parametro.PastaRaiz, idProposta));
            if (idTipoDocumento != String.Empty)
            {

                
                if (!Directory.Exists(pasta))
                {
                    Directory.CreateDirectory(pasta);
                }

                foreach (var arquivo in imagemAnexada.Arquivos)
                {
                    if (arquivo.ContentLength > 0)
                    {
                        var nomeArquivo = Path.GetFileName(arquivo.FileName);
                        var nomeArquivoCompleto = Path.Combine(pasta, nomeArquivo);
                        arquivo.SaveAs(nomeArquivoCompleto);

                        DocumentoProposta doc = new DocumentoProposta();
                        doc.IdProposta = Convert.ToInt32(idProposta);
                        doc.IdTipoDocumento = Convert.ToInt32(idTipoDocumento);
                        doc.NomeArquivo = arquivo.FileName;
                        db.DocumentosProposta.Add(doc);
                        db.SaveChanges();

                    }
                }
            }

            if (idDocumentoPropostaExcluido != String.Empty)
            {
                var doc = db.DocumentosProposta.Find(Convert.ToInt32(idDocumentoPropostaExcluido));
                if (doc != null)
                {
                    db.DocumentosProposta.Remove(doc);
                    db.SaveChanges();
                    var nomeArquivo = Path.GetFileName(doc.NomeArquivo);
                    var nomeArquivoCompleto = Path.Combine(pasta, nomeArquivo);
                    if (System.IO.File.Exists(nomeArquivoCompleto))
                    {
                        System.IO.File.Delete(nomeArquivoCompleto);
                    }
                }
            }
            return RedirectToAction("Anexos", new { id = idProposta });
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
