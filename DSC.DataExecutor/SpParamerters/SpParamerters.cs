using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC.DataExecutor
{
    public class SpParamerters
    {
        private SqlParameter parameterName;
        public SpParamerters(string param)
        {
            parameterName = new SqlParameter(param, null);
        }
        public SpParamerters(string param, int size)
        {
            parameterName = new SqlParameter(param, size);
        }
        public SpParamerters(DbType type)
        {
            parameterName = new SqlParameter();
            parameterName.DbType = type;
        }

        public SpParamerters(string paramName, DbType paramType, ParameterDirection paramDirection)
        {
            parameterName = new SqlParameter();
            parameterName.DbType = paramType;
            parameterName.Direction = paramDirection;
        }

        public SpParamerters(string paramName, DbType paramType, int paramSize, ParameterDirection paramDirection)
        {
            parameterName = new SqlParameter(paramName, paramType);
            parameterName.Size = paramSize;
            parameterName.Direction = paramDirection;
        }

        public SpParamerters(string param, DbType type, int size)
        {
            parameterName = new SqlParameter(param, type);
            parameterName.Size = size;
        }

        public SpParamerters(DbType type, int size, ParameterDirection direction)
        {
            parameterName = new SqlParameter();
            parameterName.DbType = type;
            parameterName.Size = size;
            parameterName.Direction = direction;

        }
        public SpParamerters(DbType type, ParameterDirection direction)
        {
            parameterName = new SqlParameter();
            parameterName.DbType = type;
            parameterName.Direction = direction;
        }
        public object Value
        {
            get { return parameterName.Value; }
            set { parameterName.Value = value; }
        }
        public SqlParameter Parameter
        {
            get { return parameterName; }
        }

    }

}
