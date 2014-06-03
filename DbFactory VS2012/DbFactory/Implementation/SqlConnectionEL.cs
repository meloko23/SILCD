using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DbFactory.Implementation
{
    public class SqlConnectionEL : ISqlConnectionEL
    {
        protected DatabaseProviderFactory factory = new DatabaseProviderFactory();
        protected Database db;
        private DbTransaction transaction;
        private DbCommand cmd = new SqlCommand();

        private bool IsTransaction { get; set; }

        /// <summary>
        /// Construtor 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IsTransaction"></param>
        public SqlConnectionEL(string ConnectionString, bool IsTransaction = false) {
            if (db == null) {
                db = factory.Create(ConnectionString);
            }

            if (IsTransaction)
                transaction = db.CreateConnection().BeginTransaction();

            this.IsTransaction = IsTransaction;
        }

        /// <summary>
        //     Gets or sets the text command to run against the data source.
        /// </summary>
        public string CommandText {
            get {
                return cmd.CommandText;
            }
            set {
                cmd.CommandText = value;
            }
        }

        /// <summary>
        //     Specifies how a command string is interpreted.
        /// </summary>
        public CommandType CommandType {
            get {
                return cmd.CommandType;
            }
            set {
                cmd.CommandType = value;
            }
        }

        /// <summary>
        /// Add a SQL Parameter
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="ParamValue"></param>
        public void AddParameter(string ParamName, object ParamValue) {
            cmd.Parameters.Add(new SqlParameter(ParamName, ParamValue));
        }

        /// <summary>
        /// Add a SQL Parameter
        /// </summary>
        /// <param name="sqlParameter"></param>
        public void AddParameter(SqlParameter sqlParameter) {
            cmd.Parameters.Add(sqlParameter);
        }

        /// <summary>
        /// Add SQL Parameters
        /// </summary>
        /// <param name="sqlParameter"></param>
        public void AddParameters(SqlParameter[] sqlParameters) {
            foreach (SqlParameter sqlparam in sqlParameters) {
                cmd.Parameters.Add(sqlparam);
            }
        }

        /// <summary>
        /// Clear command parameters
        /// </summary>
        public void ClearParameters() {
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonQuery() {
            try {
                return db.ExecuteNonQuery(cmd);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ExecuteScalar() {
            try {
                return Convert.ToInt32(db.ExecuteScalar(cmd));
            } catch {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDataReader ExecuteReader() {
            try {
                return db.ExecuteReader(cmd);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet() {
            try {
                return db.ExecuteDataSet(cmd);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable() {
            try {
                return db.ExecuteDataSet(cmd).Tables[0];
            } catch {
                throw;
            }
        }

        public void DbTransCommit() {
            if (transaction != null && transaction.Connection != null && transaction.Connection.State == ConnectionState.Open) {
                transaction.Commit();
            }
        }

        public void DbTransRollback() {
            if (transaction != null && transaction.Connection != null && transaction.Connection.State != ConnectionState.Closed) {
                transaction.Rollback();
            }
        }

        public void Dispose() {
            transaction.Dispose();
            cmd.Dispose();
        }

        /// <summary>
        /// Destrutor da classe
        /// </summary>
        ~SqlConnectionEL() {
            this.Dispose();
        }
    }
}
