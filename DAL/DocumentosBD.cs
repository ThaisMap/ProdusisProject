using System.Collections.Generic;
using System.Linq;
using ProdusisBD;
using System;

namespace DAL
{
    public class DocumentosBD
    {
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
                return false;
            }
        }

        public bool alterarSkuManifesto(int numManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Manifestos atual = BancoDeDados.Manifestos.FirstOrDefault(m => m.numeroManifesto == numManifesto);
                    atual.skusManifesto = getSkuManifesto(numManifesto);
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
            catch
            {
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
            catch
            {
                return false;
            }
        }

        public bool inserirCteNf(string numNF, string fornecedor, int numeroCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    NotasFiscais nfAtual = BancoDeDados.NotasFiscais.FirstOrDefault(nf => nf.numeroNF == numNF && nf.fonecedorNF == fornecedor);
                    nfAtual.CteNF = numeroCte;
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
                    return (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.numeroNF == numNF select NotasFiscais).FirstOrDefault();
                }
            }
            catch
            {
                return new NotasFiscais();
            }
        }

       
        /// <summary>
        /// Verifica se o documento indicado esta cadastrado no banco de dados
        /// </summary>
        /// <param name="tipodoc">Tipo de documento 0 - Manifesto, 1 - Cte, 2 - Nota fiscal</param>
        /// <param name="numDocumento">Parâmetro de busca</param>
        /// <returns>Id do documento se este for encontrado, -1 se nao for ou se ocorrer algum erro</returns>
        public int? verificarDocumentoCadastrado(int tipodoc, string numDocumento)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    int? documento = null;
                    if (tipodoc == 0)
                    {
                        documento = (from Manifestos in BancoDeDados.Manifestos where Manifestos.numeroManifesto == int.Parse(numDocumento) select Manifestos.numeroManifesto).FirstOrDefault();
                    }
                    else if (tipodoc == 1)
                    {
                        documento = (from Ctes in BancoDeDados.Ctes where Ctes.numeroCte == int.Parse(numDocumento) select Ctes.numeroCte).FirstOrDefault();
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
        /// Retorna uma string com o resumo dos dados do documento
        /// </summary>
        /// <param name="tipoDocumento">Tipo de documento 0 - Manifesto, 1 - Cte, 2 - Nota fiscal</param>
        /// <param name="idDoc">Parâmetro de busca</param>
        public string getDadosDocumentos(int tipoDocumento, int idDoc)
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
                }
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
                    //Funcionarios funcAtual = BancoDeDados.Funcionarios.Single(f => f.idFunc == novoFunc.idFunc);

                    Manifestos m = BancoDeDados.Manifestos.SingleOrDefault(man => man.numeroManifesto == numDoc);
                    dados = "Manifesto nº " + m.numeroManifesto + " - " + m.quantCtesManifesto + " entregas - " + m.VolumesManifesto + " volumes - " + m.VolumesManifesto + " SKU's - " + m.pesoManifesto + " Kg";
                }
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }

        /// <summary>
        /// Retorna uma string com os dados do cte indicado
        /// </summary>
        private string getDadosCte(int numDoc)
        {
            string dados;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    int numeroCte = (from Ctes in BancoDeDados.Ctes where Ctes.numeroCte == numDoc select Ctes.numeroCte).FirstOrDefault();
                    dados = "Cte n º " + numeroCte + " - " + getVolumesCte(numDoc) + " volumes - " + getSkuCte(numDoc) + " SKU's - " + getPesoCte(numDoc) + " kg";
                }
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }

        /// <summary>
        /// Retorna a soma dos sku's de todas as notas fiscais em um manifesto
        /// </summary>
        public int getSkuManifesto(int numManifesto)
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
            catch
            {
                return -2;
            }
        }

        /// <summary>
        /// Retorna a soma dos skus de cada nota componente do Cte
        /// </summary>
        /// <param name="idCte">Parâmetro de pesquisa</param>
        /// <returns>O múmero de skus ou 0, caso nao encontre alguma nota, -1 se ocorrer um erro</returns>
        public int getSkuCte(int idCte)
        {
            int sku = 0;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<NotasFiscais> ListaNFs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNF == idCte select NotasFiscais).ToList();
                    foreach (NotasFiscais n in ListaNFs)
                    {
                        if (verificarDocumentoCadastrado(2, n.numeroNF) > 0)
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

        public string getFornecedorCte(int idCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNF == idCte select NotasFiscais.fonecedorNF).FirstOrDefault();
                }
            }
            catch
            {
                return "Fornecedor não encontrado";
            }
        }

       

        /// <summary>
        /// Retorna a soma do peso de cada nota componente do Cte
        /// </summary>
        /// <param name="idCte">Parâmetro de pesquisa</param>
        /// <returns>O múmero de skus ou 0, caso nao encontre alguma nota, -1 se ocorrer um erro</returns>
        public double getPesoCte(int idCte)
        {
            double peso = 0;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<NotasFiscais> ListaNFs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNF == idCte select NotasFiscais).ToList();
                    foreach (NotasFiscais n in ListaNFs)
                    {
                        if (verificarDocumentoCadastrado(2, n.numeroNF) > 0)
                            peso += n.pesoNF;
                    }
                }
            }
            catch
            {
                return -1;
            }

            return peso;
        }

        /// <summary>
        /// Retorna a soma dos skus de cada nota componente do Cte
        /// </summary>
        /// <param name="numeroCte">Parâmetro de pesquisa</param>
        /// <returns>O múmero de skus ou 0, caso nao encontre alguma nota, -1 se ocorrer um erro</returns>
        public int getVolumesCte(int numeroCte)
        {
            int volumes = 0;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<NotasFiscais> ListaNFs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNF == numeroCte select NotasFiscais).ToList();
                    foreach (NotasFiscais n in ListaNFs)
                    {
                        if (verificarDocumentoCadastrado(2, n.numeroNF) > 0)
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
    }
}