using System;
using System.Data;
using System.Data.SqlClient;
namespace DbFactory
{
    public interface ISqlConnectionEL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlParameter"></param>
        void AddParameter(SqlParameter sqlParameter);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="ParamValue"></param>
        void AddParameter(string ParamName, object ParamValue);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlParameters"></param>
        void AddParameters(SqlParameter[] sqlParameters);
        
        /// <summary>
        /// 
        /// </summary>
        void ClearParameters();
        
        /// <summary>
        /// 
        /// </summary>
        string CommandText { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        CommandType CommandType { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        void DbTransCommit();
        
        /// <summary>
        /// 
        /// </summary>
        void DbTransRollback();
        
        /// <summary>
        /// 
        /// </summary>
        void Dispose();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int ExecuteNonQuery();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDataReader ExecuteReader();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int ExecuteScalar();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DataSet GetDataSet();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DataTable GetDataTable();
    }
}
