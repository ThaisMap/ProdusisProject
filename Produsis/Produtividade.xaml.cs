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
        public List<ItemRanking> Ranking { get; set; }

        public Produtividade(double Altura, double largura)
        {
            InitializeComponent();
            dgRanking.Height = Altura - 250;
        }

        private void geraRelatorioTabela(object sender, RoutedEventArgs e)
        {
            if (cbTipoTarefa.SelectedIndex >= 0 && (dataInicio.SelectedDate != null || dataFim.SelectedDate != null))
            {
                if (dataInicio.SelectedDate != null && dataFim.SelectedDate != null && dataInicio.SelectedDate <= dataFim.SelectedDate)
                {
                    Cursor _cursorAnterior = Mouse.OverrideCursor;
                    Mouse.OverrideCursor = Cursors.Wait;
                    Filtro filtros = new Filtro()
                    {
                        dataInicio = dataInicio.SelectedDate,
                        dataFim = dataFim.SelectedDate.Value.AddDays(1).AddSeconds(-1),
                        TipoTarefa = (cbTipoTarefa.SelectedIndex + 1).ToString()
                    };

                    TarefasBLL t = new TarefasBLL();

                    Ranking = t.GetRanking(filtros);

                    dgRanking.ItemsSource = Ranking;
                    Mouse.OverrideCursor = _cursorAnterior;
                }
            }
        }

     
        public static DateTime comecoDaSemana(DateTime dt)
        {
            while (dt.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                dt = dt.AddDays(-1);
            return dt;
        }

       


    }
}
