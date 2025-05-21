using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI
{
    public partial class Exito : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RegistroExitoso"] == null || (bool)Session["RegistroExitoso"] == false)
            {
                Response.Redirect("Inicio.aspx", true);
            }
        }

        protected void btnVolverAlInicio_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Inicio.aspx", true);
        }
    }
}