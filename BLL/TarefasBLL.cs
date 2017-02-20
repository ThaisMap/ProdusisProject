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
                if (!d.cteCadastrado((int)novaTarefa.documentoTarefa))
                    return false;
            }
            else
            {
                if (!d.manifestoCadastrado((int)novaTarefa.documentoTarefa))
                {
                    return false;
                }
            }
            FuncionariosBD f = new FuncionariosBD();
            int[] idsFuncionarios = new int[funcionarios.Length];
            for (int i = 0; i < funcionarios.Length; i++)
            {
                idsFuncionarios[i] = f.getFuncPorNome(funcionarios[i]).idFunc;
            }
            return t.cadastrar(novaTarefa, idsFuncionarios);
        }

        public List<TarefaModelo> tarefasPendentes(string tipo)
        {
            return t.getTarefasPendentes(tipo);
        }

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

        public void getRanking(List<TarefaModelo> Tarefas)
        {
            t.rankingFuncionarios(Tarefas);
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
                xlWorkSheet.Cells[1, 11] = "Pontos por hora";
                xlWorkSheet.Cells[1, 12] = "Peso(Kg)";
                xlWorkSheet.Cells[1, 13] = "Fornecedor";
                xlWorkSheet.Cells[1, 14] = "Divergencia";

                int linha = 2;

                foreach (TarefaModelo i in Tarefas)
                {
                    i.atualizaPontuação();
                    xlWorkSheet.Cells[linha, 1] = i.documentoTarefa;
                    xlWorkSheet.Cells[linha, 2] = i.tipoTarefa;
                    xlWorkSheet.Cells[linha, 3] = i.inicioTarefa.ToOADate();
                    xlWorkSheet.Cells[linha, 4] = i.horaInicio;
                    xlWorkSheet.Cells[linha, 5] = i.horaFim;
                    xlWorkSheet.Cells[linha, 6] = i.nomesFuncionarios;
                    xlWorkSheet.Cells[linha, 7] = i.tempoGasto;
                    xlWorkSheet.Cells[linha, 8] = i.volumes;
                    xlWorkSheet.Cells[linha, 9] = i.skus;
                    xlWorkSheet.Cells[linha, 10] = i.pontos;
                    xlWorkSheet.Cells[linha, 11] = i.pontosPorHora;
                    xlWorkSheet.Cells[linha, 12] = i.peso;
                    xlWorkSheet.Cells[linha, 13] = i.fornecedor;
                    xlWorkSheet.Cells[linha, 14] = i.divergenciaTarefa;
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