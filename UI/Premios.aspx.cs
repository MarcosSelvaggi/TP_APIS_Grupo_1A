using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace UI
{
    public partial class Premios : System.Web.UI.Page
    {
        ArticuloManager articuloManager = new ArticuloManager();
        ImagenManager imagenManager = new ImagenManager();

        List<Articulo> listaArticulosTotales = new List<Articulo>();
        List<Imagen> listaImagenesTotales = new List<Imagen>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["codigoVoucher"] == null)
            {
                Response.Redirect("Inicio.aspx", true);
            }

            listaArticulosTotales = articuloManager.listarArticulos();
            listaImagenesTotales = imagenManager.listarImagenes();

            foreach (Articulo articulo in listaArticulosTotales)
            {
                string carrusel = GenerarCarrusel(articulo.Id, articulo.Nombre, articulo.Descripcion, ListarImagenesPorId(articulo));

                // Usando LiteralControl se puede inyectar html puro.
                LiteralControl carruselControl = new LiteralControl();

                carruselControl.Text = carrusel;

                carruselPremios.Controls.Add(carruselControl);
            }
        }

        private List<Imagen> ListarImagenesPorId(Articulo articulo)
        {
            List<Imagen> listaImagenesPorArticulo = new List<Imagen>();

            foreach (Imagen img in listaImagenesTotales)
            {
                if (articulo.Id == img.IdArticulo)
                {
                    listaImagenesPorArticulo.Add(img);
                }
            }
            return listaImagenesPorArticulo;
        }

        private string GenerarCarrusel(int id, string nombre, string descripcion, List<Imagen> imagenes)
        {
            // Armarmos el html de manera progresiva agregando los datos para que sea dinamico
            // logrando que no importe la cantidad de articulos e imagenes agregadas.
            StringBuilder cadenaHtml = new StringBuilder();

            cadenaHtml.Append($@"
            <div class='col-md-4 text-center'>
                <div id='Premio{id}' class='carousel slide' data-bs-ride='carousel' style='margin: 20px; margin-bottom: 5px; border: 1px solid #9b9b9b; border-radius: 15px;'>
                    <div class='carousel-inner'>");

            if (imagenes.Count == 0)
            {
                cadenaHtml.Append($@"
                            <div class='carousel-item active'>
                                <img src='https://th.bing.com/th/id/OIP.mSzrXbopNaal5jPsMxNHHwHaHa?cb=iwc1&rs=1&pid=ImgDetMain';"" class='d-block w-100' style='height: 300px; object-fit: contain;' alt='ImgId0' />
                            </div>
                       </div>");
            }
            else if (imagenes.Count == 1)
            {
                cadenaHtml.Append($@"
                            <div class='carousel-item active'>
                                <img src='{imagenes[0].ToString()}' onerror=""this.onerror=null; this.src='https://th.bing.com/th/id/OIP.mSzrXbopNaal5jPsMxNHHwHaHa?cb=iwc1&rs=1&pid=ImgDetMain';"" class='d-block w-100' style='height: 300px; object-fit: contain;' alt='ImgId{1}' />
                            </div>
                        </div>");
            }
            else
            {
                for (int i = 0; i < imagenes.Count; i++)
                {
                    // Para saber cual es la primera imagen que debe aparecer en el carrusel.
                    string activeclass = (i == 0) ? "active" : "";

                    cadenaHtml.Append($@"
                            <div class='carousel-item {activeclass}'>
                                <img src='{imagenes[i].ToString()}' onerror=""this.onerror=null; this.src='https://th.bing.com/th/id/OIP.mSzrXbopNaal5jPsMxNHHwHaHa?cb=iwc1&rs=1&pid=ImgDetMain';"" class='d-block w-100' style='height: 300px; object-fit: contain;' alt='ImgId{i}' />
                            </div>");
                }
                cadenaHtml.Append($@"  
                    </div>
                    <button class='carousel-control-prev' type='button' data-bs-target='#Premio{id}' data-bs-slide='prev'>
                        <span class='carousel-control-prev-icon' style='background-color: #9b9b9b; border-radius: 30px;'></span>
                    </button>
                    <button class='carousel-control-next' type='button' data-bs-target='#Premio{id}' data-bs-slide='next'>
                        <span class='carousel-control-next-icon' style='background-color: #9b9b9b; border-radius: 30px;'></span>
                    </button>
                </div>");
            }

            cadenaHtml.Append($@"
                <div class='text-center' style='margin: 5px; padding-bottom: 5px; border: 1px solid #9b9b9b; border-radius: 15px;'>
                    <h4 style='margin: 0;'>{nombre}</h4>
                    <p'>{descripcion}</p>
                    <button type='button' class='btn btn-primary' onclick=""abrirModal({id}, '{nombre}');"">Elegir Premio</button>
                </div>
            </div>");

            return cadenaHtml.ToString();
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            string script = @"
            <script type='text/javascript'>
            function abrirModal(id, nombre) {
                var btnConfirmar = document.getElementById('btnConfirmar');
                btnConfirmar.href = 'Usuario.aspx?id=' + id;

                document.getElementById('confirmarModalLabel').innerText = 'Confirmar selección del premio: ' + nombre;

                var myModal = new bootstrap.Modal(document.getElementById('confirmarModal'));
                myModal.show();
            }
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "abrirModal", script);
        }


    }
}