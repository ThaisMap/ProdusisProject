using BLL;
using System.Windows;
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Thread t = new Thread(lerXmls);
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            if (verificaCampos())
            {
                LoginBLL l = new LoginBLL();
                if (l.validarUsuario(TxbLogin.Text))
                {
                    if (l.validarSenha(TxbLogin.Text, TxbSenha.Password))
                    {
                        //t.Start();
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

        private bool verificaCampos()
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

        static void lerXmls()
        {
            xmlBLL x = new xmlBLL();
            x.lerXML();
        }
    }
}