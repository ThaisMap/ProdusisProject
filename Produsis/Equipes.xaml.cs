using DAL;
using ProdusisBD;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interação lógica para Equipes.xam
    /// </summary>
    public partial class Equipes : UserControl
    {
        public Equipes()
        {
            InitializeComponent();
        }

        private List<Funcionarios> funcionariosAtivos = new List<Funcionarios>();
        private List<string> funcionariosEquipe = new List<string>();

        private FuncionariosBD f = new FuncionariosBD();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            funcionariosAtivos = f.getListaFuncionariosAtivos();
            Nome.ItemsSource = funcionariosAtivos;
            Nome.DisplayMemberPath = "nomeFunc";
        }

        private void Remover(object sender, RoutedEventArgs e)
        {
            var nome = ((Button)sender).CommandParameter.ToString();
            funcionariosEquipe.Remove(nome);
            dgEquipe.Items.Refresh();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (funcionariosEquipe.Count > 1)
            {
                var maxEquipe = funcionariosAtivos.Select(x => x.equipeFunc).DefaultIfEmpty(-1).Max();
                if (maxEquipe == null)
                    maxEquipe = -1;
                if (f.salvaEquipe(funcionariosEquipe, maxEquipe + 1))
                {
                    MessageBox.Show("Equipe salva com sucesso.", "Aviso");
                    funcionariosAtivos = f.getListaFuncionariosAtivos();
                }
            }
        }

        private void Incluir(object sender, RoutedEventArgs e)
        {
            Funcionarios selecionado;
            if (Nome.SelectedIndex > -1)
            {
                selecionado = Nome.SelectedItem as Funcionarios;
                if (selecionado.equipeFunc != null)
                {
                    foreach (var item in funcionariosAtivos)
                    {
                        if (item.equipeFunc == selecionado.equipeFunc && !funcionariosEquipe.Contains(item.nomeFunc))
                            funcionariosEquipe.Add(item.nomeFunc);
                    }
                }
                else
                    funcionariosEquipe.Add((Nome.SelectedItem as Funcionarios).nomeFunc);
            }
            dgEquipe.ItemsSource = funcionariosEquipe;
            dgEquipe.Items.Refresh();
        }

        private void BtnDesfazer_Click(object sender, RoutedEventArgs e)
        {
            if (funcionariosEquipe.Count > 0)
            {
                f.salvaEquipe(funcionariosEquipe, null);
                funcionariosEquipe.Clear();
                dgEquipe.ItemsSource = funcionariosEquipe;
                funcionariosAtivos = f.getListaFuncionariosAtivos();
                dgEquipe.Items.Refresh();
            }
        }
    }
}