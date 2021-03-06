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
    
    public partial class Proposta
    {
        public Proposta()
        {
            this.Dividas = new HashSet<Divida>();
            this.DocumentosProposta = new HashSet<DocumentoProposta>();
            this.TelefonesProposta = new HashSet<TelefoneProposta>();
        }
    
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public Nullable<System.DateTime> DataEntrada { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public Nullable<System.DateTime> DataNascimento { get; set; }
        public string Matricula { get; set; }
        public string SenhaConvenio { get; set; }
        public Nullable<int> IdBancoConta { get; set; }
        public string Agencia { get; set; }
        public string ContaCorrente { get; set; }
        public Nullable<int> IdTipoConta { get; set; }
        public Nullable<int> IdUsuarioDigitacao { get; set; }
        public Nullable<System.DateTime> DataDigitacao { get; set; }
        public int IdConvenio { get; set; }
        public int IdBanco { get; set; }
        public Nullable<decimal> ValorParcela { get; set; }
        public Nullable<decimal> MargemLivre { get; set; }
        public Nullable<decimal> MargemUtilizada { get; set; }
        public Nullable<double> Coeficiente { get; set; }
        public Nullable<int> QtdeParcelas { get; set; }
        public Nullable<decimal> ValorContrato { get; set; }
        public string Tabela { get; set; }
        public Nullable<System.DateTime> DataCoeficiente { get; set; }
        public string Observacao { get; set; }
        public string Endereco { get; set; }
        public string NumeroEndereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public Nullable<int> StatusDocumentacao { get; set; }
        public Nullable<System.DateTime> DataImpressao { get; set; }
    
        public virtual Banco Banco { get; set; }
        public virtual Convenio Convenio { get; set; }
        public virtual ICollection<Divida> Dividas { get; set; }
        public virtual ICollection<DocumentoProposta> DocumentosProposta { get; set; }
        public virtual TipoConta TipoConta { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario UsuarioDigitacao { get; set; }
        public virtual ICollection<TelefoneProposta> TelefonesProposta { get; set; }
        public virtual Banco BancoConta { get; set; }
    }
}
