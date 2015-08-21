using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using GDoc.Models;


namespace GDoc.ViewModels
{
    public class SenhaViewModel
    {
        public int Id { get; set; }
        public string SenhaAnterior { get; set; }
        public string SenhaNova { get; set; }
        [CompareAttribute("SenhaNova", ErrorMessage = "Senha nova não confere com a senha repetida")]
        public string SenhaNovaRepeticao { get; set; }
    }
}