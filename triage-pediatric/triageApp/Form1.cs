using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Datos;
using Dominio;
using Microsoft.VisualBasic;

/*Desarrollado por Abel Tarazona - Derechos Reservados*/


namespace triageApp
{
    public partial class TriageApp : Form
    {
        TEP_Datos tepd;
        private String estadoTEP = "";
        Dictionary<string, string> diagnostico;

        public TriageApp()
        {
            InitializeComponent();
            //Color
            Color color = Color.FromArgb(241, 241, 241);
            this.BackColor = color;
            //
            tepd = new TEP_Datos();
            diagnostico = new Dictionary<string, string>();
            cargarHashMapDiagnostico();
            listarGrupoProblema();
            listarApariencia();
            listarRespiracion();
            listarCirculacion();
            listarHidratacion();
            listarDiscapacidad(); listarPatologias();
            cargarGraficoTEP();
            panelDolorMenor3.Hide();
            panelTresSiete.Hide();
            panel8amasDolor.Hide();
            label14.Text = "";
        }

        #region Combobox de Signos sintomas

        private void listarGrupoProblema()
        {
            DataSet dsGP = tepd.selectGrupoProblema();
            cbGrupoProblema.DataSource = dsGP.Tables[0];
            cbGrupoProblema.DisplayMember = "nombre";
            cbGrupoProblema.ValueMember = "Id";
        }

        private void listarTipoPorGrupoProblea(int idTGP)
        {
            DataSet dsTGP = tepd.selectTipoProblemaPorGrupo(idTGP);
            cbTipoGP.DataSource = dsTGP.Tables[0];
            cbTipoGP.DisplayMember = "nombre_TP";
            cbTipoGP.ValueMember = "ID_TP";
        }

        private void listarDescripcionPrioridadPorGrupoProblema(int idGP, int idTGP)
        {
            DataSet dsTGP = tepd.selectDescripcionPrioridadGP(idGP,idTGP);
            cbDescripcionProblema.DataSource = dsTGP.Tables[0];
            cbDescripcionProblema.DisplayMember = "Descripcion";
            cbDescripcionProblema.ValueMember = "Num_Prioridad";
        }
        #endregion


        private void listarApariencia()
        {
            DataSet dsApariencia = tepd.SelectApariencia();
            chkListBoxApariencia.DataSource = dsApariencia.Tables[0];
            chkListBoxApariencia.DisplayMember = "componente_apariencia";
            chkListBoxApariencia.ValueMember = "ID_Apariencia";
        }

        private void listarRespiracion()
        {
            DataSet dsRespiracion = tepd.SelectRespiracion();
            chkListBoxRespiracion.DataSource = dsRespiracion.Tables[0];
            chkListBoxRespiracion.DisplayMember = "componente_respiracion";
            chkListBoxRespiracion.ValueMember = "ID_Respiracion";
        }

        private void listarCirculacion()
        {
            DataSet dsCirculacion = tepd.SelectCirculacion();
            chkListBoxCirculacion.DataSource = dsCirculacion.Tables[0];
            chkListBoxCirculacion.DisplayMember = "componente_circulacion";
            chkListBoxCirculacion.ValueMember = "ID_Circulacion";
        }

        private void listarHidratacion()
        {
            DataSet dsHid = tepd.SelectHidratacion();
            chkHidratacion.DataSource = dsHid.Tables[0];
            chkHidratacion.DisplayMember = "nombre";
            chkHidratacion.ValueMember = "ID_Hidratacion";
        }

        private void listarDiscapacidad()
        {
            DataSet dsHid = tepd.selectDiscapacidad();
            chkDiscapacidad.DataSource = dsHid.Tables[0];
            chkDiscapacidad.DisplayMember = "nombre";
            chkDiscapacidad.ValueMember = "ID_Discapacidad";
        }

