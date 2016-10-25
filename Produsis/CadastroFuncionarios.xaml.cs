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

namespace GUI
{
    /// <summary>
    /// Interaction logic for CadastroFuncionarios.xaml
    /// </summary>
    public partial class CadastroFuncionarios : UserControl
    {
        FuncionarioBLL f = new FuncionarioBLL();

        public CadastroFuncionarios()
        {
            InitializeComponent();
            DataContext = new Produsis.Validacoes.TextFieldsViewModel();
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (checarCampos())
                f.salvarNovo(montarObjeto());
        }
        
        private Funcionarios montarObjeto()
        {
            Funcionarios func = new Funcionarios();
            func.nomeFunc = Nome.Text;
            func.matriculaFunc = Matricula.Text.Replace("_", "");
            func.ocupadoFunc = false;
            func.ativoFunc = (bool)Ativo.IsChecked;
            func.senhaFunc = Senha.Password;
            func.tipoFunc = Tipo.Text;

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

        private void limpar(object sender, RoutedEventArgs e)
        {
            Nome.Text = "";
            Matricula.Text = "";
            Senha.Password = "";
            Senha2.Password = "";
            Tipo.SelectedIndex = -1;
            Ativo.IsChecked = true;
        }
    }
}
