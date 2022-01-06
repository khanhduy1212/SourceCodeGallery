using XProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace XProject.Domain.Abstract
{
    public interface IGeneralRepository<TEntity> where TEntity : EntityBase
    {
        IEnumerable<TEntity> GetAllTEntities(Expression<Func<TEntity, object>> includeProperties = null);
        IEnumerable<TEntity> GetAllTEntities(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> includeProperties = null);
        IQueryable<TEntity> GetIQueryableItems(Expression<Func<TEntity, object>> includeProperties = null);

        TEntity GetItem(int id, Expression<Func<TEntity, object>> includeProperties = null);
        TEntity CreateItem(TEntity item);
        bool UpdateItem(TEntity item);
        bool DeleteItem(int id);
      
        IEnumerable<TEntity> ExecuSql(string sql);
        System.Data.DataSet ExecuSql_Dataset(string sql);

        Object ExecuSql_Scalar(string sql);
    }
}
