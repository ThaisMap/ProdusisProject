using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Portaria
{
    /// <summary>
    /// Interação lógica para Entrada.xam
    /// </summary>
    public partial class Entrada : UserControl
    {
        public List<Veiculos> veiculos { get; set; }
        public List<Carretas> carretas { get; set; }
        private AcessoBD abd = new AcessoBD();

        public Entrada(double WindowHeight, double WindowsWidth)
        {
            InitializeComponent();
            Height = WindowHeight - 60;
            Width = WindowsWidth - 60;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            veiculos = abd.GetVeiculos();
            carretas = abd.GetPlaca2();
            cbPlaca2.ItemsSource = carretas;
            cbPlaca2.DisplayMemberPath = "PlacaCarreta";
            cbNome.ItemsSource = veiculos.OrderBy(x => x.MotoristaVeiculo);
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
            cbPlaca2.Text = (cbPlaca.SelectedItem as Veiculos).Placa2Veiculo;
        }

        private void BtnRegistra_Click(object sender, RoutedEventArgs e)
        {
            if (ChecarCampos())
            {
                if (abd.CadastrarAcessoPortaria(montarObjeto()))
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
            cbPlaca2.SelectedIndex = -1;
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
            if (cbPlaca.SelectedIndex <= -1)
            {
                cbPlaca.Focus();
                return false;
            }

            if (txtKm.Text == "")
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
                obs = checkPalete.Content.ToString() + "/";
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
                Placa2Acesso = cbPlaca2.Text,
                PlacaAcesso = cbPlaca.Text,
                EstadoGeral = txtEstado.Text,
                PorteiroEntrada = Properties.Login.Default.idUsuario
            };
            return acessos;
        }
    }
}