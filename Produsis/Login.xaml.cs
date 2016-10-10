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
            /* xmlBLL x = new xmlBLL();
             x.lerXML();
             
             */
            if (verificaCampos())
            {
                TelaPrincipal view = new TelaPrincipal();
                this.Close();
                view.Show();
                /*
                LoginBLL l = new LoginBLL();
                if (l.validarUsuario(TxbLogin.Text))
                {
                    if (l.validarSenha(TxbLogin.Text, TxbSenha.Password))
                    {
                        l.logarFuncionario(TxbLogin.Text);
                        TelaPrincipal view = new TelaPrincipal();
                        this.Close();
                        view.Show();
                    }
                    else
                        TxbSenha.Focus();
                }
                else
                    TxbLogin.Focus();*/
            }
        }

        private bool verificaCampos()
        {
            //verificar se tem coisas digitadas nos dois campos
            if(TxbLogin.Text == ""||TxbLogin.Text.Length<4)
            {
                TxbLogin.Focus();
                return false;
            }
            if (TxbSenha.Password == "")
            {
                TxbSenha.Focus();
                return false;
            }
            return true;
        }
    }
}
