using System;
using System.Collections.Generic;
//using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using XProject.Domain.Abstract;


namespace XProject.Domain.Concrete
{
    public class EFGeneralRepository<TEntity> : Repository, IGeneralRepository<TEntity> where TEntity : EntityBase
    {
        public EFGeneralRepository(DbContext db) : base(db)
        {
        }

        public IEnumerable<TEntity> GetAllTEntities(Expression<Func<TEntity, object>> includeProperties = null)
        {
            var query = includeProperties == null ? All<TEntity>() : All<TEntity>(includeProperties);

           
            return query.ToList();
        }
        public IEnumerable<TEntity> GetAllTEntities(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> includeProperties = null)
        {
            var query = includeProperties == null ? All<TEntity>() : All<TEntity>(includeProperties);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

           
            return query.ToList();
        }

        public IQueryable<TEntity> GetIQueryableItems(Expression<Func<TEntity, object>> includeProperties = null)
        {
            var query = includeProperties == null ? All<TEntity>() : All<TEntity>(includeProperties);
         
            return query;
        }

        public TEntity GetItem(int id, Expression<Func<TEntity, object>> includeProperties = null)
        {
            TEntity item;
            if (includeProperties == null)
                item = Single<TEntity>(m => m.ID == id);
            else
                item = Single<TEntity>(m => m.ID == id, includeProperties);
           
            return item;
        }

        public TEntity CreateItem(TEntity item)
        {
       
            var et= Create(item);
       
            return et;
        }

        public bool UpdateItem(TEntity item)
        {
            
           
            bool b= Update(item);
           
            return b;
        }

        public bool DeleteItem(int id)
        {
            
           
                return Delete<TEntity>(id);
           
        }

        public IEnumerable<TEntity> ExecuSql(string sql)
        {
            var data = _db.Database.SqlQuery<TEntity>(sql);
            return data;
        } 
        public IEnumerable<T> ExecuSql<T> (string sql)where T:class 
        {
            var data = _db.Database.SqlQuery<T>(sql);
            return data;
        }

        public System.Data.DataSet ExecuSql_Dataset(string sql)
        {

            var conns= ((StackExchange.Profiling.Data.ProfiledDbConnection) _db.Database.Connection).WrappedConnection;
            var ds = new System.Data.DataSet();
            var adap = new  System.Data.SqlClient.SqlDataAdapter(sql, (SqlConnection)conns);
            adap.Fill(ds);
            return ds;
        }

        public object ExecuSql_Scalar(string sql)
        {
            return  _db.Database.SqlQuery<int>(sql).Single();
        }
    }
}
