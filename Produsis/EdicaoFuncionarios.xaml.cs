using BLL;
using ProdusisBD;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;

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

                CkAdmin.IsChecked = emEdicao.tipoFunc.Contains("0");
                CkDescarrega.IsChecked = emEdicao.tipoFunc.Contains("1");
                CkConfere.IsChecked = emEdicao.tipoFunc.Contains("2");
                CkSepara.IsChecked = emEdicao.tipoFunc.Contains("3");
                CkCarrega.IsChecked = emEdicao.tipoFunc.Contains("4");
                CkEmpilha.IsChecked = emEdicao.tipoFunc.Contains("5");

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
            CkAdmin.IsChecked = false;
            CkCarrega.IsChecked = false;
            CkConfere.IsChecked = false;
            CkDescarrega.IsChecked = false;
            CkSepara.IsChecked = false;
            CkEmpilha.IsChecked = false;
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
            if (CkAdmin.IsChecked == true)
            {
                func.tipoFunc = "0";
                if (Senha2.Password != "")
                    func.senhaFunc = Senha2.Password;
                else
                    func.senhaFunc = emEdicao.senhaFunc;
            }
            else
            {
                func.senhaFunc = "";

                if ((bool)CkDescarrega.IsChecked)
                    func.tipoFunc += "1";

                if ((bool)CkConfere.IsChecked)
                    func.tipoFunc += "2";

                if ((bool)CkSepara.IsChecked)
                    func.tipoFunc += "3";

                if ((bool)CkCarrega.IsChecked)
                    func.tipoFunc += "4";

                if ((bool)CkEmpilha.IsChecked)
                    func.tipoFunc += "5";
            }

            if (func.tipoFunc == "")
                func.tipoFunc = "5";
            //Se não for selecionado nem administrativo nem nenhuma tarefa o funcionario é cadastrado sem acesso a nada
            return func;
        }

        private bool checarCampos()
        {
            if (Nome.Text != "" && Matricula.Text != "_____")
            {
                if (CkAdmin.IsChecked==true && Senha2.Password == "")
                    return false;
                return true;
            }
            return false;
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CkAdmin_Checked(object sender, RoutedEventArgs e)
        {
            if(CkAdmin.IsChecked == true)
            {
                CkCarrega.IsChecked = false;
                CkConfere.IsChecked = false;
                CkDescarrega.IsChecked = false;
                CkSepara.IsChecked = false;
            }
        }
    }
}