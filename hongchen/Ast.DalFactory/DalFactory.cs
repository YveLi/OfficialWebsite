using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ast.DalFactory
{
    public class DalFactory
    {
        public static object GetInstances(string assemblyName, string typeName)
        {
            return Assembly.Load(assemblyName).CreateInstance(typeName);
        }
    }
}
