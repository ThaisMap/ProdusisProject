using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Descarga.xaml
    /// </summary>
    public partial class Descarga : UserControl
    {
        public FuncionariosTag FuncionarioSelecionado;
        public List<String> ListaFunc;
        public FuncionarioBLL f = new FuncionarioBLL();
        public ObservableCollection<FuncionariosTag> Funcionario { get; set; }


        public Descarga()
        {

            InitializeComponent();
            ListaFunc = f.carregaFuncionarios();
            ListaFunc.Add("Paola Oliveira");
            ListaFunc.Add("Jose Silva");
            ListaFunc.Add("Astolfo Mexicano");
            ListaFunc.Add("Loconauta Mexicano Gonzalez");

            CBFuncionario.ItemsSource = ListaFunc;
        }

        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = new string[7];
            PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }

        private void ChipEx_DeleteClick(object sender, RoutedEventArgs e)
        {
        }

        private void Inserir_Click(object sender, RoutedEventArgs e)
        {
            ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
        }

        private void CBFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FuncionarioSelecionado = new FuncionariosTag(CBFuncionario.SelectedItem.ToString(), CriaChipTag(CBFuncionario.SelectedItem.ToString()));
        }
    }
}
