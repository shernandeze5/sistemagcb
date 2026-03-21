using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Business.Services;
using GestionCuentasBancarias.Data.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:5173") // URL de Vite
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
builder.Services.AddSwaggerGen();   
builder.Services.AddScoped<OracleConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("OracleConnection")
    ?? throw new InvalidOperationException("No se encontr� la cadena de conexi�n OracleConnection.");

    return new OracleConnectionFactory(connectionString);
});

builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();

builder.Services.AddScoped<ITipoMovimientoRepository, TipoMovimientoRepository>();
builder.Services.AddScoped<ITipoMovimientoService, TipoMovimientoService>();

builder.Services.AddScoped<IBancoRepository, BancoRepository>();
builder.Services.AddScoped<IBancoService, BancoService>();

builder.Services.AddScoped<ITipoCuentaRepository, TipoCuentaRespository>();
builder.Services.AddScoped<ITipoCuentaService, TipoCuentaService>();

builder.Services.AddScoped<ITipoPersonaRepository, TipoPersonaRepository>();
builder.Services.AddScoped<ITipoPersonaService, TipoPersonaService >();
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


var app = builder.Build();

app.UseCors("FrontendPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
