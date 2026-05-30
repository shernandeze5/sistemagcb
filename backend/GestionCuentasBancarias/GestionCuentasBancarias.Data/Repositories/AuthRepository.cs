using GestionCuentasBancarias.Domain.DTOS.Auth;
using Microsoft.Extensions.Configuration;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");
        }

        private IDbConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public async Task<AuthUsuarioDTO?> ObtenerUsuarioPorEmail(string email)
        {
            using var connection = GetConnection();

            var sql = @"
                SELECT
                    U.USU_USUARIO,
                    U.ROL_ROL,
                    R.ROL_NOMBRE,

                    TRIM(
                        NVL(U.USU_PRIMER_NOMBRE, '') || ' ' ||
                        NVL(U.USU_SEGUNDO_NOMBRE, '') || ' ' ||
                        NVL(U.USU_PRIMER_APELLIDO, '') || ' ' ||
                        NVL(U.USU_SEGUNDO_APELLIDO, '')
                    ) AS NombreCompleto,

                    U.USU_EMAIL,
                    U.USU_PASSWORD,
                    U.USU_ESTADO
                FROM GCB_USUARIO U
                INNER JOIN GCB_ROL R
                    ON R.ROL_ROL = U.ROL_ROL
                WHERE LOWER(U.USU_EMAIL) = LOWER(:Email)";

            return await connection.QueryFirstOrDefaultAsync<AuthUsuarioDTO>(
                sql,
                new { Email = email }
            );
        }

        public async Task<bool> ActualizarUltimoAcceso(int usuarioId)
        {
            using var connection = GetConnection();

            var sql = @"
                UPDATE GCB_USUARIO
                SET USU_ULTIMO_ACCESO = SYSDATE
                WHERE USU_USUARIO = :UsuarioId";

            var rows = await connection.ExecuteAsync(sql, new { UsuarioId = usuarioId });

            return rows > 0;
        }
    }
}
