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
        public Divergencia()
        {
            InitializeComponent();
        }

        public List<TarefaModelo> source;

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            if(cbTipoTarefa.SelectedIndex>-1 && Documento.Text != "")
            {
                TarefasBLL t = new TarefasBLL();
                source = t.filtrarDivergencias(cbTipoTarefa.SelectedIndex, int.Parse(Documento.Text));
                dgDivergencias.ItemsSource = source;
            }
        }
    }
    
}
