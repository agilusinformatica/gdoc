//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        public Usuario()
        {
            this.Propostas = new HashSet<Proposta>();
            this.PropostasDigitadas = new HashSet<Proposta>();
            this.TelefonesUsuario = new HashSet<TelefoneUsuario>();
        }
    
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public int IdPerfil { get; set; }
        public int IdFilial { get; set; }
        public string Email { get; set; }
        public string Empresa { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Endereco { get; set; }
        public string NumeroEndereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public bool Ativo { get; set; }
        public bool TrocarSenha { get; set; }
        public string Complemento { get; set; }
    
        public virtual Filial Filial { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual ICollection<Proposta> Propostas { get; set; }
        public virtual ICollection<Proposta> PropostasDigitadas { get; set; }
        public virtual ICollection<TelefoneUsuario> TelefonesUsuario { get; set; }
    }
}
