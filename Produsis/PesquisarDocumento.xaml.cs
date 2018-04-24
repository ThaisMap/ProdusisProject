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
            Fornecedor.Text = "";
            texto1.Text = "";
            valor1.Text = "";
            texto2.Text = "";
            valor2.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TipoDeDocumento.SelectedIndex > -1 && NumeroDocumento.Text != "")
            {
                DocumentosBLL d = new DocumentosBLL();
                int numDoc = int.Parse(NumeroDocumento.Text);

                if (TipoDeDocumento.SelectedIndex == 0)
                {
                    if (d.cteCadastrado(numDoc))
                    {
                        Numero.Text = NumeroDocumento.Text.Replace("_", "");
                        NumeroDeVolumes.Text = d.volumesCte(numDoc).ToString();
                        NumeroDeSKUS.Text = d.skuCte(numDoc).ToString();
                        Fornecedor.Text = d.fornecedorCte(numDoc);
                        texto1.Text = "Notas Fiscais:";
                        valor1.Text = d.getNFsCte(numDoc);
                        texto2.Text = "Manifesto(s):";
                        valor2.Text = d.getManifestosCte(numDoc);
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
                        NumeroDeSKUS.Text = "Não se aplica";
                        Fornecedor.Text = d.getFornecedorManifesto(numDoc);
                        texto1.Text = "Total de CT-es:";
                        valor1.Text = documento.quantCtesManifesto.ToString();
                        texto2.Text = "Cte's com nota importada:";
                        valor2.Text = d.NFsImportadasNoManifesto(numDoc).ToString();
                    }
                    else limpar();
                }

                else if (TipoDeDocumento.SelectedIndex == 2) 
                {
                    NotasFiscais documento = d.getDadosNF(NumeroDocumento.Text);
                    if (documento != null)
                    {
                        Numero.Text = documento.numeroNF;
                        NumeroDeVolumes.Text = documento.volumesNF.ToString();
                        NumeroDeSKUS.Text = documento.skuNF.ToString();
                        Fornecedor.Text = documento.fornecedorNF;
                        texto1.Text = "CT-e:";
                        if (documento.CteNF != null)
                            valor1.Text = documento.CteNF.ToString();
                        else
                            valor1.Text = "Não importado";
                    }
                    else limpar();
                }
            }
        }

        private void testarCaractere(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}