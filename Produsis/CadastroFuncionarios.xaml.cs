using BLL;
using DAL;
using ProdusisBD;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for CadastroFuncionarios.xaml
    /// </summary>
    public partial class CadastroFuncionarios : UserControl
    {
        private AcessoBD abd = new AcessoBD();
        private bool isEditing = false;
        Funcionarios emEdicao = new Funcionarios();
        public CadastroFuncionarios()
        {
            InitializeComponent();
            CbCadastrados.ItemsSource = abd.GetListaNomesFunc();
        }

        private void Nome_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (ChecarCampos())
            {
                if (isEditing)
                {
                    if (abd.SenhaCorreta(emEdicao.matriculaFunc, Senha.Password))
                    {
                        if (abd.EditarFuncionario(MontarObjeto()))
                        {
                            MessageBox.Show("Funcionário editado com sucesso.", "Funcionário editado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);

                            Limpar_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Funcionário não foi editado. Verifique as informações fornecidas.", "Funcionário editado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                            Senha.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Funcionário não foi editado. Verifique as informações fornecidas.", "Funcionário editado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                        Senha.Focus();
                    }
                }
                else
                {
                    if (abd.CadastrarFuncionario(MontarObjeto()))
                    {
                        MessageBox.Show("Funcionário cadastrado com sucesso.", "Funcionário cadastrado - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                        Limpar_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Funcionário não foi cadastrado. Verifique as informações fornecidas.", "Funcionário não cadastrado - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Verifique as informações fornecidas.", "Operação não realizada - Produsis", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Funcionarios MontarObjeto()
        {
            Funcionarios func = new Funcionarios
            {
                nomeFunc = Nome.Text,
                matriculaFunc = Matricula.Text.Replace("_", ""),
                ocupadoFunc = false,
                ativoFunc = (bool)Ativo.IsChecked
            };

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

            if (isEditing)
            {
                if (Senha2.Password == "")
                    func.senhaFunc = Senha.Password;
                else
                    func.senhaFunc = Senha2.Password;
            }
            else
                func.senhaFunc = Senha.Password;

            return func;
        }

        private bool ChecarCampos()
        {
            if (Nome.Text == "")
            {
                Nome.Focus();
                return false;
            }
            if (Matricula.Text == "")
            {
                Matricula.Focus();
                return false;
            }
            //se nenhuma função estiver selecionada
            if (CkAdmin.IsChecked == false && CkPortaria.IsChecked == false &&
                CkCarrega.IsChecked == false && CkConfere.IsChecked == false &&
                CkDescarrega.IsChecked == false && CkEmpilha.IsChecked == false &&
                CkSepara.IsChecked == false)
                return false;

            if (CkAdmin.IsChecked == true && Senha.Password == "")
            {
                Senha.Focus();
                return false;
            }

            if (!isEditing)
            {
                if (Senha.Password != Senha2.Password)
                {
                    Senha2.Focus();
                    return false;
                }
            }

            return true;
        }

        private void Limpar_Click(object sender, RoutedEventArgs e)
        {
            isEditing = false;
            CbCadastrados.SelectedIndex = -1;
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

        private void TestarCaractere(object sender, TextCompositionEventArgs e)
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

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (CbCadastrados.SelectedIndex > -1)
            {
                emEdicao = abd.GetFuncPorNome(CbCadastrados.SelectedItem.ToString());

                Matricula.Text = emEdicao.matriculaFunc;
                Nome.Text = emEdicao.nomeFunc;
                CkAdmin.IsChecked = emEdicao.tipoFunc.Contains("0");
                CkDescarrega.IsChecked = emEdicao.tipoFunc.Contains("1");
                CkConfere.IsChecked = emEdicao.tipoFunc.Contains("2");
                CkSepara.IsChecked = emEdicao.tipoFunc.Contains("3");
                CkCarrega.IsChecked = emEdicao.tipoFunc.Contains("4");
                CkEmpilha.IsChecked = emEdicao.tipoFunc.Contains("5");
                Ativo.IsChecked = emEdicao.ativoFunc;
                Senha.Password = "";
                Senha2.Password = "";

                isEditing = true;
            }
        }
    }
}