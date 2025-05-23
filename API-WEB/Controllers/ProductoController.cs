using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls.WebParts;
using API_WEB.Models;
using Dominio;
using Negocio;

namespace API_WEB.Controllers
{
    public class ProductoController : ApiController
    {
        // GET: api/Producto
        public IEnumerable<Articulo> Get()
        {
            ArticuloManager articuloManager = new ArticuloManager();
            ImagenManager imagenManager = new ImagenManager();

            var listaArticulos = articuloManager.listarArticulos();
            var listaImagenes = imagenManager.listarImagenes();

            foreach (var art in listaArticulos)
            {
                foreach (var img in listaImagenes)
                {
                    if (art.Id == img.IdArticulo)
                    {
                        art.Imagenes.Add(new Imagen
                        {
                            Id = img.Id,
                            IdArticulo = img.IdArticulo,
                            ImagenUrl = img.ImagenUrl
                        });
                    }
                }

            }

            return listaArticulos;
        }

        // GET: api/Producto/5
        public HttpResponseMessage Get(int id)
        {
            ArticuloManager articuloManager = new ArticuloManager();
            ImagenManager imagenManager = new ImagenManager();

            var articulo = articuloManager.obtenerArticulo(id);
            var listaImagenes = imagenManager.listarImagenes();

            if (articulo == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "El articulo no existe.");

            foreach (var img in listaImagenes)
            {
                if (id == img.IdArticulo)
                {
                    articulo.Imagenes.Add(new Imagen
                    {
                        Id = img.Id,
                        IdArticulo = img.IdArticulo,
                        ImagenUrl = img.ImagenUrl
                    });
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, articulo);
        }

        // POST: api/Producto
        public void Post([FromBody] ArticuloDto articuloNuevo)
        {
            if (ArticuloCompleto(articuloNuevo))
            {
                ArticuloManager articuloManager = new ArticuloManager();
                Articulo articulo = new Articulo
                {
                    Nombre = articuloNuevo.Nombre,
                    Descripcion = articuloNuevo.Descripcion,
                    Codigo = articuloNuevo.Codigo,
                    Precio = articuloNuevo.Precio,
                    Categoria = { Id = articuloNuevo.IdCategoria },
                    Marca = { Id = articuloNuevo.IdMarca },
                };                
                articuloManager.insertarArticulo(articulo);
            }
            else
            {
                return;
            }
        }

        // PUT: api/Producto/5
        public void Put(int id, [FromBody] ArticuloDto articuloModificado)
        {
            ArticuloManager articuloManager = new ArticuloManager();
            List<Articulo> listaArticulos = articuloManager.listarArticulos();
            List<Imagen> listaImagen = new List<Imagen>();
            ImagenManager imagenManager = new ImagenManager();

            if (ArticuloCompleto(articuloModificado) && articuloModificado.Imagenes.Count != 0)
            {
                Articulo articulo = new Articulo
                {
                    Nombre = articuloModificado.Nombre,
                    Descripcion = articuloModificado.Descripcion,
                    Codigo = articuloModificado.Codigo,
                    Precio = articuloModificado.Precio,
                    Categoria = { Id = articuloModificado.IdCategoria },
                    Marca = { Id = articuloModificado.IdMarca },
                    Id = id
                };
                articuloManager.modificarArticulo(articulo);

                foreach (var item in listaArticulos)
                {
                    if (item.Id == id)
                    {
                        foreach (var imagenes in articuloModificado.Imagenes)
                        {
                            listaImagen.Add(imagenes);
                        }
                        imagenManager.agregarImagenes(id, listaImagen);
                    }
                }
            }
            else if (!ArticuloCompleto(articuloModificado) && articuloModificado.Imagenes.Count != 0)
            {
                foreach (var item in listaArticulos)
                {
                    if (item.Id == id)
                    {
                        foreach (var imagenes in articuloModificado.Imagenes)
                        {
                            listaImagen.Add(imagenes);
                        }
                        imagenManager.agregarImagenes(id, listaImagen);
                    }
                }
            }
            else if (ArticuloCompleto(articuloModificado) && articuloModificado.Imagenes.Count == 0)
            {
                Articulo articulo = new Articulo
                {
                    Nombre = articuloModificado.Nombre,
                    Descripcion = articuloModificado.Descripcion,
                    Codigo = articuloModificado.Codigo,
                    Precio = articuloModificado.Precio,
                    Categoria = { Id = articuloModificado.IdCategoria },
                    Marca = { Id = articuloModificado.IdMarca },
                    Id = id
                };
                articuloManager.modificarArticulo(articulo);
            }
        }

        // DELETE: api/Producto/5
        public HttpResponseMessage Delete(int id)
        {
            ArticuloManager articuloManager = new ArticuloManager();

            var articulo = articuloManager.obtenerArticulo(id);

            if (articulo == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "El articulo no existe.");

            articuloManager.eliminarArticulo(id);

            return Request.CreateResponse(HttpStatusCode.OK, "Eliminacion Exitosa.");
        }

        private bool ArticuloCompleto(ArticuloDto articulo)
        {
            return true;
        }
    }
}
