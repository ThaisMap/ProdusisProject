using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for IniciadasHoje.xaml
    /// </summary>
    public partial class IniciadasHoje : UserControl
    {
        private List<TarefaIniciada> ListaIniciadasHoje = new List<TarefaIniciada>();

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

        private void geraRelatorio(object sender, System.Windows.RoutedEventArgs e)
        {
            ProdusisBD.Filtro filtros = new ProdusisBD.Filtro();
            switch (cbTipoRelatorio.SelectedIndex)
            {
                case 0: //hoje
                    filtros.dataInicio = DateTime.Today;
                    filtros.dataFim = DateTime.Today;
                    break;
                case 1://ontem
                    filtros.dataInicio = DateTime.Today.AddDays(-1);
                    filtros.dataFim = DateTime.Today.AddDays(-1);
                    break;
                case 2://esta semana
                    filtros.dataInicio = comecoDaSemana(DateTime.Today);
                    filtros.dataFim = DateTime.Today;
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
                    filtros.dataFim = mesPassado.AddDays(DateTime.DaysInMonth(mesPassado.Year, mesPassado.Month)-1);
                    break;
            }
            TarefasBLL t = new TarefasBLL();
            filtros.TipoTarefa = "-1";
            t.getRanking(t.filtrar(filtros));
        }

        public static DateTime comecoDaSemana (DateTime dt)
        {
            while (dt.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                dt = dt.AddDays(-1);
            return dt;
        }        
    }

    public class TarefaIniciada
    {
        public string Tipo { get; set; }
        public int Finalizadas { get; set; }
        public int Pendentes { get; set; }
    }


}