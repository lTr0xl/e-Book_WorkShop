using e_Book.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace e_Book.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                
                //Authors
                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(new List<Author>()
                    {
                        new Author()
                        {
                            FirstName = "Wim",
                            LastName = "Hof",
                            BirthDate = new DateOnly(1959,08,20),
                            Nationality = "Dutch",
                            Gender = "Male",
                        },
                        new Author()
                        {
                            FirstName = "George",
                            LastName = "Hilingman",
                            BirthDate = new DateOnly(1959,08,20),
                            Nationality = "Dutch",
                            Gender = "Male",
                        }
                    });
                    context.SaveChanges();
                }
                //Books
                if (!context.Books.Any())
                {
                    context.Books.AddRange(new List<Book>()
                    {
                        new Book()
                        {
                            Title = "The Wim Hof Method",
                            YearPublished = 2020,
                            NumPages = 232,
                            Publisher = "Sounds True",
                            FrontPage = "https://m.media-amazon.com/images/I/91RtIHfSj6L._SL1500_.jpg",
                            AuthorId = 1,
                        },
                    });
                    context.SaveChanges();
                }
                //Genres
                if (!context.Genres.Any())
                {
                    context.Genres.AddRange(new List<Genre>()
                    {
                        new Genre()
                        {
                            GenreName = "Fiction",
                        },
                        new Genre()
                        {
                            GenreName = "Mystery",
                        },
                        new Genre()
                        {
                            GenreName = "Science fiction",
                        },
                        new Genre()
                        {
                            GenreName = "Biography",
                        },
                        new Genre()
                        {
                            GenreName = "Fantasy",
                        },
                        new Genre()
                        {
                            GenreName = "History",
                        },
                        new Genre()
                        {
                            GenreName = "Self-help book",
                        },
                    });
                    context.SaveChanges();
                }

                //BookGenres
                if (!context.BookGenres.Any())
                {
                    context.BookGenres.AddRange(new List<BookGenre>()
                    {
                        new BookGenre()
                        {
                            BookId = 4,
                            GenreId = 4,
                        },
                        new BookGenre()
                        {
                            BookId = 4,
                            GenreId = 7,
                        },
                    });
                    context.SaveChanges();  
                }

                //Reviews
                if (!context.Reviews.Any())
                {
                    context.Reviews.AddRange(new List<Review>()
                    {
                        new Review()
                        {
                            BookId= 4,
                            AppUser = "Toshe",
                            Comment = "Nogu jaka kniga brat, svakaa chast!",
                            Rating = 5,
                        },
                        new Review()
                        {
                            BookId= 4,
                            AppUser = "Gjoshe",
                            Comment = "Dechko ovaa kniga mi go smeni zhivoto",
                            Rating = 5,
                        },
                        new Review()
                        {
                            BookId= 4,
                            AppUser = "Nikola",
                            Comment = "Ne go lazhi narodo be smotan",
                            Rating = 2,
                        },
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "app-admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
