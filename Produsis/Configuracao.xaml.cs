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
using BLL;
using WPFFolderBrowser;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Configuracao.xaml
    /// </summary>
    public partial class Configuracao : UserControl
    {
        DocumentosBLL d = new DocumentosBLL();
        public Configuracao()
        {
            InitializeComponent();
            CaminhoPastaNFs.Text = d.getPastaNFs();
            CaminhoPastaMan.Text = d.getPastaManifestos();
        }

        private void AlteraNF_Click(object sender, RoutedEventArgs e)
        {
            var dialogo = new WPFFolderBrowserDialog("Pasta de Notas Fiscais");
            dialogo.InitialDirectory = CaminhoPastaNFs.Text;
            if(dialogo.ShowDialog().Value)
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
        }
    }
}