        private void listarPatologias()
        {
            DataSet dsHid = tepd.selectPatologia();
            chkPatologias.DataSource = dsHid.Tables[0];
            chkPatologias.DisplayMember = "nombre";
            chkPatologias.ValueMember = "ID_Patologia";
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TriageApp_Load(object sender, EventArgs e)
        {

        }

        private void txtEdad_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {        
            if (validaciones())
            {
                estadoTEP = "";
                //cbNuevaPrioridad2.Items.Clear();
                DialogResult dialogResult = MessageBox.Show("¿Esta seguro de guardar estos datos?", "TEP", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    int cont = 0;
                    Char[] arregloEstadoTep;
                    cargarGraficoTEP();
                    String estA = lblApariencia.Text;
                    String estR = lblRespiracion.Text;
                    String estC = lblCirculacion.Text;

                    //Hallamos patron de lados 
                    if (estA.Equals("0")) { estadoTEP += "1"; } else { estadoTEP += "0"; }
                    if (estR.Equals("0")) { estadoTEP += "1"; } else { estadoTEP += "0"; }
                    if (estC.Equals("0")) { estadoTEP += "1"; } else { estadoTEP += "0"; }

                    //Convertimos patron de lados en arreglo y contamos cantidad de lados anormales
                    arregloEstadoTep = estadoTEP.ToCharArray();
                    for(int i = 0; i < arregloEstadoTep.Length; i++)
                    {
                        if(arregloEstadoTep[i].Equals('0')) { cont++; }
                    }

                    //Lanzamos prioridad
                    lanzarPrioridad(cont);
                    


                    if (diagnostico.TryGetValue(estadoTEP, out string value))
                    {
                        lblDiagnosticoTepFinal.Text = value;
                    }

                    //Activar prioridad de Paso #2
                    /*cbNuevaPrioridad2.DisplayMember = "Text";
                    cbNuevaPrioridad2.ValueMember = "Value";
                    int priori = int.Parse(lblPrioridad.Text);
                    for(int j = 1; j < priori; j++)
                    {
                        cbNuevaPrioridad2.Items.Add(new { Text = j, Value = j });
                    }*/

                    //Panel para Dolor

                    //Para mesarios y edad menor a 2 anos
                    if( (int.Parse(txtEdad.Text) > 0 && int.Parse(txtEdad.Text) <= 2) || cbTipoEdad.SelectedIndex == 0) { panelDolorMenor3.Show(); panelTresSiete.Hide(); panel8amasDolor.Hide(); }
                    else if((int.Parse(txtEdad.Text) >= 3 && int.Parse(txtEdad.Text) <= 7) && cbTipoEdad.SelectedIndex == 1) { panelTresSiete.Show(); panelDolorMenor3.Hide(); panel8amasDolor.Hide(); }
                    else { panel8amasDolor.Show(); panelDolorMenor3.Hide(); panelTresSiete.Hide(); }

                    //En caso tengan 4 meses para abajo.. 
                    if(int.Parse(txtEdad.Text) <= 4 && cbTipoEdad.SelectedIndex == 0) { lblPrioFinModf.Text = "3"; }
           
                }
                else if (dialogResult == DialogResult.No)
                {
                    String salida = "";
                    foreach (object i in chkListBoxApariencia.CheckedItems) {
                        DataRowView cas = i as DataRowView;
                        salida += " / " + cas["componente_apariencia"];
                    }
                    //txtAnamnesis.Text = salida;
                }
            }
        }

        public void lanzarPrioridad(int c)
        {
            switch(c)
            {
                case 0:
                    lblPrioridad.ForeColor = Color.Green;
                    lblDetalleTep.ForeColor = Color.Green;
                    String priori = Interaction.InputBox("Seleccione una prioridad:", "Estable", "Debe ser 4 o 5", -1, -1);
                    if(priori.Equals("Debe ser 4 o 5")) { lblPrioridad.Text = "5"; } else { lblPrioridad.Text = priori; }
                    lblDetalleTep.Text = "Continue con la sistemática de triage.";
                    break;
                case 1:
                    lblPrioridad.ForeColor = Color.WhiteSmoke;
                    lblDetalleTep.ForeColor = Color.WhiteSmoke;
                    lblPrioridad.Text = "3";
                    lblDetalleTep.Text = "Continue con la sistemática de triage.";
                    break;
                case 2:
                    lblPrioridad.ForeColor = Color.White;
                    lblDetalleTep.ForeColor = Color.White;
                    lblPrioridad.Text = "2";
                    lblDetalleTep.Text = "Continue con la valoración médica.";
                    break;
                case 3:
                    lblPrioridad.ForeColor = Color.Red;
                    lblDetalleTep.ForeColor = Color.Red;
                    lblPrioridad.Text = "1";
                    lblDetalleTep.Text = "Urgente, cuarto de resucitación.";
                    break;
                default:
                    lblPrioridad.Text = "--";
                    break;
            }

        }

        public void cargarHashMapDiagnostico()
        {
            diagnostico.Add("111", "Estable");
            diagnostico.Add("101", "Dificultad respiratoria");
            diagnostico.Add("001", "Fallo respiratorio");
            diagnostico.Add("011", "Disfunción SNC");
            diagnostico.Add("110", "Shock compensado");
            diagnostico.Add("010", "Shock descompensado");
            diagnostico.Add("000", "Fallo cardio-respiratorio");
        }

        public void cargarGraficoTEP()
        {
            grafTEP.Series.Clear();
            grafTEP.Series.Add("Apariencia");
            grafTEP.Series.Add("Respiracion");
            grafTEP.Series.Add("Circulacion");


            grafTEP.Series["Apariencia"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            grafTEP.Series["Respiracion"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            grafTEP.Series["Circulacion"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;

            grafTEP.Series["Apariencia"].Points.Add(Double.Parse(lblApariencia.Text));
            grafTEP.Series["Apariencia"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

                     
            
            grafTEP.Series["Respiracion"].Points.Add(Double.Parse(lblRespiracion.Text));
            grafTEP.Series["Respiracion"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;


            
            
            grafTEP.Series["Circulacion"].Points.Add(Double.Parse(lblCirculacion.Text));
            grafTEP.Series["Circulacion"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
        }

        private void chkListBoxApariencia_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int sco = chkListBoxApariencia.CheckedItems.Count;
            if(e.NewValue == CheckState.Checked) { ++sco; }
            if (e.NewValue == CheckState.Unchecked) { --sco; }
            lblApariencia.Text = "" + sco;
        }

        private void chkListBoxRespiracion_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int sco = chkListBoxRespiracion.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked) { ++sco; }
            if (e.NewValue == CheckState.Unchecked) { --sco; }
            lblRespiracion.Text = "" + sco;
        }

        private void chkListBoxCirculacion_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int sco = chkListBoxCirculacion.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked) { ++sco; }
            if (e.NewValue == CheckState.Unchecked) { --sco; }
            lblCirculacion.Text = "" + sco;
        }

        public bool validaciones()
        {

            if (String.IsNullOrEmpty(txtDNI.Text)) { MessageBox.Show("No puede dejar el campo de DNI vacio"); return false; }
            if (String.IsNullOrEmpty(txtEdad.Text)) { MessageBox.Show("No puede dejar el campo de Edad vacio"); return false; }
            if (txtDNI.Text.Length != 8) { MessageBox.Show("El DNI es de 8 digitos"); return false; }
            if (txtEdad.Text == "0" && cbTipoEdad.SelectedIndex == 1) { MessageBox.Show("En años no puede haber edad 0"); return false; }
            if (cbTipoEdad.SelectedIndex == -1) { MessageBox.Show("Seleccione un tipo de edad"); return false; }
            if (cbTipoEdad.SelectedIndex == 0) //Meses
            {
                if (int.Parse(txtEdad.Text) > 11) { MessageBox.Show("Los meses no deben ser mayor de 11"); return false; }
            }
            else //Anhos
            {
                if (int.Parse(txtEdad.Text) > 14) { MessageBox.Show("La edad no debe ser mayor de 14"); return false; }
            }

            return true;
        }

        private void txtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e);
        }

