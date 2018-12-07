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
        public DetalheManifesto(double actualHeight, double actualWidth)
        {
            InitializeComponent();
            Height = actualHeight - 100;
            Width = actualWidth - 60;
            dgDivergencias.Height = actualHeight - 270;
        }

        public List<TarefaModelo> tarefas = new List<TarefaModelo>();
     
       // AcessoBD abd = new AcessoBD();

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              //PREENCHER DATAGRID COM INFORMACOES SOBRE OS CTES DO MANIFESTO
              // QUAIS FORAM IMPORTADOS, QUAIS FORAM CONFERIDOS
            }
            catch 
            {

            }
        }
    }
}