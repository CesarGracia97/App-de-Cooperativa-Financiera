﻿namespace act_Application.Data.Data;
using act_Application.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

public static class RolesInitializer
{
    public static void CreateRoles(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var dbContext = serviceProvider.GetRequiredService<DesarrolloContext>();
        if (!dbContext.ActRols.Any())
        {
            dbContext.ActRols.AddRange(new[]
            {
                new ActRol { NombreRol = "Administrador", DescripcionRol = "Rol con acceso total al sistema" },
                new ActRol { NombreRol = "Usuario", DescripcionRol = "Rol con permisos limitados" }
            });

            dbContext.SaveChanges();
        }
    }
}