        public void soloNumeros(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Esta seguro de reiniciar el triage?", "Triage App", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                txtDNI.Text = "";
                txtEdad.Text = "";
                grafTEP.Series.Clear();
                lblApariencia.Text = "0"; lblCirculacion.Text = "0"; lblRespiracion.Text = "0";
                lblDiscapacidad.Text = "0"; lblPatologia.Text = "0";
                lblDiagnosticoTepFinal.Text = "--";
                lblPrioridad.Text = "--";
                estadoTEP = "";
                lblDetalleTep.Text = "--";
                txtDetalleDolor.Text = "--"; txtPrioridadDolor.Text = "--"; txtPuntuacionDolor.Text = "--";
                panelDolorMenor3.Hide(); panelTresSiete.Hide(); panel8amasDolor.Hide();

                foreach (int i in chkListBoxApariencia.CheckedIndices) { chkListBoxApariencia.SetItemCheckState(i, CheckState.Unchecked); }
                foreach (int i in chkListBoxCirculacion.CheckedIndices) { chkListBoxCirculacion.SetItemCheckState(i, CheckState.Unchecked); }
                foreach (int i in chkListBoxRespiracion.CheckedIndices) { chkListBoxRespiracion.SetItemCheckState(i, CheckState.Unchecked); }
                foreach (int i in chkHidratacion.CheckedIndices) { chkHidratacion.SetItemCheckState(i, CheckState.Unchecked); }
                foreach (int i in chkDiscapacidad.CheckedIndices) { chkDiscapacidad.SetItemCheckState(i, CheckState.Unchecked); }
                foreach (int i in chkPatologias.CheckedIndices) { chkPatologias.SetItemCheckState(i, CheckState.Unchecked); }

                lblPriFinTep.Text = "-"; lblPriFiSign.Text = "-"; lblPrioFinConst.Text = "-"; lblPrioFinModf.Text = "-"; lblPrioFinValo.Text = "-";
                lblPrioridadFinal.Text = "-";

                txtDetalleHid.Text = "--"; txtPrioridadHid.Text = "--"; txtPuntuacionHid.Text = "--";

                txtFrecuenciaCardiaca.Text = ""; txtFrecuenciaRespiratoria.Text = ""; txtSaturacionIn.Text = ""; txtDetalleConsVi.Text = "--"; txtPrioridadConVit.Text = "--";

                //Borrado de signo sintomas
                label14.Text = "Seleccione un Grupo de problema";
                lblPrioridadAbsSigSin.Text = "#";
                cbDescripcionProblema.DataSource = null;
                cbTipoGP.DataSource = null;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }



        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            //Guardado de signo y sintoma
            if(!lblPrioridadAbsSigSin.Text.Equals("#"))
            {
                lblPriFiSign.Text = lblPrioridadAbsSigSin.Text;
            }
            else
            {
                MessageBox.Show("Seleccione un grupo, tipo y descripcion del problema para generar una prioridad.");
            }
            
        }

