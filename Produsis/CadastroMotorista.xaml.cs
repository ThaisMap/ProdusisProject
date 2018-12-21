using DAL;
using ProdusisBD;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interação lógica para CadastroMotorista.xam
    /// </summary>
    public partial class CadastroMotorista : UserControl
    {
        public bool isEditing { get; set; }
        private List<Veiculos> motoristas;
        private AcessoBD abd = new AcessoBD();
        
        string[] tiposVeiculo = { "VAN", "HR", "VUC", "3/4", "TOCO", "TRUCK", "CAVALO", "CONJUNTO", "CARRO", "PREST SERVIÇO", "BITRUCK" };

        public CadastroMotorista()
        {
            InitializeComponent();
            cbTipo.ItemsSource = tiposVeiculo;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // ATUALIZADO
        private List<Veiculos> LoadData()
        {
            motoristas = abd.GetVeiculos();
            cbNome.ItemsSource = motoristas.OrderBy(x => x.MotoristaVeiculo);
            cbNome.DisplayMemberPath = "MotoristaVeiculo";
            cbNome.SelectedValuePath = "MotoristaVeiculo";
            return motoristas;
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (ChecarCampos())
            {
                if (abd.CadastrarVeiculo(MontarObjeto()))
                {
                    MessageBox.Show("Veículo cadastrado");
                    Limpar();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Veículo editado");
                    Limpar();
                    LoadData();
                }
            }
        }

        private void Limpar()
        {
            Ativo.IsChecked = true;
            txtCapacidade.Text = "0";
            txtNome.Text = "";
            txtPlaca2.Text = "";
            txtPlaca.Text = "";
            cbTipo.SelectedIndex = -1;
        }

        private Veiculos MontarObjeto()
        {
            Veiculos novo = new Veiculos()
            {
                AtivoVeiculo = (bool)Ativo.IsChecked,
                CapacidadePaletes = int.Parse(txtCapacidade.Text),
                MotoristaVeiculo = txtNome.Text.ToUpper(),
                PlacaVeiculo = txtPlaca.Text,
                TipoVeiculo = cbTipo.Text
            };
            if (txtPlaca2.Text == "" || txtPlaca2.Text == "       ")
                novo.Placa2Veiculo = null;
            else
                novo.Placa2Veiculo = txtPlaca2.Text;

            return novo;
        }

        // ATUALIZADO
        private void TestarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // ATUALIZADO
        private bool ValidarPlacas(string placa)
        {
            Regex regex = new Regex(@"^[a-zA-Z]{3}\d{4}$");

            if (regex.IsMatch(placa))
            {
                return true;
            }

            return false;
        }

        // ATUALIZADO
        private bool ChecarCampos()
        {
            if (txtNome.Text == "")
            {
                txtNome.Focus();
                return false;
            }
            if (txtCapacidade.Text == "")
            {
                txtCapacidade.Focus();
                return false;
            }
            if (txtPlaca.Text == "" || !ValidarPlacas(txtPlaca.Text))
            {
                txtPlaca.Focus();
                return false;
            }
            if (cbTipo.SelectedIndex == -1)
            {
                cbTipo.Focus();
                return false;
            }
            if (cbTipo.SelectedIndex == 7 && txtPlaca2.Text == "") // cbTipo 7 = conjunto onde é preciso ter a placa da carreta
            {
                cbTipo.Focus();
                return false;
            }
            if (txtPlaca2.Text != "" && !ValidarPlacas(txtPlaca2.Text))
            {
                txtPlaca2.Focus();
                return false;
            }

            return true;
        }

        // ATUALIZADO
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (cbNome.SelectedIndex > -1)
            {
                Veiculos atual = motoristas.Where(x => x.MotoristaVeiculo == cbNome.SelectedValue.ToString()).FirstOrDefault();
                txtNome.Text = atual.MotoristaVeiculo.TrimEnd(' ');
                txtCapacidade.Text = atual.CapacidadePaletes.ToString();
                txtPlaca.Text = atual.PlacaVeiculo.TrimEnd(' ');
                txtPlaca2.Text = atual.Placa2Veiculo == null ? "" : atual.Placa2Veiculo.TrimEnd(' ');
                cbTipo.Text = atual.TipoVeiculo.TrimEnd(' ');
                Ativo.IsChecked = atual.AtivoVeiculo;
            }
        }
    }
}