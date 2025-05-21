using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloManager
    {
        private List<Categoria> listaCategorias;

        private List<Marca> listaMarcas;

        public List<Articulo> listarArticulos()
        {
            CategoriaManager categoriaManager = new CategoriaManager();

            MarcaManager marcaManager = new MarcaManager();

            listaCategorias = categoriaManager.listar();

            listaMarcas = marcaManager.listar();

            List<Articulo> listaArticulos = new List<Articulo>();

            AccesoADatos conexion = new AccesoADatos();

            try
            {
                string query = "Select Id, Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio from Articulos";
                conexion.setearConsulta(query);
                conexion.ejecutarQuery();

                while (conexion.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)conexion.Lector["Id"];
                    aux.Codigo = leerDatosDeLaBD(conexion.Lector, "Codigo", "Código erroneo");
                    aux.Nombre = leerDatosDeLaBD(conexion.Lector, "Nombre", "Nombre erroneo");
                    aux.Descripcion = leerDatosDeLaBD(conexion.Lector, "Descripcion", "Descripción erronea");

                    int idCategoria = (int)conexion.Lector["IdCategoria"];
                    aux.Categoria.Id = idCategoria;
                    bool encontroCategoria = false;

                    foreach (Categoria cat in listaCategorias)
                    {
                        if (cat.Id == idCategoria)
                        {
                            aux.Categoria.Descripcion = cat.Descripcion;
                            encontroCategoria = true;
                            break;
                        }
                    }

                    if (!encontroCategoria)
                        aux.Categoria.Descripcion = "Error al cargar la categoría.";


                    int idMarca = (int)conexion.Lector["IdMarca"];
                    aux.Marca.Id = idMarca;
                    bool encontroMarca = false;

                    foreach (Marca m in listaMarcas)
                    {
                        if (m.Id == idMarca)
                        {
                            aux.Marca.Descripcion = m.Descripcion;
                            encontroMarca = true;
                            break;
                        }
                    }

                    if (!encontroMarca)
                        aux.Marca.Descripcion = "Error al cargar la Marca.";

                    try
                    {
                        aux.Precio = Decimal.Parse(conexion.Lector["Precio"].ToString());
                    }
                    catch (Exception)
                    {
                        aux.Precio = 0;
                    }

                    listaArticulos.Add(aux);
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

            return listaArticulos;
        }

        private string leerDatosDeLaBD(SqlDataReader lector, string valor, string valorEnCasoDeError)
        {
            try
            {
                return (string)lector[valor];
            }
            catch (Exception)
            {
                return valorEnCasoDeError;
            }
        }

        public void insertarArticulo(Articulo articuloNuevo)
        {
            AccesoADatos conexion = new AccesoADatos();

            try
            {
                string query = "Insert into Articulos (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) values (@Codigo, @Nombre, @Descricion, @IdMarca, @IdCategoria, @Precio)";
                conexion.setearConsulta(query);
                conexion.agregarParametros("@Codigo", articuloNuevo.Codigo);
                conexion.agregarParametros("@Nombre", articuloNuevo.Nombre);
                conexion.agregarParametros("@Descricion", articuloNuevo.Descripcion);
                conexion.agregarParametros("@IdMarca", articuloNuevo.Marca.Id);
                conexion.agregarParametros("@IdCategoria", articuloNuevo.Categoria.Id);
                conexion.agregarParametros("@Precio", articuloNuevo.Precio);
                conexion.ejecutarNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conexion.cerrarConexion();
            }
        }
    }
}