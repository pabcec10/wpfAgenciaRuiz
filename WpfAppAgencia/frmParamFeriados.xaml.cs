using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//Agregado para Datos y Combo
using System.Data;
using System.Data.SqlClient;
//using System.Data.Objects;
using System.Data.OleDb;
using System.Configuration;
using System.ComponentModel;
using System.Xml;

namespace WpfAppAgencia
{
    /// <summary>
    /// Lógica de interacción para frmParamFeriados.xaml
    /// </summary>
    public partial class frmParamFeriados : Window
    {
        int idFer = 0;
        char Acc = 'Z';
        int Anio = 0;

        SqlCommand comando = new SqlCommand();
        SqlConnection conexion = new SqlConnection();
        SqlDataAdapter adaptador = new SqlDataAdapter();
        DataSet ds = new DataSet();

        public frmParamFeriados()
        {
            InitializeComponent();
            this.gridDatosFeriado.IsEnabled = false;
            this.muestraBotones();
            this.cargaAnio();
        }
        private void cargaAnio()
        {
            int i,anio = Convert.ToInt32(DateTime.Today.Year)-5;

            for (i = anio; i <= (anio + 10); i++)
                this.cboAnio.Items.Add(i);
        }
        private void limpFeriado()
        {
            this.dateFeriado.Text = string.Empty;
            this.txtDesc.Text = string.Empty;
            gridFeriados.SelectedIndex = -1;
        }
        private void ocultaBotones()
        {
            this.btnAlta.Visibility = Visibility.Hidden;
            this.btnBaja.Visibility = Visibility.Hidden;
            this.btnModifica.Visibility = Visibility.Hidden;
            this.btnSalir.Visibility = Visibility.Hidden;
            this.btnCancelar.Visibility = Visibility.Visible;
            this.btnGrabar.Visibility = Visibility.Visible;
        }
        private void muestraBotones()
        {
            this.btnAlta.Visibility = Visibility.Visible;
            this.btnBaja.Visibility = Visibility.Visible;
            this.btnModifica.Visibility = Visibility.Visible;
            this.btnSalir.Visibility = Visibility.Visible;
            this.btnCancelar.Visibility = Visibility.Hidden;
            this.btnGrabar.Visibility = Visibility.Hidden;
        }
        private DataSet getData(string strSQL, string strDA)
        {
            string CadenaStr = ConfigurationManager.ConnectionStrings["CadConexion"].ConnectionString; //Obtiene la Cadena de Conexion de app.config
            SqlConnection conn = new SqlConnection(CadenaStr);
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, strDA);
            return ds;
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (this.txtDesc.Text.Trim() != string.Empty)
                    this.btnGrabar.Focus();
                else
                    MessageBox.Show("Debe Ingresar Una Valor de Descripción para Feriado", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void dateFeriado_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.dateFeriado.SelectedDate != null)
                this.txtDesc.Focus();
        }

