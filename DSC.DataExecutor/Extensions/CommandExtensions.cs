using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC.DataExecutor.Extensions
{
    /// <summary>
    /// Extensions for DbCommand
    /// </summary>
    public static class CommandExtensions
    {
        public static IDataParameter AddParameter(this IDbCommand command, string name, object value)
        {
            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);
            return p;
        }

        public static void AddColumn(this DataRow row, object value, int count)
        {
            try
            {
                if (value != null)
                {
                    row[count] = value;
                }
                else {
                    row[count] = DBNull.Value;
                }
            }
            catch (Exception)
            {
                row[count] = DBNull.Value;
            }
        }
    }
}
