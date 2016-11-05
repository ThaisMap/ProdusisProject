using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
using ProdusisBD;
using BLL;

namespace Produsis
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
    }
}
