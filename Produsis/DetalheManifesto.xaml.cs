using BLL;
using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Divergencia.xaml
    /// </summary>
    public partial class DetalheManifesto : UserControl
    {
        public DetalheManifesto()
        {
            InitializeComponent();
        }

        public List<TarefaModelo> tarefas = new List<TarefaModelo>();

        private void TestarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            if (Manifesto.Text != "")
            {
                Logica bll = new Logica();

                List<DetalhesManifesto> detalhes = bll.GetDetalheManifestos(int.Parse(Manifesto.Text.ToString()));

                dgDivergencias.ItemsSource = detalhes;                
            }

            Manifesto.Focus();
        }
    }
}