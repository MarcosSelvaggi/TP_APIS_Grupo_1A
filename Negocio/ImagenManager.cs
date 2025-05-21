using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ImagenManager
    {
        public List<Imagen> listarImagenes()
        {
            List<Imagen> listaImagenes = new List<Imagen>();

            AccesoADatos conexion = new AccesoADatos();

            try
            {
                conexion.setearConsulta("Select Id, IdArticulo, ImagenUrl from IMAGENES");
                conexion.ejecutarQuery();
                while (conexion.Lector.Read())
                {
                    Imagen aux = new Imagen();
                    aux.Id = (int)conexion.Lector["Id"];
                    aux.IdArticulo = (int)conexion.Lector["IdArticulo"];
                    aux.ImagenUrl = (string)conexion.Lector["ImagenUrl"];
                    listaImagenes.Add(aux);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conexion.cerrarConexion();
            }

            return listaImagenes;
        }
    }
}
