using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;
using Services;

namespace UI
{
    public partial class Usuario : System.Web.UI.Page
    {
        //private bool UsuarioNuevo { get; set; } = true;
        //private bool datosModificados { get; set; } = false;
        private Cliente clienteAux { get; set; }

        private ClienteManager clienteManager = new ClienteManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            clienteAux = new Cliente();
            if (!IsPostBack)
            {
                //Por si quiere ingresar sin haber puesto el cupón ni elegido el producto
                //if (Session["documento"] == null || Session["codigoVoucher"] == null)
                if (Session["codigoVoucher"] == null || Request.QueryString["id"] == null)
                {
                    Response.Redirect("Inicio.aspx", true);
                }
                Session["UsuarioModificado"] = false;
                /*if (UsuarioNuevo)
                {
                    activarTxTs();
                }
            }
            else
            {
                datosModificados = true;
            }*/

            }
        }
        
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Queda feo, lo sé, pero es más legible
            int valorAux = 0;
            if (datoVacio(txtNombre.Text) || datoVacio(txtApellido.Text) ||
                datoVacio(txtEmail.Text) || datoVacio(txtDireccion.Text) ||
                datoVacio(txtCiudad.Text) || !Int32.TryParse(txtCP.Text, out valorAux))
            {
                //lblAceptar.InnerText = "Por favor ingrese valores correctos en todos los campos";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "datosErroneos",
                "var modal = new bootstrap.Modal(document.getElementById('datosErroneosModal')); modal.show();", true);
            }
            else
            {
                List<Cliente> listaClientes = clienteManager.ListarClientes();

                /*foreach (var item in listaClientes)
                {
                    if (item.Documento == txtDocumento.Text)
                    {
                        buscarDocumentoModalLabel.InnerHtml = "❌ Error al ingresar el documento.";
                        pBusquedaDocumento.InnerHtml = "El documento ingresado ya se encuentra en el sistema.";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "buscarDocumentoModal",
                        "var modal = new bootstrap.Modal(document.getElementById('buscarDocumentoModal')); modal.show();", true);
                        return;
                    }
                }*/

                clienteAux.Documento = txtDocumento.Text;
                clienteAux.CP = valorAux;
                clienteAux.Nombre = txtNombre.Text;
                clienteAux.Apellido = txtApellido.Text;
                clienteAux.Direccion = txtDireccion.Text;
                clienteAux.Email = txtEmail.Text;
                clienteAux.Ciudad = txtCiudad.Text;

                // if (datosModificados)
                if ((bool)Session["UsuarioEncontrado"] == false)
                {
                    foreach (var item in listaClientes)
                    {
                        if (item.Documento == txtDocumento.Text)
                        {
                            buscarDocumentoModalLabel.InnerHtml = "❌ Error al ingresar el documento.";
                            pBusquedaDocumento.InnerHtml = "El documento ingresado ya se encuentra en el sistema.";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "buscarDocumentoModal",
                            "var modal = new bootstrap.Modal(document.getElementById('buscarDocumentoModal')); modal.show();", true);
                            return;
                        }
                    }
                    clienteManager.agregarCliente(clienteAux);
                }
                if ((bool)Session["UsuarioModificado"] == true)
                {
                    clienteManager.modificarCliente(clienteAux);
                }

                if (Session["codigoVoucher"] != null && Request.QueryString["id"] != null && Session["idCliente"] != null)
                {

                    int idArticulo = Convert.ToInt32(Request.QueryString["id"]);
                    int idCliente = Convert.ToInt32(Session["idCliente"]);
                    string codigoVoucher = Session["codigoVoucher"].ToString();

                    VoucherManager voucherManager = new VoucherManager();
                    voucherManager.AsignarVoucherACliente(codigoVoucher, idCliente, idArticulo);
                    Session.Clear();
                }

                EmailService emailService = new EmailService();
                emailService.correo(txtEmail.Text);
                try
                {
                    emailService.enviarEmail();
                }
                catch (Exception ex)
                {

                    Session.Add("error", ex);
                }

                Session["RegistroExitoso"] = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso",
                "var modal = new bootstrap.Modal(document.getElementById('registroExitosoModal')); modal.show();" +
                "setTimeout(function() { window.location.href = 'Exito.aspx'; }, 5000);", true);
            }
        }

        private bool datoVacio(string revisarValor)
        {
            return String.IsNullOrEmpty(revisarValor);
            /*if (revisarValor == null || revisarValor == "")
            {
                return true;
            }
            else
            {
                return false;
            }*/
        }
        protected void btnModificarDatos_Click(object sender, EventArgs e)
        {
            activarTxTs();
            Session["UsuarioModificado"] = true;
        }

        protected void activarTxTs()
        {
            txtNombre.ReadOnly = false;
            txtApellido.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtDireccion.ReadOnly = false;
            txtCiudad.ReadOnly = false;
            txtCP.ReadOnly = false;
            btnModificarDatos.Visible = false;
        }

        protected void desactivarTxTs()
        {
            txtNombre.ReadOnly = true;
            txtApellido.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtDireccion.ReadOnly = true;
            txtCiudad.ReadOnly = true;
            txtCP.ReadOnly = true;
            btnModificarDatos.Visible = false;
        }

        protected void limpiarTxTs()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtDireccion.Text = "";
            txtCiudad.Text = "";
            txtCP.Text = "";
        }

        protected void btnBuscarDocumento_Click(object sender, EventArgs e)
        {
            //if (txtDocumento.Text == null || txtDocumento.Text.Length == 0)
            if (string.IsNullOrEmpty(txtDocumento.Text))
            {
                desactivarTxTs();
                limpiarTxTs();
                return;
            }

            Session["Documento"] = txtDocumento.Text;
            List<Cliente> listaClientes = clienteManager.ListarClientes();

            bool UsuarioNuevo = true;
            limpiarTxTs();
            desactivarTxTs();

            foreach (var item in listaClientes)
            {
                if (item.Documento == Session["documento"].ToString())
                {
                    btnLimpiarDocumento.Visible = true;
                    btnModificarDatos.Visible = true;
                    btnBuscarDocumento.Visible = false;
                    txtDocumento.ReadOnly = true;

                    clienteAux = item;
                    txtNombre.Text = item.Nombre;
                    txtApellido.Text = item.Apellido;
                    txtEmail.Text = item.Email;
                    txtDireccion.Text = item.Direccion;
                    txtCiudad.Text = item.Ciudad;
                    txtCP.Text = item.CP.ToString();
                    UsuarioNuevo = false;

                    //Lo guardo para mandarlo por parámetro al método de asignación de voucher
                    Session["idCliente"] = item.Id;
                    Session["UsuarioEncontrado"] = true;

                    buscarDocumentoModalLabel.InnerText = "✔ Documento encontrado";
                    pBusquedaDocumento.InnerText = "Se han encontrado sus datos, confirme que son correctos.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "buscarDocumento",
                    "var modal = new bootstrap.Modal(document.getElementById('buscarDocumentoModal')); modal.show();", true);
                }
            }

            if (UsuarioNuevo)
            {
                buscarDocumentoModalLabel.InnerText = "❌ Documento no encontrado";
                pBusquedaDocumento.InnerText = "No se ha encontrado un cliente con ese documento, revise el número o complete los datos.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "buscarDocumento",
                "var modal = new bootstrap.Modal(document.getElementById('buscarDocumentoModal')); modal.show();", true);

                activarTxTs();
                Session["UsuarioEncontrado"] = false;
                Session["idCliente"] = listaClientes.Last().Id + 1;
            }

        }

        protected void btnLimpiarDocumento_Click(object sender, EventArgs e)
        {
            btnBuscarDocumento.Visible = true;
            btnLimpiarDocumento.Visible = false;
            txtDocumento.ReadOnly = false;
            txtDocumento.Text = "";
            desactivarTxTs();
            btnModificarDatos.Visible = false;
            limpiarTxTs();
        }
    }
}