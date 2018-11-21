using System.Collections.Generic;
using System.Windows.Controls;
using DAL;
using ProdusisBD;

namespace GUI
{
    /// <summary>
    /// Interação lógica para CadastroCarretas.xam
    /// </summary>
    public partial class CadastroCarretas : UserControl
    {
        List<Carretas> lista;
        VeiculosBD vBD = new VeiculosBD();
        public CadastroCarretas()
        {
            InitializeComponent();
            VeiculosBD vBD = new VeiculosBD();
            lista = vBD.getCarretas();
            dgLista.ItemsSource = lista;
        }

        private Carretas MontarObjeto ()
        {
            Carretas carro = new Carretas()
            {
                PlacaCarreta = txtPlaca2.Text,
                Ativo = true
            };

            return carro;
        }

        private void ToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Carretas atual = dgLista.SelectedItem as Carretas;
            atual.Ativo = !atual.Ativo;
            vBD.cadastrarCarretas(atual);
        }

        private void btAddPlaca_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            vBD.cadastrarCarretas(MontarObjeto());
        }
    }
}