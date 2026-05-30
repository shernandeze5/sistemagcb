using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Business.Services;
using GestionCuentasBancarias.Data.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionCuentasBancarias.API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT. Ejemplo: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<OracleConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var connectionString = configuration.GetConnectionString("OracleConnection")
        ?? throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");

    return new OracleConnectionFactory(connectionString);
});

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("No se encontró Jwt:Key en appsettings.json.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        ),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();

builder.Services.AddScoped<ITipoMovimientoRepository, TipoMovimientoRepository>();
builder.Services.AddScoped<ITipoMovimientoService, TipoMovimientoService>();

builder.Services.AddScoped<IBancoRepository, BancoRepository>();
builder.Services.AddScoped<IBancoService, BancoService>();

builder.Services.AddScoped<ITipoCuentaRepository, TipoCuentaRespository>();
builder.Services.AddScoped<ITipoCuentaService, TipoCuentaService>();

builder.Services.AddScoped<ITipoPersonaRepository, TipoPersonaRepository>();
builder.Services.AddScoped<ITipoPersonaService, TipoPersonaService>();

builder.Services.AddScoped<ITipoTelefonoRepository, TipoTelefonoRepository>();
builder.Services.AddScoped<ITipoTelefonoService, TipoTelefonoService>();

builder.Services.AddScoped<IMedioMovimientoRepository, MedioMovimientoRepository>();
builder.Services.AddScoped<IMedioMovimientoService, MedioMovimientoService>();

builder.Services.AddScoped<IEstadoMovimientoRepository, EstadoMovimientoRepository>();
builder.Services.AddScoped<IEstadoMovimientoService, EstadoMovimientoService>();

builder.Services.AddScoped<ITipoMonedaRepository, TipoMonedaRepository>();
builder.Services.AddScoped<ITipoMonedaService, TipoMonedaService>();

builder.Services.AddScoped<ITipoDireccionRepository, TipoDireccionRepository>();
builder.Services.AddScoped<ITipoDireccionService, TipoDireccionService>();

builder.Services.AddScoped<IEstadoCuentaRepository, EstadoCuentaRepository>();
builder.Services.AddScoped<IEstadoCuentaService, EstadoCuentaService>();

builder.Services.AddScoped<IEstadoChequeRepository, EstadoChequeRepository>();
builder.Services.AddScoped<IEstadoChequeService, EstadoChequeService>();

builder.Services.AddScoped<IEstadoConciliacionRepository, EstadoConciliacionRepository>();
builder.Services.AddScoped<IEstadoConciliacionService, EstadoConciliacionService>();

builder.Services.AddScoped<IEstadoDetalleConciliacionRepository, EstadoDetalleConciliacionRepository>();
builder.Services.AddScoped<IEstadoDetalleConciliacionService, EstadoDetalleConciliacionService>();

builder.Services.AddScoped<ITasaInteresRepository, TasaInteresRepository>();
builder.Services.AddScoped<ITasaInteresService, TasaInteresService>();

builder.Services.AddScoped<IinteresFrecuenciaRepository, InteresFrecuenciaRepository>();
builder.Services.AddScoped<IinteresFrecuenciaService, InteresFrecuenciaService>();

builder.Services.AddScoped<IReglaRecargoRepository, ReglaRecargoRepository>();
builder.Services.AddScoped<IReglaRecargoService, ReglaRecargoService>();

builder.Services.AddScoped<IConversionMonedaRepository, ConversionMonedaRepository>();
builder.Services.AddScoped<IConversionMonedaService, ConversionMonedaService>();

builder.Services.AddScoped<IChequeraRepository, ChequeraRepository>();
builder.Services.AddScoped<IChequeraService, ChequeraService>();

builder.Services.AddScoped<ICuentaBancariaRepository, CuentaBancariaRepository>();
builder.Services.AddScoped<ICuentaBancariaService, CuentaBancariaService>();

builder.Services.AddScoped<IChequeRepository, ChequeRepository>();
builder.Services.AddScoped<IChequeService, ChequeService>();

builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IPersonaService, PersonaService>();

builder.Services.AddScoped<ITelefonoPersonaRepository, TelefonoPersonaRepository>();
builder.Services.AddScoped<ITelefonoPersonaService, TelefonoPersonaService>();

builder.Services.AddScoped<IDireccionPersonaRepository, DireccionPersonaRepository>();
builder.Services.AddScoped<IDireccionPersonaService, DireccionPersonaService>();

builder.Services.AddScoped<IAplicacionInteresRepository, AplicacionInteresRepository>();
builder.Services.AddScoped<IAplicacionInteresService, AplicacionInteresService>();

builder.Services.AddScoped<IConciliacionRepository, ConciliacionRepository>();
builder.Services.AddScoped<IConciliacionService, ConciliacionService>();

builder.Services.AddScoped<IReporteCuentaBancariaRepository, ReporteCuentaBancariaRepository>();
builder.Services.AddScoped<IReporteCuentaBancariaService, ReporteCuentaBancariaService>();

builder.Services.AddScoped<IReporteConciliacionRepository, ReporteConciliacionRepository>();
builder.Services.AddScoped<IReporteConciliacionService, ReporteConciliacionService>();

builder.Services.AddScoped<IReporteMovimientoRepository, ReporteMovimientoRepository>();
builder.Services.AddScoped<IReporteMovimientoService, ReporteMovimientoService>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseCors("FrontendPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IAplicacionInteresService>();
    await service.AplicarInteresesAutomaticos();
}

app.Run();