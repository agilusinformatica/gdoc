using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GDoc.Models
{
    [MetadataType(typeof(UsuarioMetaData))]
    public partial class Usuario
    {
    }

    public class UsuarioMetaData
    {
        public int Id { get; set; }
        [Required(ErrorMessage="O login deve ser preenchido")]
        public string Login { get; set; }
        [Required(ErrorMessage="O nome deve ser preenchido")]
        public string Nome { get; set; }
        [DisplayName("Perfil")]        
        public int IdPerfil { get; set; }
        [DisplayName("Filial")]
        public int IdFilial { get; set; }
        public string Email { get; set; }
        public string Empresa { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        [DisplayName("Endereço")]
        public string Endereco { get; set; }
        [DisplayName("Núm.")]
        public string NumeroEndereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }
        [DisplayName("Trocar senha no próximo login?")]
        public bool TrocarSenha { get; set; }
        public string Complemento { get; set; }
    }
}