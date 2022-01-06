using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using XProject.Domain.Entities;
using XProject.Domain.Helpers;
using NS;
using NS.Entity;
using System.Data;

namespace XProject.Domain
{
    /// <summary>
    ///     Adjust Repository interaction with business object (Entity derived)
    /// </summary>
    public abstract class Repository : RepositoryBase
    {
        protected Repository(DbContext db)
            : base(db)
        {
          //  db.Configuration.AutoDetectChangesEnabled = false;
        }

       
        protected new TEntity Create<TEntity>(TEntity entity, bool force = false) where TEntity : class
        {
           
            var result = base.Create(entity, force);
            return result ? entity : null;
        }

        protected override IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> source)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(NS.Entity.Entity)))
                return Queryable.AsQueryable<TEntity>(Enumerable.Where<TEntity>((IEnumerable<TEntity>)Enumerable.ToList<TEntity>((IEnumerable<TEntity>)source), (Func<TEntity, bool>)(e =>
                {
                    if ((Enumeration)((object)e as NS.Entity.Entity).EntityStatus != (Enumeration)EntityStatus.Archived)
                        return (Enumeration)((object)e as NS.Entity.Entity).EntityStatus != (Enumeration)EntityStatus.Deleted;
                    else
                        return false;
                })));
            else
                return base.ApplyFilter<TEntity>(source);
        }

        protected override TEntity ApplyFilter<TEntity>(TEntity entity)
        {
            var entity1 = (object)entity as NS.Entity.Entity;
            if (entity1 == null)
                return entity;
            if ((Enumeration)entity1.EntityStatus == (Enumeration)EntityStatus.Archived || (Enumeration)entity1.EntityStatus == (Enumeration)EntityStatus.Deleted)
                return default(TEntity);
            else
                return base.ApplyFilter<TEntity>(entity);
        }
        //protected override bool Create<TEntity>(TEntity entity, bool force = false)
        //{
        //    var entity1 = (object)entity as NS.Entity.Entity;
        //    if (entity1 != null)
        //    {
        //        entity1.CreatedTime = UserDateTime.Now;
        //        entity1.EntityStatus = EntityStatus.Normal;
        //    }
        //    return base.Create<TEntity>(entity, force);
        //}
       /* protected override bool Update<TEntity>(TEntity entity)
        {
            var entity1 = (object)entity as NS.Entity.Entity;
            if (entity1 != null)
                entity1.UpdatedTime = new DateTime?(DateTime.Now);
            var entry = _db.Entry(entity);
           entry.State = EntityState.Modified;
            var result = base.Update(entity);
            return result;
        }
        protected override bool Delete<TEntity>(TEntity entity)
        {
            var entity1 = (object)entity as NS.Entity.Entity;
            if (entity1 == null)
                return base.Delete<TEntity>(entity);
            entity1.DeletedTime = new DateTime?(DateTime.Now);
            entity1.EntityStatus = EntityStatus.Deleted;
            return base.Update<TEntity>(entity);
        }*/
        /// <summary>
        /// Permanently delete record from database.
        /// 
        /// </summary>
     /*   protected virtual bool PermanentlyDelete<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Delete<TEntity>(entity);
        }*/

        /// <summary>
        /// Archive a specific record so it will no longer retrievable.
        /// 
        /// </summary>
        protected bool Archive(Entity entity)
        {
            entity.EntityStatus = EntityStatus.Archived;
            return base.Update<Entity>(entity);
        }

        /// <summary>
        /// Unarchive a specific record to normal state.
        /// 
        /// </summary>
        protected bool Unarchive(Entity entity)
        {
            entity.EntityStatus = EntityStatus.Normal;
            return base.Update<Entity>(entity);
        }
        //protected List<SelectItem> QuerySelectors<T>(string query, params object[] parameters) where T : class
        //{
        //    //            DbSet<T> dbSet = this._db.Set<T>();
        //    //            if (dbSet != null)
        //    //                return Enumerable.ToList<object>((IEnumerable<T>)dbSet.SqlQuery(query, parameters));
        //    //            else
        //    //                return Enumerable.ToList<object>(this._db.Database.SqlQuery<T>(query, parameters));
        //    return Enumerable.ToList<SelectItem>(this._db.Database.SqlQuery<SelectItem>(query, parameters));
        //}


        public string GetTableNameOfEntity<TEntity>() where TEntity : class
        {
            return _db.GetTableName<TEntity>();
        }

        public System.Data.DataTable ExecuteSqlQuery(string sql)
        {
            var adap = new System.Data.Odbc.OdbcDataAdapter(sql, _db.Database.Connection.ConnectionString);
            var data = new System.Data.DataTable();
            adap.Fill(data);
            return data;
        }
    }

    static class Repo
    {
        public static string GetTableName<T>(this System.Data.Entity.DbContext context) where T : class
        {
            System.Data.Objects.ObjectContext objectContext =
                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this System.Data.Objects.ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
}
