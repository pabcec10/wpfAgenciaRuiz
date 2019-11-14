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
    /// Lógica de interacción para frmRegVehiculo.xaml
    /// </summary>
    public partial class frmRegVehiculo : Window
    {
        int IdMod = 0;
        int IdMarc = 0;
        int IdProc = 0;
        int IdUso = 0;
        int xIdTipo = 0;

        public frmRegVehiculo()
        {
            InitializeComponent();
            cargaCboMarca();
            cargaCboModelo(0);
            cargaCboProcedencia();
            cargaCboUso();
            cargaCboTipo();
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
        private void cargaCboMarca()
        {
            cboMarca.ItemsSource = null;
            DataSet ds = getData("Select * From Marca  Order By Marca", "Marca");
            DataTable dt = ds.Tables[0];
            cboMarca.ItemsSource = ((IListSource)dt).GetList();
            cboMarca.DisplayMemberPath = "Marca";
            cboMarca.SelectedValue = "IdMarca";
        }
        private void cargaCboModelo(int xIdMarca)
        {
            cboModelo.ItemsSource = null;
            DataSet ds = getData("Select * From Modelo Where IdMarca="+ xIdMarca +"  Order By Modelo", "Modelo");
            DataTable dt = ds.Tables[0];
            cboModelo.ItemsSource = ((IListSource)dt).GetList();
            cboModelo.DisplayMemberPath = "Modelo";
            cboModelo.SelectedValue = "IdModelo";
        }

        private void cboMarca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboMarca.SelectedItem != null)
            {
                IdMarc = Convert.ToInt32(((DataRowView)cboMarca.SelectedItem)["IdMarca"]);
                cargaCboModelo(IdMarc);
                cboModelo.Focus();
            }
        }

        private void cboModelo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboModelo.SelectedItem != null)
            {
                IdMod = Convert.ToInt32(((DataRowView)cboModelo.SelectedItem)["IdModelo"]);
                txtAnio.Focus();
            }
        }
        private void cargaCboProcedencia()
        {
            cboProcedencia.ItemsSource = null;
            DataSet ds = getData("Select * From Procedencia  Order By Procedencia", "Procedencia");
            DataTable dt = ds.Tables[0];
            cboProcedencia.ItemsSource = ((IListSource)dt).GetList();
            cboProcedencia.DisplayMemberPath = "Procedencia";
            cboProcedencia.SelectedValue = "IdProcedencia";
        }
        private void cargaCboUso()
        {
            cboUso.ItemsSource = null;
            DataSet ds = getData("Select * From AutoUso  Order By Uso", "AutoUso");
            DataTable dt = ds.Tables[0];
            cboUso.ItemsSource = ((IListSource)dt).GetList();
            cboUso.DisplayMemberPath = "Uso";
            cboUso.SelectedValue = "IdUso";
        }
        private void cargaCboTipo()
        {
            cboTipo.ItemsSource = null;
            DataSet ds = getData("Select * From TipoAutomotores  Order By TipoAutomotor", "TipoAutomotores");
            DataTable dt = ds.Tables[0];
            cboTipo.ItemsSource = ((IListSource)dt).GetList();
            cboTipo.DisplayMemberPath = "TipoAutomotor";
            cboTipo.SelectedValue = "IdTipoAutomotor";
        }
        private void cboProcedencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboProcedencia.SelectedItem != null)
            {
                IdProc = Convert.ToInt32(((DataRowView)cboProcedencia.SelectedItem)["IdProcedencia"]);
                dateInscripcion.Focus();
            }
        }

        private void cboUso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboUso.SelectedItem != null)
            {
                IdUso = Convert.ToInt32(((DataRowView)cboUso.SelectedItem)["IdUso"]);
                txtCantPlacas.Focus();
            }
        }

        private void cboTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTipo.SelectedItem != null)
            {
                xIdTipo = Convert.ToInt32(((DataRowView)cboTipo.SelectedItem)["IdTipo"]);
                txtMarcaChasis.Focus();
            }
        }

        private void txtDominio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (txtDominio.Text != string.Empty)
                    cboMarca.Focus();
                else
                    MessageBox.Show("El Dominio no puede ser Nulo", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void txtAnio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (txtAnio.Text != string.Empty)
                    cboProcedencia.Focus();
                else
                    MessageBox.Show("El Año no puede ser Nulo", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void dateInscripcion_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateInscripcion.Text != string.Empty)
                txtCodAutomotor.Focus();
        }

        private void txtMarcaMotor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtNroMotor.Focus();
        }

        private void txtNroMotor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cboTipo.Focus();
        }

        private void txtMarcaChasis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtNroChasis.Focus();
        }

        private void txtNroChasis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtPeso.Focus();
        }

        private void dateAdquisicion_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateAdquisicion.Text != string.Empty)
                txtCarroceria.Focus();
        }

        private void txtCarroceria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtCondicion.Focus();
        }

        private void txtPeso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtCarga.Focus();
        }

        private void txtCarga_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                dateAdquisicion.Focus();
        }

        private void txtCondicion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cboUso.Focus();
        }
    }
}
