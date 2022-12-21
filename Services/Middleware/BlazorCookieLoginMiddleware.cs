// using System.Collections.Concurrent;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// // using WanTaskManagement.Data;
// // using WanTaskManagement.DTO;
// // using WanTaskManagement.Models;
// using Task = System.Threading.Tasks.Task;
//
// namespace WanTaskManagement.Services.Middleware;
//
// public class BlazorCookieLoginMiddleware
// {
//     private readonly RequestDelegate _next;
//
//     public BlazorCookieLoginMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }
//
//     public static IDictionary<Guid, LoginInfo> Logins { get; } = new ConcurrentDictionary<Guid, LoginInfo>();
//
//     public async Task InvokeAsync(HttpContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IDbContextFactory<ApplicationDbContext> dbContextFactory, GlobalAppState globalAppState)
//     {
//         if (context.Request.Path == "/loginWithCookie" && context.Request.Query.ContainsKey("key"))
//         {
//             var key = Guid.Parse(context.Request.Query["key"]);
//             var info = Logins[key];
//
//             var result = await signInManager.PasswordSignInAsync(info.UserName, info.Password, false, true);
//             info.Password = null;
//             if (result.Succeeded)
//             {
//                 Logins.Remove(key);
//                 context.Response.Redirect("/");
//
//                 return;
//             }
//
//             if (result.RequiresTwoFactor)
//             {
//                 //TODO: redirect to 2FA razor component
//                 context.Response.Redirect($"/loginwith2fa/{key}");
//                 return;
//             }
//
//             //TODO: Proper error handling
//             context.Response.Redirect("/loginfailed");
//             return;
//         }
//
//         if (context.Request.Path.Value != null && context.Request.Path != "/login" &&
//             !context.Request.Path.Value.StartsWith("/Files/images") &&
//             !context.Request.Path.Value.StartsWith("/_blazor") &&
//             !context.Request.Path.Value.StartsWith("/api"))
//         {
//             if (context.User.Identity != null)
//             {
//                 if (!context.User.Identity.IsAuthenticated)
//                 {
//                     // if (context.Request.Path == "/") context.Request.Path = "/Dashboard";
//                     context.Response.Redirect($"/login?returnUrl={context.Request.Path}");
//                 }
//                 else
//                 {
//                     var appUser = await userManager.GetUserAsync(context.User);
//                     var appStateData = new AppState
//                     {
//                         ApplicationUser = await userManager.Users.AsQueryable().FirstOrDefaultAsync(x => x.Id == appUser.Id)
//                     };
//                     if (!GlobalAppState.IsAvailable(appUser.UserName))
//                         GlobalAppState.SetState(appUser.UserName, appStateData);
//                 }
//             }
//             else
//             {
//                 // if (context.Request.Path == "/") context.Request.Path = "/Dashboard";
//                 context.Response.Redirect($"/login?returnUrl={context.Request.Path}");
//             }
//         }
//
//         if (context.Request.Path.Value != null && context.Request.Path == "/login" &&
//             !context.Request.Path.Value.StartsWith("/Files/images") &&
//             !context.Request.Path.Value.StartsWith("/_blazor") &&
//             !context.Request.Path.Value.StartsWith("/api"))
//             if (context.User.Identity is {IsAuthenticated: true})
//                 context.Response.Redirect("/");
//
//         if (context.Request.Path.Value != null && context.Request.Path == "/logout" &&
//             !context.Request.Path.Value.StartsWith("/Files/images") &&
//             !context.Request.Path.Value.StartsWith("/_blazor") &&
//             !context.Request.Path.Value.StartsWith("/api"))
//         {
//             await signInManager.SignOutAsync();
//             context.Response.Redirect("/");
//         }
//
//         await _next.Invoke(context);
//     }
// }