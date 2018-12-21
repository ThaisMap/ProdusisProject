using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interação lógica para MudarTema.xam
    /// </summary>
    public partial class MudarTema : UserControl
    {
        private List<Cores> TemasGlobais = new List<Cores>();
        private List<Cores> CoresDeTema = new List<Cores>();
        private List<Cores> CoresDestaques = new List<Cores>();

        public MudarTema()
        {
            InitializeComponent();
            GerarListas();
            cbxTema.ItemsSource = TemasGlobais;
            cbxCor.ItemsSource = CoresDeTema;
            cbxDestaque.ItemsSource = CoresDestaques;
            cbxTema.DisplayMemberPath = "CorNome";
            cbxCor.DisplayMemberPath = "CorNome";
            cbxDestaque.DisplayMemberPath = "CorNome";
            cbxTema.SelectedItem = TemasGlobais.Single(x => x.CorXaml == MudarCores.Fundo);
            cbxCor.SelectedItem = CoresDeTema.Single(x => x.CorXaml == MudarCores.Cor);
            cbxDestaque.SelectedItem = CoresDestaques.Single(x => x.CorXaml == MudarCores.Destaque);
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            MudarCores.MudarCor();
            DAL.Properties.Perfumaria.Default.Cor = MudarCores.Cor;
            DAL.Properties.Perfumaria.Default.Tema = MudarCores.Fundo;
            DAL.Properties.Perfumaria.Default.Destaque = MudarCores.Destaque;
            DAL.Properties.Perfumaria.Default.Save();
        }

        private void GerarListas()
        {
            #region Tema

            TemasGlobais.Add(new Cores { CorNome = "Claro", CorXaml = "Light" });
            TemasGlobais.Add(new Cores { CorNome = "Escuro", CorXaml = "Dark" });

            #endregion Tema

            #region Destaques

            CoresDestaques.Add(new Cores { CorNome = "Vermelho", CorXaml = "Red" });
            CoresDestaques.Add(new Cores { CorNome = "Azul", CorXaml = "Blue" });
            CoresDestaques.Add(new Cores { CorNome = "Indigo", CorXaml = "Indigo" });
            CoresDestaques.Add(new Cores { CorNome = "Roxo", CorXaml = "Purple" });
            CoresDestaques.Add(new Cores { CorNome = "Amarelo", CorXaml = "Yellow" });
            CoresDestaques.Add(new Cores { CorNome = "Laranja", CorXaml = "Orange" });
            CoresDestaques.Add(new Cores { CorNome = "Verde", CorXaml = "Green" });
            CoresDestaques.Add(new Cores { CorNome = "Lima", CorXaml = "Lime" });
            CoresDestaques.Add(new Cores { CorNome = "Rosa", CorXaml = "Pink" });
            CoresDestaques.Add(new Cores { CorNome = "Laranja Escuro", CorXaml = "DeepOrange" });
            CoresDestaques.Add(new Cores { CorNome = "Azul Claro", CorXaml = "LightBlue" });
            CoresDestaques.Add(new Cores { CorNome = "Ambar", CorXaml = "Amber" });
            CoresDestaques.Add(new Cores { CorNome = "Verde Petróleo", CorXaml = "Teal" });
            CoresDestaques.Add(new Cores { CorNome = "Ciano", CorXaml = "Cyan" });
            CoresDestaques.Add(new Cores { CorNome = "Roxo Escuro", CorXaml = "DeepPurple" });
            CoresDestaques.Add(new Cores { CorNome = "Verde Claro", CorXaml = "LightGreen" });

            #endregion Destaques

            #region Cores

            CoresDeTema.Add(new Cores { CorNome = "Vermelho", CorXaml = "Red" });
            CoresDeTema.Add(new Cores { CorNome = "Azul", CorXaml = "Blue" });
            CoresDeTema.Add(new Cores { CorNome = "Indigo", CorXaml = "Indigo" });
            CoresDeTema.Add(new Cores { CorNome = "Roxo", CorXaml = "Purple" });
            CoresDeTema.Add(new Cores { CorNome = "Amarelo", CorXaml = "Yellow" });
            CoresDeTema.Add(new Cores { CorNome = "Laranja", CorXaml = "Orange" });
            CoresDeTema.Add(new Cores { CorNome = "Verde", CorXaml = "Green" });
            CoresDeTema.Add(new Cores { CorNome = "Lima", CorXaml = "Lime" });
            CoresDeTema.Add(new Cores { CorNome = "Laranja Escuro", CorXaml = "DeepOrange" });
            CoresDeTema.Add(new Cores { CorNome = "Azul Claro", CorXaml = "LightBlue" });
            CoresDeTema.Add(new Cores { CorNome = "Ambar", CorXaml = "Amber" });
            CoresDeTema.Add(new Cores { CorNome = "Rosa", CorXaml = "Pink" });
            CoresDeTema.Add(new Cores { CorNome = "Verde Petróleo", CorXaml = "Teal" });
            CoresDeTema.Add(new Cores { CorNome = "Ciano", CorXaml = "Cyan" });
            CoresDeTema.Add(new Cores { CorNome = "Roxo Escuro", CorXaml = "DeepPurple" });
            CoresDeTema.Add(new Cores { CorNome = "Verde Claro", CorXaml = "LightGreen" });
            CoresDeTema.Add(new Cores { CorNome = "Cinza Azulado", CorXaml = "BlueGrey" });
            CoresDeTema.Add(new Cores { CorNome = "Marron", CorXaml = "Brown" });
            CoresDeTema.Add(new Cores { CorNome = "Cinza", CorXaml = "Grey" });

            #endregion Cores
        }

        private void cbxTema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Fundo = (Cores)cbxTema.SelectedItem;
            if (Fundo != null)
                MudarCores.Fundo = Fundo.CorXaml;
        }

        private void cbxCor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Cor = (Cores)cbxCor.SelectedItem;
            if (Cor != null)
                MudarCores.Cor = Cor.CorXaml;
        }

        private void cbxDestaque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Destaque = (Cores)cbxDestaque.SelectedItem;
            if (Destaque != null)
                MudarCores.Destaque = Destaque.CorXaml;
        }
    }
}