        private void btnAlta_Click(object sender, RoutedEventArgs e)
        {
            Acc = 'A';
            this.cboAnio.IsEnabled = false;
            this.gridDatosFeriado.IsEnabled = true;
            this.gridFeriados.IsEnabled = false;
            this.ocultaBotones();
            this.limpFeriado();
            this.dateFeriado.Focus();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Acc = 'Z';
            this.gridDatosFeriado.IsEnabled = false;
            this.gridFeriados.IsEnabled = true;
            this.muestraBotones();
            this.limpFeriado();
            this.cboAnio.IsEnabled = true;
            //this.cboAnio.Text = string.Empty;
            this.gridFeriados.Columns.Clear();
            this.gridFeriados.ItemsSource = null;
        }
        private bool ValidaFeriado()
        {
            bool Band = true;
            if ((this.txtDesc.Text.Trim() == string.Empty) || (this.dateFeriado.Text.Trim() == string.Empty))
                Band = false;
            return Band;
        }
        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidaFeriado() == true)
            {
                string CadenaStr1 = ConfigurationManager.ConnectionStrings["CadConexion"].ConnectionString;
                SqlConnection conn = new SqlConnection(CadenaStr1);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "sp_ABM_ConfigFeriados";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@xIdFeriados", SqlDbType.Int);
                cmd.Parameters["@xIdFeriados"].Value = idFer;

                cmd.Parameters.Add("@xFecha", SqlDbType.Date);
                cmd.Parameters["@xFecha"].Value = Convert.ToDateTime(this.dateFeriado.Text);

                cmd.Parameters.Add("@xDesc", SqlDbType.VarChar);
                cmd.Parameters["@xDesc"].Value = this.txtDesc.Text.Trim();

                cmd.Parameters.Add("@xAcc", SqlDbType.VarChar);
                cmd.Parameters["@xAcc"].Value = Acc;

                cmd.Connection = conn;

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();
                Acc = 'Z';
                this.cboAnio.IsEnabled = true;
                //this.cboAnio.Text = string.Empty;
                this.gridDatosFeriado.IsEnabled = false;
                this.gridFeriados.IsEnabled = true;
                this.limpFeriado();
                this.muestraBotones();
                //this.gridFeriados.Columns.Clear();
                //this.gridFeriados.ItemsSource = null;
                this.cargaGrillaFeriados(Anio);
            }
            else
                MessageBox.Show("Faltan especificar valores para uno o más de los campos obligatorios", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void cargaGrillaFeriados(int xAnio)
        {
            this.gridFeriados.Columns.Clear();
            this.gridFeriados.ItemsSource = null;

            DataTable dt = new DataTable();
            conexion.ConnectionString = ConfigurationManager.ConnectionStrings["CadConexion"].ConnectionString;
            SqlDataAdapter da = new SqlDataAdapter("sp_listapersFeriados", conexion);

            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            da.SelectCommand.Parameters.Add("@xPeriodo", SqlDbType.Int);
            da.SelectCommand.Parameters["@xPeriodo"].Value = xAnio;

            da.Fill(dt);

            this.gridFeriados.ItemsSource = dt.DefaultView;
            conexion.Close();

            this.gridFeriados.Columns[0].Visibility = Visibility.Hidden;
            this.gridFeriados.Columns[1].Header = "Fecha";
            this.gridFeriados.Columns[2].Header = "Corresponde a";
            this.gridFeriados.Columns[3].Visibility = Visibility.Hidden;

            foreach (DataGridColumn column in this.gridFeriados.Columns)
            {
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
        private void cboAnio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboAnio.SelectedItem != null)
            {
                Anio = Convert.ToInt32(this.cboAnio.SelectedItem.ToString());
                this.cargaGrillaFeriados(Anio);
            }

        }
        private void editaFeriado(int xIdFer)
        {
            DataSet ds = getData("Select * From ConfigFeriados Where IdFeriados=" + xIdFer, "persConfigFeriados");
            DataTable dt = ds.Tables[0];
            
            this.dateFeriado.Text = dt.Rows[0]["FechaFeriado"].ToString();
            this.txtDesc.Text = dt.Rows[0]["FeriadoDesc"].ToString();
        }

        private void gridFeriados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridFeriados.SelectedItem != null)
            {
                object item = gridFeriados.SelectedItem;
                string id_D = ((DataRowView)gridFeriados.SelectedItem).Row["IdFeriados"].ToString();
                idFer = Convert.ToInt32(id_D);
                editaFeriado(idFer);
            }
        }

        private void btnBaja_Click(object sender, RoutedEventArgs e)
        {
            if (this.dateFeriado.Text != string.Empty)
            {
                Acc = 'B';
                this.cboAnio.IsEnabled = false;
                this.gridDatosFeriado.IsEnabled = false;
                this.gridFeriados.IsEnabled = false;
                this.ocultaBotones();
                this.dateFeriado.Focus();
            }
            else
                MessageBox.Show("Debe Elegir un Feriado Para Dar de Baja", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnModifica_Click(object sender, RoutedEventArgs e)
        {
            if (this.dateFeriado.Text != string.Empty)
            {
                Acc = 'M';
                this.cboAnio.IsEnabled = false;
                this.gridDatosFeriado.IsEnabled = true;
                this.gridFeriados.IsEnabled = false;
                this.ocultaBotones();
                this.dateFeriado.Focus();
            }
            else
                MessageBox.Show("Debe Elegir un Feriado Para Modificar", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
