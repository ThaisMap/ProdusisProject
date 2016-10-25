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
using BLL;
using ProdusisBD;

namespace Produsis
{
    /// <summary>
    /// Interaction logic for EdicaoFuncionarios.xaml
    /// </summary>
    public partial class EdicaoFuncionarios : UserControl
    {
        FuncionarioBLL f = new FuncionarioBLL();
        Funcionarios emEdicao;
        public EdicaoFuncionarios()
        {
            InitializeComponent();
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            Nome.ItemsSource = f.carregaFuncionarios();
        }

        private void Nome_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Nome.SelectedIndex > -1)
            {
                emEdicao = f.dadosFuncionario(Nome.SelectedItem.ToString());
                Matricula.Text = emEdicao.matriculaFunc;
                int aux = 1;
                bool tentaTipo = int.TryParse(emEdicao.tipoFunc, out aux);
                Tipo.SelectedIndex = aux;
                Ativo.IsChecked = emEdicao.ativoFunc;
            }
        }

        private void limpar(object sender, RoutedEventArgs e)
        {
            Nome.SelectedIndex = -1;
            Matricula.Text = "";
            Senha.Password = "";
            Senha2.Password = "";
            Tipo.SelectedIndex = -1;
            Ativo.IsChecked = true;
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (f.validarSenha(emEdicao.matriculaFunc, Senha.Password))
            {
                Funcionarios novosDados = montarObjeto();
                f.editar(novosDados);
            }
            else
            {
                Senha.Focus();
            }
        }


        private Funcionarios montarObjeto()
        {
            Funcionarios func = new Funcionarios();
            func.idFunc = emEdicao.idFunc;
            func.nomeFunc = Nome.Text;
            func.matriculaFunc = Matricula.Text.Replace("_", "");
            func.ocupadoFunc = false;
            func.ativoFunc = (bool)Ativo.IsChecked;
            func.tipoFunc = Tipo.SelectedIndex.ToString();
            if (Senha2.Password != "")
                func.senhaFunc = Senha2.Password;

            else
                func.senhaFunc = emEdicao.senhaFunc;

            return func;
        }

        private bool checarCampos()
        {
            if (Nome.Text != "" && Matricula.Text != "_____" && Senha.Password != "" && Tipo.Text != "")
            {
                return true;
            }
            return false;
        }
                   
    }
}
