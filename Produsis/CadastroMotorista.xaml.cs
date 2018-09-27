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
        public FuncionariosBD motoristaBD = new FuncionariosBD();
        public CapacidadeMotoristas emEdicao { get; set; }

        public CadastroMotorista(double Altura, double largura)
        {
            InitializeComponent();
            dgMotoristas.Height = Altura - 250;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
            emEdicao = new CapacidadeMotoristas();
        }

        private List<CapacidadeMotoristas> loadData()
        {
            List<CapacidadeMotoristas> retorno = motoristaBD.getAllMotoristas();
            dgMotoristas.ItemsSource = retorno;
            return retorno;
        }

        private void Apagar(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((Button)sender).CommandParameter.ToString());
            if (MessageBox.Show("Apagar motorista da lista? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                motoristaBD.deletarMotorista(id); ;
                loadData();
            }
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
            if (emEdicao.Motorista != null && emEdicao.Motorista != "")
            {
                emEdicao.Motorista = txtNome.Text;
                emEdicao.Capacidade = int.Parse(txtCapacidade.Text);
                motoristaBD.editarMotorista(emEdicao);
            }
            else
            {
                if(txtNome.Text!="" && txtCapacidade.Text != "")
                {
                    emEdicao = new CapacidadeMotoristas();
                    emEdicao.Motorista = txtNome.Text;
                    emEdicao.Capacidade = int.Parse(txtCapacidade.Text);
                    motoristaBD.cadastrarMotorista(emEdicao);
                }
            }
            MessageBox.Show("Alteração salva.", "Produsis");
            emEdicao = new CapacidadeMotoristas();
            txtNome.Text = "";
            txtCapacidade.Text = "";
            loadData();
            txtNome.Focus();
        }
               

        private void dgMotoristas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && dgMotoristas.SelectedItems.Count > 0 && !isEditing)
            {
                if (MessageBox.Show("Apagar motorista definitivamente? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var linha = dgMotoristas.SelectedItem as CapacidadeMotoristas;
                    Deletar(linha.Id);
                }
            }
        }

        private void Deletar(int id)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var atual = BancoDeDados.CapacidadeMotoristas.Where(x => x.Id == id).FirstOrDefault();
                    BancoDeDados.CapacidadeMotoristas.Remove(atual);
                    BancoDeDados.SaveChanges();
                }
            }
            catch
            {
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
