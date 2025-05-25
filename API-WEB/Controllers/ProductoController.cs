using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_WEB.Models;
using Dominio;
using Microsoft.Ajax.Utilities;
using Negocio;

namespace API_WEB.Controllers
{
    public class ProductoController : ApiController
    {
        // GET: api/Producto
        public HttpResponseMessage Get()
        {
            try
            {
                ArticuloManager articuloManager = new ArticuloManager();
                ImagenManager imagenManager = new ImagenManager();

                var listaArticulos = articuloManager.listarArticulos();
                var listaImagenes = imagenManager.listarImagenes();

                foreach (var art in listaArticulos)
                {
                    AsignarImagenes(art, listaImagenes);
                }
                return Request.CreateResponse(HttpStatusCode.OK, listaArticulos);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
            }

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

            AsignarImagenes(articulo, listaImagenes);

            return Request.CreateResponse(HttpStatusCode.OK, articulo);
        }

        // POST: api/Producto
        public HttpResponseMessage Post([FromBody] ArticuloDto articuloNuevo)
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
                /*
                if (!MarcaExiste(articuloNuevo.IdMarca))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La marca no existe.");

                if (!CategoriaExiste(articuloNuevo.IdCategoria))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La categoría no existe.");

                if (!PrecioValido(articuloNuevo.Precio))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El precio es un campo obligatorio y debe ser mayor a cero.");
                */
                var request = ValidarArticulo(articuloNuevo);
                if ((int)request.StatusCode == 200)
                {
                    try
                    {
                        articuloManager.insertarArticulo(articulo);
                        return request;
                    }
                    catch (Exception)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
                    }
                }
                else
                {
                    return request;
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Datos incompletos o no válidos.");
            }
        }

        public HttpResponseMessage Post(int Id, [FromBody] List<ImagenDto> listaImagenes)
        {
            ArticuloManager articuloManager = new ArticuloManager();
            List<Articulo> listaArticulos = articuloManager.listarArticulos();

            if (listaImagenes == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se ha ingresado ninguna imagen.");
            }
            else if (listaImagenes.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se ha ingresado ninguna imagen.");
            }
            else if (listaArticulos.Exists(X => X.Id == Id))
            {
                try
                {
                    List<Imagen> listaImagenesAagregar = new List<Imagen>();
                    ImagenManager imagenManager = new ImagenManager();

                    foreach (var item in listaImagenes)
                    {
                        if (string.IsNullOrWhiteSpace(item.ImagenUrl))
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Hay valores no validos en la solicitud.");
                        listaImagenesAagregar.Add(new Imagen { ImagenUrl = item.ImagenUrl });
                    }

                    imagenManager.agregarImagenes(Id, listaImagenesAagregar);
                    return Request.CreateResponse(HttpStatusCode.OK, "Imagen(es) agregada(s) correctamente.");
                }
                catch (Exception)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "El ID ingresado no corresponde con ningún artículo.");
            }
        <}

        // PUT: api/Producto/5
        public HttpResponseMessage Put(int id, [FromBody] ArticuloDto articuloModificado)
        {
            try
            {
                ArticuloManager articuloManager = new ArticuloManager();
                ImagenManager imagenManager = new ImagenManager();

                if (!ProductoExiste(id))
                    return Request.CreateResponse(HttpStatusCode.NotFound, "El articulo no existe.");

                if (articuloModificado == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No se recibió ningún artículo.");

                if (ArticuloCompleto(articuloModificado))
                {
                    /*
                    if (!MarcaExiste(articuloModificado.IdMarca))
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "La marca no existe.");
                    if (!CategoriaExiste(articuloModificado.IdCategoria))
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "La categoría no existe.");
                    if (!PrecioValido(articuloModificado.Precio))
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "El precio es un campo obligatorio y debe ser mayor a cero.");
                    */

                    var request = ValidarArticulo(articuloModificado);
                    if ((int)request.StatusCode == 200)
                    {
                        var articulo = ConstruirArticuloDesdeDto(articuloModificado, id);
                        articuloManager.modificarArticulo(articulo);
                        return Request.CreateResponse(HttpStatusCode.OK, "Artículo modificado e Imágenes agregadas correctamente.");
                    }
                    else
                    {
                        return request;
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Datos incompletos o no válidos.");
                }
                /*
                else if (ArticuloCompleto(articuloModificado) && articuloModificado.Imagenes == null)
                {

                    if (!MarcaExiste(articuloModificado.IdMarca))
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "La marca no existe.");
                    if (!CategoriaExiste(articuloModificado.IdCategoria))
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "La categoría no existe.");
                    if (!PrecioValido(articuloModificado.Precio))
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "El precio es un campo obligatorio y debe ser mayor a cero.");

                    var articulo = ConstruirArticuloDesdeDto(articuloModificado, id);
                    articuloManager.modificarArticulo(articulo);
                    return Request.CreateResponse(HttpStatusCode.OK, "Artículo modificado correctamente.");

                var request = ValidarArticulo(articuloModificado);
                    if ((int)request.StatusCode == 200)
                    {
                        var articulo = ConstruirArticuloDesdeDto(articuloModificado, id);
                        articuloManager.modificarArticulo(articulo);
                        return Request.CreateResponse(HttpStatusCode.OK, "Artículo modificado correctamente.");
                    }
                    else
                    {
                        return request;
                    }
                }*/
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
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
            return !string.IsNullOrWhiteSpace(articulo.Codigo) &&
            !string.IsNullOrWhiteSpace(articulo.Nombre) &&
            !string.IsNullOrWhiteSpace(articulo.Descripcion) &&
            articulo.IdMarca > 0 &&
            articulo.IdCategoria > 0;
        }
        private Articulo ConstruirArticuloDesdeDto(ArticuloDto dto, int id)
        {
            return new Articulo
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Codigo = dto.Codigo,
                Precio = dto.Precio,
                Categoria = { Id = dto.IdCategoria },
                Marca = { Id = dto.IdMarca },
                Id = id
            };
        }
        private bool MarcaExiste(int id)
        {
            return new MarcaManager().listar().Any(x => x.Id == id);
        }
        private bool ProductoExiste(int id)
        {
            return new ArticuloManager().listarArticulos().Any(x => x.Id == id);
        }
        private bool CategoriaExiste(int id)
        {
            return new CategoriaManager().listar().Any(x => x.Id == id);
        }
        private bool PrecioValido(decimal precio)
        {
            return precio > 0;
        }

        private HttpResponseMessage ValidarArticulo(ArticuloDto articulo)
        {
            if (!MarcaExiste(articulo.IdMarca))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "La marca no existe.");
            if (!CategoriaExiste(articulo.IdCategoria))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "La categoría no existe.");
            if (!PrecioValido(articulo.Precio))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "El precio es un campo obligatorio y debe ser mayor a cero.");
            return Request.CreateResponse(HttpStatusCode.OK, "Artículo agregado correctamente.");
        }

        private void AsignarImagenes(Articulo articulo, List<Imagen> listaImagenes)
        {
            foreach (var img in listaImagenes)
            {
                if (articulo.Id == img.IdArticulo)
                {
                    articulo.Imagenes.Add(new Imagen
                    {
                        Id = img.Id,
                        IdArticulo = img.IdArticulo,
                        ImagenUrl = img.ImagenUrl
                    });
                }
            }
        }

    }
}
