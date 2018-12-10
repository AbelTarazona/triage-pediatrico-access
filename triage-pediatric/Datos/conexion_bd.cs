using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class conexion_bd
    {
        public static string cadenaConexion
        {
            //Si es la versión de Access obviar esto.
            get
            {
                return "data source=(local); initial catalog=BD_Triage; integrated security=SSPI;";
            }
        }
    }
}
