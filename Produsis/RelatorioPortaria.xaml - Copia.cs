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
using DAL;
using ProdusisBD;

namespace GUI
{
    /// <summary>
    /// Interação lógica para RelatorioPortaria.xam
    /// </summary>
    public partial class RelatorioPortaria : UserControl
    {
        VeiculosBD vBD = new VeiculosBD();


        public RelatorioPortaria(double Altura, double largura)
        {
            InitializeComponent();
            Height = Altura - 50;
            Width = largura - 50;
            dgAcessos.Height = Altura - 180;
        }

        List<AcessosPortaria> source;

        public List<Veiculos> veiculos { get; private set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            veiculos = vBD.getVeiculos();
            cbNome.ItemsSource = veiculos;
            cbPlaca.ItemsSource = veiculos;
            cbPlaca.DisplayMemberPath = "PlacaVeiculo";
            cbNome.DisplayMemberPath = "MotoristaVeiculo";
            dataFinal.SelectedDate = DateTime.Now;
        }

        private void IndicarDoca_Click(object sender, RoutedEventArgs e)
        {
            AcessosPortaria acesso = dgAcessos.SelectedItem as AcessosPortaria;
            AddDoca.registrarDoca(acesso.idAcesso, acesso.PlacaAcesso);
            BtnPesquisa_Click(sender, e);
        }

        private void BtnPesquisa_Click(object sender, RoutedEventArgs e)
        {
            source = vBD.FiltrarAcessos(montarObjeto());
            dgAcessos.ItemsSource = source;
        }

        private Filtro montarObjeto()
        {
            if (dataFinal.SelectedDate == null)
            {
                dataFinal.SelectedDate = DateTime.Today;
            }
            Filtro filtros = new Filtro()
            {
                placa = cbPlaca.SelectedIndex == -1 ? "" : cbPlaca.Text,
                nomeFuncionario = cbNome.SelectedIndex == -1 ? "" : cbNome.Text,
                dataInicio = dataInicio.SelectedDate,
                dataFim = dataFinal.SelectedDate.Value.AddDays(1).AddSeconds(-1),
                acessoPendente = checkPendentes.IsChecked
            };

            return filtros;
        }
    }
}
