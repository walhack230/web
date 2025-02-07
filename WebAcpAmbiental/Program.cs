using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;
using Microsoft.Extensions.Logging; // Importa el espacio de nombres necesario para los logs

namespace WebAcpAmbiental
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar el sistema de logging
            builder.Logging.ClearProviders(); // Limpia los proveedores predeterminados
            builder.Logging.AddConsole(); // Agrega la salida de logs a la consola
            builder.Logging.AddDebug(); // Agrega la salida de logs al depurador (opcional)

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient(); // Registrar IHttpClientFactory

            // Configurar la cadena de conexión a la base de datos
            builder.Services.AddDbContext<AcpAmbientalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions
            .EnableRetryOnFailure(
                maxRetryCount: 3,       // Número máximo de reintentos
                maxRetryDelay: TimeSpan.FromSeconds(5), // Tiempo máximo entre reintentos
                errorNumbersToAdd: null) // Lista de códigos de error adicionales a los que aplicar reintentos
    ));


            // Configuración de autenticación y autorización
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Index"; // Redirige a esta ruta si no está autenticado
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Jefe", policy => policy.RequireRole("Jefe"));
                options.AddPolicy("Vendedor", policy => policy.RequireRole("Vendedor"));
            });

            // Habilitar el uso de sesiones
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Configurar el tiempo de espera de la sesión
                options.Cookie.HttpOnly = true; // La cookie solo será accesible por el servidor
                options.Cookie.IsEssential = true; // Necesario para el funcionamiento básico de la aplicación
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapControllers();

            // Habilitar el uso de sesiones
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
