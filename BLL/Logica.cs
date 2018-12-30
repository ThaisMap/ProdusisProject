using DAL;
using ProdusisBD;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class Logica
    {
        private AcessoBD abd = new AcessoBD();
          

        public int IdCteDisponivelMaisRecente(int numCte)
        {
            var lista = abd.GetNovoCtePorNum(numCte);
            if (lista.Count == 0)
                return -1;
            for (int i = lista.Count - 1; i >= 0; i--)
            {
                if (!abd.VerificaDocumentoTarefa(lista[i].idCte, "2"))
                    return lista[i].idCte;
            }
            return 0;
        }

        public bool NumeroCteExiste(int num)
        {
            var idCtes = abd.GetNovoCtePorNum(num);
            if (idCtes.Count != 0)
            {
                return true;
            }
            else return false;
        }

        public bool CadastraObservacao(string nomeFunc, System.DateTime data, string texto)
        {
            int idFuncionario = abd.GetFuncPorNome(nomeFunc).idFunc;
            Observacoes obs = new Observacoes
            {
                FuncObs = idFuncionario,
                DataObs = data,
                TextoObs = texto
            };
            return abd.CadastrarObservacao(obs);
        }

        public bool InserirTarefa(Tarefas novaTarefa, string[] funcionarios)
        {
            if (novaTarefa.tipoTarefa != "2" && !abd.ManifestoExiste(novaTarefa.documentoTarefa))
            {
                return false;
            }

            int[] idsFuncionarios = new int[funcionarios.Length];

            novaTarefa.divergenciaTarefa = "Tabela";
            for (int i = 0; i < funcionarios.Length; i++)
            {
                idsFuncionarios[i] = abd.GetFuncPorNome(funcionarios[i]).idFunc;
            }
            return abd.CadastrarTarefa(novaTarefa, idsFuncionarios);
        }

        public void AlterarSkuNFs(int idCte, int sku)
        {
            var ctes = abd.GetCtePorID(idCte);
            var notas = ctes.notasCte.Split('\\');
            sku = sku / notas.Count();
            foreach (var item in notas)
            {
                abd.AlterarSKUNF(idCte, item, sku);
            }
        }

        public List<DetalhesManifesto> GetDetalheManifestos(int numManifesto)
        {
            List<DetalhesManifesto> detalhes = new List<DetalhesManifesto>();
            var ctes = abd.CtesNoManifesto(numManifesto);
            var importados = abd.CtesImportadosNoManifesto(numManifesto);
            var conferidos = abd.CtesConferidosNoManifesto(numManifesto);

            foreach (var item in ctes)
            {
                DetalhesManifesto aux = new DetalhesManifesto();
                aux.cte = item.numeroCte;
                aux.importado = importados.Contains(item.idCte) ? "Sim" : "Pendente";
                aux.conferido = conferidos.Contains(item.idCte) ? "Sim" : "Não";

                detalhes.Add(aux);

            }

            return detalhes;
        }

        public void ExportarProdutividade(Filtro f, string nomeArquivo)
        {
            var funcs = abd.GetFuncionariosAtivos();
            List<ItemRanking> listaFinal = new List<ItemRanking>();
            for (int i = 1; i < 6; i++)
            {
                f.TipoTarefa = i.ToString();
                listaFinal.AddRange(abd.GetRanking(f));
            }

            listaFinal = MesclarCargaDescarga(listaFinal);

            foreach (var item in listaFinal)
            {
                item.Matricula = funcs.Where(x => x.nomeFunc == item.NomeFuncionario).Select(x => x.matriculaFunc).FirstOrDefault();
                item.TipoTarefa = TipoExtenso(item.TipoTarefa);
            }
            ExcelBLL exporta = new ExcelBLL();
            exporta.ExportarExcelProdut(listaFinal, nomeArquivo);
        }

        public List<ItemRanking> MesclarCargaDescarga(List<ItemRanking> lista)
        {
            var LISTACARGA = lista.Where(x => x.TipoTarefa == "4").ToList();
            var listaDescarga = lista.Where(x => x.TipoTarefa == "1").ToList();

            lista.RemoveAll(x => x.TipoTarefa == "1");
            lista.RemoveAll(x => x.TipoTarefa == "4");

            foreach (var item in listaDescarga)
            {
                if (LISTACARGA.Where(x => x.NomeFuncionario == item.NomeFuncionario).Any())
                {
                    LISTACARGA.Where(x => x.NomeFuncionario == item.NomeFuncionario).First().Pontuacao += item.Pontuacao;
                    LISTACARGA.Where(x => x.NomeFuncionario == item.NomeFuncionario).First().QuantidadeTarefas += item.QuantidadeTarefas;
                    LISTACARGA.Where(x => x.NomeFuncionario == item.NomeFuncionario).First().Erros += item.Erros;
                }
                else
                    LISTACARGA.Add(item);
            }
            lista.AddRange(LISTACARGA);

            return lista;
        }
        
        private string TipoExtenso(string tipo)
        {
            switch (tipo)
            {
                case "1":
                case "4":
                    return "Carregamento e Descarga";

                case "2":
                    return "Conferência";

                case "3":
                    return "Movim. de Paletes";
                    
                case "5":
                    return "Empilhadeira";

                default:
                    return "Não identificado";
            }
        }
    }
}