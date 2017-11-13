﻿using BLL;
using System.Windows;
using System.Threading;

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
            if (verificaCampos())
            {
                FuncionarioBLL l = new FuncionarioBLL();
                if (l.validarUsuario(TxbLogin.Text))
                {
                    if (l.validarSenha(TxbLogin.Text, TxbSenha.Password))
                    {

                        TelaPrincipal view = new TelaPrincipal();
                        if (l.tipoFuncionario(TxbLogin.Text) == "1")
                        {
                            view.BtnAdministrativo.Visibility = Visibility.Collapsed;
                            view.BtnDivergencia.Visibility = Visibility.Collapsed;
                        }
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

        static void importarArquivos()
        {
            xmlBLL x = new xmlBLL();
            x.triagemArquivos();
        }
    }
}