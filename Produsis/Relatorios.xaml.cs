﻿using BLL;
using Microsoft.Win32;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Relatorios.xaml
    /// </summary>
    public partial class Relatorios : UserControl
    {
        public Relatorios()
        {
            InitializeComponent();
        }

        public List<TarefaModelo> source;

        public Filtro montarObjeto()
        {
            Filtro filtros = new Filtro()
            {
                TipoTarefa = cbTipoTarefa.SelectedIndex.ToString(),
                dataInicio = dataInicio.SelectedDate,
                dataFim = dataFinal.SelectedDate.Value.AddDays(1).AddSeconds(-1)
            };
            var aux = int.TryParse(tbDocumento.Text, out filtros.numDocumento);
            aux = int.TryParse(volumeInicial.Text, out filtros.volumeInicio);
            aux = int.TryParse(volumeFinal.Text, out filtros.volumeFim);
            aux = int.TryParse(skuInicio.Text, out filtros.skuInicio);
            aux = int.TryParse(skuFinal.Text, out filtros.skuFim);
            filtros.nomeFuncionario = cbFuncionario.SelectionBoxItem.ToString();
            return filtros;
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            TarefasBLL t = new TarefasBLL();
            source = t.filtrar(montarObjeto());
            dgTarefas.ItemsSource = source;
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            dataInicio.SelectedDate = null;
            dataFinal.SelectedDate = DateTime.Today;
            cbTipoTarefa.SelectedIndex = -1;
            tbDocumento.Text = "";
            volumeInicial.Text = "";
            volumeFinal.Text = "";
            skuInicio.Text = "";
            skuFinal.Text = "";
            cbFuncionario.SelectedIndex = -1;
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9][-]?[0-9]?");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FuncionarioBLL f = new FuncionarioBLL();
            dataInicio.SelectedDate = null;
            dataFinal.SelectedDate = DateTime.Today;
            cbFuncionario.ItemsSource = f.carregaFuncionarios();
        }

        private void btnExportar_Click(object sender, RoutedEventArgs e)
        {
            if (dgTarefas.Items.Count > 0)
            {
                SaveFileDialog dialogo = new SaveFileDialog()
                {
                    DefaultExt = "xls",
                    Title = "Salvar relatório - Produsis",
                    AddExtension = true
                };                

                if (dialogo.ShowDialog() == true)
                {
                    TarefasBLL t = new TarefasBLL();
                    t.exportarExcel(source, dialogo.FileName);
                }
            }
        }

        private string extensao(string nomeArquivo)
        {
            nomeArquivo += ".csv";
            return nomeArquivo;
        }
    }
}