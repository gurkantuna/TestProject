using BlogApp.Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using BlogApp.Core.Constants;
using BlogApp.DataAccess.Concrete.EntityFramework;

namespace BlogApp.DataAccess.Concrete {
    public static class SeedData {

        public static void Seed(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {

            using var context = new BlogDbContext();
            context.Database.Migrate();

            if (!context.Users.Any()) {

                var user = new AppUser {
                    UserName = "gurkan",
                    Email = "gurkan@gurkantuna.com"
                };

                var result = userManager?.CreateAsync(user, "123");
                if (result != null && result.Result.Succeeded) {
                    var role = new IdentityRole(ConstStrings.Admin);
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
                context.Categories.AddRange(
                       new Category { Name = "Category1" },
                                    new Category { Name = "Category2" },
                                    new Category { Name = "Category3" },
                                    new Category { Name = "Category4" },
                                    new Category { Name = "Category5" }
                    );
                context.SaveChanges();
            }

            if (!context.Blogs.Any()) {

                const string body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam sed ligula a nunc euismod elementum sit amet sed ipsum. Suspendisse potenti. Morbi in risus efficitur, tincidunt dui id, malesuada diam. Fusce faucibus ante mi, nec rutrum metus laoreet eu. Quisque sit amet diam pellentesque, aliquet neque sed, vulputate lectus. Praesent at libero risus. Phasellus a dignissim mauris, sit amet suscipit risus. Integer ultricies eu turpis a convallis. Morbi vehicula, purus at ultricies dapibus, enim neque porta nunc, eu posuere quam tortor ut neque. Maecenas et sapien scelerisque, tempus mauris in, porttitor lacus.";
                const string bodyWithLightBox = @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam sed ligula a nunc euismod elementum sit amet sed ipsum. Suspendisse potenti.
                                                Morbi in risus efficitur, tincidunt dui id, malesuada diam. Fusce faucibus ante mi, nec rutrum metus laoreet eu. Quisque sit amet diam pellentesque, aliquet neque sed, vulputate lectus.
                                                Praesent at libero risus. Phasellus a dignissim mauris, sit amet suscipit risus. Integer ultricies eu turpis a convallis. Morbi vehicula, purus at ultricies dapibus, enim neque porta nunc,
                                                eu posuere quam tortor ut neque. Maecenas et sapien scelerisque, tempus mauris in, porttitor lacus.</p>
                                                
                                                <p><a href='https://images.pexels.com/photos/1292301/pexels-photo-1292301.jpeg?auto=compress&amp;cs=tinysrgb' data-lightbox='trees' data-title='Sample image1'>
                                                <img alt='image-1' src='https://images.pexels.com/photos/1292301/pexels-photo-1292301.jpeg?auto=compress&amp;cs=tinysrgb' /> </a></p>
                                                
                                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam sed ligula a nunc euismod elementum sit amet sed ipsum. Suspendisse potenti. Morbi in risus efficitur, tincidunt dui id, malesuada diam. 
                                                Fusce faucibus ante mi, nec rutrum metus laoreet eu. Quisque sit amet diam pellentesque, aliquet neque sed, vulputate lectus. Praesent at libero risus. Phasellus a dignissim mauris, sit amet suscipit risus.
                                                Integer ultricies eu turpis a convallis. Morbi vehicula, purus at ultricies dapibus, enim neque porta nunc, eu posuere quam tortor ut neque. Maecenas et sapien scelerisque, tempus mauris in, porttitor lacus.</p>
                                                
                                                <p><a href='https://images.pexels.com/photos/774861/pexels-photo-774861.jpeg?auto=compress&cs=tinysrgb' data-lightbox='trees' data-title='Sample image2'>
                                                <img alt='image-2' src='https://images.pexels.com/photos/774861/pexels-photo-774861.jpeg?auto=compress&cs=tinysrgb' /> </a></p>";
                const string desc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam sed ligula a nunc euismod elementum sit amet sed ipsum.";

                context.Set<Blog>().AddRange(
                               new Blog {
                                   Title = "Title1",
                                   Body = bodyWithLightBox,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category1")?.Id,
                                   Description = desc,
                                   Image = "1.jpg",
                                   ImageThumb = "1_thumb.jpg",
                                   IsApproved = true
                               },
                               new Blog {
                                   Title = "Title2",
                                   Body = body,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category1")?.Id,
                                   Description = desc,
                                   Image = "2.jpg",
                                   ImageThumb = "2_thumb.jpg",
                                   IsApproved = true
                               },
                               new Blog {
                                   Title = "Title3",
                                   Body = body,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category2")?.Id,
                                   Description = desc,
                                   Image = "3.jpg",
                                   ImageThumb = "3_thumb.jpg",
                                   IsApproved = true
                               },
                               new Blog {
                                   Title = "Title4",
                                   Body = body,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category2")?.Id,
                                   Description = desc,
                                   Image = "4.jpg",
                                   ImageThumb = "4_thumb.jpg",
                                   IsApproved = true
                               },
                               new Blog {
                                   Title = "Title5",
                                   Body = body,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category3")?.Id,
                                   Description = desc,
                                   Image = "5.jpg",
                                   ImageThumb = "5_thumb.jpg",
                                   IsApproved = true
                               },
                               new Blog {
                                   Title = "Title6",
                                   Body = body,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category3")?.Id,
                                   Description = desc,
                                   Image = "6.jpg",
                                   ImageThumb = "6_thumb.jpg",
                                   IsApproved = true
                               },
                               new Blog {
                                   Title = "Title7",
                                   Body = body,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category4")?.Id,
                                   Description = desc,
                                   Image = "7.jpg",
                                   ImageThumb = "7_thumb.jpg",
                                   IsApproved = true
                               },
                               new Blog {
                                   Title = "Title8",
                                   Body = body,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category4")?.Id,
                                   Description = desc,
                                   Image = "8.jpg",
                                   ImageThumb = "8_thumb.jpg",
                                   IsApproved = true
                               },
                               new Blog {
                                   Title = "Title9",
                                   Body = bodyWithLightBox,
                                   Date = DateTime.Now,
                                   CategoryId = context.Categories.FirstOrDefault(x => x.Name == "Category4")?.Id,
                                   Description = desc,
                                   Image = "9.jpg",
                                   ImageThumb = "9_thumb.jpg",
                                   IsApproved = true
                               }
                               );

                context.SaveChanges();
            }
        }
    }
}
