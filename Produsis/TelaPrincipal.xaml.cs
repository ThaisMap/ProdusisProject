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
           Conteudo.Content = new Conferencia();
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
            Conteudo.Content = new Descarga();
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
            Conteudo.Content = new Carregamento();
        }

        private void BtnSeparacaoCarga_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnSeparacaoCarga.Opacity = 0.5;
            Conteudo.Content = new Movimentacao();
        }

        private void BtnConferencia_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            BtnConferencia.Opacity = 0.5;
            Conteudo.Content = new Conferencia();
        }       
        
        private void BtnPesquisaDocs_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new PesquisarDocumento();
        }

        private void BtnRelatorios_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Relatorios();
        }

        private void BtnPastas_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Configuracao();
        }

        private void BtnProdutividade_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Produtividade();
        }

        private void BtnTemas_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new MudarTema();
        }

        private void BtnCadFunc_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new CadastroFuncionarios();
        }       

        private void BtnObserv_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Observacao();
        }

        private void BtnEquipe_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new Equipes();
        }

        private void BtnMotorista_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new CadastroMotorista();
        }

        private void BtnDivergenciaN_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            Conteudo.Content = new DivergenciasNovo();
            BtnDivergenciaN.Opacity = 0.5;
        }

        private void BtnEmpilhadeira_Click(object sender, RoutedEventArgs e)
        {
            ResetarOpacidade();
            Conteudo.Content = new Empilhadeira();
            BtnEmpilhadeira.Opacity = 0.5;
        }

        private void BtnPortaria_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new RelatorioPortaria();
        }

        private void BtnCarretas_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new CadastroCarretas();
        }

        private void BtnDetalheManif_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = new DetalheManifesto();
        }
    }
}