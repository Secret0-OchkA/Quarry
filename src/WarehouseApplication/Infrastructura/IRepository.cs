﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructura
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> GetById(Guid Id);
        Task<IEnumerable<TEntity>> GetAll(int page = 0,int pageSize = 10);

        Task<int> Cereate(TEntity entity);

        Task<int> Update(Guid Id, TEntity entity);

        Task<int> Delete(Guid Id);
    }
}