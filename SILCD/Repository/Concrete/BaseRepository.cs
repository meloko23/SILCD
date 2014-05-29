using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DbFactory.Implementation;
using System.Configuration;

namespace SILCD.Repository.Concrete
{
    public abstract class BaseRepository
    {
        //protected DbConnection db;

        //public BaseRepository() {
        //    if (db == null) {
        //        try { 
        //            db = new DbConnection("connectionStringAzure");
        //        }
        //        catch (Exception erro) {
        //            throw new Exception(erro.Message);
        //        }
        //    }
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (db != null)
        //        {
        //            db.Dispose();
        //        }
        //    }
        //}

        //~BaseRepository()
        //{
        //    Dispose(true);
        //}
    }
}