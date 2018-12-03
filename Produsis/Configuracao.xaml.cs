using DAL;
using System.Windows;
using System.Windows.Controls;
using WPFFolderBrowser;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Configuracao.xaml
    /// </summary>
    public partial class Configuracao : UserControl
    {
        private AcessoBD abd = new AcessoBD();

        public Configuracao()
        {
            InitializeComponent();
            CaminhoPastaNFs.Text = abd.GetPastaNFs();
            CaminhoPastaMan.Text = abd.GetPastaManifestos();
            CaminhoPastaPreMan.Text = abd.GetPastaPreManifestos();
        }

        private void AlteraNF_Click(object sender, RoutedEventArgs e)
        {
            var dialogo = new WPFFolderBrowserDialog("Pasta de Notas Fiscais");
            dialogo.InitialDirectory = CaminhoPastaNFs.Text;
            if (dialogo.ShowDialog().Value)
                CaminhoPastaNFs.Text = dialogo.FileName;
        }

        private void AlteraMan_Click(object sender, RoutedEventArgs e)
        {
            var dialogo = new WPFFolderBrowserDialog("Pasta de Manifestos");
            dialogo.InitialDirectory = CaminhoPastaMan.Text;
            if (dialogo.ShowDialog().Value)
                CaminhoPastaMan.Text = dialogo.FileName;
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            abd.SetPastasNF(CaminhoPastaNFs.Text);
            abd.SetPastasManifesto(CaminhoPastaMan.Text);
            abd.SetPastasPreManifesto(CaminhoPastaPreMan.Text);
        }

        private void AlteraPreMan_Click(object sender, RoutedEventArgs e)
        {
            var dialogo = new WPFFolderBrowserDialog("Pasta de Pré-Manifestos");
            dialogo.InitialDirectory = CaminhoPastaPreMan.Text;
            if (dialogo.ShowDialog().Value)
                CaminhoPastaPreMan.Text = dialogo.FileName;
        }
    }
}