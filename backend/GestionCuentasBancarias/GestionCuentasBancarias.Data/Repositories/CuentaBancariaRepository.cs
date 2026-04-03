using Dapper;
using GestionCuentasBancarias.Domain.DTOS.CuentaBancaria;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class CuentaBancariaRepository : ICuentaBancariaRepository
    {
        private readonly string connectionString;

        public CuentaBancariaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection");
        }

        private const string SELECT_BASE = @"
            SELECT
                c.CUB_Cuenta,
                c.BAN_Banco,
                b.BAN_Nombre,
                c.CUB_Numero_Cuenta,
                c.CUB_Primer_Nombre,
                c.CUB_Segundo_Nombre,
                c.CUB_Primer_Apellido,
                c.CUB_Segundo_Apellido,
                c.TCU_Tipo_Cuenta,
                t.TCU_Descripcion,
                c.TMO_Tipo_Moneda,
                m.TMO_Descripcion,
                m.TMO_Simbolo,
                c.CUB_Saldo_Inicial,
                c.CUB_Saldo_Actual,
                c.ESC_Estado_Cuenta,
                e.ESC_Descripcion,
                c.CUB_Estado,
                c.CUB_Fecha_Creacion
            FROM GCB_CUENTA_BANCARIA c
            INNER JOIN GCB_BANCO           b ON c.BAN_Banco        = b.BAN_Banco
            INNER JOIN GCB_TIPO_CUENTA     t ON c.TCU_Tipo_Cuenta  = t.TCU_Tipo_Cuenta
            INNER JOIN GCB_TIPO_MONEDA     m ON c.TMO_Tipo_Moneda  = m.TMO_Tipo_Moneda
            INNER JOIN GCB_ESTADO_CUENTA   e ON c.ESC_Estado_Cuenta = e.ESC_Estado_Cuenta";

        public async Task<List<ResponseCuentaBancariaDTO>> ObtenerCuentas()
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $"{SELECT_BASE} ORDER BY b.BAN_Nombre, c.CUB_Numero_Cuenta";
            var result = await connection.QueryAsync<ResponseCuentaBancariaDTO>(sql);
            return result.ToList();
        }

        public async Task<List<ResponseCuentaBancariaDTO>> ObtenerCuentasPorBanco(int bancoId)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $"{SELECT_BASE} WHERE c.BAN_Banco = :BancoId ORDER BY c.CUB_Numero_Cuenta";
            var result = await connection.QueryAsync<ResponseCuentaBancariaDTO>(sql, new { BancoId = bancoId });
            return result.ToList();
        }

        public async Task<ResponseCuentaBancariaDTO> ObtenerCuentaPorId(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = $"{SELECT_BASE} WHERE c.CUB_Cuenta = :Id";
            return await connection.QueryFirstOrDefaultAsync<ResponseCuentaBancariaDTO>(sql, new { Id = id });
        }

        public async Task CrearCuenta(CreateCuentaBancariaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            // Validar número de cuenta único por banco
            string sqlDup = @"SELECT COUNT(*) FROM GCB_CUENTA_BANCARIA
                              WHERE BAN_Banco = :Banco
                                AND CUB_Numero_Cuenta = :Numero";

            var existe = await connection.ExecuteScalarAsync<int>(sqlDup, new
            {
                Banco = dto.BAN_Banco,
                Numero = dto.CUB_Numero_Cuenta
            });

            if (existe > 0)
                throw new InvalidOperationException(
                    "Ya existe una cuenta con ese número en el banco seleccionado.");

            string sql = @"INSERT INTO GCB_CUENTA_BANCARIA
                               (BAN_Banco, CUB_Numero_Cuenta,
                                CUB_Primer_Nombre, CUB_Segundo_Nombre,
                                CUB_Primer_Apellido, CUB_Segundo_Apellido,
                                TCU_Tipo_Cuenta, TMO_Tipo_Moneda,
                                CUB_Saldo_Inicial, CUB_Saldo_Actual,
                                ESC_Estado_Cuenta)
                           VALUES
                               (:Banco, :Numero,
                                :PrimerNombre, :SegundoNombre,
                                :PrimerApellido, :SegundoApellido,
                                :TipoCuenta, :TipoMoneda,
                                :SaldoInicial, :SaldoInicial,
                                :EstadoCuenta)";

            await connection.ExecuteAsync(sql, new
            {
                Banco = dto.BAN_Banco,
                Numero = dto.CUB_Numero_Cuenta,
                PrimerNombre = dto.CUB_Primer_Nombre,
                SegundoNombre = dto.CUB_Segundo_Nombre,
                PrimerApellido = dto.CUB_Primer_Apellido,
                SegundoApellido = dto.CUB_Segundo_Apellido,
                TipoCuenta = dto.TCU_Tipo_Cuenta,
                TipoMoneda = dto.TMO_Tipo_Moneda,
                SaldoInicial = dto.CUB_Saldo_Inicial,
                EstadoCuenta = dto.ESC_Estado_Cuenta
            });
        }

        public async Task<bool> ActualizarCuenta(int id, UpdateCuentaBancariaDTO dto)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = @"UPDATE GCB_CUENTA_BANCARIA
                           SET CUB_Primer_Nombre    = :PrimerNombre,
                               CUB_Segundo_Nombre   = :SegundoNombre,
                               CUB_Primer_Apellido  = :PrimerApellido,
                               CUB_Segundo_Apellido = :SegundoApellido,
                               ESC_Estado_Cuenta    = :EstadoCuenta
                           WHERE CUB_Cuenta = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                PrimerNombre = dto.CUB_Primer_Nombre,
                SegundoNombre = dto.CUB_Segundo_Nombre,
                PrimerApellido = dto.CUB_Primer_Apellido,
                SegundoApellido = dto.CUB_Segundo_Apellido,
                EstadoCuenta = dto.ESC_Estado_Cuenta,
                Id = id
            });

            return rows > 0;
        }

        // Baja lógica → CUB_Estado = 'I'
        public async Task<bool> EliminarCuenta(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_CUENTA_BANCARIA
                           SET CUB_Estado = 'I'
                           WHERE CUB_Cuenta = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        // Reactivación → CUB_Estado = 'A'
        public async Task<bool> ReactivarCuenta(int id)
        {
            using var connection = new OracleConnection(connectionString);
            string sql = @"UPDATE GCB_CUENTA_BANCARIA
                           SET CUB_Estado = 'A'
                           WHERE CUB_Cuenta = :Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
