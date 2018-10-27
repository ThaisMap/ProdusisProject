using ProdusisBD;
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
using DAL;
using System.Text.RegularExpressions;

namespace GUI
{
    /// <summary>
    /// Interação lógica para CadastroMotorista.xam
    /// </summary>
    public partial class CadastroMotorista : UserControl
    {
        public bool isEditing { get; set; }
        List<CapacidadeMotoristas> motoristas;
        public FuncionariosBD motoristaBD = new FuncionariosBD();
        public CapacidadeMotoristas emEdicao { get; set; }

        public CadastroMotorista(double Altura, double largura)
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
            emEdicao = new CapacidadeMotoristas();
        }

        // ATUALIZADO
        private List<CapacidadeMotoristas> loadData()
        {
            motoristas = motoristaBD.getAllMotoristas();
            cbNome.ItemsSource = motoristas;
            cbNome.DisplayMemberPath = "Motorista";
            cbNome.SelectedValuePath = "Motorista";
            return motoristas;
        }


        private void Editar(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((Button)sender).CommandParameter.ToString());
            emEdicao = loadData().Where(x => x.Id == id).First();
            txtNome.Text = emEdicao.Motorista;
            txtCapacidade.Text = emEdicao.Capacidade.ToString();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (ChecarCampos())
            {
/* montar objeto
           * salvar no bd veiculos
           */
            }
          
        }

        private Veiculos MontarObjeto()
        {
            Veiculos novo = new Veiculos();


            return novo;
        }

        // ATUALIZADO
        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // ATUALIZADO
        private bool validarPlacas(string placa)
        {
            Regex regex = new Regex(@"^[a-zA-Z]{3}d{4}$");

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
            if (txtPlaca.Text == "" || !validarPlacas(txtPlaca.Text))
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
            if (txtPlaca2.Text == "" || !validarPlacas(txtPlaca2.Text))
            {
                txtPlaca2.Focus();
                return false;
            }

            return true;

        }

        // ATUALIZADO
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (cbNome.SelectedIndex > -1)
            {
                CapacidadeMotoristas atual = motoristas.Where(x => x.Motorista == cbNome.SelectedValue.ToString()).FirstOrDefault();
                txtNome.Text = atual.Motorista;
                txtCapacidade.Text = atual.Capacidade.ToString();
            }
        }
    }
}
