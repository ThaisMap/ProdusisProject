using DAL;
using ProdusisBD;
using System.Collections.Generic;

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

    }
}