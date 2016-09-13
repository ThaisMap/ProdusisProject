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

namespace Produsis
{
    /// <summary>
    /// Interaction logic for Descarga.xaml
    /// </summary>
    public partial class Descarga : UserControl
    {
        public Descarga()
        {
            InitializeComponent();
            ChipEx.Icon = CriaChipIcon(ChipEx.Content.ToString());
            ChipEx2.Icon = CriaChipIcon(ChipEx2.Content.ToString());

        }

        public static string CriaChipIcon(string Nome)
        {
            string[] PrimeirosNomes = new string[7];
            PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }

        private void ChipEx_DeleteClick(object sender, RoutedEventArgs e)
        {
            WPListaDeFuncionarios.Children.Remove(ChipEx);
        }
    }
}
