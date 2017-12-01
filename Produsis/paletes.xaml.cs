﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{  
    /// <summary>
    /// Lógica interna para paletes.xaml
    /// </summary>
    public partial class paletes : Window
    {      
        public paletes(string capacidade)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);            
            txtTotal.Text = capacidade;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtQtde.Focus();
        }

        public static int[] Perguntar(string capacidade)
        {
            paletes tela = new paletes(capacidade);
            tela.ShowDialog();

            if (tela.DialogResult == true)
                try
                {
                    int[] retornoOK = { int.Parse(tela.txtQtde.Text), int.Parse(tela.txtTotal.Text) };
                    return retornoOK;
                }
                catch
                {
                    int [] retornoErro = { -1, 1 };
                    return retornoErro;
                }
            int[] retorno = { -1, -1 };
            return retorno;
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);            
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtQtde.Text == "")
                txtQtde.Focus();
            else
                if (txtTotal.Text == "")
                txtTotal.Focus();
            else
            {
                if (int.Parse(txtPorcentagem.Text) > 100)
                    MessageBox.Show("A quantidade paletizada deve ser menor ou igual à capacidade de paletes do veículo.", "Erro - Produsis", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void calcularPorcentagem(object sender, TextChangedEventArgs e)
        {
            if (txtQtde.Text != "" && txtTotal.Text != "0" && txtTotal.Text != "")
            {
                var x = int.Parse(txtQtde.Text) / double.Parse(txtTotal.Text);
                txtPorcentagem.Text = string.Format("{0:P2}", x);
            }
            else
                txtPorcentagem.Text = "0,00%";
        }
    }
}
