using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Negocio
{
    public class AccesoADatos
    {
        private SqlConnection conexion;
        private SqlDataReader lector;
        public SqlCommand comando;

        public SqlDataReader Lector
        {
            get
            {
                return lector;
            }
        }

        public AccesoADatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=PROMOS_DB; integrated security=true;");
            comando = new SqlCommand();
        }

        public void ejecutarQuery()
        {
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ejecutarNonQuery()
        {
            int filasAfectadas = 0;
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                filasAfectadas = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
            return filasAfectadas;
        }

        public void setearConsulta(string query)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = query;
        }

        public void agregarParametros(string key, object value)
        {
            comando.Parameters.AddWithValue(key, value);
        }

        public void cerrarConexion()
        {
            if (lector != null)
            {
                lector.Close();
            }
            conexion.Close();
        }

        /*public int ObtenerUltimoIdInsertado()
        {

            try
            {
                setearConsulta("Select Scope_Identity()");

                object resultado = EjecutarScalar();


                if (resultado == DBNull.Value)
                {
                    throw new Exception("No se pudo obtener el ID.");
                }


                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        public object EjecutarScalar()
        {
            object resultado = null;
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                resultado = comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
            return resultado;
        }

        public void limpiarParametros()
        {
            if (comando.Parameters != null)
            {
                comando.Parameters.Clear();
            }
        }
    }
}
