using BLL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Conferencia.xaml
    /// </summary>
    public partial class Conferencia : UserControl
    {
        private DocumentosBLL d = new DocumentosBLL();
        private FuncionarioBLL f = new FuncionarioBLL();
        private FuncionariosTag FuncionarioSelecionado;
        private List<string> ListaFunc;
        private TarefasBLL t = new TarefasBLL();

        public Conferencia(double actualHeight, double actualWidth)
        {
            InitializeComponent();
            ListaFunc = f.carregaConferentesLivres("2");
            CBFuncionario.ItemsSource = ListaFunc;
            dgTarefas.ItemsSource = t.tarefasPendentes("2");
            Height = actualHeight - 100;
            Width = actualWidth - 60;
            Scrooler.Height = actualHeight - (int.Parse(formSuperior.Height.ToString()) + 225);
            lerXmls();
        }

        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }

        private void AtualizarDg_Click(object sender, RoutedEventArgs e)
        {
            dgTarefas.ItemsSource = t.tarefasPendentes("2");
        }

        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            TarefaModelo item = (TarefaModelo)dgTarefas.SelectedItem;
            if (t.finalizarTarefa(item.idTarefa, 0, 0))
                MessageBox.Show("Conferência finalizada após " + item.tempoGasto, "Conferência finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Houve um erro e a conferência não pode ser finalizada.", "Conferência não finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
            ListaFunc = f.carregaConferentesLivres("2");
            CBFuncionario.ItemsSource = ListaFunc;

            if (ListaFunc.Contains(item.nomesFuncionarios))
                CBFuncionario.SelectedValue = item.nomesFuncionarios;
            MessageBoxResult novaTarefa = MessageBox.Show("Deseja Abrir uma nova tarefa para o funcionário?", "Nova Tarefa", MessageBoxButton.YesNo,MessageBoxImage.Question);
            if(novaTarefa.ToString().ToUpper() == "YES")
            {
                FuncionarioSelecionado = new FuncionariosTag(CBFuncionario.SelectedItem.ToString(), CriaChipTag(CBFuncionario.SelectedItem.ToString()));
                if (!ListaDeFuncionarios.Items.Contains(FuncionarioSelecionado) && CBFuncionario.SelectedIndex > -1)
                {
                    ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
                }
                Documento.Focus();
            }

            this.TabIndex = 1;
            AtualizarDg_Click(sender, e);
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
            if (checarCampos() && t.tarefaRepetida(int.Parse(Documento.Text.Replace("_", "")), "2"))
            {
                if (t.inserirTarefa(montarTarefa(), funcionarios()))
                {
                    dgTarefas.ItemsSource = t.tarefasPendentes("2");
                    MessageBox.Show("Conferência iniciada para o " + d.linhaDadosCte(int.Parse(Documento.Text.Replace("_", ""))), "Conferência iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                    Documento.Text = "";
                    CBFuncionario.SelectedIndex = -1;
                    ListaDeFuncionarios.Items.Clear();
                }
                else
                {
                    MessageBox.Show("Não foi possível iniciar a conferência.", "Conferência não iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Não foi possível iniciar a conferência.", "Conferência não iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Documento.Focus();
        }

        private void Inserir_Click(object sender, RoutedEventArgs e)
        {
            if (!ListaDeFuncionarios.Items.Contains(FuncionarioSelecionado) && CBFuncionario.SelectedIndex > -1)
            {
                ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
            }
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private static void lerXmls()
        {
            xmlBLL x = new xmlBLL();
            x.triagemArquivos();
        }

        private Tarefas montarTarefa()
        {
            Tarefas novaTarefa = new Tarefas();
            novaTarefa.documentoTarefa = int.Parse(Documento.Text.Replace("_", ""));
            novaTarefa.inicioTarefa = DateTime.Now;
            novaTarefa.tipoTarefa = "2";
            return novaTarefa;
        }

        private void normalizarDocumento(object sender, RoutedEventArgs e)
        {
            if (Documento.Text.Length == 44)
            {
                Documento.Text = Documento.Text.Remove(0, 25);
                Documento.Text = Documento.Text.Remove(9);
                Documento.Text = int.Parse(Documento.Text).ToString();
            }
        }

       
    }
}