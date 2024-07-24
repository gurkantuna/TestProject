using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlogApp.Core.EntityFramework;
using BlogApp.DataAccess.Concrete.EntityFrameworkCore.Context;
using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DataAccess.EntitFrameworkCore.Concrete {
    public class ArticleRepo : RepoBase<Article, BlogDbContext>, IArticleRepo {

        public IEnumerable<Article> GetList() {

            using var context = new BlogDbContext();

            //TODO : Çok fazla makale ya da kategori olacdağı planlanırsa lazy loading yerine manuel çekilmeli 
            /*
            var query = from article in context.Articles
                        join category in context.Categories
                            on article.CategoryId equals category.Id into articleCategory
                        from bc in articleCategory.DefaultIfEmpty()                        
                        select new BlogCategoryDTO{};

            return query.ToList();
            */

            return context.Articles.Include("Category")
                .Where(x => x.IsApproved)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        public new IEnumerable<Article> GetList(Expression<Func<Article, bool>> expression) {
            using var context = new BlogDbContext();
            return context.Articles.Include("Category").Where(expression).ToList();
        }

        public new Article Get(Expression<Func<Article, bool>> expression) {
            using var context = new BlogDbContext();
            return context.Articles.Include("Category").FirstOrDefault(expression);
        }
    }
}
