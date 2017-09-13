using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace BLL
{
    public class TarefasBLL
    {
        private TarefasBD t = new TarefasBD();
        private DocumentosBLL d = new DocumentosBLL();

        public bool inserirTarefa(Tarefas novaTarefa, string[] funcionarios)
        {
            if (novaTarefa.tipoTarefa == "2") 
            {
                if (!d.cteCadastrado(novaTarefa.documentoTarefa))
                    return false;
            }
            else
            {
                if (!d.manifestoCadastrado(novaTarefa.documentoTarefa))
                {
                    return false;
                }
            }

            FuncionariosBD f = new FuncionariosBD();
            int[] idsFuncionarios = new int[funcionarios.Length];

            novaTarefa.divergenciaTarefa = "-;0;-;0;-;0";
            for (int i = 0; i < funcionarios.Length; i++)
            {
                idsFuncionarios[i] = f.getFuncPorNome(funcionarios[i]).idFunc;
            }
            return t.cadastrar(novaTarefa, idsFuncionarios);
        }

        public List<TarefaModelo> tarefasPendentes(string tipo1, string tipo2)
        {
            var teste = t.getTarefasPendentes(tipo1);
            teste.AddRange(t.getTarefasPendentes(tipo2));
            return teste;
        }

        public List<TarefaModelo> tarefasPendentes(string tipo)
        {
            return t.getTarefasPendentes(tipo);
        }

        /// <summary>
        /// Retorna true se não for repetido
        /// </summary>
        public bool tarefaRepetida(int documento, string tipo)
        {
            return t.verificaDocumentoTarefa(documento, tipo);
        }

        public bool inserirDivergencias(List<TarefaModelo> tarefasDivergencia)
        {
            return t.inserirDivergencia(tarefasDivergencia);
        }

        public bool finalizarTarefa(int idTarefa)
        {
            return t.finalizarTarefa(idTarefa);
        }

        public int iniciadaHojePendente(string tipo)
        {
            return t.getTarefasHojePendentes(tipo);
        }

        public int iniciadaHojeFinalizada(string tipo)
        {
            return t.getTarefasHojeFinalizadas(tipo);
        }

        public List<TarefaModelo> filtrarDivergencias(int Tipo, int Manifesto)
        {
            return t.getTarefasDivergencia(Tipo, Manifesto);
        }

        public List<TarefaModelo> filtrar(Filtro f)
        {
            return t.getTarefasFiltradas(f);
        }

        public List<TarefaModelo> filtraRanking(Filtro f)
        {
            return t.getTarefasFiltradasRanking(f);
        }

        private double calculaHorasPeriodo(DateTime inicio, DateTime fim)
        {
            double horas = 0;
            for (DateTime dt = inicio; dt < fim.AddDays(1); dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (dt.DayOfWeek == DayOfWeek.Saturday)
                        horas += 4;
                    else
                        horas+=7.5;
                }
            }
            return horas;
        }

        public List<ItemRanking> getRanking(Filtro f)
        {
            var rank = t.rankingFuncionarios(filtraRanking(f), calculaHorasPeriodo((DateTime)f.dataInicio, (DateTime)f.dataFim));
            foreach (var item in rank)
            {
                if(!item.nomesFuncionarios.Contains("/"))
                    item.observacoes = getLinhaObs(f.dataInicio, (DateTime)f.dataFim, item.nomesFuncionarios);
            }
            return rank;
        }

        public string getLinhaObs(DateTime? inicio, DateTime fim, string nomeFunc)
        {
            string linha = "";
            FuncionariosBD f = new FuncionariosBD();
            var obs = f.getObservacoes(inicio, fim, f.getFuncPorNome(nomeFunc).idFunc);
            foreach (var item in obs)
            {
                linha += item.DataObs.ToShortDateString() +" "+ item.TextoObs + " * ";
            }
            if (linha.Length > 3)
                linha = linha.Remove(linha.Length - 3);

            return linha;
        }
      
        public void exportarExcel(List<TarefaModelo> Tarefas, string nomeArquivo)
        {
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = "Documento";
                xlWorkSheet.Cells[1, 2] = "Tipo";
                xlWorkSheet.Cells[1, 3] = "Data";
                xlWorkSheet.Cells[1, 4] = "Hora Início";
                xlWorkSheet.Cells[1, 5] = "Hora Fim";
                xlWorkSheet.Cells[1, 6] = "Funcionário(s)";
                xlWorkSheet.Cells[1, 7] = "Tempo Gasto";
                xlWorkSheet.Cells[1, 8] = "Volumes";
                xlWorkSheet.Cells[1, 9] = "SKU's";
                xlWorkSheet.Cells[1, 10] = "Pontos";
                xlWorkSheet.Cells[1, 11] = "Fornecedor";
                xlWorkSheet.Cells[1, 12] = "Cliente";
                xlWorkSheet.Cells[1, 13] = "Divergências";

                int linha = 2;
                            
                foreach (TarefaModelo i in Tarefas)
                {
                    i.atualizaPontuação();
                    xlWorkSheet.Cells[linha, 1] = i.documentoTarefa;
                    xlWorkSheet.Cells[linha, 2] = i.tipoTarefa;
                    xlWorkSheet.Cells[linha, 3] = i.inicioTarefa.Date.ToString("MM/dd/yyyy");
                    xlWorkSheet.Cells[linha, 4] = i.horaInicio;
                    xlWorkSheet.Cells[linha, 5] = i.horaFim;
                    xlWorkSheet.Cells[linha, 6] = i.nomesFuncionarios;
                    xlWorkSheet.Cells[linha, 7] = i.tempoGasto;
                    xlWorkSheet.Cells[linha, 8] = i.volumes;
                    xlWorkSheet.Cells[linha, 9] = i.skus;
                    xlWorkSheet.Cells[linha, 10] = i.pontos;
                    xlWorkSheet.Cells[linha, 11] = i.fornecedor;
                    xlWorkSheet.Cells[linha, 12] = i.cliente;
                    xlWorkSheet.Cells[linha, 13] = i.divergenciaTarefa;
                    
                    linha++;
                }
                
                xlWorkBook.SaveAs(nomeArquivo, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
 Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                liberarObjetos(xlWorkSheet);
                liberarObjetos(xlWorkBook);
                liberarObjetos(xlApp);
            }

            catch (Exception ex)
            {
                var erro = ex;
            }
        }

        private void liberarObjetos(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                var erro = ex;
            }

            finally
            {
                GC.Collect();
            }
        }
    }
}