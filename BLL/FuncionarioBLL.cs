using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class FuncionarioBLL
    {
        private AcessoBD abd = new AcessoBD();

        public bool cadastraObservacao(string nomeFunc, DateTime data, string texto)
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

    }
}