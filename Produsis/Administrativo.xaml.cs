﻿using Produsis;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Administrativo.xaml
    /// </summary>
    public partial class Administrativo : UserControl
    {
        public Administrativo()
        {
            InitializeComponent();
            NavegadorInterno.Navigate(new IniciadasHoje());
        }

        private void BtnCadFuncionarios_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new CadastroFuncionarios());
        }

        private void BtnDocumentos_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new PesquisarDocumento());
        }

        private void BtnEditarFuncionarios_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new EdicaoFuncionarios());
        }

        private void BtnRelatorios_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new Relatorios());
        }

        private void BtnConfiguracoes_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new Configuracao());
        }

        private void BtnTarefasInitHoje_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new IniciadasHoje());
        }

        private void BtnProdutividade_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new Produtividade());
        }
    }
}