using act_Application.Data.Data;
using act_Application.Data.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Cadena de conexion BD
builder.Services.AddDbContext<ActDesarrolloContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Cadena")));

//Autenticacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(16); // Tiempo de expiracion de sesion inactiva
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

    options.AddPolicy("AdminReferenteOnly", policy =>
        policy.RequireClaim("IdRol", "1", "3")); // Admin o Referente

    options.AddPolicy("SocioReferenteOnly", policy =>
        policy.RequireClaim("IdRol", "2", "3")); // Socio o Referente

    options.AddPolicy("AllOnly", policy =>
        policy.RequireClaim("IdRol", "1", "2", "3")); // Todos

});

var app = builder.Build();

builder.AddHostedService<MyBackgroundService>();

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

public class MyBackgroundService : BackgroundService
{
    private readonly ILogger<MyBackgroundService> _logger;

    public MyBackgroundService(ILogger<MyBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("El servicio en segundo plano ha comenzado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Coloca aquí el código que deseas que se ejecute cada 5 minutos
            _logger.LogInformation("Este código se ejecuta cada 5 minutos - {0}", DateTime.Now);

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        _logger.LogInformation("El servicio en segundo plano se ha detenido.");
    }
}