using DSC.DataExecutor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC.DataExecutor.Extensions
{
    class DBCommand
    {
        private SqlConnection MyConnection;
        private SqlConnection GetConnection
        {
            get
            {
                if ((MyConnection == null) || MyConnection.State == ConnectionState.Open)
                {
                    MyConnection = new SqlConnection(new AppConfiguration().SqlDataConnection);
                }
                return MyConnection;
            }
        }
        public SqlCommand command = new SqlCommand();
        public DBCommand()
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = GetConnection;
        }
    }
}
