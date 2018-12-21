using BLL;
using DAL;
using ProdusisBD;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interação lógica para Observacao.xam
    /// </summary>
    public partial class Observacao : UserControl
    {
        private AcessoBD abd = new AcessoBD();
        private Logica bll = new Logica();

        public Observacao()
        {
            InitializeComponent();
            Nome.ItemsSource = abd.GetListaNomesFunc();
            dataObs.SelectedDate = DateTime.Today;
            dataFim.SelectedDate = DateTime.Today;
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            string nome = Nome.SelectedItem.ToString(); // aqui pode ser selected item pq o itemsource é uma lista de strings
            DateTime data = (DateTime)dataObs.SelectedDate;
            string texto = "";
            if (SelectTime.SelectedTime != null)
                texto = SelectTime.Text + " - ";
            texto += TextoObs.Text;

            if (bll.CadastraObservacao(nome, data, texto))
            {
                MessageBox.Show("Observação cadastrada");
            }
            else
                MessageBox.Show("Ocorreu um erro");
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {

            dgObs.ItemsSource = abd.GetObservacoes(dataInicio.SelectedDate, dataFim.SelectedDate);           
        }

        private void DgObs_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && dgObs.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Apagar observação definitivamente? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var linha = dgObs.SelectedItem as Observacoes;
                    abd.DeletarObservacao(linha.idObs);
                }
            }
        }      
    }
}