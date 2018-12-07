using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interação lógica para Divergencias.xam
    /// </summary>
    public partial class DivergenciasNovo : UserControl
    {
        List<Divergencias> lista = new List<Divergencias>();
        int idTarefa = 0;
        bool isEditing = false;

        AcessoBD abd = new AcessoBD();
        List<string> itensCombo = new List<string>();


        public DivergenciasNovo(double actualHeight, double actualWidth)
        {
            InitializeComponent();
            Height = actualHeight - 100;
            Width = actualWidth - 60;
            itensCombo.Add("Falta");
            itensCombo.Add("Sobra");
            itensCombo.Add("Avaria");
            itensCombo.Add("Outro");
            ColunaTipo.ItemsSource = itensCombo;
            dgOcorrencias.Height = Height - 300;
        }

        private void TestarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ReloadDataGrid()
        {
            foreach (var item in lista)
            {
                item.TipoDivergencia = Tipo(item.TipoDivergencia);
            }

            dgOcorrencias.ItemsSource = lista;
        }

        private void LimparTela()
        {
            Codigo.Text = "";
            Quantidade.Text = "";
            txtFuncionario.Text = "";
            cbTipoDivergencia.SelectedIndex = 0;
            lista = new List<ProdusisBD.Divergencias>();
            dgOcorrencias.ItemsSource = lista;
        }

        private void Consultar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LimparTela();
                if (cbTipoTarefa.SelectedIndex > -1 && Documento.Text != "")
                {
                    lista = abd.GetNovaDivergencia(int.Parse(Documento.Text), cbTipoTarefa.SelectedIndex + 1);

                    if (lista.Count > 0)
                    {
                        idTarefa = lista[0].TarefaDivergencia;
                        txtFuncionario.Text = abd.GetNomesFuncTarefa(lista[0].TarefaDivergencia);
                    }
                    else
                    {
                        var item = abd.GetTarefaDivergencia(cbTipoTarefa.SelectedIndex + 1, int.Parse(Documento.Text));
                        idTarefa = item.idTarefa;
                        txtFuncionario.Text = item.nomesFuncionarios;
                    }
                    ReloadDataGrid();
                }
            }

            catch (Exception)
            {
            }
        }

        private void Incluir_Click(object sender, RoutedEventArgs e)
        {
            if (ChecarCampos())
            {
                abd.CadastrarNovaDivergencia(MontarObjeto());
                Consultar_Click(sender, e);
            }
        }

        private bool ChecarCampos()
        {
            bool retorno = true;

            if (Codigo.Text == "")
                return false;

            if (cbTipoDivergencia.SelectedIndex == 3 && Quantidade.Text == "")
                return false;

            if (idTarefa == 0)
                return false;

            return retorno;
        }

        private Divergencias MontarObjeto()
        {
            ProdusisBD.Divergencias div = new ProdusisBD.Divergencias()
            {
                TarefaDivergencia = idTarefa,
                QtdeDivergencia = Quantidade.Text,
                TextoDivergencia = Codigo.Text,
                TipoDivergencia = (cbTipoDivergencia.SelectedIndex + 1).ToString()
            };
            return div;
        }

        private void DgOcorrencias_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isEditing = true;
        }

        private void Registrar_Alteracao(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit) //caso a alteração seja confirmada
            {
                //para descobrir qual propriedade esta ligada com a coluna alterada
                if (e.Column is DataGridBoundColumn column)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path; //esse é o nome da propriedade

                    ProdusisBD.Divergencias alterado = e.Row.Item as ProdusisBD.Divergencias;  //o item da coleção correspondente a linha alterada
                    var el = e.EditingElement as TextBox;       //o campo alterado
                    alterado.TipoDivergencia = Tipo(alterado.TipoDivergencia);
                    switch (bindingPath)
                    {
                        case "QtdeDivergencia":
                            alterado.QtdeDivergencia = el.Text;
                            break;

                        case "TextoDivergencia":
                            alterado.TextoDivergencia = el.Text;
                            break;

                        case "TipoDivergencia":
                            alterado.TipoDivergencia = Tipo(el.Text);
                            break;
                    }
                    abd.CadastrarNovaDivergencia(alterado);
                }
            }
            isEditing = false;
        }

        private string Tipo(string oldTipo)
        {
            switch (oldTipo)
            {
                case "1":
                    return "Falta";

                case "2":
                    return "Sobra";

                case "3":
                    return "Avaria";

                case "4":
                    return "Outro";

                case "Falta":
                    return "1";

                case "Sobra":
                    return "2";

                case "Avaria":
                    return "3";

                default: //"Outro":
                    return "4";
            }
        }

        private void DgOcorrencias_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && dgOcorrencias.SelectedItems.Count > 0 && !isEditing)
            {
                if (MessageBox.Show("Apagar divergência? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var linha = dgOcorrencias.SelectedItem as ProdusisBD.Divergencias;
                    abd.ApagarNovaDivergencia(linha.idDivergencia);
                    Consultar_Click(sender, e);

                }
            }
        }

        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (dgOcorrencias.SelectedItem != null)
            {
                var selecao = combobox.SelectedIndex + 1;

                var alterado = dgOcorrencias.SelectedItem as ProdusisBD.Divergencias;

                alterado.TipoDivergencia = selecao.ToString();
                abd.CadastrarNovaDivergencia(alterado);
            }
        }

    }
}