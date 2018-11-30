using BLL;
using DAL;
using Portaria.Properties;
using System.Windows;

namespace Portaria
{
    /// <summary>
    /// Lógica interna para LoginPortaria.xaml
    /// </summary>
    public partial class LoginPortaria : Window
    {
        public LoginPortaria()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (VerificaCampos())
            {
                AcessoBD abd = new AcessoBD();
                if (abd.UsuarioExiste(TxbLogin.Text))
                {
                    if (abd.SenhaCorreta(TxbLogin.Text, TxbSenha.Password))
                    {
                        var usuario = abd.GetFuncPorMatricula(TxbLogin.Text);
                        Login.Default.idUsuario = usuario.idFunc;
                        Login.Default.NomeUsuario = usuario.nomeFunc;

                        MainWindow view = new MainWindow();
                        this.Close();
                        view.Show();
                    }
                    else
                    {
                        MessageBox.Show("Senha incorreta.", "Login - Produsis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        TxbSenha.Focus();
                    }
                }
                else
                    TxbLogin.Focus();
            }
        }

        private bool VerificaCampos()
        {
            //verificar se tem coisas digitadas nos dois campos
            if (TxbLogin.Text == "" || TxbLogin.Text.Length < 4)
            {
                MessageBox.Show("Digite login e senha", "Login - Produsis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                TxbLogin.Focus();
                return false;
            }
            if (TxbSenha.Password == "")
            {
                MessageBox.Show("Digite sua senha", "Login - Produsis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                TxbSenha.Focus();
                return false;
            }
            return true;
        }
    }
}