using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC.DataExecutor
{
    public struct PropertyType
    {
        public static string StringType = "System.String";
        public static string IntType = "System.int";
    }
   public class SubPropertyName : Attribute
    {
        public string Name { get; set; }

    }
}
