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
    /// Lógica de interacción para frmRegFabrica.xaml
    /// </summary>
    public partial class frmRegFabrica : Window
    {
        int IdFab = 0;
        char Acc = 'Z';
        public frmRegFabrica()
        {
            InitializeComponent();
            cargaCboFabrica();
            muestraBotones();
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
        private void cargaCboFabrica()
        {
            cboFabrica.ItemsSource = null;
            DataSet ds = getData("Select * From Fabrica  Order By Fabrica", "Fabrica");
            DataTable dt = ds.Tables[0];
            cboFabrica.ItemsSource = ((IListSource)dt).GetList();
            cboFabrica.DisplayMemberPath = "Fabrica";
            cboFabrica.SelectedValue = "IdFabrica";
        }
        private void ocultaBotones()
        {
            btnAlta.Visibility = Visibility.Hidden;
            btnBaja.Visibility = Visibility.Hidden;
            btnModifica.Visibility = Visibility.Hidden;
            btnSalir.Visibility = Visibility.Hidden;
            btnCancelar.Visibility = Visibility.Visible;
            btnGrabar.Visibility = Visibility.Visible;
        }
        private void muestraBotones()
        {
            btnAlta.Visibility = Visibility.Visible;
            btnBaja.Visibility = Visibility.Visible;
            btnModifica.Visibility = Visibility.Visible;
            btnSalir.Visibility = Visibility.Visible;
            btnCancelar.Visibility = Visibility.Hidden;
            btnGrabar.Visibility = Visibility.Hidden;
        }
        private void LimpForm()
        {
            txtFabrica.Text = string.Empty;
        }
        private void CargaFabrica(int param)
        {
            DataSet ds = getData("Select * From Fabrica Where IdFabrica=" + param, "Fabrica");
            DataTable dt = ds.Tables[0];
            txtCodigo.Text = dt.Rows[0]["IdFabrica"].ToString();
            txtFabrica.Text = dt.Rows[0]["Fabrica"].ToString().ToUpper().Trim();
        }
        private void cboFabrica_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboFabrica.SelectedItem != null)
            {
                IdFab = Convert.ToInt32(((DataRowView)cboFabrica.SelectedItem)["IdFabrica"]);
                CargaFabrica(IdFab);
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAlta_Click(object sender, RoutedEventArgs e)
        {
            Acc = 'A';
            cboFabrica.Text = string.Empty;
            cboFabrica.IsEnabled = false;
            LimpForm();
            ocultaBotones();
            gridFabrica.IsEnabled = true;
            txtFabrica.Focus();
        }

        private void btnBaja_Click(object sender, RoutedEventArgs e)
        {
            if (cboFabrica.Text != string.Empty)
            {
                Acc = 'B';
                cboFabrica.IsEnabled = false;
                ocultaBotones();
                gridFabrica.IsEnabled = false;
                btnGrabar.Focus();
            }
            else
                MessageBox.Show("Debe Elegir Una Fábrica Para Borrar", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void btnModifica_Click(object sender, RoutedEventArgs e)
        {
            if (cboFabrica.Text != string.Empty)
            {
                Acc = 'M';
                cboFabrica.IsEnabled = false;
                ocultaBotones();
                gridFabrica.IsEnabled = true;
                txtFabrica.Focus();
            }
            else
                MessageBox.Show("Debe Elegir Una Fábrica Para Modificar", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            cboFabrica.Text = string.Empty;
            cboFabrica.IsEnabled = true;
            gridFabrica.IsEnabled = false;
            LimpForm();
            muestraBotones();
            cboFabrica.Focus();
        }
        private void cargaFabricas()
        {
            string CadenaStr1 = ConfigurationManager.ConnectionStrings["CadConexion"].ConnectionString; //Obtiene la Cadena de Conexion de app.config
            SqlConnection conn = new SqlConnection(CadenaStr1);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_ABM_Fabricas";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@xIdFabrica", SqlDbType.Int);
            cmd.Parameters["@xIdFabrica"].Value = IdFab;

            cmd.Parameters.Add("@xFabrica", SqlDbType.VarChar);
            cmd.Parameters["@xFabrica"].Value = txtFabrica.Text.Trim();

            cmd.Parameters.Add("@xAcc", SqlDbType.VarChar);
            cmd.Parameters["@xAcc"].Value = Acc;

            cmd.Connection = conn;

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            btnCancelar_Click(null, null);
        }
        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            cargaFabricas();
        }
    }
}
