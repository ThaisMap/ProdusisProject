using BLL;
using ProdusisBD;
using System.Windows;
using System.Windows.Controls;

namespace Produsis
{
    /// <summary>
    /// Interaction logic for EdicaoFuncionarios.xaml
    /// </summary>
    public partial class EdicaoFuncionarios : UserControl
    {
        private FuncionarioBLL f = new FuncionarioBLL();
        private Funcionarios emEdicao;

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
                Senha.Password = "";
                Senha2.Password = "";
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
            if (f.validarSenha(emEdicao.matriculaFunc, Senha.Password) && checarCampos())
            {
                Funcionarios novosDados = montarObjeto();
                if (f.editar(novosDados))
                    MessageBox.Show("Funcionário editado com sucesso.", "Funcionário editado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);

                limpar(sender, e);
            }
            else
            {
                MessageBox.Show("Funcionário não foi editado. Verifique as informações fornecidas.", "Funcionário editado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
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