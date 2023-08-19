using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE
{
    public class Data
    {
        //private static string ConnectionString = @"Data Source=ALIEN;Initial Catalog=GESFARM;User ID=SA;Password=jorge21176439";
        //private static string ConnectionString = @"Data Source=HP_OMEN\SQLOMEN;Initial Catalog=GESFARM2;User ID=SA;Password=jorge21176439";
        private static string ConnectionString = @"Data Source=SERVER\SQLEXPRESS;Initial Catalog=GESFARM;User ID=SA;Password=Farma12*";
        //private static string ConnectionString = @"Data Source=SERVER\SERVER2016;Initial Catalog=GESFARM;User ID=SA;Password=Server123456";
        //private static string ConnectionString = @"Data Source=SERVER\SQLSERVERFFD;Initial Catalog=GESFARM2;User ID=SA;Password=Server123456";
        //private static string ConnectionString = @"Data Source=SERVER;Initial Catalog=GESFARM3;User ID=SA;Password=Server123456";

        private SqlConnection connection = new SqlConnection(ConnectionString);

        public SqlCommand cmd;

        public DataTable DT = new DataTable();

        public Data()
        {

            if (DateTime.Now <= new DateTime(2025, 05, 01))
            {
                cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                throw new VersionNotFoundException("Licencia expirada");
            }

        }

        public DataTable CallDBList(string SP, SqlParameter[] parametros)
        {

            cmd.CommandText = SP;
            cmd.Parameters.AddRange(parametros);

            SqlDataAdapter DA = new SqlDataAdapter(cmd);

            DA.Fill(DT);

            return DT;

        }

        public void CallDBParameters(string SP, SqlParameter[] parametros)
        {
            cmd.CommandText = SP;
            cmd.Parameters.AddRange(parametros);

            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();

        }

        public dynamic GetOut(string parameter)
        {

            return cmd.Parameters[parameter].Value.ToString();

        }

        public static SqlParameter NewIN(string name, SqlDbType type, object value)
        {
            SqlParameter p = new SqlParameter(name, type, int.MaxValue);
            p.Direction = ParameterDirection.Input;
            p.Value = (value == null) ? DBNull.Value : value;
            return p;
        }

        public static SqlParameter NewOUT(string name, SqlDbType type)
        {
            SqlParameter p = new SqlParameter(name, type, int.MaxValue);
            p.Direction = ParameterDirection.Output;
            return p;
        }

    }
}
