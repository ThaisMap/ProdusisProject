using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace BLL
{
    public class ExcelBLL
    {                     
        public void ExportarExcelProdut(List<ItemRanking> Tarefas, string nomeArquivo)
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
                xlWorkSheet.Cells[1, 2] = "Pontuação";
                xlWorkSheet.Cells[1, 3] = "Quantidade";
                xlWorkSheet.Cells[1, 4] = "Erros";

                int linha = 2;

                foreach (ItemRanking i in Tarefas)
                {
                    xlWorkSheet.Cells[linha, 1] = i.NomesFuncionarios;
                    xlWorkSheet.Cells[linha, 2] = i.Pontuacao;
                    xlWorkSheet.Cells[linha, 3] = i.QuantidadeTarefas;
                    xlWorkSheet.Cells[linha, 4] = i.Erros;

                    linha++;
                }

                xlWorkBook.SaveAs(nomeArquivo, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
 Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                LiberarObjetos(xlWorkSheet);
                LiberarObjetos(xlWorkBook);
                LiberarObjetos(xlApp);
            }
            catch (Exception ex)
            {
                var erro = ex;
            }
        }

        public void ExportarExcel(List<ItemRelatorio> Tarefas, string nomeArquivo)
        {
            var excel = new Excel.Application
            {
                DisplayAlerts = false
            };

            var workbook = excel.Workbooks.Add(Type.Missing);
            var worksheet = (Excel.Worksheet)workbook.Sheets[1];

            object misValue = System.Reflection.Missing.Value;
            string[,] array = new string[Tarefas.Count + 1, 15];
            AcessoBD abd = new AcessoBD();

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
                    //if (Tarefas[linha].horaFim != null)
                    //{
                    //    abd.AlterarPontuacao(Tarefas[linha].idTarefa);
                    //}

                    array[linha + 1, 0] = Tarefas[linha].documentoTarefa.ToString("00");
                    array[linha + 1, 1] = Tarefas[linha].tipoTarefa;
                    array[linha + 1, 2] = Tarefas[linha].inicioTarefa.Date.ToString("dd\\/MM\\/yyyy");
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

                LiberarObjetos(worksheet);
                LiberarObjetos(workbook);
                LiberarObjetos(excel);
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

                LiberarObjetos(worksheet);
                LiberarObjetos(workbook);
                LiberarObjetos(excel);
            }
        }

        private void LiberarObjetos(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}