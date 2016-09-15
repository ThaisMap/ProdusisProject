using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdusisBD
{
    class DocumentosBD
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

        public NotasFiscais getNFPorNumero(int numNF)
        {
            return new NotasFiscais();
        }

        public Ctes getCtePorNumero(int numCte)
        {
            return new Ctes();
        }

        public bool verificarDocumentoCadastrado(int numDocumento)
        {
            return true;
        }

        public string getDadosDocumentos(int tipoDocumento, int numDocumento)
        {
            return "dados do documento";
        }

        public bool verificaNotasManifesto(int numManifesto)
        {
            return true;
        }

        public int getSkuManifesto(int numManifesto)
        {
            return 1;
        }

        public int getSkuCte(int numCte)
        {
            return 1;
        }

        public double getPesoManifesto(int numCte)
        {
            return 1;
        }

        public int getVolumesManifesto(int numCte)
        {
            return 1;
        }

        private int getSkuNF(int numNF)
        {
            return 1;
        }

        private double getPesoNF(int numNF)
        {
            return 1;
        }

        private int getVolumesNF(int numNF)
        {
            return 1;
        }

    }
}
