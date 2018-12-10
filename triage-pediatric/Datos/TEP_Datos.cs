using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Dominio;

namespace Datos
{
    public class TEP_Datos
    {
        
        OleDbConnection conexion;

        public TEP_Datos()
        {
            
            conexion = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;" +
            @"Data Source=|DataDirectory|\\BD_Triage.mdb;" +
            @"Jet OLEDB:Database Password=; Persist Security Info=False;");
        }


#region SELECTs

        public DataSet SelectApariencia()
        {

            string select = "select * from T_Apariencia_Tep";
            DataSet dsTap = new DataSet();
            OleDbDataAdapter unAdaptador = new OleDbDataAdapter(select,conexion);
            unAdaptador.Fill(dsTap);
            return dsTap;
        }

        public DataSet SelectRespiracion()
        {
            string select = "select * from T_Respiracion_Tep";
            DataSet dsTap = new DataSet();
            OleDbDataAdapter unAdaptador = new OleDbDataAdapter(select, conexion);
            unAdaptador.Fill(dsTap);
            return dsTap;
        }

        public DataSet SelectCirculacion()
        {
            string select = "select * from T_Circulacion_Tep";
            DataSet dsTap = new DataSet();
            OleDbDataAdapter unAdaptador = new OleDbDataAdapter(select, conexion);
            unAdaptador.Fill(dsTap);
            return dsTap;
        }

        public DataSet SelectHidratacion()
        {
            string select = "select * from T_Hidratacion";
            DataSet dsTap = new DataSet();
            OleDbDataAdapter unAdaptador = new OleDbDataAdapter(select, conexion);
            unAdaptador.Fill(dsTap);
            return dsTap;
        }

        public DataSet selectDiscapacidad()
        {
            string select = "select * from T_Discapacidad";
            DataSet dsTap = new DataSet();
            OleDbDataAdapter unAdaptador = new OleDbDataAdapter(select, conexion);
            unAdaptador.Fill(dsTap);
            return dsTap;
        }

        public DataSet selectPatologia()
        {
            string select = "select * from T_Patologia";
            DataSet dsTap = new DataSet();
            OleDbDataAdapter unAdaptador = new OleDbDataAdapter(select, conexion);
            unAdaptador.Fill(dsTap);
            return dsTap;
        }

        public DataSet selectGrupoProblema()
        {
            string select = "select * from T_Grupo_Problema";
            DataSet dsTapp = new DataSet();
            OleDbDataAdapter unad = new OleDbDataAdapter(select, conexion);
            unad.Fill(dsTapp);
            return dsTapp;
        }

        public DataSet selectTipoProblemaPorGrupo(int idGrupoProblema)
        {
            string select = "SELECT DISTINCT  T_Grupo_Problema.Id as ID_GP,T_Grupo_Problema.nombre as nombre_GP,T_Tipo_Problema.Id as ID_TP,T_Tipo_Problema.nombre as nombre_TP from (T_Tipo_Problema INNER JOIN T_Grupo_x_Tipo ON T_Tipo_Problema.Id = T_Grupo_x_Tipo.ID_Tipo) INNER JOIN T_Grupo_Problema ON T_Grupo_Problema.Id = T_Grupo_x_Tipo.ID_Grupo where T_Grupo_Problema.Id =" + idGrupoProblema+";";
            DataSet dsTapp = new DataSet();
            OleDbDataAdapter unad = new OleDbDataAdapter(select, conexion);
            unad.Fill(dsTapp);
            return dsTapp;
        }

        public DataSet selectDescripcionPrioridadGP(int idGrupoProblema, int idTipoProblema)
        {
            string select = "SELECT Num_Prioridad, Descripcion from T_Grupo_x_Tipo where ID_Grupo = " + idGrupoProblema+" and ID_Tipo = " + idTipoProblema + ";";
            DataSet dsTapp = new DataSet();
            OleDbDataAdapter unad = new OleDbDataAdapter(select, conexion);
            unad.Fill(dsTapp);
            return dsTapp;
        }

        #endregion


    }
}
