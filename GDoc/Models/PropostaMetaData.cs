using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GDoc.Models
{

    [MetadataType(typeof(PropostaMetaData))]
    public partial class Proposta
    {
    }

    public class PropostaMetaData
    {
        public int Id { get; set; }
        [DisplayName("Agente")]        
        public int IdUsuario { get; set; }
        [DisplayName("Data de Entrada")]        
        public Nullable<System.DateTime> DataEntrada { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        [DisplayName("Data de Nascimento")]        
        public Nullable<System.DateTime> DataNascimento { get; set; }
        [DisplayName("Matrícula")]        
        public string Matricula { get; set; }
        [DisplayName("Senha")]        
        public string SenhaConvenio { get; set; }
        [DisplayName("Banco")]        
        public Nullable<int> IdBancoConta { get; set; }
        [DisplayName("Agência")]        
        public string Agencia { get; set; }
        [DisplayName("Conta Corrente")]        
        public string ContaCorrente { get; set; }
        [DisplayName("Tipo de Conta")]        
        public Nullable<int> IdTipoConta { get; set; }
        public Nullable<int> IdUsuarioDigitacao { get; set; }
        public Nullable<System.DateTime> DataDigitacao { get; set; }
        [DisplayName("Convênio")]        
        public int IdConvenio { get; set; }
        [DisplayName("Banco")]        
        public int IdBanco { get; set; }
        [DisplayName("Valor Parcela")]        
        public Nullable<decimal> ValorParcela { get; set; }
        [DisplayName("Margem Livre")]        
        public Nullable<decimal> MargemLivre { get; set; }
        [DisplayName("Margem Utilizada")]        
        public Nullable<decimal> MargemUtilizada { get; set; }
        public Nullable<double> Coeficiente { get; set; }
        [DisplayName("Prazo")]        
        public Nullable<int> QtdeParcelas { get; set; }
        [DisplayName("Valor Contrato")]        
        public Nullable<decimal> ValorContrato { get; set; }
        public string Tabela { get; set; }
        [DisplayName("Data Fator")]        
        public Nullable<System.DateTime> DataCoeficiente { get; set; }
        [DisplayName("Observação")]        
        public string Observacao { get; set; }
        [DisplayName("Endereço")]        
        public string Endereco { get; set; }
        [DisplayName("Núm.")]        
        public string NumeroEndereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }

    }
}