        private void lblPrioridad_Click(object sender, EventArgs e)
        {

        }

        private void btnIntensidadDolorMenorTres_Click(object sender, EventArgs e)
        {
            //Calculos
            List<int> numberList = new List<int>();

            int i = 2;
            foreach(RadioButton rdo in gbExpFacial.Controls.OfType<RadioButton>())
            {
                if(rdo.Checked) { numberList.Add(i); }
                i--;
            }

            int ii = 2;
            foreach (RadioButton rdo in gbPiernas.Controls.OfType<RadioButton>())
            {
                if (rdo.Checked) { numberList.Add(ii); }
                ii--;
            }

            int iii = 2;
            foreach (RadioButton rdo in gbActividad.Controls.OfType<RadioButton>())
            {
                if (rdo.Checked) { numberList.Add(iii); }
                iii--;
            }

            int iiii = 2;
            foreach (RadioButton rdo in gbLlanto.Controls.OfType<RadioButton>())
            {
                if (rdo.Checked) { numberList.Add(iiii); }
                iiii--;
            }

            int iiiii = 2;
            foreach (RadioButton rdo in gbSusceptible.Controls.OfType<RadioButton>())
            {
                if (rdo.Checked) { numberList.Add(iiiii); }
                iiiii--;
            }

            //Salida
            int puntuacion = numberList.Take(5).Sum();
            txtPuntuacionDolor.Text = "" + puntuacion;
            generarResultadoDolor(puntuacion);
            
        }

