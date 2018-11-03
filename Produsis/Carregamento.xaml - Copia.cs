using BLL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAL;
using System.Linq;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Carregamento.xaml
    /// </summary>
    public partial class Carregamento : UserControl
    {
        private DocumentosBLL d = new DocumentosBLL();
        private FuncionarioBLL f = new FuncionarioBLL();
        private VeiculosBD funcionariosBD = new VeiculosBD();
        private List<Veiculos> ListaMotoristas;
        private List<FuncionariosTag> FuncionarioSelecionado = new List<FuncionariosTag>();
        private List<Funcionarios> ListaFunc;
        private TarefasBLL t = new TarefasBLL();
        private int[] pallets = {0, 0};

        public Carregamento(double actualHeight, double actualWidth)
        {
            InitializeComponent();
            ListaFunc = f.carregaFuncionariosLivres("4");
            CBFuncionario.ItemsSource = ListaFunc;
            CBFuncionario.DisplayMemberPath = "nomeFunc";
            ListaMotoristas = funcionariosBD.getVeiculos();
            CBmotorista.ItemsSource = ListaMotoristas;
            CBmotorista.DisplayMemberPath = "MotoristaVeiculo";
            CBmotorista.SelectedValuePath = "CapacidadeVeiculo";
            dgTarefas.ItemsSource = t.TarefasPendentes("4");
            Height = actualHeight - 150;
            Width = actualWidth - 60;
            lerXmls();
        }

        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }
        
        private void AtualizarDg_Click(object sender, RoutedEventArgs e)
        {
            dgTarefas.ItemsSource = t.TarefasPendentes("4");
        }

        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            TarefaModelo item = (TarefaModelo)dgTarefas.SelectedItem;
            pallets = paletes.Perguntar(item.totalPaletes.ToString());

            if (pallets[1] > 0)
            {
                if (t.FinalizarTarefa(item.idTarefa, pallets[0], pallets[1]))
                    MessageBox.Show("Carregamento finalizado após " + item.tempoGasto, "Carregamento finalizado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Houve um erro e o carregamento não pode ser finalizado.", "Carregamento não finalizado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                ListaFunc = f.carregaFuncionariosLivres("4");
                CBFuncionario.ItemsSource = ListaFunc;
                AtualizarDg_Click(sender, e);
            }
            Documento.Focus();

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
                if (t.TarefaRepetida(int.Parse(Documento.Text.Replace("_", "")), "4") && t.TarefaRepetida(int.Parse(Documento.Text.Replace("_", "")), "6"))
                {
                    if (t.InserirTarefa(montarTarefa(), funcionarios()))
                    {
                        dgTarefas.ItemsSource = t.TarefasPendentes("4");
                        MessageBox.Show("Carregamento iniciado para o " + d.linhaDadosManifesto(int.Parse(Documento.Text.Replace("_", ""))), "Carregamento iniciado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                        Documento.Text = "";
                        CBFuncionario.SelectedIndex = -1;
                        ListaDeFuncionarios.Items.Clear();
                        Documento.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Manifesto não importado.", "Carregamento não iniciado - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Já existe um carregamento para esse manifesto.", "Carregamento não iniciado - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
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

        static void lerXmls()
        {
            xmlBLL x = new xmlBLL();
            x.triagemArquivos();
        }

        private Tarefas montarTarefa()
        {
            var paletes = 30;
            if (CBmotorista.SelectedValue != null)
                paletes = (int)CBmotorista.SelectedValue;
            
            Tarefas novaTarefa = new Tarefas()
            {
                documentoTarefa = int.Parse(Documento.Text.Replace("_", "")),
                totalPaletes = paletes,
                tipoTarefa = "4",
                inicioTarefa = DateTime.Now
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