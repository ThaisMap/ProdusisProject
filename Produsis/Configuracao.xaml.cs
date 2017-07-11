using BLL;
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
        private DocumentosBLL d = new DocumentosBLL();

        public Configuracao()
        {
            InitializeComponent();
            CaminhoPastaNFs.Text = d.getPastaNFs();
            CaminhoPastaMan.Text = d.getPastaManifestos();
            CaminhoPastaPreMan.Text = d.getPastaPreManifestos();
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
            d.setPastasNF(CaminhoPastaNFs.Text);
            d.setPastasManifesto(CaminhoPastaMan.Text);
            d.setPastasPreManifesto(CaminhoPastaPreMan.Text);
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