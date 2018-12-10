using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    class TEP
    {
        private int ID_Apariencia; 
        private int ID_Respiracion;
        private int ID_Circulacion;
        private string componente_apariencia;
        private string componente_respiracion;
        private string componente_circulacion;

        public int ID_Apariencia1 { get => ID_Apariencia; set => ID_Apariencia = value; }
        public int ID_Respiracion1 { get => ID_Respiracion; set => ID_Respiracion = value; }
        public int ID_Circulacion1 { get => ID_Circulacion; set => ID_Circulacion = value; }
        public string Componente_apariencia { get => componente_apariencia; set => componente_apariencia = value; }
        public string Componente_respiracion { get => componente_respiracion; set => componente_respiracion = value; }
        public string Componente_circulacion { get => componente_circulacion; set => componente_circulacion = value; }
    }
}
