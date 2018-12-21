using BLL;
using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Carregamento.xaml
    /// </summary>
    public partial class Carregamento : UserControl
    {
        private AcessoBD abd = new AcessoBD();

        private List<Veiculos> ListaMotoristas;
        private List<FuncionariosTag> FuncionarioSelecionado = new List<FuncionariosTag>();
        private List<Funcionarios> ListaFunc;
        private int[] pallets = { 0, 0 };

        public Carregamento()
        {
            InitializeComponent();

            ListaFunc = abd.GetFuncionariosLivres("4");
            CBFuncionario.ItemsSource = ListaFunc;
            CBFuncionario.DisplayMemberPath = "nomeFunc";
            ListaMotoristas = abd.GetVeiculos();
            ListaMotoristas = ListaMotoristas.OrderBy(x => x.MotoristaVeiculo).ToList();
            CBmotorista.ItemsSource = ListaMotoristas;
            CBmotorista.DisplayMemberPath = "MotoristaVeiculo";
            CBmotorista.SelectedValuePath = "CapacidadePaletes";

            RecarregarPendentes();
            Importar();
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
            dgTarefas.ItemsSource = abd.GetTarefasPendentes("4");
        }


        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            TarefaModelo item = (TarefaModelo)dgTarefas.SelectedItem;
            pallets = paletes.Perguntar(item.totalPaletes.ToString());

            if (pallets[1] > 0)
            {
                if (abd.FinalizarTarefa(item.idTarefa, pallets[0], pallets[1]))
                    MessageBox.Show("Carregamento finalizado após " + item.tempoGasto, "Carregamento finalizado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Houve um erro e o carregamento não pode ser finalizado.", "Carregamento não finalizado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                ListaFunc = abd.GetFuncionariosLivres("4");
                CBFuncionario.ItemsSource = ListaFunc;
                RecarregarPendentes();
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
                if (!abd.VerificaDocumentoTarefa(int.Parse(Documento.Text.Replace("_", "")), "4"))
                {
                    if (bll.InserirTarefa(montarTarefa(), Funcionarios()))
                    {
                        RecarregarPendentes();
                        MessageBox.Show("Carregamento iniciado para o " + abd.GetDadosManifesto(int.Parse(Documento.Text.Replace("_", ""))), "Carregamento iniciado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private static void LerXmls()
        {
            xmlBLL x = new xmlBLL();
            x.TriagemArquivos();
        }

        private Tarefas montarTarefa()
        {
            var paletes = 30;

            if (CBmotorista.SelectedValue != null)
                paletes = (int)CBmotorista.SelectedValue;
            else
            {
                int? paleteSeparacao = abd.GetPaletesSeparacao(int.Parse(Documento.Text.Replace("_", "")));

                if (paleteSeparacao != null)
                    paletes = (int)paleteSeparacao;
            }                  

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