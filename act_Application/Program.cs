using act_Application.Data.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Cadena de conexion BD
builder.Services.AddDbContext<DesarrolloContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Cadena")));

//Autenticacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15); // Tiempo de expiraci�n de sesi�n inactiva
    });

//Autorizacion (cualquier cambio o agregado directamente en los Claims en LoginController).
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("IdRol", "1")); //Admin

    options.AddPolicy("SocioOnly", policy =>
        policy.RequireClaim("IdRol", "2")); //Socio

    options.AddPolicy("ReferenteOnly", policy =>
        policy.RequireClaim("IdRol", "3")); //Referente

    options.AddPolicy("AdminSocioOnly", policy =>
        policy.RequireClaim("IdRol", "1", "2")); // Admin o Socio

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //Habilitada la Autenticacion
app.UseAuthorization();  //Habilitada la Autorizacion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

RolesInitializer.CreateRoles(app);

app.Run();