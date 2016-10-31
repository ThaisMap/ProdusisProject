using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BLL;
using ProdusisBD;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Descarga.xaml
    /// </summary>
    public partial class Descarga : UserControl
    {
        private FuncionariosTag FuncionarioSelecionado;
        private List<String> ListaFunc;

        private FuncionarioBLL f = new FuncionarioBLL();
        private ObservableCollection<FuncionariosTag> Funcionario { get; set; }
        private TarefasBLL t = new TarefasBLL();

        public Descarga()
        {
            InitializeComponent();
            ListaFunc = f.carregaFuncionariosLivres();
            CBFuncionario.ItemsSource = ListaFunc;
            dgTarefas.ItemsSource = t.tarefasPendentes("0");
        }

        
        
        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = new string[7];
            PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
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

        private void Inserir_Click(object sender, RoutedEventArgs e)
        {
            if (!ListaDeFuncionarios.Items.Contains(FuncionarioSelecionado))
            {
                ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
            }
        }

        private void CBFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FuncionarioSelecionado = new FuncionariosTag(CBFuncionario.SelectedItem.ToString(), CriaChipTag(CBFuncionario.SelectedItem.ToString()));
        }

        private void Iniciar_Click(object sender, RoutedEventArgs e)
        {
            if (checarCampos())
            {
                t.inserirTarefa(montarTarefa(), funcionarios());
                dgTarefas.ItemsSource = t.tarefasPendentes("0");
            }
        }

        private Tarefas montarTarefa()
        {
            Tarefas novaTarefa = new Tarefas();
            novaTarefa.documentoTarefa = int.Parse(Documento.Text.Replace("_", ""));
            novaTarefa.inicioTarefa = DateTime.Now;
            novaTarefa.tipoTarefa = "0";
            return novaTarefa;
        }

        private bool checarCampos()
        {
            if (Documento.Text.Replace("_", "") == "" || ListaDeFuncionarios.Items.Count == 0)
            {
                return false;
            }
            return true;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tarefas item = (Tarefas)dgTarefas.SelectedItem;
            t.finalizarTarefa(item.idTarefa);
        }
    }
   
}