        public void generarResultadoDolor(int puntuacion)
        {
            if (puntuacion >= 0 && puntuacion <= 3) { txtDetalleDolor.Text = "No dolor \no dolor leve"; txtPrioridadDolor.Text = "4"; }
            else if (puntuacion >= 4 && puntuacion <= 7) { txtDetalleDolor.Text = "Dolor \nmoderado"; txtPrioridadDolor.Text = "3"; }
            else { txtDetalleDolor.Text = "Dolor \nsevero"; txtPrioridadDolor.Text = "2"; }           
        }

        private void btnDolor0_Click(object sender, EventArgs e)
        {
            txtPuntuacionDolor.Text = "0";
            generarResultadoDolor(int.Parse(txtPuntuacionDolor.Text));
        }

        private void btnDolor2_Click(object sender, EventArgs e)
        {
            txtPuntuacionDolor.Text = "2";
            generarResultadoDolor(int.Parse(txtPuntuacionDolor.Text));
        }

        private void btnDolor4_Click(object sender, EventArgs e)
        {
            txtPuntuacionDolor.Text = "4";
            generarResultadoDolor(int.Parse(txtPuntuacionDolor.Text));
        }

        private void btnDolor6_Click(object sender, EventArgs e)
        {
            txtPuntuacionDolor.Text = "6";
            generarResultadoDolor(int.Parse(txtPuntuacionDolor.Text));
        }

        private void btnDolor8_Click(object sender, EventArgs e)
        {
            txtPuntuacionDolor.Text = "8";
            generarResultadoDolor(int.Parse(txtPuntuacionDolor.Text));
        }

        private void btnDolor10_Click(object sender, EventArgs e)
        {
            txtPuntuacionDolor.Text = "10";
            generarResultadoDolor(int.Parse(txtPuntuacionDolor.Text));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPuntuacionDolor.Text = comboBox1.SelectedItem.ToString();
            generarResultadoDolor(int.Parse(txtPuntuacionDolor.Text));
        }

