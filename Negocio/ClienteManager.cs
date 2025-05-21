using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ClienteManager
    {
        public List<Cliente> ListarClientes()
        {
            List<Cliente> listaClientes = new List<Cliente>();
            AccesoADatos conexion = new AccesoADatos();

            try
            {
                string query = "Select Id, Documento, Nombre, Apellido, Email, Direccion, Ciudad, CP from Clientes";
                conexion.setearConsulta(query);
                conexion.ejecutarQuery();

                while (conexion.Lector.Read())
                {
                    Cliente aux = new Cliente();
                    aux.Id = (int)conexion.Lector["ID"];
                    aux.Documento = (string)conexion.Lector["Documento"];
                    aux.Nombre = (string)conexion.Lector["Nombre"];
                    aux.Apellido = (string)conexion.Lector["Apellido"];
                    aux.Email = (string)conexion.Lector["Email"];
                    aux.Direccion = (string)conexion.Lector["Direccion"];
                    aux.Ciudad = (string)conexion.Lector["Ciudad"];
                    aux.CP = (int)conexion.Lector["CP"];

                    listaClientes.Add(aux);
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
            return listaClientes;
        }

        public void modificarCliente(Cliente clienteModificado)
        {
            AccesoADatos conexion = new AccesoADatos();
            try
            {
                string query = "UPDATE Clientes SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, " +
                              "Direccion = @Direccion, Ciudad = @Ciudad, CP = @CP WHERE Documento = @Documento";
                conexion.setearConsulta(query);
                conexion.agregarParametros("@Documento", clienteModificado.Documento);
                conexion.agregarParametros("@Nombre", clienteModificado.Nombre);
                conexion.agregarParametros("@Apellido", clienteModificado.Apellido);
                conexion.agregarParametros("@Email", clienteModificado.Email);
                conexion.agregarParametros("@Direccion", clienteModificado.Direccion);
                conexion.agregarParametros("@Ciudad", clienteModificado.Ciudad);
                conexion.agregarParametros("@CP", clienteModificado.CP);

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
        public void agregarCliente(Cliente clienteNuevo)
        {
            AccesoADatos conexion = new AccesoADatos();
            try
            {
                string query = "INSERT INTO Clientes (Documento, Nombre, Apellido, Email, Direccion, Ciudad, CP) " +
               "VALUES (@Documento,@Nombre, @Apellido, @Email, @Direccion, @Ciudad, @CP)";
                conexion.setearConsulta(query);
                conexion.agregarParametros("@Documento", clienteNuevo.Documento);
                conexion.agregarParametros("@Nombre", clienteNuevo.Nombre);
                conexion.agregarParametros("@Apellido", clienteNuevo.Apellido);
                conexion.agregarParametros("@Email", clienteNuevo.Email);
                conexion.agregarParametros("@Direccion", clienteNuevo.Direccion);
                conexion.agregarParametros("@Ciudad", clienteNuevo.Ciudad);
                conexion.agregarParametros("@CP", clienteNuevo.CP);

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
