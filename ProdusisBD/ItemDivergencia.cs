namespace ProdusisBD
{
    public class ItemDivergencia
    {
        public int cte { get; set; }
        public string fornecedor { get; set; }
        public string nomesFuncionarios { get; set; }
        public string codFalta { get; set; }
        public string qtdFalta { get; set; }
        public string codSobra { get; set; }
        public string qtdSobra { get; set; }
        public string codAvaria { get; set; }
        public string qtdAvaria { get; set; }
        private string[] valores = { "-", "0", "-", "0", "-", "0" };

        public ItemDivergencia(TarefaModelo tarefa)
        {
            cte = tarefa.documentoTarefa;
            fornecedor = tarefa.fornecedor;
            nomesFuncionarios = tarefa.nomesFuncionarios;

            if (tarefa.divergenciaTarefa == null)
                tarefa.divergenciaTarefa = "-;0;-;0;-;0";

            valores = tarefa.divergenciaTarefa.Split(';');
            codFalta = valores[0];
            qtdFalta = valores[1];
            codSobra = valores[2];
            qtdSobra = valores[3];
            codAvaria = valores[4];
            qtdAvaria = valores[5];
        }

        public string getDivergencia()
        {
            string linha = codFalta + ";" + qtdFalta + ";" + codSobra + ";" + qtdSobra + ";" + codAvaria + ";" + qtdAvaria + " ";
            if (linha.Length > 120)
                linha = linha.Remove(120);
            return linha;
        }
    }
}