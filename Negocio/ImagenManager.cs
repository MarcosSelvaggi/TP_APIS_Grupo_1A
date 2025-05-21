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

        public void agregarImagenes(int id, List<Imagen> listaImagenes)
        {
            AccesoADatos conexion = new AccesoADatos();

            try
            {
                foreach (var item in listaImagenes)
                {
                    //Todo el código comentado era porque no funcionaba inicialmente, pero ahora funciona por alguna razón...

                    //conexion.setearConsulta("Begin Transaction");
                    //conexion.ejecutarNonQuery();
                    //conexion.comando.Parameters.Clear();

                    conexion.setearConsulta("Insert into Imagenes (IdArticulo, ImagenURL) VALUES (@IdArticulo, @ImagenURL)");
                    conexion.agregarParametros("@IdArticulo", id);
                    conexion.agregarParametros("@ImagenURL", item.ImagenUrl);
                    conexion.ejecutarNonQuery();
                    conexion.comando.Parameters.Clear();
                }
                //conexion.setearConsulta("Commit Transaction");
                //conexion.ejecutarNonQuery();
            }
            catch (Exception)
            {
                //conexion.comando.Parameters.Clear();
                //conexion.setearConsulta("Rollback");
                //conexion.ejecutarNonQuery();
                throw;
            }
            finally
            {  
                conexion.cerrarConexion();
            }
        }
    }
}
