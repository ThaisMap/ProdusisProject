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
        private AcessoBD abd = new AcessoBD();
        private List<Funcionarios> ListaFunc;
        private FuncionarioBLL f = new FuncionarioBLL();
        private int idDescarga;

        public AddFuncionario(int idTarefa)
        {
            InitializeComponent();
            idDescarga = idTarefa;
            CBFuncionario.DisplayMemberPath = "nomeFunc";
            CBFuncionario.SelectedValuePath = "idFunc";
            ListaFunc = abd.GetFuncionariosLivres("1");
            CBFuncionario.ItemsSource = ListaFunc;
        }

        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            if (CBFuncionario.SelectedIndex > -1)
            {
                abd.CadastrarFunc_Tarefa((int)CBFuncionario.SelectedValue, idDescarga);
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