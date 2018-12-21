using BLL;
using DAL;
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
        private AcessoBD abd = new AcessoBD();

        private Logica bll = new Logica();
        private FuncionariosTag FuncionarioSelecionado;
        private List<string> ListaFunc;
        private int checagemDeCte = -1;

        public Conferencia()
        {
            InitializeComponent();
            Importar();
            ListaFunc = abd.GetConferentesLivres("2");
            CBFuncionario.ItemsSource = ListaFunc;
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
            dgTarefas.ItemsSource = abd.GetTarefasPendentes("2");
        }

        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            TarefaModelo item = (TarefaModelo)dgTarefas.SelectedItem;
            item.AtualizaTempoGasto();
            if (MessageBox.Show("Confirma finalização da conferência de " + item.nomesFuncionarios + " após " + item.tempoGasto + "? ", "Produsis", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (!abd.FinalizarTarefa(item.idTarefa, 0, 0))
                {
                    MessageBox.Show("Houve um erro e a conferência não pode ser finalizada.", "Conferência não finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ListaFunc = abd.GetConferentesLivres("2");
                    CBFuncionario.ItemsSource = ListaFunc;

                    if (ListaFunc.Contains(item.nomesFuncionarios))
                    {
                        CBFuncionario.SelectedValue = item.nomesFuncionarios;

                        MessageBoxResult novaTarefa = MessageBox.Show("Deseja Abrir uma nova tarefa para o funcionário?", "Nova Tarefa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (novaTarefa.ToString().ToUpper() == "YES")
                        {
                            FuncionarioSelecionado = new FuncionariosTag(CBFuncionario.SelectedItem.ToString(), CriaChipTag(CBFuncionario.SelectedItem.ToString()));
                            if (!ListaDeFuncionarios.Items.Contains(FuncionarioSelecionado) && CBFuncionario.SelectedIndex > -1)
                            {
                                ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
                            }
                        }
                    }
                }
                Documento.Focus();
            }
            this.TabIndex = 1;
            RecarregarPendentes();
        }
         
        private void CBFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBFuncionario.SelectedIndex > -1)
                FuncionarioSelecionado = new FuncionariosTag(CBFuncionario.SelectedItem.ToString(), CriaChipTag(CBFuncionario.SelectedItem.ToString()));
        }

        private bool ChecarCampos()
        {
            if (Documento.Text == "" || ListaDeFuncionarios.Items.Count == 0)
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
            checagemDeCte = bll.IdCteDisponivelMaisRecente(int.Parse(Documento.Text));
            if (ChecarCampos())
                if (checagemDeCte > -1)
                {
                    if (checagemDeCte > 0)
                    {
                        if (bll.InserirTarefa(MontarTarefa(), Funcionarios()))
                        {
                            RecarregarPendentes();
                            MessageBox.Show("Conferência iniciada para o " + abd.GetDadosCte(int.Parse(Documento.Text)), "Conferência iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                            Documento.Text = "";
                            CBFuncionario.SelectedIndex = -1;
                            ListaDeFuncionarios.Items.Clear();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Já existe conferência pare este Cte.", "Conferência não iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Cte não importado.", "Conferência não iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void TestarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private static void LerXmls()
        {
            xmlBLL x = new xmlBLL();
            x.TriagemArquivos();
        }

        private Tarefas MontarTarefa()
        {
            Tarefas novaTarefa = new Tarefas
            {
                documentoTarefa = checagemDeCte,
                inicioTarefa = DateTime.Now,
                tipoTarefa = "2"
            };
            return novaTarefa;
        }

        private void NormalizarDocumento(object sender, RoutedEventArgs e)
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