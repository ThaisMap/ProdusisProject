using DAL;
using ProdusisBD;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Portaria
{
    /// <summary>
    /// Interação lógica para Pendentes.xam
    /// </summary>
    public partial class Pendentes : UserControl
    {

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
            AcessoBD abd = new AcessoBD();
            List<AcessosPortaria> lista = abd.GetAcessosPendentes();
            dgPendentes.ItemsSource = lista;
        }
    }
}