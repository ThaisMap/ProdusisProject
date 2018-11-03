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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAL;
using ProdusisBD;

namespace Portaria
{
    /// <summary>
    /// Interação lógica para Pendentes.xam
    /// </summary>
    public partial class Pendentes : UserControl
    {

        VeiculosBD vBD = new VeiculosBD();
        public Pendentes(double WindowHeight, double WindowsWidth)
        {
            InitializeComponent();
            Height = WindowHeight - 60;
            Width = WindowsWidth - 60;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AcessosPortaria item = (AcessosPortaria)dgPendentes.SelectedItem;
            Saida telaSaida = new Saida();
            telaSaida.registrar(item.idAcesso);            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<AcessosPortaria> lista = vBD.GetAcessosPendentes();
            dgPendentes.ItemsSource = lista;
        }
    }
}
