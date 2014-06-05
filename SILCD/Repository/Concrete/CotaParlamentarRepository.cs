using SILCD.br.gov.camara.www;
using SILCD.sessoesReunioes.br.gov.camara.www;
using SILCD.Models;
using SILCD.Repository.Abstract;
using SILCD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SILCD.Helper;
using System.Data;
using SILCD.Enum;
using SILCD.Resources;
using System.Data.SqlClient;
using System.Configuration;
using DbFactory.Implementation;

namespace SILCD.Repository.Concrete {
    public class CotaParlamentarRepository : BaseRepository, ICotaParlamentarRepository {

        private List<CotaParlamentarViewModels> listaDeCotaParlamentar = null;

        private DbConnection db;
        public CotaParlamentarRepository() {
            if (db == null) {
                db = new DbConnection("connectionString");
            }
        }

        public List<CotaParlamentarViewModels> Listar() {

            try {
                db.CommandText = " SELECT * " +
                                 " FROM [dbo].[CotaParlamentar] " +
                                 " ORDER BY UPPER(txtDescricao) ";

                DataSet dsRetorno = db.GetDataSet();
                if (dsRetorno != null && dsRetorno.Tables[0].Rows.Count > 0) {
                    listaDeCotaParlamentar = new List<CotaParlamentarViewModels>();
                    foreach (DataRow cotaParlamentarRow in dsRetorno.Tables[0].Rows) {
                        listaDeCotaParlamentar.Add(new CotaParlamentarViewModels {
                            Id = int.Parse(cotaParlamentarRow["Id"].ToString()),
                            txNomeParlamentar = cotaParlamentarRow["txNomeParlamentar"].ToString(),
                            nuCarteiraParlamentar = cotaParlamentarRow["nuCarteiraParlamentar"] == DBNull.Value ? 0 : (int?)cotaParlamentarRow["nuCarteiraParlamentar"],
                            nuLegislatura = int.Parse(cotaParlamentarRow["nuLegislatura"].ToString()),
                            sgUF = cotaParlamentarRow["sgUF"].ToString(),
                            sgPartido = cotaParlamentarRow["sgPartido"].ToString(),
                            codLegislatura = cotaParlamentarRow["codLegislatura"] == DBNull.Value ? 0 : (int?)cotaParlamentarRow["codLegislatura"],
                            numSubCota = int.Parse(cotaParlamentarRow["numSubCota"].ToString()),
                            txtDescricao = cotaParlamentarRow["txtDescricao"].ToString(),
                            numEspecificacaoSubCota = int.Parse(cotaParlamentarRow["numEspecificacaoSubCota"].ToString()),
                            txtDescricaoEspecificacao = cotaParlamentarRow["txtDescricaoEspecificacao"].ToString(),
                            txtFornecedor = cotaParlamentarRow["txtFornecedor"].ToString(),
                            txtCNPJCPF = cotaParlamentarRow["txtCNPJCPF"].ToString(),
                            txtNumero = cotaParlamentarRow["txtNumero"].ToString(),
                            indTipoDocumento = int.Parse(cotaParlamentarRow["indTipoDocumento"].ToString()),
                            datEmissao = cotaParlamentarRow["datEmissao"] == DBNull.Value ? new DateTime(1900, 1, 1) : (DateTime?)cotaParlamentarRow["datEmissao"],
                            vlrDocumento = decimal.Parse(cotaParlamentarRow["vlrDocumento"].ToString()),
                            vlrGlosa = decimal.Parse(cotaParlamentarRow["vlrGlosa"].ToString()),
                            vlrLiquido = decimal.Parse(cotaParlamentarRow["vlrLiquido"].ToString()),
                            numMes = int.Parse(cotaParlamentarRow["numMes"].ToString()),
                            numAno = int.Parse(cotaParlamentarRow["numAno"].ToString()),
                            numParcela = int.Parse(cotaParlamentarRow["numParcela"].ToString()),
                            txtPassageiro = cotaParlamentarRow["txtPassageiro"].ToString(),
                            txtTrecho = cotaParlamentarRow["txtTrecho"].ToString(),
                            numLote = int.Parse(cotaParlamentarRow["numLote"].ToString()),
                            numRessarcimento = int.Parse(cotaParlamentarRow["numRessarcimento"].ToString()),
                            ideCadastro = int.Parse(cotaParlamentarRow["ideCadastro"].ToString())
                        });
                    }
                }
            } catch {
                throw new Exception();
            } finally {
                db.Dispose();
            }
            return listaDeCotaParlamentar;

        }

