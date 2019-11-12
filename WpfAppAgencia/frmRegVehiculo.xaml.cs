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

        public frmRegVehiculo()
        {
            InitializeComponent();
            cargaCboMarca();
            cargaCboModelo(0);
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
    }
}
