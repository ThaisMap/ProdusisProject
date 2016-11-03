using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using BLL;
namespace GUI
{
    /// <summary>
    /// Interaction logic for IniciadasHoje.xaml
    /// </summary>
    public partial class IniciadasHoje : UserControl
    {
        List<TarefaIniciada> ListaIniciadasHoje = new List<TarefaIniciada>();
        public IniciadasHoje()
        {
            InitializeComponent();
            tarefasHoje();
        }

        private void tarefasHoje()
        {
            TarefasBLL t = new TarefasBLL();
            TarefaIniciada aux;
            string[] nomes = { "Descarga", "Separação", "Conferência", "Separação para carregar", "Carregamento" };
            for (int i = 0; i < 5; i++)
            {
                aux = new TarefaIniciada();
                aux.Tipo = nomes[i];
                aux.Finalizadas = t.iniciadaHojeFinalizada(i.ToString());
                aux.Pendentes = t.iniciadaHojePendente(i.ToString());
                ListaIniciadasHoje.Add(aux);
            }
            dgTarefas.ItemsSource = ListaIniciadasHoje;
        }
    }

    public class TarefaIniciada
    {
        public string Tipo { get; set; }
        public int Finalizadas { get; set; }
        public int Pendentes { get; set; }
    }
}
