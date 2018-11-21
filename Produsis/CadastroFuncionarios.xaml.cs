using BLL;
using ProdusisBD;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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

            if ((bool)CkAdmin.IsChecked)
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

                if ((bool)CkEmpilha.IsChecked)
                    func.tipoFunc += "5";
            }

            if (func.tipoFunc == "")
                func.tipoFunc = "12345";
            return func;
        }

        private bool checarCampos()
        {
            if (Nome.Text != "" && Matricula.Text != "_____" && Senha.Password == Senha2.Password)
            {
                if (CkAdmin.IsChecked == true && Senha.Password == "")
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
            CkAdmin.IsChecked = false;
            CkCarrega.IsChecked = false;
            CkConfere.IsChecked = false;
            CkDescarrega.IsChecked = false;
            CkSepara.IsChecked = false;
            CkEmpilha.IsChecked = false;
            Ativo.IsChecked = true;
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CkAdmin_Checked(object sender, RoutedEventArgs e)
        {
            if (CkAdmin.IsChecked == true)
            {
                CkCarrega.IsChecked = false;
                CkConfere.IsChecked = false;
                CkDescarrega.IsChecked = false;
                CkSepara.IsChecked = false;
                CkEmpilha.IsChecked = false;
            }
        }
    }
}