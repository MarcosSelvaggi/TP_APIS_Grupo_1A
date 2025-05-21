using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Imagen
    {
        public int Id { get; set; }
        public int IdArticulo { get; set; }
        public string ImagenUrl { get; set; }

        public Imagen() { }
        public Imagen(int idArticulo, string imagenUrl)
        {
            IdArticulo = idArticulo;
            ImagenUrl = imagenUrl;
        }

        public override string ToString()
        {
            return ImagenUrl;
        }
    }
}
