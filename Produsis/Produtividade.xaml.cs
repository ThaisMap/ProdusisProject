using BLL;
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
using System.Globalization;
using LiveCharts;
using LiveCharts.Wpf;
using ProdusisBD;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Produtividade.xaml
    /// </summary>
    public partial class Produtividade : UserControl
    {
        public SeriesCollection Colecao { get; set; }
        public string[] Rotulos { get; set; }
        public Func<double, string> Formatter { get; set; }

        List<double> valores = new List<double>();
        List<string> nomes = new List<string>();

        public Produtividade()
        {
            InitializeComponent();          

        }


        private void geraRelatorio(object sender, RoutedEventArgs e)
        {
            Filtro filtros = new Filtro();
            filtros.dataInicio = dataInicio.SelectedDate;
            filtros.dataFim = dataFim.SelectedDate.Value.AddDays(1).AddSeconds(-1);
            TarefasBLL t = new TarefasBLL();
            filtros.TipoTarefa = "-1";

            var ranking = t.getRanking(t.filtrar(filtros));

            valores.Clear();
            nomes.Clear();

            foreach (var item in ranking)
            {
                valores.Add(item.mediaPorHora);
                nomes.Add(item.nomesFuncionarios);
            }

      
            ColumnSeries colunas = new ColumnSeries
            {
                Title = "Pontos por hora",
                Values = new ChartValues<double>(valores)
            };

            colunas.Stroke = Brushes.Red;
            colunas.Fill = Brushes.Firebrick;

            Colecao = new SeriesCollection
            {    colunas    };

            Rotulos = nomes.ToArray();
            Formatter = value => value.ToString("N");            
            
       
            DataContext = this;
        } 

        public static DateTime comecoDaSemana(DateTime dt)
        {
            while (dt.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                dt = dt.AddDays(-1);
            return dt;
        }

       


    }
}
