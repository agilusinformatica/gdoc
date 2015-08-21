using GDoc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDoc.Models
{
    public static class Seguranca
    {
        public static void ValidaLogin(LoginViewModel op)
        {
            using (var db = new Entities())
            {
                var usu = db.Usuarios.Where(u => u.Login == op.Usuario && u.Senha == op.Senha).ToList();
                if (usu.Count == 0)
                    throw new Exception("Usuário ou Senha inválidos");

                UsuarioLogado.IdUsuario = usu[0].Id;
                UsuarioLogado.IdPerfil = usu[0].IdPerfil;
                UsuarioLogado.IdFilial = usu[0].IdFilial;
                UsuarioLogado.LimitaProposta = usu[0].Perfil.LimitaPropostaUsuarioLogado;

                if(!usu[0].Ativo)
                {
                    throw new Exception("Usuário não está ativo");
                }

                if ((op.Senha == null) || (op.Senha.Trim() == String.Empty))
                {
                    throw new Exception("Senha não pode ser em branco");
                }
            }
        }

        public static string Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA256.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }

        public static void ValidaAcesso(int idFuncao)
        {
            using (var db = new Entities())
            {
                var acesso = db.Acessos.Where(a => a.IdFuncao == idFuncao && a.IdPerfil == UsuarioLogado.IdPerfil).ToList();
                if (acesso.Count == 0)
                    throw new Exception("Acesso não autorizado");
            }
        }
    }

    public static class UsuarioLogado
    {
        public static int IdUsuario { 
            get 
            {
                int retorno;
                if (HttpContext.Current.Session["IdUsuarioLogado"] != null)
                {
                    retorno = (int)HttpContext.Current.Session["IdUsuarioLogado"]; 
                }
                else
                {
                    retorno = 0;
                }
                return retorno;
            } 
            set { HttpContext.Current.Session["IdUsuarioLogado"] = value; } }
        public static int IdPerfil { get { return (int)HttpContext.Current.Session["IdPerfilLogado"]; } set { HttpContext.Current.Session["IdPerfilLogado"] = value; } }
        public static int IdFilial { get { return (int)HttpContext.Current.Session["IdFilialLogada"]; } set { HttpContext.Current.Session["IdFilialLogada"] = value; } }
        public static bool LimitaProposta { get { return (bool)HttpContext.Current.Session["LimitaProposta"]; } set { HttpContext.Current.Session["LimitaProposta"] = value; } }
    }
}