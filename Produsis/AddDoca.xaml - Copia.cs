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
using DAL;
using ProdusisBD;

namespace GUI
{
    /// <summary>
    /// Lógica interna para AddDoca.xaml
    /// </summary>
    public partial class AddDoca : Window
    {
        public int idAcesso { get; set; }
        public AddDoca()
        {
            InitializeComponent();
        }

        public static void registrarDoca(int id, string placa)
        {
            AddDoca doca = new AddDoca();
            doca.idAcesso = id;
            doca.txtPlaca.Text = placa;
            doca.ShowDialog();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (cbDoca.SelectedIndex > -1)
            {
                VeiculosBD vBD = new VeiculosBD();
                vBD.RegistrarDoca(idAcesso, cbDoca.SelectedIndex + 1);
                this.Close();
            }
            else
                cbDoca.Focus();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
