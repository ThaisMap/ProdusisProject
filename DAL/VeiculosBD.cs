using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace DAL
{
    public class VeiculosBD
    {
        public bool cadastrarVeiculo(Veiculos novoVeiculo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Veiculos.Add(novoVeiculo);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                var qualErro = ex;
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var VeiculoAlterado = BancoDeDados.Veiculos.Where(x => x.PlacaVeiculo == novoVeiculo.PlacaVeiculo).First();
                    VeiculoAlterado.AtivoVeiculo = novoVeiculo.AtivoVeiculo;
                    VeiculoAlterado.CapacidadePaletes = novoVeiculo.CapacidadePaletes;
                    VeiculoAlterado.MotoristaVeiculo = novoVeiculo.MotoristaVeiculo;
                    VeiculoAlterado.Placa2Veiculo = novoVeiculo.Placa2Veiculo;
                    VeiculoAlterado.TipoVeiculo = novoVeiculo.TipoVeiculo;

                    BancoDeDados.SaveChanges();
                }
                return false;
            }
        }

        public List<Veiculos> getVeiculos()
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var lista = BancoDeDados.Veiculos.OrderBy(x => x.PlacaVeiculo).ToList();
                    return lista;
                }
            }
            catch (Exception erro)
            {
                return new List<Veiculos>();
            }
        }

        public List<CapacidadeMotoristas> getAllMotoristas()
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var lista = BancoDeDados.CapacidadeMotoristas.OrderBy(x => x.Motorista).ToList();
                    return lista;
                }
            }
            catch
            {
                return new List<CapacidadeMotoristas>();
            }
        }

        public bool cadastrarAcessoPortaria(AcessosPortaria novo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.AcessosPortaria.Add(novo);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                var qualErro = ex;
                return false;
            }
        }

        public List<AcessosPortaria> FiltrarAcessos()
        {
            List<AcessosPortaria> acessosPendentes;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    acessosPendentes = (from AcessosPortaria in BancoDeDados.AcessosPortaria select AcessosPortaria).ToList();
                }
                return acessosPendentes;
            }

            catch (Exception)
            {
                return new List<AcessosPortaria>();
            }
        }

        public List<AcessosPortaria> FiltrarAcessos(Filtro f)
        {
            List<AcessosPortaria> acessosFiltrados;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var query = BancoDeDados.AcessosPortaria.AsQueryable();
                    if (f.acessoPendente == true)
                        query = query.Where(a => a.SaidaAcesso == null);
                    if (f.dataInicio != null)
                        query = query.Where(a => a.EntradaAcesso >= f.dataInicio);
                    if (f.dataFim != null)
                        query = query.Where(a => a.EntradaAcesso <= f.dataFim);
                    if (f.nomeFuncionario != "")
                        query = query.Where(a => a.NomeMotoristaAcesso.Contains(f.nomeFuncionario));
                    if (f.placa != "")
                        query = query.Where(a => a.PlacaAcesso == f.placa);
                    acessosFiltrados = query.ToList();
                }

                return acessosFiltrados;

            }
           
            catch (Exception)
            {
                return new List<AcessosPortaria>();
            }
        }

        public List<AcessosPortaria> GetAcessosPendentes()
        {
            List<AcessosPortaria> acessosPendentes;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    acessosPendentes = (from AcessosPortaria in BancoDeDados.AcessosPortaria where AcessosPortaria.SaidaAcesso == null select AcessosPortaria).ToList();
                }
                return acessosPendentes;
            }

            catch (Exception)
            {
                return new List<AcessosPortaria>();
            }
        }

        public AcessosPortaria getAcessoPorID(int idAcesso)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from AcessosPortaria in BancoDeDados.AcessosPortaria where AcessosPortaria.idAcesso == idAcesso select AcessosPortaria).FirstOrDefault();
                }
            }

            catch (Exception)
            {
                return new AcessosPortaria();
            }
        }

        public void RegistrarSaída(AcessosPortaria acesso)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var antigo =  (from AcessosPortaria in BancoDeDados.AcessosPortaria where AcessosPortaria.idAcesso == acesso.idAcesso select AcessosPortaria).FirstOrDefault();
                    antigo.SaidaAcesso = DateTime.Now;
                    antigo.KmAcesso = acesso.KmAcesso;
                    antigo.Placa2Acesso = acesso.Placa2Acesso;
                    antigo.PorteiroSaida = acesso.PorteiroSaida;
                    BancoDeDados.SaveChanges();
                }
            }

            catch (Exception erro)
            {
               
            }
        }

        public void RegistrarDoca(int IdAcesso, int Doca)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var acesso = (from AcessosPortaria in BancoDeDados.AcessosPortaria where AcessosPortaria.idAcesso == IdAcesso select AcessosPortaria).FirstOrDefault();
                    acesso.DocaAcesso = Doca;
                    BancoDeDados.SaveChanges();
                }
            }

            catch (Exception erro)
            {

            }
        }
    }
}

