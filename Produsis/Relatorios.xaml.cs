using BLL;
using Microsoft.Win32;
using ProdusisBD;
using System;
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

        public Filtro montarObjeto()
        {
            Filtro filtros = new Filtro();
            filtros.TipoTarefa = cbTipoTarefa.SelectedIndex.ToString();
            var aux = int.TryParse(tbDocumento.Text, out filtros.numDocumento);
            filtros.dataInicio = dataInicio.SelectedDate;
            filtros.dataFim = dataFinal.SelectedDate.Value.AddDays(1).AddSeconds(-1);
            aux = int.TryParse(volumeInicial.Text, out filtros.volumeInicio);
            aux = int.TryParse(volumeFinal.Text, out filtros.volumeFim);
            aux = int.TryParse(skuInicio.Text, out filtros.skuInicio);
            aux = int.TryParse(skuFinal.Text, out filtros.skuFim);
            aux = double.TryParse(pesoInicio.Text, out filtros.pesoInicio);
            aux = double.TryParse(pesoFinal.Text, out filtros.pesoFim);
            filtros.nomeFuncionario = cbFuncionario.SelectionBoxItem.ToString();
            return filtros;
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            TarefasBLL t = new TarefasBLL();
            dgTarefas.ItemsSource = t.filtrar(montarObjeto());
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
            pesoInicio.Text = "";
            pesoFinal.Text = "";
            cbFuncionario.SelectedIndex = -1;
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
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
                SaveFileDialog dialogo = new SaveFileDialog();
                dialogo.Title = "Salvar relatório - Produsis";

                if (dialogo.ShowDialog() == true)
                {
                    dgTarefas.SelectAllCells();
                    dgTarefas.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                    ApplicationCommands.Copy.Execute(null, dgTarefas);
                    dgTarefas.UnselectAllCells();
                    String Clipboardresult = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                    Clipboardresult = Clipboardresult.Replace(",", ";");
                    StreamWriter swObj = new StreamWriter(extensao(dialogo.FileName), false, Encoding.GetEncoding(CultureInfo.GetCultureInfo("pt-BR").TextInfo.ANSICodePage));
                    swObj.WriteLine(Clipboardresult);
                    swObj.Close();
                }
            }
        }

        private string extensao(string nomeArquivo)
        {
            if (nomeArquivo.Contains("."))
            {
                nomeArquivo = nomeArquivo.Remove(nomeArquivo.Length - 3);
            }
            nomeArquivo += "csv";
            return nomeArquivo;
        }
    }
}