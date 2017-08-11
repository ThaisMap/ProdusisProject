using BLL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for Divergencia.xaml
    /// </summary>
    public partial class Divergencia : UserControl
    {
        public Divergencia(double actualHeight, double actualWidth)
        {
            InitializeComponent();
            Height = actualHeight - 100;
            Width = actualWidth - 60;
        }

        public List<ItemDivergencia> source { get; set; }
        public List<TarefaModelo> tarefas = new List<TarefaModelo>();

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbTipoTarefa.SelectedIndex > -1 && Documento.Text != "")
                {
                    TarefasBLL t = new TarefasBLL();
                    tarefas = t.filtrarDivergencias(cbTipoTarefa.SelectedIndex, int.Parse(Documento.Text));
                    source = new List<ItemDivergencia>();
                    foreach (TarefaModelo item in tarefas)
                    {
                        source.Add(new ItemDivergencia(item));
                    }
                    dgDivergencias.ItemsSource = source;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (source != null)
            {
                TarefasBLL t = new TarefasBLL();
                for (int i = 0; i < source.Count; i++)
                {
                    tarefas[i].divergenciaTarefa = source[i].getDivergencia();
                }

                t.inserirDivergencias(tarefas);
            }
        }
    }
    
}
