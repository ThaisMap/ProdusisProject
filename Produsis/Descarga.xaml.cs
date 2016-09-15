﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Produsis
{
    /// <summary>
    /// Interaction logic for Descarga.xaml
    /// </summary>
    public partial class Descarga : UserControl
    {
        public Funcionarios FuncionarioSelecionado;
        public Descarga()
        {
            InitializeComponent();
            Funcionario = new ObservableCollection<Produsis.Funcionarios>();
            Funcionario.Add(new Funcionarios { Nome = "Paola Oliveira", Tag = CriaChipTag("Paola Oliveira") });
            Funcionario.Add(new Funcionarios { Nome = "Jose Silva", Tag = CriaChipTag("Jose Silva") });
            Funcionario.Add(new Funcionarios { Nome = "Astolfo Mexicano", Tag = CriaChipTag("Astolfo Mexicano") });
            Funcionario.Add(new Funcionarios { Nome = "Dutra Mexicano", Tag = CriaChipTag("Dutra Mexicano") });
            Funcionario.Add(new Funcionarios { Nome = "Loconauta Mexicano Gonzalez", Tag = CriaChipTag("Astolfo Zicado") });

            //ListaDeFuncionarios.ItemsSource = Funcionario;
            CBFuncionario.ItemsSource = Funcionario;
        }

        public ObservableCollection<Funcionarios> Funcionario { get; set; }
        public static string CriaChipTag(string Nome)
        {
            string[] PrimeirosNomes = new string[7];
            PrimeirosNomes = Nome.Split(' ');
            return PrimeirosNomes[0].Substring(0, 1).ToUpper() + PrimeirosNomes[1].Substring(0, 1).ToUpper();
        }

        private void ChipEx_DeleteClick(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListaDeFuncionarios.Items.Add(FuncionarioSelecionado);
        }

        private void CBFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Funcionarios Funcionario = (Funcionarios)CBFuncionario.SelectedItem;
            FuncionarioSelecionado = new Funcionarios {Nome = Funcionario.Nome , Tag = Funcionario.Tag};
        }
    }
}
