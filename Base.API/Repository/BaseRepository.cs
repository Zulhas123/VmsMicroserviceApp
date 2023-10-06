using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Base.API.Interface.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Base.API.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private DbContext db;

        public BaseRepository(DbContext dbContext)
        {
            db = dbContext;
        }

        protected DbSet<T> Table
        {
            get
            {
                return db.Set<T>();

            }
        }

        public bool Add(T entity)
        {
            Table.Add(entity);
            return db.SaveChanges() > 0;

        }

        public bool Add(ICollection<T> entities)
        {
            Table.AddRange(entities);
            return db.SaveChanges() > 0;
        }

        public bool Delete(T entity)
        {
            Table.Remove(entity);
            return db.SaveChanges() > 0;
        }

        public bool Delete(ICollection<T> entities)
        {
            Table.RemoveRange(entities);
            return db.SaveChanges() > 0;
        }

        public DataTable ExecuteRawSql(string sql)
        {
            DataTable dt = new DataTable();
            var con = db.Database.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(con))
            {
                
                SqlCommand command = new SqlCommand(sql, connection);
                
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add(reader.GetName(i));
                    }
                    while (reader.Read())
                    {
                        DataRow dr = dt.NewRow();                       
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var value = reader[i];                           
                            dr[i] = value;
                            
                        }
                        dt.Rows.Add(dr);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    
                }
              
            }
            return dt;
        }


        public int ExecuteNonQuerySql(string sql)
        {
            int res = 0;
            var con = db.Database.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(con))
            {

                SqlCommand command = new SqlCommand(sql, connection);

                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception ex)
                {
                    
                }

            }
            return res;
        }

        public ICollection<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return includes
                .Aggregate(
                    Table.AsNoTracking().AsQueryable(),
                    (current, include) => current.Include(include),
                    c => c.Where(predicate)
                ).ToList();
        }

        public ICollection<T> GetAll(params Expression<Func<T, object>>[] include)
        {
            return include
                .Aggregate(
                    Table.AsNoTracking().AsQueryable(),
                    (current, includes) => current.Include(includes)
                ).ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return includes
                .Aggregate(
                    Table.AsNoTracking().AsQueryable(),
                    (current, include) => current.Include(include),
                    c => c.FirstOrDefault(predicate)
                );
        }

        public bool Update(T entity)
        {
            Table.Update(entity);
            return db.SaveChanges() > 0;
        }

        public bool Update(ICollection<T> entities)
        {
            Table.UpdateRange(entities.ToArray());
            return db.SaveChanges() > 0;
        }
    }
}
