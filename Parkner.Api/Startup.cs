using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Parkner.Api.Models;
using Parkner.Api.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Parkner.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuracion) => this.Configuracion = configuracion;

        private IConfiguration Configuracion { get; }

        public void ConfigureServices(IServiceCollection servicios)
        {
            servicios.AddCors
            (o => o.AddPolicy
                (
                    "Permitidos",
                    b => b
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         //.AllowAnyOrigin()
                         .AllowCredentials()
                         .WithOrigins
                         (
                             "https://localhost:44301",
                             "https://localhost:5001",
                             "https://parkner.bedol.la",
                             "https://parkner.azurewebsites.net",
                             "https://web.postman.co"
                         )
                )
            );

            servicios.AddScoped<IRepositorioSesion, RepositorioSesion>();
            servicios.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();
            servicios.AddScoped<IRepositorioClientes, RepositorioClientes>();
            servicios.AddScoped<IRepositorioResponsables, RepositorioResponsables>();
            servicios.AddScoped<IRepositorioEmpleados, RepositorioEmpleados>();
            servicios.AddScoped<IRepositorioEstacionamientos, RepositorioEstacionamientos>();
            servicios.AddScoped<IRepositorioGanancias, RepositorioGanancias>();
            servicios.AddScoped<IRepositorioCajones, RepositorioCajones>();
            servicios.AddScoped<IRepositorioHorarios, RepositorioHorarios>();
            servicios.AddScoped<IRepositorioReservas, RepositorioReservas>();

            servicios.AddControllers();

            servicios.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer
                     (o => o.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,
                             ValidateAudience = true,
                             ValidateLifetime = true,
                             ValidateIssuerSigningKey = true,
                             ValidIssuer = this.Configuracion["JWT:Issuer"],
                             ValidAudience = this.Configuracion["JWT:Audience"],
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuracion["JWT:ClaveSecreta"]))
                         }
                     );

            servicios.AddMvc(o => o.EnableEndpointRouting = false)
                     .AddNewtonsoftJson(options =>
                     {
                         options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                         options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                     });

            servicios.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Parkner",
                    Version = "v1",
                    Description = "API privada de Parkner",
                    TermsOfService = new Uri("https://www.pichancha.com/terminos"),
                    Contact = new OpenApiContact {Name = "Pichancha Collective", Email = "Info@Pichancha.com", Url = new Uri("https://www.pichancha.com")},
                    License = new OpenApiLicense {Name = "MIT", Url = new Uri("https://www.pichancha.com/licencia")}
                });

                o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {In = ParameterLocation.Header, Description = "Autorización JWT. Formato: \"Bearer {Token}\"", Name = "Authorization", Type = SecuritySchemeType.ApiKey});

                o.AddSecurityRequirement(new OpenApiSecurityRequirement {{new OpenApiSecurityScheme {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}}, new List<string>()}});
            });

            servicios.AddDbContext<Contexto>(o => o.UseSqlServer(this.Configuracion.GetConnectionString("Remoto")));

            servicios.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void Configure(IApplicationBuilder aplicacion, IWebHostEnvironment entorno, ILoggerFactory registros)
        {
            if (entorno.IsDevelopment()) aplicacion.UseDeveloperExceptionPage();

            aplicacion.UseDefaultFiles();

            aplicacion.UseStaticFiles();

            aplicacion.UseHttpsRedirection();

            aplicacion.UseRouting();

            aplicacion.UseCors("Permitidos");

            aplicacion.UseAuthorization();

            aplicacion.UseSwagger();

            aplicacion.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "Versión 1");
                o.RoutePrefix = String.Empty;
            });

            aplicacion.UseEndpoints(e => e.MapControllers());

            registros.AddFile("Logs/{Date}.txt");

            aplicacion.UseAuthentication();

            aplicacion.UseMvc();
        }
    }
}
