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
    
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            this.BancoDocumentos = new HashSet<BancoDocumento>();
            this.DocumentosProposta = new HashSet<DocumentoProposta>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
    
        public virtual ICollection<BancoDocumento> BancoDocumentos { get; set; }
        public virtual ICollection<DocumentoProposta> DocumentosProposta { get; set; }
    }
}
