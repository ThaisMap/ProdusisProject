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
using DAL;

namespace GUI
{
    /// <summary>
    /// Interação lógica para Divergencias.xam
    /// </summary>
    public partial class Divergencias : UserControl
    {
        public Divergencias(double actualHeight, double actualWidth)
        {
            InitializeComponent();
            Height = actualHeight - 100;
            Width = actualWidth - 60;
        }

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
                    TarefasBD t = new TarefasBD();
                    var tarefa = t.GetTarefaDivergencia(cbTipoTarefa.SelectedIndex + 1, int.Parse(Documento.Text));
                    //Dá erro quando é cte, pois nao busca pelo id
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
