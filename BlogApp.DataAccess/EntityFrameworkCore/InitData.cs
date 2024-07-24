using System;
using System.Collections.Generic;
using System.Linq;
using BlogApp.Core.Constants;
using BlogApp.DataAccess.Concrete.EntityFrameworkCore.Context;
using BlogApp.Entity.Concrete;
using LoremNET;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DataAccess.EntitFrameworkCore {
    public static class InitData {

        public static void Init(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {

            using var context = new BlogDbContext();
            context.Database.Migrate();

            if (!context.Users.Any()) {

                var user = new AppUser {
                    UserName = "gurkan",
                    Email = "gurkan@gurkantuna.com"
                };

                var result = userManager?.CreateAsync(user, "123");
                if (result != null && result.Result.Succeeded) {
                    var role = new IdentityRole(Strings.Admin);
                    var resultRole = roleManager?.CreateAsync(role);

                    if (resultRole != null && resultRole.Result.Succeeded) {
                        var resultAddTole = userManager.AddToRoleAsync(user, role.Name);

                        if (resultAddTole.Result.Succeeded) {
                            context.SaveChanges();
                        }
                    }
                }
            }

            if (!context.Categories.Any()) {
                var categories = new List<Category>();

                for (int i = 1; i <= 5; i++) {
                    categories.Add(new Category { Name = $"Category{i}" });
                }

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Articles.Any()) {
                var body = @$"<p>{Lorem.Words(20)}.</p>
                                 
                                 <p><a href='https://images.pexels.com/photos/1292301/pexels-photo-1292301.jpeg?auto=compress&amp;cs=tinysrgb' data-lightbox='trees' data-title='Sample image1'>
                                 <img alt='image-1' src='https://images.pexels.com/photos/1292301/pexels-photo-1292301.jpeg?auto=compress&amp;cs=tinysrgb' /> </a></p>
                                 
                                 <p>{Lorem.Words(20)}.</p>
                                 
                                 <p><a href='https://images.pexels.com/photos/774861/pexels-photo-774861.jpeg?auto=compress&cs=tinysrgb' data-lightbox='trees' data-title='Sample image2'>
                                 <img alt='image-2' src='https://images.pexels.com/photos/774861/pexels-photo-774861.jpeg?auto=compress&cs=tinysrgb' /> </a></p>";
                var desc = Lorem.Words(20);

                const int count = 9;

                var articles = new List<Article>();

                var random = new Random();

                for (int i = 1; i <= count; i++) {
                    articles.Add(new Article {
                        Title = $"Title{i}",
                        Body = body,
                        Date = DateTime.Now,
                        CategoryId = random.Next(1, 6),
                        Description = desc,
                        Image = $"{i}.jpg",
                        ImageThumb = $"{i}_thumb.jpg",
                        IsApproved = true
                    });
                }

                context.Set<Article>().AddRange(articles);

                context.SaveChanges();
            }
        }
    }
}
