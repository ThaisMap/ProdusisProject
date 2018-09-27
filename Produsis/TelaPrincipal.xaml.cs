using Produsis;
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
            BtnDivergencia.Opacity = 1;
        }

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Navegador.Height = ActualHeight - 60;
            Navegador.Width = ActualWidth - 60;
            Navegador.Navigate(new Conferencia(ActualHeight, ActualWidth));
        }

        private void BtnPesquisaDocs_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new PesquisarDocumento());
        }

        private void BtnRelatorios_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Relatorios(ActualHeight, ActualWidth));
        }

        private void BtnPastas_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Configuracao());
        }

        private void BtnProdutividade_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Produtividade(ActualHeight, ActualWidth));
        }

        private void BtnTemas_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new MudarTema(ActualHeight, ActualWidth));
        }

        private void BtnCadFunc_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new CadastroFuncionarios());
        }

        private void BtnEditFunc_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new EdicaoFuncionarios());
        }

        private void BtnObserv_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Observacao());
        }

        private void BtnEquipe_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Equipes());
        }

        private void BtnMotorista_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new CadastroMotorista(ActualHeight, ActualWidth));
        }
    }
}