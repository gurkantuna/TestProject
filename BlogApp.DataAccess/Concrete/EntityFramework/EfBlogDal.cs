using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlogApp.Core.EntityFramework;
using BlogApp.DataAccess.Abstract;
using BlogApp.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DataAccess.Concrete.EntityFramework {
    public class EfBlogDal : EfRepositoryBase<Blog, BlogDbContext>, IBlogDal {

        public IEnumerable<Blog> GetList() {

            using var context = new BlogDbContext();

            //TODO : Çok fazla blog ya da kategori olacdağı planlanırsa lazy loading yerine manuel çekilmeli 
            /*
            var query = from blog in context.Blogs
                        join category in context.Categories
                            on blog.CategoryId equals category.Id into blogCategory
                        from bc in blogCategory.DefaultIfEmpty()                        
                        select new BlogCategoryDTO{};

            return query.ToList();
            */

            return context.Blogs.Include("Category")
                .Where(x => x.IsApproved)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        public new IEnumerable<Blog> GetList(Expression<Func<Blog, bool>> expression) {
            using var context = new BlogDbContext();
            return context.Blogs.Include("Category").Where(expression).ToList();
        }

        public new Blog Get(Expression<Func<Blog, bool>> expression) {
            using var context = new BlogDbContext();
            return context.Blogs.Include("Category").FirstOrDefault(expression);
        }
    }
}