        public List<CotaParlamentarViewModels> ListarCotaParlamentarPorReferenciaTipo() {
            try {
                db.CommandText = " SELECT numAno, numMes, UPPER(txtDescricao) [txtDescricao], SUM(vlrDocumento) [vlrDocumento], SUM(vlrGlosa) [vlrGlosa], SUM(vlrLiquido) [vlrLiquido] " +
                                 " FROM [dbo].[CotaParlamentar] " +
                                 " GROUP BY numAno, numMes, UPPER(txtDescricao) " +
                                 " ORDER BY numAno, numMes, UPPER(txtDescricao) ";

                DataSet dsRetorno = db.GetDataSet();
                if (dsRetorno != null && dsRetorno.Tables[0].Rows.Count > 0) {
                    listaDeCotaParlamentar = new List<CotaParlamentarViewModels>();
                    foreach (DataRow cotaParlamentarRow in dsRetorno.Tables[0].Rows) {
                        listaDeCotaParlamentar.Add(new CotaParlamentarViewModels {
                            txtDescricao = cotaParlamentarRow["txtDescricao"].ToString(),
                            vlrDocumento = decimal.Parse(cotaParlamentarRow["vlrDocumento"].ToString()),
                            vlrGlosa = decimal.Parse(cotaParlamentarRow["vlrGlosa"].ToString()),
                            vlrLiquido = decimal.Parse(cotaParlamentarRow["vlrLiquido"].ToString()),
                            numMes = int.Parse(cotaParlamentarRow["numMes"].ToString()),
                            numAno = int.Parse(cotaParlamentarRow["numAno"].ToString())
                        });
                    }
                }
            } catch {
                throw new Exception();
            } finally {
                db.Dispose();
            }
            return listaDeCotaParlamentar;
        }

        public List<CotaParlamentarViewModels> ListarCotaParlamentarPorTipo() {
            try {
                db.CommandText = " SELECT UPPER(txtDescricao) [txtDescricao], SUM(vlrDocumento) [vlrDocumento], SUM(vlrGlosa) [vlrGlosa], SUM(vlrLiquido) [vlrLiquido] " +
                                 " FROM [dbo].[CotaParlamentar] " +
                                 " GROUP BY UPPER(txtDescricao) " +
                                 " ORDER BY UPPER(txtDescricao) ";

                DataSet dsRetorno = db.GetDataSet();
                if (dsRetorno != null && dsRetorno.Tables[0].Rows.Count > 0) {
                    listaDeCotaParlamentar = new List<CotaParlamentarViewModels>();
                    foreach (DataRow cotaParlamentarRow in dsRetorno.Tables[0].Rows) {
                        listaDeCotaParlamentar.Add(new CotaParlamentarViewModels {
                            txtDescricao = cotaParlamentarRow["txtDescricao"].ToString(),
                            vlrDocumento = decimal.Parse(cotaParlamentarRow["vlrDocumento"].ToString()),
                            vlrGlosa = decimal.Parse(cotaParlamentarRow["vlrGlosa"].ToString()),
                            vlrLiquido = decimal.Parse(cotaParlamentarRow["vlrLiquido"].ToString())
                        });
                    }
                }
            } catch {
                throw new Exception();
            } finally {
                db.Dispose();
            }
            return listaDeCotaParlamentar;
        }

    }
}