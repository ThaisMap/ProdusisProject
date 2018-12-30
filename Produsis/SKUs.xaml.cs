using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Lógica interna para SKUs.xaml
    /// </summary>
    public partial class SKUs : Window
    {
        private int volumes;
        private int sku;

        public SKUs(int volumes)
        {
            InitializeComponent();
        }

        public static int Perguntar(int volumes)
        {
            SKUs tela = new SKUs(volumes);
            tela.ShowDialog();

            if (tela.DialogResult == true)
                return tela.sku;
           
            return 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtQtde.Focus();
        }

        private void TestarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            sku = int.Parse(txtQtde.Text);
            if (sku < 1)
                MessageBox.Show("A quantidade de SKU's deve ser maior ou igual a 1.", "Erro - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
            else 
            if (sku > volumes)
                MessageBox.Show("A quantidade de SKU's deve ser menor que a quantidade de volumes.", "Erro - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                DialogResult = true;
                Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
