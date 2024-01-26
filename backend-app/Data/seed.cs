// using Microsoft.AspNetCore.Identity;
// using backEndApp.Models;

// namespace backEndApp.Data
// {
//     public class Seed
//     {
//         public static void SeedData(IApplicationBuilder applicationBuilder)
//         {
//             using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
//             {
//                 // var context = serviceScope.ServiceProvider.GetService<TemperatureItemContext>();
//                 // var context = serviceScope.ServiceProvider.GetService<UserContext>();

//                 context.Database.EnsureCreated();
//                 // userContext.Database.EnsureCreated();

//                 if(!context.Users.Any()) {
//                      context.Users.AddRange(new List<User>() {
//                             new User() {
//                                 UserType = "admin", 
//                                 UserName = "Han", 
//                                 UserPassword = "123", 
//                                 UserEmail = "",
//                                 UserPhone = "",
//                                 UserNotifications = true
//                             },
//                             new User() {
//                                 UserType = "basic", 
//                                 UserName = "Luke", 
//                                 UserPassword = "456", 
//                                 UserEmail = "",
//                                 UserPhone = "",
//                                 UserNotifications = false
//                             },
//                             new User() {
//                                 UserType = "basic", 
//                                 UserName = "Leia", 
//                                 UserPassword = "789", 
//                                 UserEmail = "",
//                                 UserPhone = "",
//                                 UserNotifications = true
//                             },
//                         });
//                     context.SaveChanges();
//                 }

//                 // if (!context.TemperatureItems.Any())
//                 // {
//                 //     context.TemperatureItems.AddRange(new List<TemperatureItem>()
//                 //     {
//                 //         new TemperatureItem()
//                 //         {
//                 //             TemperatureValue  = 21,
//                 //             TemperatureUnit = 'F',
//                 //             TemperatureTime = "2023-10-1 13:30:00",
//                 //         },
//                 //         new TemperatureItem()
//                 //         {
//                 //             TemperatureValue  = 22,
//                 //             TemperatureUnit = 'F',
//                 //             TemperatureTime = "2023-10-1 14:00:00",
//                 //         },                        
//                 //         new TemperatureItem()
//                 //         {
//                 //             TemperatureValue  = 23,
//                 //             TemperatureUnit = 'F',
//                 //             TemperatureTime = "2023-10-1 14:30:00",
//                 //         },
//                 //         new TemperatureItem()
//                 //         {
//                 //             TemperatureValue  = 24,
//                 //             TemperatureUnit = 'F',
//                 //             TemperatureTime = "2023-10-1 15:00:00",
//                 //         },
//                 //         new TemperatureItem()
//                 //         {
//                 //             TemperatureValue  = 25,
//                 //             TemperatureUnit = 'F',
//                 //             TemperatureTime = "2023-10-1 15:30:00",
//                 //         },                        
//                 //         new TemperatureItem()
//                 //         {
//                 //             TemperatureValue  = 26,
//                 //             TemperatureUnit = 'F',
//                 //             TemperatureTime = "2023-10-1 16:00:00",
//                 //         },
//                 //         new TemperatureItem()
//                 //         {
//                 //             TemperatureValue  = 27,
//                 //             TemperatureUnit = 'F',
//                 //             TemperatureTime = "2023-10-1 16:30:00",
//                 //         },
//                 //         new TemperatureItem()
//                 //         {
//                 //             TemperatureValue  = 28,
//                 //             TemperatureUnit = 'F',
//                 //             TemperatureTime = "2023-10-1 17:00:00",
//                 //         },
//                 //     });
//                 //     context.SaveChanges();
//                 // }
//             }
//         }

//         // public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
//         // {
//         //     using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
//         //     {
//         //         //Roles
//         //         var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//         //         if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
//         //             await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
//         //         if (!await roleManager.RoleExistsAsync(UserRoles.User))
//         //             await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

//         //         //Users
//         //         var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
//         //         string adminUserEmail = "teddysmithdeveloper@gmail.com";

//         //         var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
//         //         if (adminUser == null)
//         //         {
//         //             var newAdminUser = new AppUser()
//         //             {
//         //                 UserName = "teddysmithdev",
//         //                 Email = adminUserEmail,
//         //                 EmailConfirmed = true,
//         //                 Address = new Address()
//         //                 {
//         //                     Street = "123 Main St",
//         //                     City = "Charlotte",
//         //                     State = "NC"
//         //                 }
//         //             };
//         //             await userManager.CreateAsync(newAdminUser, "Coding@1234?");
//         //             await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
//         //         }

//         //         string appUserEmail = "user@etickets.com";

//         //         var appUser = await userManager.FindByEmailAsync(appUserEmail);
//         //         if (appUser == null)
//         //         {
//         //             var newAppUser = new AppUser()
//         //             {
//         //                 UserName = "app-user",
//         //                 Email = appUserEmail,
//         //                 EmailConfirmed = true,
//         //                 Address = new Address()
//         //                 {
//         //                     Street = "123 Main St",
//         //                     City = "Charlotte",
//         //                     State = "NC"
//         //                 }
//         //             };
//         //             await userManager.CreateAsync(newAppUser, "Coding@1234?");
//         //             await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
//         //         }
//         //     }
//         // }
//     }
// }