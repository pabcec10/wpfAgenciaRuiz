﻿using System;
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
    /// Lógica de interacción para frmParamMarcas.xaml
    /// </summary>
    public partial class frmParamMarcas : Window
    {
        int IdFab = 0;
        int IdMarc = 0;
        char Acc = 'Z';
        public frmParamMarcas()
        {
            InitializeComponent();
            cargaCboFabrica();
            muestraBotones();
            gridMarca.IsEnabled=false;
            cargaCboMarca(0);
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
        private void cargaCboMarca(int xIdFab)
        {
            cboMarca.ItemsSource = null;
            DataSet ds = getData("Select * From Marca Where IdFabrica="+ xIdFab +" Order By Marca", "Marca");
            DataTable dt = ds.Tables[0];
            cboMarca.ItemsSource = ((IListSource)dt).GetList();
            cboMarca.DisplayMemberPath = "Marca";
            cboMarca.SelectedValue = "IdMarca";
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
        private void cboFabrica_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboFabrica.SelectedItem != null)
            {
                IdFab = Convert.ToInt32(((DataRowView)cboFabrica.SelectedItem)["IdFabrica"]);
                cargaCboMarca(IdFab);
            }
        }
        private void CargaMarca(int param)
        {
            DataSet ds = getData("Select * From Marca Where IdMarca=" + param, "Marca");
            DataTable dt = ds.Tables[0];
            txtCodigo.Text = dt.Rows[0]["IdMarca"].ToString();
            txtMarca.Text = dt.Rows[0]["Marca"].ToString().ToUpper().Trim();
        }
        private void cboMarca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboMarca.SelectedItem != null)
            {
                IdMarc = Convert.ToInt32(((DataRowView)cboMarca.SelectedItem)["IdMarca"]);
                CargaMarca(IdMarc);
            }
        }
        private void LimpForm()
        {
            txtCodigo.Text = string.Empty;
            txtMarca.Text = string.Empty;
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAlta_Click(object sender, RoutedEventArgs e)
        {
            if (cboFabrica.Text!=string.Empty)
            {
                Acc = 'A';
                cboFabrica.IsEnabled = false;
                cboMarca.Text = string.Empty;
                cboMarca.IsEnabled = false;
                LimpForm();
                gridMarca.IsEnabled = true;
                ocultaBotones();
                txtMarca.Focus();
            }
            else
            {
                MessageBox.Show("Debe Seleccionar una Fábrica para dar de Alta a una Marca", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBaja_Click(object sender, RoutedEventArgs e)
        {
            if (cboMarca.Text != string.Empty)
            {
                Acc = 'B';
                cboFabrica.IsEnabled = false;
                cboMarca.IsEnabled = false;
                ocultaBotones();
                btnGrabar.Focus();
            }
            else
            {
                MessageBox.Show("Debe Seleccionar una Marca para dar de Baja", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnModifica_Click(object sender, RoutedEventArgs e)
        {
            if (cboMarca.Text != string.Empty)
            {
                Acc = 'M';
                cboFabrica.IsEnabled = false;
                cboMarca.IsEnabled = false;
                gridMarca.IsEnabled = true;
                ocultaBotones();
                txtMarca.Focus();
            }
            else
            {
                MessageBox.Show("Debe Seleccionar una Marca para Modificar", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void grabaMarca()
        {
            string CadenaStr1 = ConfigurationManager.ConnectionStrings["CadConexion"].ConnectionString; //Obtiene la Cadena de Conexion de app.config
            SqlConnection conn = new SqlConnection(CadenaStr1);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_ABM_Marca";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@xIdMarca", SqlDbType.Int);
            cmd.Parameters["@xIdMarca"].Value = IdMarc;

            cmd.Parameters.Add("@xIdFabrica", SqlDbType.Int);
            cmd.Parameters["@xIdFabrica"].Value = IdFab;

            cmd.Parameters.Add("@xMarca", SqlDbType.VarChar);
            cmd.Parameters["@xMarca"].Value = txtMarca.Text.Trim();

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
            if (txtMarca.Text != string.Empty)
                grabaMarca();
            else
                MessageBox.Show("Faltan datos para Grabar la Marca", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpForm();
            muestraBotones();
            cargaCboMarca(IdFab);
            gridMarca.IsEnabled = false;
            cboMarca.Text = string.Empty;
            cboFabrica.IsEnabled = true;
            cboMarca.IsEnabled = true;

        }
    }
}
