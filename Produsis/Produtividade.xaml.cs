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
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        List<double> valores = new List<double>();
        List<string> nomes = new List<string>();

        public Produtividade()
        {
            InitializeComponent();

            

        }


        private void geraRelatorio(object sender, System.Windows.RoutedEventArgs e)
        {
            ProdusisBD.Filtro filtros = new ProdusisBD.Filtro();
            switch (cbTipoRelatorio.SelectedIndex)
            {
                case 0: //hoje
                    filtros.dataInicio = DateTime.Today;
                    filtros.dataFim = DateTime.Today.AddDays(1).AddSeconds(-1);
                    break;
                case 1://ontem
                    filtros.dataInicio = DateTime.Today.AddDays(-1);
                    filtros.dataFim = DateTime.Today.AddDays(-1);
                    break;
                case 2://esta semana
                    filtros.dataInicio = comecoDaSemana(DateTime.Today);
                    filtros.dataFim = DateTime.Today.AddDays(1).AddSeconds(-1);
                    break;
                case 3://semana passada
                    filtros.dataInicio = comecoDaSemana(DateTime.Today.AddDays(-7));
                    filtros.dataFim = comecoDaSemana(DateTime.Today).AddDays(-1);
                    break;
                case 4:// este mes
                    filtros.dataInicio = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                    filtros.dataFim = DateTime.Today;
                    break;
                default://mes passado
                    var mesPassado = DateTime.Today.AddDays(-DateTime.Today.Day + 1).AddMonths(-1);
                    filtros.dataInicio = mesPassado;
                    filtros.dataFim = mesPassado.AddDays(DateTime.DaysInMonth(mesPassado.Year, mesPassado.Month) - 1);
                    break;
            }
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

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Pontos por hora",
                    Values = new ChartValues<double> (valores)

                }
            };
            Labels = nomes.ToArray();
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
