using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class VoucherManager
    {
        public List<Voucher> listarVouchers()
        {
            List<Voucher> listaVouchers = new List<Voucher>();
            AccesoADatos conexion = new AccesoADatos();

            try
            {
                string query = "Select CodigoVoucher, IdCliente, FechaCanje, IdArticulo from Vouchers";
                conexion.setearConsulta(query);
                conexion.ejecutarQuery();

                while (conexion.Lector.Read())
                {
                    Voucher aux = new Voucher();
                    aux.CodigoVoucher = (string)conexion.Lector["CodigoVoucher"];
                    aux.IdCliente = (int)conexion.Lector["IdCliente"];
                    aux.FechaCanje = (DateTime)conexion.Lector["FechaCanje"];
                    aux.IdArticulo = (int)conexion.Lector["IdArticulo"];

                    listaVouchers.Add(aux);
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
            return listaVouchers;
        }


        public bool ValidarCodigo(string codigo)
        {
            AccesoADatos datos = new AccesoADatos();

            try
            {
                string query = "Select Count(*) From Vouchers Where CodigoVoucher = @Codigo AND FechaCanje IS NULL";
                datos.setearConsulta(query);
                datos.limpiarParametros();
                datos.agregarParametros("@Codigo", codigo);

                object resultado = datos.EjecutarScalar();

                int cantidad = Convert.ToInt32(resultado);
                return cantidad > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void AsignarVoucherACliente(string codigoVoucher, int idCliente, int idArticulo)
        {
            AccesoADatos conexion = new AccesoADatos();

            try
            {
                conexion.setearConsulta("Update Vouchers Set IdCliente = @IdCliente, FechaCanje = GETDATE(), IdArticulo = @IdArticulo Where CodigoVoucher = @CodigoVoucher");
                conexion.agregarParametros("@IdCliente", idCliente);
                conexion.agregarParametros("@CodigoVoucher", codigoVoucher);
                conexion.agregarParametros("@IdArticulo", idArticulo);
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