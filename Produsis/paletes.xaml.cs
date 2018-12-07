using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Lógica interna para paletes.xaml
    /// </summary>
    public partial class paletes : Window
    {
        private string total;

        public paletes(string capacidade)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
            txtTotal.Text = capacidade;
            total = capacidade;
            if (capacidade == "0")
                txtTotal.IsReadOnly = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtQtde.Focus();
        }

        public static int[] Perguntar(string capacidade)
        {
            paletes tela = new paletes(capacidade);
            tela.ShowDialog();

            if (tela.DialogResult == true)
                try
                {
                    int[] retornoOK = { int.Parse(tela.txtQtde.Text), int.Parse(tela.txtTotal.Text) };
                    return retornoOK;
                }
                catch
                {
                    int[] retornoErro = { -1, 1 };
                    return retornoErro;
                }
            int[] retorno = { -1, -1 };
            return retorno;
        }

        private void TestarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            double p = 101;
            if (txtQtde.Text != "" && txtTotal.Text != "0" && txtTotal.Text != "")
                p = int.Parse(txtQtde.Text) / double.Parse(txtTotal.Text);

            if (txtQtde.Text == "")
                txtQtde.Focus();
            else
            if (txtTotal.Text == "" || txtTotal.Text == "0")
                txtTotal.Focus();
            else
            {
                if (p > 1)
                    MessageBox.Show("A quantidade paletizada deve ser menor ou igual ao total.", "Erro - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CalcularPorcentagem(object sender, TextChangedEventArgs e)
        {
            txtQtde.Text = txtQtde.Text.Replace(" ", string.Empty);
            if (txtQtde.Text != "" && txtQtde.Text != " " && txtTotal.Text != "0" && txtTotal.Text != "")
            {
                var x = int.Parse(txtQtde.Text) / double.Parse(txtTotal.Text);
                txtPorcentagem.Text = string.Format("{0:P2}", x);
            }
            else
                txtPorcentagem.Text = "0,00%";
            if (total == "0")
            {
                txtTotal.Text = txtQtde.Text;
            }
        }
    }
}