using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BlogApp.Core.EntityFrameworkCore;
using BlogApp.Entity.Concrete;

namespace BlogApp.DataAccess.EntitFrameworkCore.Abstract {
    public interface IArticleRepo : IRepoBase<Article> {

        IEnumerable<Article> GetList();
        new IEnumerable<Article> GetList(Expression<Func<Article, bool>> expression);
        new Article Get(Expression<Func<Article, bool>> expression);
    }
}
