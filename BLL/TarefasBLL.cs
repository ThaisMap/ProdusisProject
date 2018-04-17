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

        public bool finalizarTarefa(int idTarefa, int quantPalet, int totalPalet)
        {
            return t.finalizarTarefa(idTarefa, quantPalet, totalPalet);
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

        public List<ItemRelatorio> filtrar(Filtro f)
        {
            return t.getTarefasFiltradas(f, true);
        }

        public List<ItemRanking> filtraRanking(Filtro f)
        {
            return t.getRanking(f);
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

        public void exportarExcelProdut(List<ItemRanking> Tarefas, string nomeArquivo)
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
                xlWorkSheet.Cells[1, 1] = "Nome";
                xlWorkSheet.Cells[1, 2] = "Media de pontos";
                xlWorkSheet.Cells[1, 3] = "Observações";

                int linha = 2;

                foreach (ItemRanking i in Tarefas)
                {
                    xlWorkSheet.Cells[linha, 1] = i.nomesFuncionarios;
                    xlWorkSheet.Cells[linha, 2] = i.mediaPorHora;
                    xlWorkSheet.Cells[linha, 3] = i.observacoes;
                  
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

        public void exportarExcel(List<ItemRelatorio> Tarefas, string nomeArquivo)
        {
            var excel = new Excel.Application();
            excel.DisplayAlerts = false;
            var workbooks = excel.Workbooks;
            var workbook = workbooks.Add(Type.Missing);
            var worksheets = workbook.Sheets;
            var worksheet = (Excel.Worksheet)worksheets[1];
            object misValue = System.Reflection.Missing.Value;
                string[,] array = new string[Tarefas.Count + 1, 15];

            try
            {
                array[0, 0] = "Documento";
                array[0, 1] = "Tipo";
                array[0, 2] = "Data";
                array[0, 3] = "Hora Início";
                array[0, 4] = "Hora Fim";
                array[0, 5] = "Funcionário(s)";
                array[0, 6] = "Tempo Gasto";
                array[0, 7] = "Volumes";
                array[0, 8] = "SKU's";
                array[0, 9] = "Pontos";
                array[0, 10] = "Fornecedor";
                array[0, 11] = "Divergências";
                array[0, 12] = "Paletes";
                array[0, 13] = "Cap paletes";
                array[0, 14] = "Qtde Ctes no manifesto";

                for (int linha = 0; linha < Tarefas.Count; linha++)
                {
                    if (Tarefas[linha].horaFim != null)
                    {                       
                        Tarefas[linha].atualizaPontuação();
                        t.inserirPontuacao(Tarefas[linha].idTarefa, (float)Tarefas[linha].pontos);                        
                    }

                    array[linha + 1, 0] = Tarefas[linha].documentoTarefa.ToString("00");
                    array[linha + 1, 1] = Tarefas[linha].tipoTarefa;
                    array[linha + 1, 2] = Tarefas[linha].inicioTarefa.Date.ToString("MM/dd/yyyy");
                    array[linha + 1, 3] = Tarefas[linha].horaInicio;
                    array[linha + 1, 4] = Tarefas[linha].horaFim;
                    array[linha + 1, 5] = Tarefas[linha].nomesFunc;
                    array[linha + 1, 6] = Tarefas[linha].tempoGasto;
                    array[linha + 1, 7] = Tarefas[linha].volumes.ToString();
                    array[linha + 1, 8] = Tarefas[linha].sku.ToString();
                    array[linha + 1, 9] = Tarefas[linha].pontos.ToString();
                    array[linha + 1, 10] = Tarefas[linha].fornecedor;
                    array[linha + 1, 11] = Tarefas[linha].divergenciaTarefa;
                    array[linha + 1, 12] = Tarefas[linha].quantPaletizado.ToString();
                    array[linha + 1, 13] = Tarefas[linha].totalPaletes.ToString();
                    array[linha + 1, 14] = Tarefas[linha].ctesNoManifesto.ToString();
                }

                var startCell = (Excel.Range)worksheet.Cells[1, 1];
                var endCell = (Excel.Range)worksheet.Cells[Tarefas.Count + 1, 15];
                var writeRange = worksheet.Range[startCell, endCell];

                writeRange.Value2 = array;

                workbook.SaveAs(nomeArquivo, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                workbook.Close(true, misValue, misValue);
                excel.Quit();

                liberarObjetos(worksheet);
                liberarObjetos(workbook);
                liberarObjetos(excel);
            }

            catch (Exception ex)
            {
                var erro = ex;
                var startCell = (Excel.Range)worksheet.Cells[1, 1];
                var endCell = (Excel.Range)worksheet.Cells[Tarefas.Count + 1, 15];
                var writeRange = worksheet.Range[startCell, endCell];

                writeRange.Value2 = array;

                workbook.SaveAs(nomeArquivo, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                workbook.Close(true, misValue, misValue);
                excel.Quit();

                liberarObjetos(worksheet);
                liberarObjetos(workbook);
                liberarObjetos(excel);
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