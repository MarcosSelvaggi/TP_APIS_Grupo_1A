using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;

namespace UI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Session.Clear();
        }
        protected void btnValidar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigoVoucher.Text.Trim();

            if (string.IsNullOrEmpty(codigo))
            {
                lblResultado.Text = "Por favor, ingrese un código.";
                return;
            }

            if (!Regex.IsMatch(codigo, @"^[a-zA-Z0-9]+$"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorModal",
                "var modal = new bootstrap.Modal(document.getElementById('errorModal')); modal.show();", true);
                return;
            }

            VoucherManager manager = new VoucherManager();
            bool esValido = manager.ValidarCodigo(codigo);

            if (esValido)
            {
                //lblResultado.Text = "El código es válido.";
                Session["codigoVoucher"] = codigo;
                Response.Redirect("Premios.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorModal",
                "var modal = new bootstrap.Modal(document.getElementById('errorModal')); modal.show();", true);
            }
        }
    }
}