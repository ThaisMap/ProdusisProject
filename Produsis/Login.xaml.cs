using BLL;
using DAL;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            MudarCores iniciarTema = new MudarCores();
            TabIndex = 0;
            TxbLogin.Focus();
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
                        TelaPrincipal view = new TelaPrincipal();
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

        private static void ImportarArquivos()
        {
            xmlBLL x = new xmlBLL();
            x.triagemArquivos();
        }
    }
}