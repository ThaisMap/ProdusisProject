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

namespace Portaria
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BtnEntrada.Opacity = 0.5;
            txtUsuario.Text = Properties.Login.Default.NomeUsuario;
        }

        private void BtnEntrada_Click(object sender, RoutedEventArgs e)
        {
            BtnEntrada.Opacity = 0.5;
            BtnPendentes.Opacity = 1;
            Navegador.Navigate(new Entrada(ActualHeight, ActualWidth));
        }       

        private void BtnPendentes_Click(object sender, RoutedEventArgs e)
        {
            BtnEntrada.Opacity = 1;
            BtnPendentes.Opacity = 0.5;
            Navegador.Navigate(new Pendentes(ActualHeight, ActualWidth));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Entrada(ActualHeight, ActualWidth));
        }
    }
}
