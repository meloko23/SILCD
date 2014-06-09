using DbFactory.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILCD.CargaDados {
    class Program {

        static void Main(string[] args) {
            Console.Clear();
            Console.WriteLine("Informe o arquivo xml para efetuar a carga: ");
            var arquivo = Console.ReadLine();

            if (!File.Exists(arquivo)) {
                Console.WriteLine("Arquivo não existe! Informe um arquivo válido/existente!");                
            }

            //CarregarCotaParlamentar(@"C:\CotaParlamentar\CotaParlamentar2014.xml");
            //CarregarCotaParlamentar(@arquivo);

            Console.ReadLine();
        }

        static void CarregarCotaParlamentar(string xmlFile) {
            DataSet dataSetCotaParlamentar = new DataSet();
            dataSetCotaParlamentar.ReadXml(xmlFile);
            DataTable dataTable = dataSetCotaParlamentar.Tables["DESPESA"];
            DataTable dataTableDestino = new DataTable();
            dataTableDestino.TableName = "CotaParlamentar";
            BulkInsert(dataTable, dataTableDestino);
            Console.ReadLine();
        }

        private static void BulkInsert(DataTable dtOrigen, DataTable dtDestino) {
            try {
                using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionStringLocal"].ToString())) {
                    dbConnection.Open();
                    try {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(dbConnection)) {
                            try {
                                sqlBulkCopy.BulkCopyTimeout = 999999999;
                                sqlBulkCopy.DestinationTableName = dtDestino.TableName;
                                sqlBulkCopy.WriteToServer(dtOrigen);
                            } catch { 
                                throw new Exception(); 
                            } finally { 
                                sqlBulkCopy.Close();
                                dbConnection.Close();
                                dbConnection.Dispose();
                            }
                        }
                    } catch {
                        throw new Exception();
                    } finally {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                }
            } catch {
                throw new Exception();
            }
        }

    }
}
