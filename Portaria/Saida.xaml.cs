using DAL;
using ProdusisBD;
using System.Windows;

namespace Portaria
{
    /// <summary>
    /// Interação lógica para Saida.xam
    /// </summary>
    public partial class Saida : Window
    {
        private VeiculosBD vBD = new VeiculosBD();
        private AcessosPortaria AcessoAtual;

        public Saida()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void BtnRegistra_Click(object sender, RoutedEventArgs e)
        {
            if (txtKm.Text != "")
            {
                AcessoAtual.KmAcesso = int.Parse(txtKm.Text);
                AcessoAtual.Placa2Acesso = txtPlaca2.Text == "" ? null : txtPlaca2.Text;
                AcessoAtual.PorteiroSaida = Properties.Login.Default.idUsuario;
                vBD.RegistrarSaída(AcessoAtual);
                this.Close();
            }
            else
                txtKm.Focus();
        }

        public void registrar(int IdAcesso)
        {
            AcessoAtual = vBD.getAcessoPorID(IdAcesso);
            txtPlaca.Text = AcessoAtual.PlacaAcesso;
            txtNome.Text = AcessoAtual.NomeMotoristaAcesso;
            txtPlaca2.Text = AcessoAtual.Placa2Acesso;
            this.ShowDialog();
        }

        private void BtnCancela_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}