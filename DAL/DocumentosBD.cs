using ProdusisBD;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DAL
{
    public class DocumentosBD
    {
        #region Novo Cte

        public bool cadastrarNovoCte(Cte novoCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    if (verificarNovoCte(novoCte.numeroCte, novoCte.notasCte) <= 0)
                    {
                        BancoDeDados.Cte.Add(novoCte);
                        BancoDeDados.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                var olho = ex;
                return false;
            }
        }

        public Cte getNovoCtePorID(int idCte)
        {
                Cte novo = new Cte();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                   novo =  (from Cte in BancoDeDados.Cte where Cte.idCte == idCte select Cte).FirstOrDefault();
                }
                return novo;
            }
            catch
            {
                return novo;
            }
        }
          
        public string getNfsNovoCte(int idCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Cte in BancoDeDados.Cte where Cte.idCte == idCte select Cte.notasCte).ToString();
                }
            }
            catch
            {
                return "Não atrelado a nenhuma NF";
            }
        }
        
        /// <summary>
        /// Verifica se está cadastrado um cte com o numero e notas fiscais fornecidos
        /// </summary>
        /// <returns>-1 para erros, 0 se não houver correspondencia, o id do cte se houver correspondencia (>=1)</returns>
        public int verificarNovoCte(int nCte, string nfs)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Cte in BancoDeDados.Cte where Cte.numeroCte == nCte where Cte.notasCte == nfs select Cte.idCte).FirstOrDefault();
                }
            }
            catch
            {
                return -1;
            }
        }
        
        private string getDadosNovoCte(int numDoc)
        {
            string dados;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Cte idCte = (from Cte in BancoDeDados.Cte where Cte.numeroCte == numDoc select Cte).FirstOrDefault();
                    dados = "Cte n º " + idCte.numeroCte + " - " + getVolumesCte(numDoc) + " volumes - " + getSkuCte(numDoc) + " SKU's";
                }
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }

        public List<Cte> getNovoCtePorNum(int numCte)
        {
            List<Cte> lista = new List<Cte>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    lista = (from Cte in BancoDeDados.Cte where Cte.numeroCte == numCte select Cte).ToList();
                }
            }
            catch
            {
               
            }
            return lista;
        }

        #endregion

        /// <summary>
        /// Insere um registro de cte no banco de dados
        /// </summary>
        /// <param name="novoCte">Dados do novo registro</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool cadastrarCte(Ctes novoCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Ctes.Add(novoCte);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                var olho = ex;
                return false;
            }
        }

        /// <summary>
        /// verifica se ja foi criada a relação cte_Manifesto 
        /// </summary>
        /// <param name="novo"></param>
        /// <returns></returns>
        public bool checarCteManifesto(Cte_Manifesto novo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var quantos = (from Cte_Manifesto in BancoDeDados.Cte_Manifesto
                                   where Cte_Manifesto.Cte == novo.Cte
                                   where Cte_Manifesto.Manifesto == novo.Manifesto
                                   where Cte_Manifesto.CteNovo == novo.CteNovo
                                   select Cte_Manifesto).FirstOrDefault();
                    return quantos == null;
                }
            }
            catch (Exception e)
            {
                var erro = e;
                return false;
            }
        }

        /// <summary>
        /// Insere  um registro de cte em manifesto no banco de dados
        /// </summary>
        /// <param name="novo">Dados do novo registro</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool cadastrarCteManifesto(Cte_Manifesto novo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Cte_Manifesto.Add(novo);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                var olho = ex;
                return false;
            }
        }

        /// <summary>
        /// Insere um registro de manifesto no banco de dados
        /// </summary>
        /// <param name="novoManifesto">Dados do novo registro</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool cadastrarManifesto(Manifestos novoManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Manifestos.Add(novoManifesto);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var manifestoAlterado = BancoDeDados.Manifestos.Where(x=> x.numeroManifesto == novoManifesto.numeroManifesto).First();
                    manifestoAlterado.pesoManifesto = novoManifesto.pesoManifesto;
                    manifestoAlterado.quantCtesManifesto = novoManifesto.quantCtesManifesto;
                    manifestoAlterado.VolumesManifesto = novoManifesto.VolumesManifesto;
                    
                    BancoDeDados.SaveChanges();
                }
                return false;
            }
        }

        public bool alterarPreManifesto(Manifestos novoManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Manifestos atual = BancoDeDados.Manifestos.Where(x=> x.numeroManifesto == novoManifesto.numeroManifesto).FirstOrDefault();
                    atual.quantCtesManifesto = novoManifesto.quantCtesManifesto;
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Insere um registro de nota fiscal no banco de dados
        /// </summary>
        /// <param name="novaNf">Dados do novo registro</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool cadastrarNF(NotasFiscais novaNf)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.NotasFiscais.Add(novaNf);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                var olho = ex;
                return false;
            }
        }

        /// <summary>
        /// Lista os manifestos onde o cte foi importado
        /// </summary>
        public string get_ListaManifestosCte(int numCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var manifestos = (from Cte_Manifesto in BancoDeDados.Cte_Manifesto where Cte_Manifesto.Cte == numCte select Cte_Manifesto).ToList();
                    if (manifestos != null)
                    {
                        string notas = "";
                        foreach (var item in manifestos)
                        {
                            if (notas == "")
                                notas = item.Manifesto.ToString();
                            else
                                notas += "/" + item.Manifesto.ToString();
                        }
                        return notas;
                    }
                    return "Não foi encontrado.";
                }
            }
            catch
            {
                return "Não foi encontrado.";
            }
        }

        /// <summary>
        /// Gera uma lista com todos os ctes de um manifesto
        /// </summary>
        public List<Cte_Manifesto> getCtesNoManifesto(int numeroManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Cte_Manifesto in BancoDeDados.Cte_Manifesto where Cte_Manifesto.Manifesto == numeroManifesto select Cte_Manifesto).ToList();
                }
            }
            catch (Exception EX)
            {
                var OLHO = EX;
                return null;
            }
        }

        /// <summary>
        /// Retorna uma string com o resumo dos dados do documento
        /// </summary>
        /// <param name="tipoDocumento">Tipo de documento 0 - Manifesto, 1 - Cte, 2 - Nota fiscal, 3 - Novo Cte</param>
        /// <param name="idDoc">Parâmetro de busca</param>
        public string getDadosDocumentos(int tipoDocumento, int idDoc)  // alterado para novo cte
        {
            string dados = "Dados não encontrados";
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    if (tipoDocumento == 0)
                    {
                        dados = getDadosManifesto(idDoc);
                    }
                    else if (tipoDocumento == 1)
                    {
                        dados = getDadosCte(idDoc);
                    }
                    else if (tipoDocumento == 3)
                    {
                        dados = getDadosNovoCte(idDoc);
                    }
                }
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }

        /// <summary>
        /// Retorna uma string com o Fornecedor das NFs no Cte informado
        /// </summary>
        public string getFornecedorCte(int numCte)  // Funciona com o novo cte
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var nota = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNovoNF == numCte select NotasFiscais).FirstOrDefault();
                    return nota.fornecedorNF;
                }
            }
            catch (Exception e)
            {
                return "Fornecedor não encontrado";
            }
        }

        public string getFornecedorManifesto(int numManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var result = (from a in BancoDeDados.NotasFiscais
                                  join b in BancoDeDados.Cte_Manifesto on a.CteNF equals b.Cte
                                  where b.Manifesto == numManifesto
                                  select a.fornecedorNF).ToList();
                    if (result.Count == 0)
                    {
                        return "Fornecedor não encontrado";
                    }
                    if (result.Count == 1)
                    {
                        return result[0];
                    }
                    if (result.Any(o => o != result[0]))
                        return "VARIOS FORNECEDORES";
                    else
                        return result[0];                                       
                }
            }
            catch(Exception e)
            {
                var olho = e;
                return "Fornecedor não encontrado";
            }
        }

        /// <summary>
        /// Retorna um manifesto a partir do seu número
        /// </summary>
        /// <param name="numManifesto">Parametros de busca</param>
        public Manifestos getManifestoPorNumero(int numManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Manifestos in BancoDeDados.Manifestos where Manifestos.numeroManifesto == numManifesto select Manifestos).FirstOrDefault();
                }
            }
            catch
            {
                return new Manifestos();
            }
        }

        /// <summary>
        /// Retorna uma nota fiscal a partir do seu numero
        /// </summary>
        /// <param name="numNF">Parametros de busca (com série)</param>
        public NotasFiscais getNFPorNumero(string numNF)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.numeroNF.StartsWith(numNF) select NotasFiscais).FirstOrDefault();
                }
            }
            catch
            {
                return new NotasFiscais();
            }
        }

        /// <summary>
        /// Retorna uma string com as NFs atreladas ao Cte informado
        /// </summary>
        public string getNfsCte(int idCte)
        {            
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var nfs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNF == idCte select NotasFiscais).ToList();
                    string notas = "";
                    foreach (var item in nfs)
                        {
                            if (notas == "")

                                notas = item.numeroNF;
                            else
                                notas += "\\" + item.numeroNF;
                        }
                        return notas;
                }
            }
            catch
            {
                return "Não atrelado a nenhuma NF";
            }
        }

        /// <summary>
        /// Retorna a soma dos skus de cada nota componente do Cte
        /// </summary>
        /// <param name="idCte">Parâmetro de pesquisa</param>
        /// <returns>O múmero de skus ou 0, caso nao encontre alguma nota, -1 se ocorrer um erro</returns>
        public int getSkuCte(int idCte) //  Funciona com o novo cte
        {
            int sku = 0;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<NotasFiscais> ListaNFs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNovoNF == idCte select NotasFiscais).ToList();
                    foreach (NotasFiscais n in ListaNFs)
                    {
                            sku += n.skuNF;
                    }
                }
            }
            catch
            {
                return -1;
            }
            return sku;
        }

        /// <summary>
        /// Retorna a soma dos sku's de todas as notas fiscais em um manifesto
        /// </summary>
        public int getSkuManifesto(int numManifesto)    //  Funciona com o novo cte
        {
            int sku = 0;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<NotasFiscais> ListaNFs = new List<NotasFiscais>();
                    List<Cte_Manifesto> ListaCte = (from Cte_Manifesto in BancoDeDados.Cte_Manifesto where Cte_Manifesto.Manifesto == numManifesto select Cte_Manifesto).ToList();
                    foreach (Cte_Manifesto c in ListaCte)
                    {
                        ListaNFs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNF == c.Cte select NotasFiscais).ToList();
                        foreach (NotasFiscais n in ListaNFs)
                        {
                            sku += n.skuNF;
                        }
                    }
                    return sku;
                }
            }
            catch (Exception ex)
            {
                var olho = ex;
                return -2;
            }
        }

        /// <summary>
        /// Retorna a soma dos volumes de cada nota componente do Cte, passar id do Cte
        /// </summary>
        /// <param name="numeroCte">Parâmetro de pesquisa</param>
        /// <returns>O múmero de skus ou 0, caso nao encontre alguma nota, -1 se ocorrer um erro</returns>
        public int getVolumesCte(int numeroCte) //  Funciona com o novo cte
        {
            int volumes = 0;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<NotasFiscais> ListaNFs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNovoNF == numeroCte select NotasFiscais).ToList();
                    foreach (NotasFiscais n in ListaNFs)
                    {
                        volumes += n.volumesNF;
                    }
                }
            }
            catch
            {
                return -1;
            }

            return volumes;
        }

        /// <summary>
        /// Altera uma NF para constar o número do Cte
        /// </summary>
        public bool inserirCteNf(string numNF, int numeroCte) //  Funciona com o novo cte
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    NotasFiscais nfAtual = BancoDeDados.NotasFiscais.FirstOrDefault(nf => nf.numeroNF == numNF);
                    var ctes = getNovoCtePorNum(numeroCte);
                    nfAtual.CteNF = numeroCte;
                    nfAtual.CteNovoNF = ctes.Where(x => x.notasCte.Contains(numNF)).Select(x => x.idCte).FirstOrDefault();

                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch(Exception ex)
            {
                var erro = ex;
                return false;
            }
        }

        /// <summary>
        /// Altera uma NF para constar o número do Cte
        /// </summary>
        public bool inserirCteNfPorId(int id, int numeroCte)    //  Funciona com o novo cte
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    NotasFiscais nfAtual = BancoDeDados.NotasFiscais.FirstOrDefault(nf => nf.idNF == id);
                    nfAtual.CteNF = numeroCte;
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                var erro = ex;
                return false;
            }
        }

        /// <summary>
        /// Verifica se o documento indicado esta cadastrado no banco de dados não usar com o novo cte
        /// </summary>
        /// <param name="tipodoc">Tipo de documento 0 - Manifesto, 1 - Cte, 2 - Nota fiscal</param>
        /// <param name="numDocumento">Parâmetro de busca</param>
        /// <returns>Id do documento se este for encontrado, -1 se nao for ou se ocorrer algum erro</returns>
        public int? verificarDocumentoCadastrado(int tipodoc, string numDocumento) //  Não usar com o novo cte
        {
            int doc;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    int? documento = null;
                    if (tipodoc == 0)
                    {
                        doc = int.Parse(numDocumento);
                        documento = (from Manifestos in BancoDeDados.Manifestos where Manifestos.numeroManifesto == doc select Manifestos.numeroManifesto).FirstOrDefault();
                    }
                    else if (tipodoc == 1)
                    {
                        doc = int.Parse(numDocumento);
                        documento = (from Ctes in BancoDeDados.Ctes where Ctes.numeroCte == doc select Ctes.numeroCte).FirstOrDefault();
                    }
                    else
                    {
                        documento = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.numeroNF == numDocumento select NotasFiscais.idNF).FirstOrDefault();
                    }
                    return documento;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retorna uma string com os dados do cte indicado
        /// </summary>
        private string getDadosCte(int numDoc)
        {
            string dados;
            try
            {
                dados = "Cte n º " + numDoc + " - " + getVolumesCte(numDoc) + " volumes - " + getSkuCte(numDoc) + " SKU's";
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }
  
        /// <summary>
        /// Retorna uma string com os dados do manifesto indicado
        /// </summary>
        private string getDadosManifesto(int numDoc)
        {
            string dados;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Manifestos m = BancoDeDados.Manifestos.SingleOrDefault(man => man.numeroManifesto == numDoc);
                    dados = "Manifesto nº " + m.numeroManifesto + " - " + m.quantCtesManifesto + " cte's - " + m.VolumesManifesto + " volumes - " + m.pesoManifesto + " Kg";
                }
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }
    }
}