using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Produsis;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interação lógica para NavFuncionarios.xam
    /// </summary>
    public partial class NavFuncionarios : UserControl
    {
        public NavFuncionarios()
        {
            InitializeComponent();
        }

        private void BtnCadFuncionarios_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new CadastroFuncionarios());
        }

        private void BtnEditarFuncionarios_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new EdicaoFuncionarios());
        }

        private void BtnObservacoes_Click(object sender, RoutedEventArgs e)
        {
            NavegadorInterno.Navigate(new Observacao());
        }
    }
}
