using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using OnlineShopManagement.Models;
using OnlineShopManagement.Models.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option=>
{
    option.Password.RequiredLength = 8;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequiredUniqueChars = 3;

}).AddEntityFrameworkStores<AppDbContext>();

//Global Authorization
builder.Services.AddMvc(option =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    option.Filters.Add(new AuthorizeFilter(policy));
});

//Setting up Access Denied Path 
builder.Services.ConfigureApplicationCookie(option =>
{
    option.AccessDeniedPath = new PathString("/Administrator/AccessDenied");
});

//Claim Based Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true"));
    options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role", "true"));


    //Building Custom policy for Super Admin
    options.AddPolicy("EditRolePolicy",
        policy=> policy.RequireAssertion(context=>
        context.User.IsInRole("Admin") &&
        context.User.HasClaim(claim=> claim.Type == "Edit Role" && claim.Value =="true") ||
        context.User.IsInRole("Super Admin")
        ));

    //Custom authorization requirements from Security folder, Where logged in user can only edit others role 
    options.AddPolicy("CustomEditRolePolicy",
    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
});

//builder.Services.AddAuthentication()
//                .AddGoogle(options =>
//                {
//                    options.ClientId = "742774279118-7ob2re5sv1m4pcvh9a37r7o0pdgdsofe.apps.googleusercontent.com";
//                    options.ClientSecret = "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";
//                });
//                //.AddFacebook(options =>
//                //{
//                //    options.AppId = "2316662895109472";
//                //    options.AppSecret = "e25c1b8d4145034ed426d7a05efe1481";
//                //});

builder.Services.AddScoped<IProductRepositories, SQLProduct>();
builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
