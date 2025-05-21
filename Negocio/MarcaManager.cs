using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class MarcaManager
    {
        public List<Marca> listar()
        {
            List<Marca> listaMarcas = new List<Marca>();

            AccesoADatos conexion = new AccesoADatos();

            try
            {
                conexion.setearConsulta("Select Id, Descripcion from Marcas");
                conexion.ejecutarQuery();
                while (conexion.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)conexion.Lector["Id"];
                    try
                    {
                        aux.Descripcion = (string)conexion.Lector["Descripcion"];
                    }
                    catch (Exception)
                    {
                        aux.Descripcion = "Error al leer la marca";
                    }

                    listaMarcas.Add(aux);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.cerrarConexion();
            }

            return listaMarcas;
        }
    }
}
