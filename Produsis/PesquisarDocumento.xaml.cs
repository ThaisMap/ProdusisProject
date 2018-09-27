using BLL;
using DAL;
using ProdusisBD;
using System.Collections.Generic;
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
                DocumentosBD dbd = new DocumentosBD();
                DocumentosBLL d = new DocumentosBLL();

                string[] rotulosNF = { "Número", "Volumes", "SKU's", "CT-e", "Fornecedor", "Cliente" };
                string[] rotulosCte = { "Número", "Volumes", "SKU's", "Fornecedor", "Notas Fiscais", "Manifesto" };
                string[] rotulosManifesto = { "Número", "Volumes", "CT-es", "CT-es importados", "CT-es conferidos" };
                int numDoc = int.Parse(NumeroDocumento.Text);
                List<dadosPesquisa> listaDados = new List<dadosPesquisa>();
                dadosPesquisa aux;
                ListaDados.Items.Clear();
                if (TipoDeDocumento.SelectedIndex == 0)
                {
                    if (d.cteCadastrado(numDoc))
                    {
                        var ctes = dbd.getNovoCtePorNum(numDoc);
                        foreach (var item in ctes)
                        {
                            aux = new dadosPesquisa()
                            {
                                numero = NumeroDocumento.Text.Replace("_", ""),
                                volumes = d.volumesCte(item.idCte).ToString(),
                                dado3 = d.skuCte(item.idCte).ToString(),
                                dado4 = d.fornecedorCte(item.idCte),
                                dado5 = item.notasCte, 
                                dado6 = d.getManifestosCte(item.idCte)
                            };
                            listaDados.Add(aux);
                        }
                        ListaDados.ItemsSource = listaDados;


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

    public class dadosPesquisa
    {
        public string numero { get; set; }
        public string volumes { get; set; }
        public string dado3 { get; set; }
        public string dado4 { get; set; }
        public string dado5 { get; set; }
        public string dado6 { get; set; }
    }
}