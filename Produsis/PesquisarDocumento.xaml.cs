using BLL;
using ProdusisBD;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Produsis
{
    /// <summary>
    /// Interaction logic for PesquisarDocumento.xaml
    /// </summary>
    public partial class PesquisarDocumento : UserControl
    {
        public PesquisarDocumento()
        {
            InitializeComponent();
        }

        private void limpar()
        {
            Numero.Text = "Não encontrado";
            NumeroDeVolumes.Text = "";
            NumeroDeSKUS.Text = "";
            Peso.Text = "";
            Fornecedor.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TipoDeDocumento.SelectedIndex > -1)
            {
                DocumentosBLL d = new DocumentosBLL();
                int numDoc;
                if (int.TryParse(NumeroDocumento.Text.Replace("_", ""), out numDoc))
                {
                    if (TipoDeDocumento.SelectedIndex == 0)
                    {
                        if (d.cteCadastrado(numDoc))
                        {
                            Numero.Text = NumeroDocumento.Text.Replace("_", "");
                            NumeroDeVolumes.Text = d.volumesCte(numDoc).ToString();
                            NumeroDeSKUS.Text = d.skuCte(numDoc).ToString();
                            Peso.Text = d.pesoCte(numDoc).ToString();
                            Fornecedor.Text = d.fornecedorCte(numDoc);
                        }
                        else limpar();
                    }
                    else if (TipoDeDocumento.SelectedIndex == 1)
                    {
                        Manifestos documento = d.getDadosManifesto(numDoc);

                        if (documento != null)
                        {
                            Numero.Text = documento.numeroManifesto.ToString();
                            NumeroDeVolumes.Text = documento.VolumesManifesto.ToString();
                            NumeroDeSKUS.Text = documento.skusManifesto.ToString();
                            Peso.Text = documento.pesoManifesto.ToString();
                            Fornecedor.Text = "Não se aplica";
                        }
                        else limpar();
                    }
                }
                else if (TipoDeDocumento.SelectedIndex == 2)
                {
                    NotasFiscais documento = d.getDadosNF(NumeroDocumento.Text);
                    if (documento != null)
                    {
                        Numero.Text = documento.numeroNF;
                        NumeroDeVolumes.Text = documento.volumesNF.ToString();
                        NumeroDeSKUS.Text = documento.skuNF.ToString();
                        Peso.Text = documento.pesoNF.ToString();
                        Fornecedor.Text = documento.fonecedorNF;
                    }
                    else limpar();
                }
            }
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9][^-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}