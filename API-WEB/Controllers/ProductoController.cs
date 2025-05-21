using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_WEB.Models;
using Dominio;
using Negocio;

namespace API_WEB.Controllers
{
    public class ProductoController : ApiController
    {
        // GET: api/Producto
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Producto/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Producto
        public void Post([FromBody] ArticuloDto articuloNuevo)
        {
            ArticuloManager articuloManager = new ArticuloManager();
            Articulo articulo = new Articulo();
            articulo.Nombre = articuloNuevo.Nombre;
            articulo.Descripcion = articuloNuevo.Descripcion;
            articulo.Codigo = articuloNuevo.Codigo;
            articulo.Precio = articuloNuevo.Precio;
            articulo.Categoria.Id = articuloNuevo.IdCategoria;
            articulo.Marca.Id = articuloNuevo.IdMarca;

            articuloManager.insertarArticulo(articulo);
        }

        // PUT: api/Producto/5
        public void Put(int id, [FromBody] ArticuloDto articuloNuevo)
        {
            ArticuloManager ArticuloManager = new ArticuloManager();
            List<Articulo> listaArticulos = ArticuloManager.listarArticulos();

            foreach (var item in listaArticulos)
            {
                if (item.Id == id)
                {
                    if(articuloNuevo.Imagenes.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        List<Imagen> listaImagen = new List<Imagen>();
                        
                        foreach (var imagenes in articuloNuevo.Imagenes)
                        {
                            listaImagen.Add(imagenes);
                        }

                        ImagenManager imagenManager = new ImagenManager();
                        imagenManager.agregarImagenes(id, listaImagen);
                    }
                }
            }

            
        }

        // DELETE: api/Producto/5
        public void Delete(int id)
        {
        }
    }
}
