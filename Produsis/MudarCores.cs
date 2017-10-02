using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public class MudarCores
    {
        public static string Fundo { get; set; }
        public static string Cor { get; set; }
        public static string Destaque { get; set; }

        public MudarCores()
        {
            try
            {
                Fundo = DAL.Properties.Perfumaria.Default.Tema;
                Cor = DAL.Properties.Perfumaria.Default.Cor;
                Destaque = DAL.Properties.Perfumaria.Default.Destaque;
                MudarCor();
            }
            catch
            {
                LimparTema();
                ComporTema();
            }
        }

        public static void MudarCor()
        {
            if(Cor is null || Cor.ToString() == "")
            {
                Cor = "Indigo";
            }
            if (Fundo is null || Fundo.ToString() == "")
            {
                Fundo = "Light";
            }
            if (Destaque is null || Destaque.ToString() == "")
            {
                Destaque = "Blue";
            }
            LimparTema();
            ComporTema();
        }

        private static void LimparTema()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
        }

        private static void ComporTema()
        {
            #region Material Theme
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme." + Fundo + ".xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor." + Cor + ".xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor." + Destaque + ".xaml", UriKind.RelativeOrAbsolute)
            });


#endregion
        }
    }
}
