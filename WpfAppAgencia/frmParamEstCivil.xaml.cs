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
using System.IO;
//using System.Windows.Forms.Integration;
//Agregado para Datos y Combo
using System.Data;
using System.Data.SqlClient;
//using System.Data.Objects;
using System.Data.OleDb;
using System.Configuration;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WpfAppAgencia
{
    /// <summary>
    /// Lógica de interacción para frmParamEstCivil.xaml
    /// </summary>
    public partial class frmParamEstCivil : Window
    {
        int idEstCiv = 0;
        string Acc = "S";

        SqlCommand comando = new SqlCommand();
        SqlConnection conexion = new SqlConnection();
        SqlDataAdapter adaptador = new SqlDataAdapter();
        DataSet ds = new DataSet();

        public frmParamEstCivil()
        {
            InitializeComponent();
            this.cargaCboEstCiv();
            this.muestraBotones();
            this.gridEstCivil.IsEnabled = false;
            this.lblCodigo.Visibility = Visibility.Hidden;
            this.txtCodigo.Visibility = Visibility.Hidden;
        }
        private void cargaCboEstCiv()
        {
            cboEstadoCivil.ItemsSource = null;
            DataSet ds = getData("Select * From EstadoCivil  Order By EstadoCivil", "EstadoCivil");
            DataTable dt = ds.Tables[0];
            this.cboEstadoCivil.ItemsSource = ((IListSource)dt).GetList();
            this.cboEstadoCivil.DisplayMemberPath = "EstadoCivil";
            this.cboEstadoCivil.SelectedValue = "IdEStadoCivil";
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
        private void limpFormEStCiv()
        {
            this.txtCodigo.Text = string.Empty;
            this.txtEstCivil.Text = string.Empty;
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

        private void CargaEstCivil(int param)
        {
            DataSet ds = getData("Select * From EstadoCivil Where IdEstadoCivil=" + param, "EstadoCivil");
            DataTable dt = ds.Tables[0];
            this.txtCodigo.Text = dt.Rows[0]["IdEstadoCivil"].ToString();
            this.txtEstCivil.Text = dt.Rows[0]["EstadoCivil"].ToString().ToUpper();
        }
        private void cboEstadoCivil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.cboEstadoCivil.SelectedItem != null)
                {
                    idEstCiv = Convert.ToInt32(((DataRowView)cboEstadoCivil.SelectedItem)["IdEStadoCivil"]);
                    CargaEstCivil(idEstCiv);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se Ha Producido Un Error: " + Ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtEstCivil_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (this.txtEstCivil.Text != string.Empty)
                    this.btnGrabar.Focus();
                else
                    MessageBox.Show("El Estado Civil No puede ser Nulo", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void btnAlta_Click(object sender, RoutedEventArgs e)
        {
            Acc = "A";
            this.cboEstadoCivil.Text = string.Empty;
            this.cboEstadoCivil.IsEnabled = false;
            this.limpFormEStCiv();
            this.ocultaBotones();
            this.gridEstCivil.IsEnabled = true;
            txtEstCivil.Focus();
        }

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.txtEstCivil.Text != string.Empty)
                {
                    string CadenaStr1 = ConfigurationManager.ConnectionStrings["CadConexion"].ConnectionString; //Obtiene la Cadena de Conexion de app.config
                    SqlConnection conn = new SqlConnection(CadenaStr1);
                    SqlCommand cmd = new SqlCommand();

                    cmd.CommandText = "sp_ABM_EstadoCivil";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@xIdEstadoCivil", SqlDbType.Int);
                    cmd.Parameters["@xIdEstadoCivil"].Value = idEstCiv;

                    cmd.Parameters.Add("@EstadoCivil", SqlDbType.VarChar);
                    cmd.Parameters["@EstadoCivil"].Value = txtEstCivil.Text.Trim();

                    cmd.Parameters.Add("@Accion", SqlDbType.VarChar);
                    cmd.Parameters["@Accion"].Value = Acc;

                    cmd.Connection = conn;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    btnCancelar_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Debe Ingresar una Descripción", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Se Ha Producido Un Error: " + Ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBaja_Click(object sender, RoutedEventArgs e)
        {
            if (this.cboEstadoCivil.Text!=string.Empty)
            {
                Acc="B";
                this.cboEstadoCivil.IsEnabled = false;
                this.ocultaBotones();
                this.gridEstCivil.IsEnabled = false;
                this.btnGrabar.Focus();
            }
            else
                MessageBox.Show("Debe Elegir Un Estado Civil Para Borrar","Mensaje",MessageBoxButton.OK,MessageBoxImage.Error);
        }

        private void btnModifica_Click(object sender, RoutedEventArgs e)
        {
            if (this.cboEstadoCivil.Text != string.Empty)
            {
                Acc = "M";
                this.cboEstadoCivil.IsEnabled = false;
                this.ocultaBotones();
                this.gridEstCivil.IsEnabled = true;
                this.txtEstCivil.Focus();
            }
            else
                MessageBox.Show("Debe Elegir Un Estado Civil Para Modificar", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.cboEstadoCivil.Text = string.Empty;
            this.cboEstadoCivil.IsEnabled = true;
            this.gridEstCivil.IsEnabled = false;
            this.limpFormEStCiv();
            this.muestraBotones();
            this.cboEstadoCivil.Focus();
        }

    }
}
