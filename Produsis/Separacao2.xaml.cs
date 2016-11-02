﻿using BLL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Separacao2.xaml
    /// </summary>
    public partial class Separacao2 : UserControl
    {
        private DocumentosBLL d = new DocumentosBLL();
        private FuncionarioBLL f = new FuncionarioBLL();
        private FuncionariosTag FuncionarioSelecionado;
        private List<string> ListaFunc;
        private TarefasBLL t = new TarefasBLL();

        public Separacao2()
        {
            InitializeComponent();
            ListaFunc = f.carregaFuncionariosLivres();
            CBFuncionario.ItemsSource = ListaFunc;
            dgTarefas.ItemsSource = t.tarefasPendentes("3");
        }
        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = new string[7];
            PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }

        private void AtualizarDg_Click(object sender, RoutedEventArgs e)
        {
            dgTarefas.ItemsSource = t.tarefasPendentes("3");
        }

        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            Tarefas item = (Tarefas)dgTarefas.SelectedItem;
            if (t.finalizarTarefa(item.idTarefa))
                MessageBox.Show("Separação para carregamento finalizada após" + item.tempoGasto, "Separação finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Houve um erro e a separação para carregamento não pode ser finalizada.", "Separação não finalizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
            ListaFunc = f.carregaFuncionariosLivres();
            CBFuncionario.ItemsSource = ListaFunc;
            AtualizarDg_Click(sender, e);
        }

        private void CBFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            if (checarCampos() && t.tarefaRepetida(int.Parse(Documento.Text.Replace("_", "")), "3"))
            {
                t.inserirTarefa(montarTarefa(), funcionarios());
                dgTarefas.ItemsSource = t.tarefasPendentes("3");
                MessageBox.Show("Separação iniciada para carregar o " + d.linhaDados(int.Parse(Documento.Text.Replace("_", ""))), "Separação iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Não foi possível iniciar a separação para carregamento.", "Separação não iniciada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Inserir_Click(object sender, RoutedEventArgs e)
        {
            if (!ListaDeFuncionarios.Items.Contains(FuncionarioSelecionado) && CBFuncionario.SelectedIndex > -1)
            {
                ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
            }
        }
        private Tarefas montarTarefa()
        {
            Tarefas novaTarefa = new Tarefas();
            novaTarefa.documentoTarefa = int.Parse(Documento.Text.Replace("_", ""));
            novaTarefa.inicioTarefa = DateTime.Now;
            novaTarefa.tipoTarefa = "3";
            return novaTarefa;
        }
    }
}