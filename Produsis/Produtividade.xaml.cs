using BLL;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using LiveCharts;
using LiveCharts.Wpf;
using ProdusisBD;
using System.Windows.Input;

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

        public Produtividade(double Altura, double largura)
        {
            InitializeComponent();
            dgRanking.Height = Altura - 150 - 100;
        }

        private void geraRelatorioTabela(object sender, RoutedEventArgs e)
        {
            Cursor _cursorAnterior = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
            Filtro filtros = new Filtro()
            {
                dataInicio = dataInicio.SelectedDate,
                dataFim = dataFim.SelectedDate.Value.AddDays(1).AddSeconds(-1),
                TipoTarefa = "2"
            };
            TarefasBLL t = new TarefasBLL();
           
            var ranking = t.getRanking(filtros);

            dgRanking.ItemsSource = ranking;
            Mouse.OverrideCursor = _cursorAnterior;
        }

        private void geraRelatorio(object sender, RoutedEventArgs e)
        {
            Filtro filtros = new Filtro()
            {
                dataInicio = dataInicio.SelectedDate,
                dataFim = dataFim.SelectedDate.Value.AddDays(1).AddSeconds(-1),
                TipoTarefa = "2"
            };
            TarefasBLL t = new TarefasBLL();

            int dias = (filtros.dataFim - filtros.dataInicio).Value.Days + 1;

            var ranking = t.getRanking(filtros);

            valores.Clear();
            nomes.Clear();

            foreach (var item in ranking)
            {
                valores.Add(item.mediaPorHora);
                nomes.Add(item.nomesFuncionarios);
            }
      
            ColumnSeries colunas = new ColumnSeries
            {
                Title = "Pontos Acumulados",
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
