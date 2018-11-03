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
using ProdusisBD;
using DAL;
using System.Text.RegularExpressions;

namespace Portaria
{
    /// <summary>
    /// Interação lógica para Entrada.xam
    /// </summary>
    public partial class Entrada : UserControl
    {
        public List<Veiculos> veiculos { get; set; }
        private VeiculosBD veiculosBD = new VeiculosBD();

        public Entrada(double WindowHeight, double WindowsWidth)
        {
            InitializeComponent();
            Height = WindowHeight - 60;
            Width = WindowsWidth - 60;
           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            veiculos = veiculosBD.getVeiculos();
            cbNome.ItemsSource = veiculos;
            cbPlaca.ItemsSource = veiculos;
            cbPlaca.DisplayMemberPath = "PlacaVeiculo";
            cbNome.DisplayMemberPath = "MotoristaVeiculo";           
        }

        private void cbNome_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbPlaca.Text = (cbNome.SelectedItem as Veiculos).PlacaVeiculo;
        }

        private void cbPlaca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbNome.Text = (cbPlaca.SelectedItem as Veiculos).MotoristaVeiculo;
            txtPlaca2.Text = (cbPlaca.SelectedItem as Veiculos).Placa2Veiculo;
        }

        private void BtnRegistra_Click(object sender, RoutedEventArgs e)
        {
            if (ChecarCampos())
            {
               if( veiculosBD.cadastrarAcessoPortaria(montarObjeto()))
                {
                    MessageBox.Show("Entrada de veículo registrada.");
                }
            }
        }

        private void Limpar()
        {
            cbNome.SelectedIndex = -1;
            cbPlaca.SelectedIndex = -1;
            txtKm.Text = "";
            txtLacre.Text = "";
            txtPlaca2.Text = "";
            checkColeta.IsChecked = false;
            checkDevolucao.IsChecked = false;
            checkPalete.IsChecked = false;
        }
        
        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool ChecarCampos()
        {
            if(cbPlaca.SelectedIndex <= -1)
            {
                cbPlaca.Focus();
                return false;
            }

            if(txtKm.Text == "")
            {
                txtKm.Focus();
                return false;
            }

            return true;
        }

        private AcessosPortaria montarObjeto()
        {
            string obs = "";
            if ((bool)checkPalete.IsChecked)
            {
                obs = checkPalete.Content.ToString()+"/";
            }

            if ((bool)checkDevolucao.IsChecked)
            {
                obs = checkDevolucao.Content.ToString() + "/";
            }

            if ((bool)checkColeta.IsChecked)
            {
                obs = checkColeta.Content.ToString();
            }

            AcessosPortaria acessos = new AcessosPortaria()
            {
                EntradaAcesso = DateTime.Now,
                KmAcesso = int.Parse(txtKm.Text),
                LacreAcesso = txtLacre.Text,
                ObservacaoAcesso = obs,
                NomeMotoristaAcesso = cbNome.Text,
                Placa2Acesso = txtPlaca2.Text,
                PlacaAcesso = cbPlaca.Text,
                EstadoGeral = txtEstado.Text,
                PorteiroEntrada = Properties.Login.Default.idUsuario
            };
            return acessos;
        }
    }
}
