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
    /// Lógica interna para paletes.xaml
    /// </summary>
    public partial class paletes : Window
    {      
        public paletes(string capacidade)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);            
            txtTotal.Text = capacidade;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtQtde.Focus();
        }

        public static float Perguntar(string capacidade)
        {
            paletes inst = new paletes(capacidade);
            inst.ShowDialog();

            if (inst.DialogResult == true)
                try
                {
                   return int.Parse(inst.txtQtde.Text) / float.Parse(inst.txtTotal.Text);
                }
                catch
                {
                    return 0;
                }
            return -1;
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);            
        }    

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void calcularPorcentagem(object sender, TextChangedEventArgs e)
        {
            if (txtQtde.Text != "" && txtTotal.Text != "0" && txtTotal.Text != "")
            {
                var x = int.Parse(txtQtde.Text) / double.Parse(txtTotal.Text);
                txtPorcentagem.Text = string.Format("{0:P2}", x);
            }
            else
                txtPorcentagem.Text = "0,00%";
        }
    }
}
