using System.Windows;

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
            Conteudo.Content = new Entrada();
        }

        private void BtnPendentes_Click(object sender, RoutedEventArgs e)
        {
            BtnEntrada.Opacity = 1;
            BtnPendentes.Opacity = 0.5;
            Conteudo.Content = new Pendentes();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Entrada();
        }
    }
}