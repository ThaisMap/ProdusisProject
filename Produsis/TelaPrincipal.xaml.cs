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
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           Navegador.Navigate(new Conferencia());
        }

        public void ResetarOpacidade()
        {
            BtnAdministrativo.Opacity = 1;
            BtnCarregamento.Opacity = 1;
            BtnConferencia.Opacity = 1;
            BtnDescarga.Opacity = 1;
            BtnSeparacaoCarga.Opacity = 1;
            BtnDivergenciaN.Opacity = 1;
            BtnEmpilhadeira.Opacity = 1;
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
        }

        private void BtnCarregamento_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnCarregamento.Opacity = 0.5;
            Navegador.Navigate(new Carregamento());
        }

        private void BtnSeparacaoCarga_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnSeparacaoCarga.Opacity = 0.5;
            Navegador.Navigate(new Movimentacao());
        }

        private void BtnConferencia_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnConferencia.Opacity = 0.5;
            Navegador.Navigate(new Conferencia());
        }       
        
        private void BtnPesquisaDocs_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new PesquisarDocumento());
        }

        private void BtnRelatorios_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Relatorios());
        }

        private void BtnPastas_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Configuracao());
        }

        private void BtnProdutividade_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new Produtividade());
        }

        private void BtnTemas_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new MudarTema());
        }

        private void BtnCadFunc_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new CadastroFuncionarios());
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
            Navegador.Navigate(new CadastroMotorista());
        }

        private void BtnDivergenciaN_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            Navegador.Navigate(new DivergenciasNovo());
            BtnDivergenciaN.Opacity = 0.5;
        }

        private void BtnEmpilhadeira_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            Navegador.Navigate(new Empilhadeira());
            BtnEmpilhadeira.Opacity = 0.5;
        }

        private void BtnPortaria_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new RelatorioPortaria());
        }

        private void BtnCarretas_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new CadastroCarretas());
        }

        private void BtnDetalheManif_Click(object sender, RoutedEventArgs e)
        {
            Navegador.Navigate(new DetalheManifesto());
        }
    }
}