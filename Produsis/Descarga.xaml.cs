using BLL;
using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Descarga.xaml
    /// </summary>
    public partial class Descarga : UserControl
    {
        private AcessoBD abd = new AcessoBD();

        private List<FuncionariosTag> FuncionarioSelecionado = new List<FuncionariosTag>();
        private List<Funcionarios> ListaFunc;
         private int[] pallets = { 0, 0 };

        public Descarga()
        {
            InitializeComponent();
            Importar();
            ListaFunc = abd.GetFuncionariosLivres("1");
            CBFuncionario.ItemsSource = ListaFunc;
            CBFuncionario.DisplayMemberPath = "nomeFunc";
            RecarregarPendentes();
        }

        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }

        private void AtualizarDg_Click(object sender, RoutedEventArgs e)
        {
            RecarregarPendentes();
            Importar();
        }

        private void Importar()
        {
            Thread thread = new Thread(LerXmls);
            thread.Start();
        }

        private void RecarregarPendentes()
        {
            dgTarefas.ItemsSource = abd.GetTarefasPendentes("1");
        }

        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            pallets = paletes.Perguntar("28");
            if (pallets[1] > 0)
            {
                TarefaModelo item = (TarefaModelo)dgTarefas.SelectedItem;
                item.AtualizaTempoGasto();
                if (abd.FinalizarTarefa(item.idTarefa, pallets[0], pallets[1]))
                    MessageBox.Show("Descarga finalizada após " + item.tempoGasto, "Descarga finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Houve um erro e a descarga não pode ser finalizada.", "Descarga não finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                ListaFunc = abd.GetFuncionariosLivres("1");
                CBFuncionario.ItemsSource = ListaFunc;
                RecarregarPendentes();
            }
            else
                MessageBox.Show("Houve um erro e a descarga não pode ser finalizada.", "Descarga não finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CBFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBFuncionario.SelectedIndex > -1)
            {
                Funcionarios select = CBFuncionario.SelectedItem as Funcionarios;
                FuncionarioSelecionado.Clear();

                if (select.equipeFunc != null)
                    foreach (var item in ListaFunc.Where(x => x.equipeFunc == select.equipeFunc))
                    {
                        FuncionarioSelecionado.Add(new FuncionariosTag(item.nomeFunc, CriaChipTag(item.nomeFunc)));
                    }
                else
                    FuncionarioSelecionado.Add(new FuncionariosTag(select.nomeFunc, CriaChipTag(select.nomeFunc)));
            }
        }

        private bool ChecarCampos()
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

        private string[] Funcionarios()
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
            Logica bll = new Logica();

            if (ChecarCampos())
                if (!abd.VerificaDocumentoTarefa(int.Parse(Documento.Text.Replace("_", "")), "1"))
                {
                    if (bll.InserirTarefa(MontarTarefa(), Funcionarios()))
                    {
                        RecarregarPendentes();
                        MessageBox.Show("Descarga iniciada para o " + abd.GetDadosManifesto(int.Parse(Documento.Text.Replace("_", ""))), "Descarga iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                        Documento.Text = "";
                        CBFuncionario.SelectedIndex = -1;
                        ListaDeFuncionarios.Items.Clear();
                        Documento.Focus();
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
            if (CBFuncionario.SelectedIndex > -1)
            {
                bool adicionar = true;
                foreach (FuncionariosTag item in FuncionarioSelecionado)
                {
                    foreach (FuncionariosTag ItemDaLista in ListaDeFuncionarios.Items)
                    {
                        if (ItemDaLista.Nome == item.Nome)
                        {
                            adicionar = false;
                            break;
                        }
                    }
                    if (adicionar)
                    {
                        ListaDeFuncionarios.Items.Add(item);
                    }
                }
                FuncionarioSelecionado.Clear();
            }
        }

        private static void LerXmls()
        {
            xmlBLL x = new xmlBLL();
            x.TriagemArquivos();
        }

        private Tarefas MontarTarefa()
        {
            Tarefas novaTarefa = new Tarefas()
            {
                documentoTarefa = int.Parse(Documento.Text.Replace("_", "")),
                inicioTarefa = DateTime.Now,
                tipoTarefa = "1"
            };

            return novaTarefa;
        }

        private void TestarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AdicionarFuncionario(object sender, RoutedEventArgs e)
        {
            Tarefas item = (Tarefas)dgTarefas.SelectedItem;
            AddFuncionario add = new AddFuncionario(item.idTarefa);
            add.ShowDialog();
            AtualizarDg_Click(sender, e);
        }
    }
}