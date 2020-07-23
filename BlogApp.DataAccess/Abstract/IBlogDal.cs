using BlogApp.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BlogApp.Core;

namespace BlogApp.DataAccess.Abstract {
    public interface IBlogDal : IEntityRepository<Blog> {

        IEnumerable<Blog> GetList();
        new IEnumerable<Blog> GetList(Expression<Func<Blog, bool>> expression);
        new Blog Get(Expression<Func<Blog, bool>> expression);
    }
}
