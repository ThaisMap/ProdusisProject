using BLL;
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
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Lógica interna para AlterarTarefa.xaml
    /// </summary>
    public partial class AlterarTarefa : Window
    {
        private FuncionariosTag FuncionarioSelecionado;
        private List<string> ListaFunc;
        private DocumentosBLL d = new DocumentosBLL();
        private FuncionarioBLL f = new FuncionarioBLL();
        private TarefasBLL t = new TarefasBLL();
        ProdusisBD.TarefaModelo TarefaSelecionada;

        public AlterarTarefa(ProdusisBD.TarefaModelo tarefa)
        {
            InitializeComponent();
            ListaFunc = f.carregaFuncionariosLivres(tarefa.tipoTarefa);
            cbxFuncionarios.ItemsSource = ListaFunc;
            TarefaSelecionada = tarefa;
            txbDocumentoTarefa.Text = TarefaSelecionada.documentoTarefa.ToString();
            foreach(string nome in TarefaSelecionada.nomesFuncionarios.Split('/'))
            {
                ListaDeFuncionarios.Items.Add(new FuncionariosTag(nome, CriaChipTag(nome)));
            }
            timepHoraInicio.SelectedTime = TarefaSelecionada.inicioTarefa;
        }

        private string[] funcionarios()
        {
            List<string> nomes = new List<string>();
            foreach (FuncionariosTag tag in ListaDeFuncionarios.Items)
            {
                nomes.Add(tag.Nome);
            }
            return nomes.ToArray();
        }

        private void ChipEx_DeleteClick(object sender, RoutedEventArgs e)
        {
            MaterialDesignThemes.Wpf.Chip novo = (MaterialDesignThemes.Wpf.Chip)sender;
            foreach (FuncionariosTag tag in ListaDeFuncionarios.Items)
            {
                if (tag.Nome == novo.Content.ToString())
                {
                    ListaDeFuncionarios.Items.Remove(tag);
                    break;
                }
            }
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Inserir_Click(object sender, RoutedEventArgs e)
        {
            if (!ListaDeFuncionarios.Items.Contains(FuncionarioSelecionado) && cbxFuncionarios.SelectedIndex > -1)
            {
                ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
            }
        }

        private void cbxFuncionarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ListaDeFuncionarios.Items.Contains(FuncionarioSelecionado) && cbxFuncionarios.SelectedIndex > -1)
            {
                FuncionarioSelecionado = new FuncionariosTag(cbxFuncionarios.SelectedItem.ToString(), CriaChipTag(cbxFuncionarios.SelectedItem.ToString()));
            }
        }

        private bool checarCampos()
        {
            if (txbDocumentoTarefa.Text.Replace("_", "") == "" || ListaDeFuncionarios.Items.Count == 0)
            {
                return false;
            }
            return true;
        }

        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnExcluirTarefa_Click(object sender, RoutedEventArgs e)
        {
            //Exclui tarefa
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            //Alterar Tarefa
        }
    }
}
