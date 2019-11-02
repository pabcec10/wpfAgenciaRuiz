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
using System.Data.OleDb;
using System.Configuration;
using System.ComponentModel;
using System.Xml;

namespace WpfAppAgencia
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int xIdUsuario = 0;
        public static int xTipoUsuario = 0;
        public MainWindow()
        {
            InitializeComponent();
            txtUsuario.Focus();
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
            Close();
        }
        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (string.Equals(this.txtUsuario.Text, "") == false)
                    this.txtPassWord.Focus();
                else
                    MessageBox.Show("El Usuario No puede ser Blanco", "Mensaje");
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (string.Equals(this.txtPassWord.Password, "") == false)
                    this.btnAceptar.Focus();
                else
                    MessageBox.Show("La Clave No puede ser Blanco", "Mensaje");
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DataSet ds = getData("Select * From Config_Usuarios Where NombUser='" + this.txtUsuario.Text + "' And Pass_Word='" + this.txtPassWord.Password + "'", "Config_Usuarios");
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count != 0)
            {
                xIdUsuario = Convert.ToInt32(dt.Rows[0]["IdUser"].ToString().Trim());
                xTipoUsuario = Convert.ToInt32(dt.Rows[0]["Administrador"].ToString().Trim());
                frmMenuPpal f = new frmMenuPpal();
                f.Title = "Menú Principal - Usuario: " + dt.Rows[0]["ApellidoNombre"].ToString().Trim();
                f.Show();
            }
            else
                MessageBox.Show("Usuario invalido", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
    }
}
