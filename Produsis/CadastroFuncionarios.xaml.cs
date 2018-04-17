using BLL;
using ProdusisBD;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interaction logic for CadastroFuncionarios.xaml
    /// </summary>
    public partial class CadastroFuncionarios : UserControl
    {
        private FuncionarioBLL f = new FuncionarioBLL();

        public CadastroFuncionarios()
        {
            InitializeComponent();
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (checarCampos())
            {
                if (f.salvarNovo(montarObjeto()))
                {
                    MessageBox.Show("Funcionário cadastrado com sucesso.", "Funcionário cadastrado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpar(sender, e);
                }
                else
                {
                    MessageBox.Show("Funcionário não foi cadastrado. Verifique as informações fornecidas.", "Funcionário não cadastrado - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Funcionário não foi cadastrado. Verifique as informações fornecidas.", "Funcionário não cadastrado - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Funcionarios montarObjeto()
        {
            Funcionarios func = new Funcionarios();
            func.nomeFunc = Nome.Text;
            func.matriculaFunc = Matricula.Text.Replace("_", "");
            func.ocupadoFunc = false;
            func.ativoFunc = (bool)Ativo.IsChecked;
            func.senhaFunc = Senha.Password;

            if (Tipo.SelectedIndex == 0)
                func.tipoFunc = "0";

            else
            {
                if ((bool)CkDescarrega.IsChecked)
                    func.tipoFunc += "1";

                if ((bool)CkConfere.IsChecked)
                    func.tipoFunc += "2";

                if ((bool)CkSepara.IsChecked)
                    func.tipoFunc += "3";

                if ((bool)CkCarrega.IsChecked)
                    func.tipoFunc += "4";
            }

            if (func.tipoFunc == "")
                func.tipoFunc = "1234";
            return func;
        }
         
        private bool checarCampos()
        {
            if (Nome.Text != "" && Matricula.Text != "_____" && Tipo.Text != "" && Senha.Password == Senha2.Password)
            {
                if (Tipo.Text == "Administrativo" && Senha.Password == "")
                    return false;

                //estando tudo certo
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

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}