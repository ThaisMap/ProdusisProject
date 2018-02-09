using BLL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Descarga.xaml
    /// </summary>
    public partial class Descarga : UserControl
    {
        private DocumentosBLL d = new DocumentosBLL();
        private FuncionarioBLL f = new FuncionarioBLL();
        private FuncionariosTag FuncionarioSelecionado;
        private List<string> ListaFunc;
        private TarefasBLL t = new TarefasBLL();
        private int[] pallets = {0, 0};

        public Descarga()
        {
            InitializeComponent();
            ListaFunc = f.carregaFuncionariosLivres();
            CBFuncionario.ItemsSource = ListaFunc;
            dgTarefas.ItemsSource = t.tarefasPendentes("1","5");
            lerXmls();
        }

        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }

        private void AtualizarDg_Click(object sender, RoutedEventArgs e)
        {
            dgTarefas.ItemsSource = t.tarefasPendentes("1", "5");
        }

        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            pallets = paletes.Perguntar("30");
            if (pallets[1] > 0)
            {
                Tarefas item = (Tarefas)dgTarefas.SelectedItem;
                if (t.finalizarTarefa(item.idTarefa, pallets[0], pallets[1]))
                    MessageBox.Show("Descarga finalizada após " + item.tempoGasto, "Descarga finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Houve um erro e a descarga não pode ser finalizada.", "Descarga não finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                ListaFunc = f.carregaFuncionariosLivres();
                CBFuncionario.ItemsSource = ListaFunc;
                AtualizarDg_Click(sender, e);
            }
            else
                MessageBox.Show("Houve um erro e a descarga não pode ser finalizada.", "Descarga não finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void CBFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBFuncionario.SelectedIndex > -1)
                FuncionarioSelecionado = new FuncionariosTag(CBFuncionario.SelectedItem.ToString(), CriaChipTag(CBFuncionario.SelectedItem.ToString()));
        }

        private bool checarCampos()
        {
            if (Documento.Text.Replace("_", "") == "" || ListaDeFuncionarios.Items.Count == 0)
            {
                MessageBox.Show("Digite o documento e inclua os funcionários.", "Descarga não iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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

        private string[] funcionarios()
        {
            List<string> nomes = new List<string>();
            foreach (FuncionariosTag tag in ListaDeFuncionarios.Items)
            {
                nomes.Add(tag.Nome);
            }
            return nomes.ToArray();
        }

        private void Iniciar_Click(object sender, RoutedEventArgs e)
        {
            if (checarCampos())
                if (t.tarefaRepetida(int.Parse(Documento.Text.Replace("_", "")), "1") && t.tarefaRepetida(int.Parse(Documento.Text.Replace("_", "")), "5"))
            {    
                if (t.inserirTarefa(montarTarefa(), funcionarios()))
                {
                    dgTarefas.ItemsSource = t.tarefasPendentes("1");
                    MessageBox.Show("Descarga iniciada para o " + d.linhaDadosManifesto(int.Parse(Documento.Text.Replace("_", ""))), "Descarga iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                    Documento.Text = "";
                    CBFuncionario.SelectedIndex = -1;
                    ListaDeFuncionarios.Items.Clear();
                }
                else
                {
                    MessageBox.Show("Manifesto não importado.", "Descarga não iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Já existe uma descarga para esse manifesto.", "Descarga não iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Inserir_Click(object sender, RoutedEventArgs e)
        {
            if (!ListaDeFuncionarios.Items.Contains(FuncionarioSelecionado) && CBFuncionario.SelectedIndex > -1)
            {
                ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
            }
        }

        static void lerXmls()
        {
            xmlBLL x = new xmlBLL();
            x.triagemArquivos();
        }

        private Tarefas montarTarefa()
        {            
            Tarefas novaTarefa = new Tarefas()
            {
                documentoTarefa = int.Parse(Documento.Text.Replace("_", "")),
                inicioTarefa = DateTime.Now,
                tipoTarefa = "1"
            };

            return novaTarefa;
        }
        
        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}