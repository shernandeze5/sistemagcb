using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Business.Services;
using GestionCuentasBancarias.Data.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using GestionCuentasBancarias.Domain.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<OracleConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("OracleConnection");
     throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");


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


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
