using BLL;
using DAL;
using ProdusisBD;
using System.Collections.Generic;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Lógica interna para AddFuncionario.xaml
    /// </summary>
    public partial class AddFuncionario : Window
    {
        private List<Funcionarios> ListaFunc;
        private FuncionarioBLL f = new FuncionarioBLL();
        private TarefasBD tbd = new TarefasBD();
        private int idDescarga;

        public AddFuncionario(int idTarefa)
        {
            InitializeComponent();
            idDescarga = idTarefa;
            CBFuncionario.DisplayMemberPath = "nomeFunc";
            CBFuncionario.SelectedValuePath = "idFunc";
            ListaFunc = f.carregaFuncionariosLivres("1"); // 1 = Descarga
            CBFuncionario.ItemsSource = ListaFunc;
        }

        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            if (CBFuncionario.SelectedIndex > -1)
            {
                tbd.IncluirFunc_Tarefa((int)CBFuncionario.SelectedValue, idDescarga);
                this.Close();
            }
            else
                CBFuncionario.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}