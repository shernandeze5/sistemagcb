using Microsoft.AspNetCore.Mvc;
using GestionCuentasBancarias.Data.Context;

[ApiController]
[Route("api/[controller]")]
public class ConexionBaseDatosController : ControllerBase
{
    private readonly OracleConnectionFactory _connectionFactory;

    public ConexionBaseDatosController(OracleConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpGet("probar")]
    public IActionResult ProbarConexion()
    {
        try
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                return Ok("✅ Conexión exitosa a Oracle");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"❌ Error: {ex.Message}");
        }
    }
}