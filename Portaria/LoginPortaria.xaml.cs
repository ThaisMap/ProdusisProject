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
using DAL;
using Portaria.Properties;
using Portaria;

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
            if (verificaCampos())
            {
                FuncionarioBLL l = new FuncionarioBLL();
                if (l.validarUsuario(TxbLogin.Text))
                {
                    if (l.validarSenha(TxbLogin.Text, TxbSenha.Password))
                    {

                             FuncionariosBD fBD = new FuncionariosBD();
                        var usuario = fBD.getFuncPorMatricula(TxbLogin.Text);
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
    }
}
