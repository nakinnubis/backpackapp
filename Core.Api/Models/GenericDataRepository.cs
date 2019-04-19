using Core.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Api.Models
{
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new AppDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }
            return list;
        }

    }


    public interface INationalityRepository : IGenericDataRepository<Nationalities>
    {
    }
    public class NationalityRepository : GenericDataRepository<Nationalities>, INationalityRepository
    {
     
    }
    public interface IIdentification_typesRepository : IGenericDataRepository<Identification_types>
    {
    }
    public class Identification_typesRepository : GenericDataRepository<Identification_types>, IIdentification_typesRepository
    {

    }
    public interface IGenericDataRepository<T> where T : class
    {
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
     //   IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
     
    }

}

