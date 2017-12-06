using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for TelaPrincipal.xaml
    /// </summary>
    public partial class TelaPrincipal : Window
    {
        public TelaPrincipal()
        {
            InitializeComponent();
            BtnConferencia.Opacity = 0.5;
        }

        public void ResetarOpacidade()
        {
            BtnAdministrativo.Opacity = 1;
            BtnCarregamento.Opacity = 1;
            BtnConferencia.Opacity = 1;
            BtnDescarga.Opacity = 1;
            BtnSeparacaoCarga.Opacity = 1;
          //  BtnSerapacao.Opacity = 1;
            BtnDivergencia.Opacity = 1;
        }
        #region Botoes

        private void BtnDescarga_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnDescarga.Opacity = 0.5;
            Navegador.Navigate(new Descarga());
        }

        private void BtnAdministrativo_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnAdministrativo.Opacity = 0.5;
            Navegador.Navigate(new Administrativo());
        }

        private void BtnCarregamento_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnCarregamento.Opacity = 0.5;
            Navegador.Navigate(new Carregamento(ActualHeight, ActualWidth));
        }

        private void BtnSeparacaoCarga_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnSeparacaoCarga.Opacity = 0.5;
            Navegador.Navigate(new Separacao2(ActualHeight, ActualWidth));
        }

        private void BtnConferencia_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnConferencia.Opacity = 0.5;
            Navegador.Navigate(new Conferencia(ActualHeight, ActualWidth));
        }

        private void BtnDivergencia_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnDivergencia.Opacity = 0.5;
            Navegador.Navigate(new Divergencia(ActualHeight, ActualWidth));
        }

        #endregion Botoes

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Navegador.Height = ActualHeight - 60;
            Navegador.Width = ActualWidth - 60;
            Navegador.Navigate(new Conferencia(ActualHeight, ActualWidth));
        }
    }
}