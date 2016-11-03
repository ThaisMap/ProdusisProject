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
using Produsis;

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
        }

        private void BtnCadFuncionarios_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new CadastroFuncionarios());
        }

        private void BtnDocumentos_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new PesquisarDocumento());
        }

        private void BtnEditarFuncionarios_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new EdicaoFuncionarios());
        }

        private void BtnConfigurações_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new Configuracao());
        }
    }
}
