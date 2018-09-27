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
            Navegador.Navigate(new IniciadasHoje());
        }

        public Administrativo(double actualHeight, double actualWidth)
        {
            InitializeComponent();
            Navegador.Navigate(new IniciadasHoje());
            Height = actualHeight;
            Width = actualWidth;
        }

        private void ResetarOpacidade(Button b)
        {
            BtnConfiguracoes.Opacity = 1;
            BtnDocumentos.Opacity = 1;
            BtnFuncionarios.Opacity = 1;
            BtnProdutividade.Opacity = 1;
            BtnRelatorios.Opacity = 1;
            BtnTarefasInitHoje.Opacity = 1;
            b.Opacity = 0.5;
        }

        private void BtnFuncionarios_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new NavFuncionarios());
            ResetarOpacidade(sender as Button);
        }

        private void BtnDocumentos_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new PesquisarDocumento());
            ResetarOpacidade(sender as Button);
        }

       
        private void BtnRelatorios_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Relatorios(ActualHeight, ActualWidth));
            ResetarOpacidade(sender as Button);
        }

        private void BtnConfiguracoes_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Configuracao());
            ResetarOpacidade(sender as Button);
        }

        private void BtnTarefasInitHoje_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new IniciadasHoje());
            ResetarOpacidade(sender as Button);
        }

        private void BtnProdutividade_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Produtividade(ActualHeight, ActualWidth));
            ResetarOpacidade(sender as Button);
        }

        private void Temas_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new MudarTema(ActualHeight, ActualWidth));
            ResetarOpacidade(sender as Button);
        }
    }
}