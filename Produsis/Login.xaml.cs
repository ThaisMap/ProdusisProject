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
using System.Windows.Shapes;
using BLL;

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
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (verificaCampos())
            {
                BLL.LoginBLL l = new BLL.LoginBLL();
                if (l.validarUsuario(TxbLogin.Text))
                {
                    if (l.validarSenha(TxbLogin.Text, TxbSenha.Password))
                    {
                        l.logarFuncionario(TxbLogin.Text);
                        TelaPrincipal view = new TelaPrincipal();
                        this.Close();
                        view.Show();
                    }
                    // se a senha nao for igual, pisca o password box
                }
                // se o usuário nao for válido, pisca o text box
            }
            // se os campos nao estiverem preenchidos, avisa que tem q preencher
           
        }

        private bool verificaCampos()
        {
            //verificar se tem coisas digitadas nos dois campos
            return true;
        }
    }
}
