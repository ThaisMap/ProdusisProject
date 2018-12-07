using DAL;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Lógica interna para AddDoca.xaml
    /// </summary>
    public partial class AddDoca : Window
    {
        public int idAcesso { get; set; }

        public AddDoca()
        {
            InitializeComponent();
        }

        public static void RegistrarDoca(int id, string placa)
        {
            AddDoca doca = new AddDoca();
            doca.idAcesso = id;
            doca.txtPlaca.Text = placa;
            doca.ShowDialog();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (cbDoca.SelectedIndex > -1)
            {
                AcessoBD abd = new AcessoBD();
                abd.RegistrarDoca(idAcesso, cbDoca.SelectedIndex + 1);
                this.Close();
            }
            else
                cbDoca.Focus();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}