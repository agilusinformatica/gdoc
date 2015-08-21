﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GDoc.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Acesso> Acessos { get; set; }
        public virtual DbSet<Banco> Bancos { get; set; }
        public virtual DbSet<BancoDocumento> BancoDocumentos { get; set; }
        public virtual DbSet<Convenio> Convenios { get; set; }
        public virtual DbSet<Divida> Dividas { get; set; }
        public virtual DbSet<DocumentoProposta> DocumentosProposta { get; set; }
        public virtual DbSet<Filial> Filiais { get; set; }
        public virtual DbSet<Funcao> Funcoes { get; set; }
        public virtual DbSet<Parametro> Parametros { get; set; }
        public virtual DbSet<Perfil> Perfis { get; set; }
        public virtual DbSet<Proposta> Propostas { get; set; }
        public virtual DbSet<TelefoneProposta> TelefonesProposta { get; set; }
        public virtual DbSet<TelefoneUsuario> TelefonesUsuario { get; set; }
        public virtual DbSet<TipoConta> TiposConta { get; set; }
        public virtual DbSet<TipoDocumento> TiposDocumento { get; set; }
        public virtual DbSet<TipoTelefone> TiposTelefone { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
    
        public virtual ObjectResult<GravaAcessos_Result> GravaAcessos(Nullable<int> seq, Nullable<int> idPerfil)
        {
            var seqParameter = seq.HasValue ?
                new ObjectParameter("seq", seq) :
                new ObjectParameter("seq", typeof(int));
    
            var idPerfilParameter = idPerfil.HasValue ?
                new ObjectParameter("IdPerfil", idPerfil) :
                new ObjectParameter("IdPerfil", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GravaAcessos_Result>("GravaAcessos", seqParameter, idPerfilParameter);
        }
    
        public virtual int GravaBancoDocumentos(Nullable<int> seq, Nullable<int> idBanco)
        {
            var seqParameter = seq.HasValue ?
                new ObjectParameter("seq", seq) :
                new ObjectParameter("seq", typeof(int));
    
            var idBancoParameter = idBanco.HasValue ?
                new ObjectParameter("IdBanco", idBanco) :
                new ObjectParameter("IdBanco", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GravaBancoDocumentos", seqParameter, idBancoParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> ObtemSequencia()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ObtemSequencia");
        }
    
        public virtual int GravaTelefonesUsuario(Nullable<int> seq, Nullable<int> idUsuario)
        {
            var seqParameter = seq.HasValue ?
                new ObjectParameter("seq", seq) :
                new ObjectParameter("seq", typeof(int));
    
            var idUsuarioParameter = idUsuario.HasValue ?
                new ObjectParameter("IdUsuario", idUsuario) :
                new ObjectParameter("IdUsuario", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GravaTelefonesUsuario", seqParameter, idUsuarioParameter);
        }
    
        public virtual int GravaDividasProposta(Nullable<int> seq, Nullable<int> idProposta)
        {
            var seqParameter = seq.HasValue ?
                new ObjectParameter("seq", seq) :
                new ObjectParameter("seq", typeof(int));
    
            var idPropostaParameter = idProposta.HasValue ?
                new ObjectParameter("IdProposta", idProposta) :
                new ObjectParameter("IdProposta", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GravaDividasProposta", seqParameter, idPropostaParameter);
        }
    
        public virtual int GravaTelefonesProposta(Nullable<int> seq, Nullable<int> idProposta)
        {
            var seqParameter = seq.HasValue ?
                new ObjectParameter("seq", seq) :
                new ObjectParameter("seq", typeof(int));
    
            var idPropostaParameter = idProposta.HasValue ?
                new ObjectParameter("IdProposta", idProposta) :
                new ObjectParameter("IdProposta", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GravaTelefonesProposta", seqParameter, idPropostaParameter);
        }
    }
}