        private void chkHidratacion_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int sco = chkHidratacion.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked) { ++sco; }
            if (e.NewValue == CheckState.Unchecked) { --sco; }
            txtPuntuacionHid.Text = "" + sco;
            generarResultadoHidratacion(sco);
        }

        public void generarResultadoHidratacion(int puntuacion)
        {
            if (puntuacion >= 0 && puntuacion <= 2) { txtDetalleHid.Text = "No Deshidratado"; txtPrioridadHid.Text = "4"; }
            else if (puntuacion >= 3 && puntuacion <= 6) { txtDetalleHid.Text = "Regular Hidratacion"; txtPrioridadHid.Text = "3"; }
            else { txtDetalleHid.Text = "Mala Hidratacion"; txtPrioridadHid.Text = "2"; }
        }

        private void cbPulso_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(cbPulso.SelectedIndex == 0) { txtPrioridadPulso.Text = "2"; } else { txtPrioridadPulso.Text = "--"; }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSatO2_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtSaturacionIn.Text) == false)
            {
                txtDetalleConsVi.Text = "Saturacion de O2";
                int a = int.Parse(txtSaturacionIn.Text);
                generarResultadoSaturacion(a);
            } else { MessageBox.Show("Ingrese valor de 02"); }
            

        }
        public void generarResultadoSaturacion(int puntuacion)
        {
            if (puntuacion > 94) { txtPrioridadConVit.Text = "4"; }
            else if (puntuacion >= 91 && puntuacion <= 94) { txtPrioridadConVit.Text = "3"; }
            else { txtPrioridadConVit.Text = "2"; }
        }

        private void btnFrecCard_Click(object sender, EventArgs e)
        {
            if(validaciones() && String.IsNullOrEmpty(txtFrecuenciaCardiaca.Text) == false)
            {

                int edad;
                int fc;
                int moa = 3;

                txtDetalleConsVi.Text = "Frecuencia Cardiaca";
                    
                edad = int.Parse(txtEdad.Text);

                fc = int.Parse(txtFrecuenciaCardiaca.Text);

                if (cbTipoEdad.SelectedIndex == 0) { moa = 0; } else { moa = 1; }

                calcularPrioridadFC(edad, fc, moa);
            }


        }

        public void calcularPrioridadFC(int edad, int frecCard, int tipoEdad)
        {
            if(tipoEdad == 0)
            {
                //Si es meses
                if(edad >= 0 && edad <= 3)
                {
                        if(frecCard < 40 || frecCard > 230) { txtPrioridadConVit.Text = "1"; }
                        else if ( (frecCard >= 40 && frecCard <= 65) || (frecCard >= 206 && frecCard <= 230)) { txtPrioridadConVit.Text = "2"; }
                        else if ((frecCard >= 66 && frecCard <= 90) || (frecCard >= 181 && frecCard <= 205)) { txtPrioridadConVit.Text = "3"; }
                        else if (frecCard >= 91 && frecCard <= 180) { txtPrioridadConVit.Text = "5"; }

                } else if (edad >= 4 && edad <= 6)
                {
                    if (frecCard < 40 || frecCard > 210) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 40 && frecCard <= 63) || (frecCard >= 181 && frecCard <= 210)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 64 && frecCard <= 80) || (frecCard >= 161 && frecCard <= 180)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 81 && frecCard <= 160) { txtPrioridadConVit.Text = "5"; }
                } else
                {
                    if (frecCard < 40 || frecCard > 180) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 40 && frecCard <= 60) || (frecCard >= 161 && frecCard <= 180)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 61 && frecCard <= 80) || (frecCard >= 141 && frecCard <= 160)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 81 && frecCard <= 140) { txtPrioridadConVit.Text = "5"; }
                }
            }
            else if (tipoEdad == 1)
            {
                //Si es anhos
                if (edad >= 1 && edad <= 3)
                {
                    if (frecCard < 40 || frecCard > 165) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 40 && frecCard <= 58) || (frecCard >= 146 && frecCard <= 165)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 59 && frecCard <= 75) || (frecCard >= 131 && frecCard <= 145)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 76 && frecCard <= 130) { txtPrioridadConVit.Text = "5"; }

                }
                else if (edad >= 6 && edad <= 10)
                {
                    if (frecCard < 40 || frecCard > 140) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 40 && frecCard <= 55) || (frecCard >= 126 && frecCard <= 140)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 56 && frecCard <= 70) || (frecCard >= 111 && frecCard <= 125)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 71 && frecCard <= 110) { txtPrioridadConVit.Text = "5"; }
                }
                else if(edad > 10)
                {
                    if (frecCard < 30 || frecCard > 120) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 30 && frecCard <= 45) || (frecCard >= 106 && frecCard <= 120)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 46 && frecCard <= 60) || (frecCard >= 91 && frecCard <= 105)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 61 && frecCard <= 90) { txtPrioridadConVit.Text = "5"; }
                }
            }
        }

        public void calcularPrioridadFR(int edad, int frecCard, int tipoEdad)
        {
            if (tipoEdad == 0)
            {
                //Si es meses
                if (edad >= 0 && edad <= 6)
                {
                    if (frecCard < 10 || frecCard > 80) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 10 && frecCard <= 20) || (frecCard >= 71 && frecCard <= 80)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 21 && frecCard <= 30) || (frecCard >= 61 && frecCard <= 70)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 31 && frecCard <= 60) { txtPrioridadConVit.Text = "5"; }

                }
                else
                {
                    if (frecCard < 10 || frecCard > 60) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 10 && frecCard <= 17) || (frecCard >= 56 && frecCard <= 60)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 18 && frecCard <= 25) || (frecCard >= 46 && frecCard <= 55)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 26 && frecCard <= 45) { txtPrioridadConVit.Text = "5"; }
                }
            }
            else if (tipoEdad == 1)
            {
                //Si es anhos
                if (edad >= 1 && edad <= 3)
                {
                    if (frecCard < 10 || frecCard > 40) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 10 && frecCard <= 15) || (frecCard >= 36 && frecCard <= 40)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 16 && frecCard <= 20) || (frecCard >= 31 && frecCard <= 35)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 21 && frecCard <= 30) { txtPrioridadConVit.Text = "5"; }

                }
                else if (edad == 6)
                {
                    if (frecCard < 8 || frecCard > 32) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 8 && frecCard <= 12) || (frecCard >= 29 && frecCard <= 32)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 13 && frecCard <= 16) || (frecCard >= 25 && frecCard <= 28)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 17 && frecCard <= 24) { txtPrioridadConVit.Text = "5"; }
                }
                else if (edad == 10)
                {
                    if (frecCard < 8 || frecCard > 26) { txtPrioridadConVit.Text = "1"; }
                    else if ((frecCard >= 8 && frecCard <= 12) || (frecCard >= 25 && frecCard <= 26)) { txtPrioridadConVit.Text = "2"; }
                    else if ((frecCard >= 13 && frecCard <= 14) || (frecCard >= 21 && frecCard <= 24)) { txtPrioridadConVit.Text = "3"; }
                    else if (frecCard >= 15 && frecCard <= 20) { txtPrioridadConVit.Text = "5"; }
                }
            }
        }

        private void btnFrecResp_Click(object sender, EventArgs e)
        {
            if(validaciones() && String.IsNullOrEmpty(txtFrecuenciaRespiratoria.Text) == false)
            {
                int edad;
                int fr;
                int moa = 3;

                txtDetalleConsVi.Text = "Frecuencia Respiratoria";

                edad = int.Parse(txtEdad.Text);
                fr = int.Parse(txtFrecuenciaRespiratoria.Text);

                if (cbTipoEdad.SelectedIndex == 0) { moa = 0; } else { moa = 1; }

                calcularPrioridadFR(edad, fr, moa);
            }
            
        }

        private void label55_Click(object sender, EventArgs e)
        {

        }

        private void lblPrioridad_TextChanged(object sender, EventArgs e)
        {
            if (lblPrioridad.Text != "--") { lblPriFinTep.Text = lblPrioridad.Text; }
            
        }

        private void txtPrioridadConVit_TextChanged(object sender, EventArgs e)
        {
            if (txtPrioridadConVit.Text != "--")
            { lblPrioFinConst.Text = txtPrioridadConVit.Text; }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Esta seguro de guardar estos datos? No podra cambiarlo.", "TEP", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                int a;
                int b;
                //int c;

                if (txtPrioridadDolor.Text == "--") { a = 10; } else { a = int.Parse(txtPrioridadDolor.Text); }
                if (txtPrioridadHid.Text == "--") { b = 10; } else { b = int.Parse(txtPrioridadHid.Text); }
                //if (txtPrioridadPulso.Text == "--") { c = 10; } else { c = int.Parse(txtPrioridadPulso.Text); }



                List<int> toCount = new List<int>() { a, b};

                int min = toCount.Min();

                if (min == 10) { lblPrioFinValo.Text = "5"; } else { lblPrioFinValo.Text = min.ToString(); }
            }
            else { }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

        }

        public void encontrarMinimaPrioridad()
        {
            int a, b, c, d, ee;

            if (lblPriFinTep.Text == "-") { a = 10; } else { a = int.Parse(lblPriFinTep.Text); }
            if (lblPriFiSign.Text == "-") { b = 10; } else { b = int.Parse(lblPriFiSign.Text); }
            if (lblPrioFinValo.Text == "-") { c = 10; } else { c = int.Parse(lblPrioFinValo.Text); }
            if (lblPrioFinConst.Text == "-") { d = 10; } else { d = int.Parse(lblPrioFinConst.Text); }
            if (lblPrioFinModf.Text == "-") { ee = 10; } else { ee = int.Parse(lblPrioFinModf.Text); }

            List<int> toCount = new List<int>() { a, b, c, d, ee };

            int min = toCount.Min();

            lblPrioridadFinal.Text = min.ToString();
        }

        private void lblPriFinTep_TextChanged(object sender, EventArgs e)
        {
            encontrarMinimaPrioridad(); 
        }

        private void lblPriFiSign_TextChanged(object sender, EventArgs e)
        {
            
                encontrarMinimaPrioridad();
            
        }

        private void lblPrioFinValo_TextChanged(object sender, EventArgs e)
        {
            
            encontrarMinimaPrioridad();
        }

        private void lblPrioFinConst_TextChanged(object sender, EventArgs e)
        {
            
            encontrarMinimaPrioridad();
        }

        private void lblPrioFinModf_TextChanged(object sender, EventArgs e)
        {
            
            encontrarMinimaPrioridad();
        }


        //PRIORIDAD FINAL LABEL
        private void lblPrioridadFinal_TextChanged(object sender, EventArgs e)
        {
            if (lblPrioridadFinal.Text == "1") { lblPrioridadFinal.Text = "1"; lblUbicacion.Text = "Cuarto de reanimación/\nresucitación/box vital"; MessageBox.Show("Prioridad 1 encontrada, prioridad final ya ha sido generada. Termina el proceso de triage."); }
            else if (lblPrioridadFinal.Text == "5" | lblPrioridadFinal.Text == "4" | lblPrioridadFinal.Text == "3") { lblUbicacion.Text = "Sala de espera"; }
            else if (lblPrioridadFinal.Text == "3") { lblUbicacion.Text = "Sala de observación"; } else { lblUbicacion.Text = "Sala de exploración"; }
        }

        private void chkDiscapacidad_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int sco = chkDiscapacidad.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked) { ++sco; }
            if (e.NewValue == CheckState.Unchecked) { --sco; }
            lblDiscapacidad.Text = "" + sco;
        }

        private void chkPatologias_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int sco = chkPatologias.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked) { ++sco; }
            if (e.NewValue == CheckState.Unchecked) { --sco; }
            lblPatologia.Text = "" + sco;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Esta seguro de guardar estos datos? No podra cambiarlo.", "TEP", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                if (int.Parse(lblDiscapacidad.Text) >= 1 || int.Parse(lblPatologia.Text) >= 1) { lblPrioFinModf.Text = "3"; }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Esta seguro de guardar estos datos? No podra cambiarlo.", "TEP", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
            }
        
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        String grupoProblemaTexto;
        String tipoProblemaTexto;
        String descripcionProblemaTexto;

        String idGrupoProblema;
        String idTipoProblema;

        private void cbGrupoProblema_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Cargamos data para los tipos de problemas #2
            idGrupoProblema = cbGrupoProblema.SelectedValue.ToString();

            listarTipoPorGrupoProblea(int.Parse(idGrupoProblema));

         
        }

        private void cbTipoGP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //#2
            tipoProblemaTexto = cbTipoGP.Text;
            label14.Text = "Grupo Problema -> " + grupoProblemaTexto + "\nTipo -> " + tipoProblemaTexto + "\nDescripción -> -";
            lblPrioridadAbsSigSin.Text = "#";
        }

        private void cbGrupoProblema_SelectedIndexChanged(object sender, EventArgs e)
        {
            //#1
            grupoProblemaTexto = cbGrupoProblema.Text;
            label14.Text = "Grupo Problema -> " + grupoProblemaTexto + "\nTipo -> - " + "\nDescripción -> -";

            //Se reinicia la descripcion y la prioridad
            cbDescripcionProblema.DataSource = null;
            lblPrioridadAbsSigSin.Text = "#";
        }

        private void cbTipoGP_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Cargamos data para descripcion #3
            idTipoProblema = cbTipoGP.SelectedValue.ToString();

            listarDescripcionPrioridadPorGrupoProblema(int.Parse(idGrupoProblema), int.Parse(idTipoProblema));
        }

        private void cbDescripcionProblema_SelectedIndexChanged(object sender, EventArgs e)
        {
            //#3
            descripcionProblemaTexto = cbDescripcionProblema.Text;
            label14.Text = "Grupo Problema -> " + grupoProblemaTexto + "\nTipo -> " + tipoProblemaTexto + "\nDescripción -> " + descripcionProblemaTexto;
        }

        private void cbDescripcionProblema_SelectionChangeCommitted(object sender, EventArgs e)
        {
            lblPrioridadAbsSigSin.Text = cbDescripcionProblema.SelectedValue.ToString();
        }
    }
}
