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
        public PesquisarDocumento(double Altura, double largura)
        {
            InitializeComponent();
            ListaDados.Height = Altura - 250;
        }

        private void limpar()
        {
            ListaDados.ItemsSource = new List<dadosPesquisa>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TipoDeDocumento.SelectedIndex > -1 && NumeroDocumento.Text != "")
            {
                AcessoBD abd = new AcessoBD();
                Logica d = new Logica();

                string[] rotulosCte = { "Número", "Volumes", "SKU's", "Fornecedor", "Notas Fiscais", "Manifesto" };
                string[] rotulosManifesto = { "Número", "Volumes", "CT-es", "CT-es importados", "CT-es conferidos" };
                string[] rotulosNF = { "Número", "Volumes", "SKU's", "CT-e", "Fornecedor", "Cliente" };

                int numDoc = int.Parse(NumeroDocumento.Text);
                List<dadosPesquisa> listaDados = new List<dadosPesquisa>();
                dadosPesquisa aux;
                listaDados.Clear();

                // CT-e
                if (TipoDeDocumento.SelectedIndex == 0)
                {
                    if (d.NumeroCteExiste(numDoc))
                    {
                        var ctes = abd.GetNovoCtePorNum(numDoc);
                        aux = new dadosPesquisa()
                        {
                            numero = rotulosCte[0],
                            volumes = rotulosCte[1],
                            dado3 = rotulosCte[2],
                            dado4 = rotulosCte[3],
                            dado5 = rotulosCte[4],
                            dado6 = rotulosCte[5]
                        };
                        listaDados.Add(aux);
                        foreach (var item in ctes)
                        {
                            aux = new dadosPesquisa()
                            {
                                numero = NumeroDocumento.Text.Replace("_", ""),
                                volumes = abd.GetVolumesCte(item.idCte).ToString(),
                                dado3 = abd.GetSkuCte(item.idCte).ToString(),
                                dado4 = abd.GetFornecedorCte(item.idCte),
                                dado5 = item.notasCte,
                                dado6 = abd.GetListaManifestosCte(item.idCte)
                            };
                            listaDados.Add(aux);
                        }
                        ListaDados.ItemsSource = listaDados;
                    }
                    else limpar();
                }

                // MANIFESTO
                else if (TipoDeDocumento.SelectedIndex == 1)
                {
                    Manifestos documento = abd.GetManifestoPorNumero(numDoc);

                    if (documento != null)
                    {
                        aux = new dadosPesquisa()
                        {
                            numero = rotulosManifesto[0],
                            volumes = rotulosManifesto[1],
                            dado3 = rotulosManifesto[2],
                            dado4 = rotulosManifesto[3],
                            dado5 = rotulosManifesto[4],
                            dado6 = " "
                        };
                        listaDados.Add(aux);

                        aux = new dadosPesquisa()
                        {
                            numero = documento.numeroManifesto.ToString(),
                            volumes = documento.VolumesManifesto.ToString(),
                            dado3 = documento.quantCtesManifesto.ToString(),
                            dado4 = abd.CtesImportadosNoManifesto(numDoc).ToString(),
                            dado5 = abd.CtesConferidosNoManifesto(numDoc).ToString(),
                            dado6 = " "
                        };
                        listaDados.Add(aux);

                        ListaDados.ItemsSource = listaDados;
                    }
                    else limpar();
                }

                // NOTA FISCAL
                else if (TipoDeDocumento.SelectedIndex == 2)
                {
                    var documentos = abd.GetNFPorNumero(NumeroDocumento.Text);
                    if (documentos.Count > 0)
                    {
                        aux = new dadosPesquisa()
                        {
                            numero = rotulosNF[0],
                            volumes = rotulosNF[1],
                            dado3 = rotulosNF[2],
                            dado4 = rotulosNF[3],
                            dado5 = rotulosNF[4],
                            dado6 = rotulosNF[5]
                        };
                        listaDados.Add(aux);
                        foreach (var documento in documentos)
                        {
                            string numCTE = "Não vinculado";
                            if (documento.CteNovoNF != null)
                                numCTE = abd.GetCtePorID((int)documento.CteNovoNF).numeroCte.ToString();
                            aux = new dadosPesquisa()
                            {
                                numero = documento.numeroNF.ToString(),
                                volumes = documento.volumesNF.ToString(),
                                dado3 = documento.skuNF.ToString(),
                                dado4 = numCTE,
                                dado5 = documento.fornecedorNF,
                                dado6 = documento.clienteNF
                            };
                            listaDados.Add(aux);
                        }
                        ListaDados.ItemsSource = listaDados;
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