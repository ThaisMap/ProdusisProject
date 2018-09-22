using BLL;
using System;
using ProdusisBD;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interação lógica para Observacao.xam
    /// </summary>
    public partial class Observacao : UserControl
    {
        private FuncionarioBLL f = new FuncionarioBLL();

        public Observacao()
        {
            InitializeComponent();
            Nome.ItemsSource = f.carregaFuncionarios();
            dataObs.SelectedDate = DateTime.Today;
            dataFim.SelectedDate = DateTime.Today;
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            string nome = Nome.SelectedItem.ToString(); // aqui pode ser selected item pq o itemsource é uma lista de strings
            DateTime data = (DateTime)dataObs.SelectedDate;
            string texto = TextoObs.SelectionBoxItem.ToString(); //aqui já não pode pq é uma lista de combobox items

            if(f.cadastraObservacao(nome, data, texto))
            {
                MessageBox.Show("Observação cadastrada");
            }
            else
                MessageBox.Show("Ocorreu um erro");
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            dgObs.ItemsSource = f.getObservacoes(dataInicio.SelectedDate, dataFim.SelectedDate, Nome.SelectedItem.ToString());
        }

        private void dgObs_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key && dgObs.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Apagar motorista definitivamente? ", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    var linha = dgObs.SelectedItem as Observacoes;
                    f.deletaObservacao(linha.idObs);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    DAL.DocumentosBD doc = new DAL.DocumentosBD();
                    var ctes = BancoDeDados.Ctes.ToList();
                    for (int i = 48800; i < ctes.Count; i++)
                    {
                        string notas = doc.getNfsCte(ctes[i].numeroCte);
                        if (notas.Length >= 100)
                            notas = notas.Remove(99);
                        var cadastrado = doc.verificarNovoCte(ctes[i].numeroCte, notas);
                        if (notas != "" && cadastrado == 0)
                        {
                            BancoDeDados.Cte.Add(new Cte(ctes[i].numeroCte, notas));
                            BancoDeDados.SaveChanges();
                        }
                    }

                  /*  foreach (var item in ctes)
                    {
                        string notas = doc.getNfsCte(item.numeroCte);
                        var cadastrado = doc.verificarNovoCte(item.numeroCte, notas);
                        if (notas != "" && cadastrado == 0)
                        {
                            BancoDeDados.Cte.Add(new Cte(item.numeroCte, notas));
                            BancoDeDados.SaveChanges();
                        }
                    }*/
      
                    var cteManif = BancoDeDados.Cte_Manifesto.ToList();

                    for (int i = 0; i < cteManif.Count; i++)
                    {
                        var idNovoCte = doc.getNovoCtePorNum(cteManif[i].Cte);
                        cteManif[i].CteNovo = idNovoCte[0].idCte;
                        BancoDeDados.SaveChanges();

                    }


                    var Notas = BancoDeDados.NotasFiscais.Where(x=> x.CteNF != null).ToList();
                    foreach (var item in Notas)
                    {
                        var idNovoCte = doc.getNovoCtePorNum((int)item.CteNF);
                        item.CteNovoNF = idNovoCte[0].idCte;
                    }
                    BancoDeDados.SaveChanges();

                }
            }
            catch (Exception erro)
            {
                var olho = erro;
            }
        }
    }
}
