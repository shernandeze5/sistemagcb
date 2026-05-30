using GestionCuentasBancarias.Domain.DTOS.Usuario;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");
        }

        private IDbConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public async Task<IEnumerable<UsuarioDTO>> ObtenerTodos()
        {
            using var connection = GetConnection();

            var sql = @"
                SELECT
                    U.USU_USUARIO,
                    U.ROL_ROL,
                    R.ROL_NOMBRE,

                    U.USU_PRIMER_NOMBRE,
                    U.USU_SEGUNDO_NOMBRE,
                    U.USU_PRIMER_APELLIDO,
                    U.USU_SEGUNDO_APELLIDO,

                    TRIM(
                        NVL(U.USU_PRIMER_NOMBRE, '') || ' ' ||
                        NVL(U.USU_SEGUNDO_NOMBRE, '') || ' ' ||
                        NVL(U.USU_PRIMER_APELLIDO, '') || ' ' ||
                        NVL(U.USU_SEGUNDO_APELLIDO, '')
                    ) AS NombreCompleto,

                    U.USU_EMAIL,
                    U.USU_ESTADO,
                    U.USU_FECHA_CREACION,
                    U.USU_ULTIMO_ACCESO
                FROM GCB_USUARIO U
                INNER JOIN GCB_ROL R
                    ON R.ROL_ROL = U.ROL_ROL
                ORDER BY U.USU_USUARIO DESC";

            return await connection.QueryAsync<UsuarioDTO>(sql);
        }

        public async Task<UsuarioDTO?> ObtenerPorId(int id)
        {
            using var connection = GetConnection();

            var sql = @"
                SELECT
                    U.USU_USUARIO,
                    U.ROL_ROL,
                    R.ROL_NOMBRE,

                    U.USU_PRIMER_NOMBRE,
                    U.USU_SEGUNDO_NOMBRE,
                    U.USU_PRIMER_APELLIDO,
                    U.USU_SEGUNDO_APELLIDO,

                    TRIM(
                        NVL(U.USU_PRIMER_NOMBRE, '') || ' ' ||
                        NVL(U.USU_SEGUNDO_NOMBRE, '') || ' ' ||
                        NVL(U.USU_PRIMER_APELLIDO, '') || ' ' ||
                        NVL(U.USU_SEGUNDO_APELLIDO, '')
                    ) AS NombreCompleto,

                    U.USU_EMAIL,
                    U.USU_ESTADO,
                    U.USU_FECHA_CREACION,
                    U.USU_ULTIMO_ACCESO
                FROM GCB_USUARIO U
                INNER JOIN GCB_ROL R
                    ON R.ROL_ROL = U.ROL_ROL
                WHERE U.USU_USUARIO = :Id";

            return await connection.QueryFirstOrDefaultAsync<UsuarioDTO>(sql, new { Id = id });
        }

        public async Task<UsuarioDTO?> ObtenerPorEmail(string email)
        {
            using var connection = GetConnection();

            var sql = @"
                SELECT
                    U.USU_USUARIO,
                    U.ROL_ROL,
                    R.ROL_NOMBRE,

                    U.USU_PRIMER_NOMBRE,
                    U.USU_SEGUNDO_NOMBRE,
                    U.USU_PRIMER_APELLIDO,
                    U.USU_SEGUNDO_APELLIDO,

                    TRIM(
                        NVL(U.USU_PRIMER_NOMBRE, '') || ' ' ||
                        NVL(U.USU_SEGUNDO_NOMBRE, '') || ' ' ||
                        NVL(U.USU_PRIMER_APELLIDO, '') || ' ' ||
                        NVL(U.USU_SEGUNDO_APELLIDO, '')
                    ) AS NombreCompleto,

                    U.USU_EMAIL,
                    U.USU_ESTADO,
                    U.USU_FECHA_CREACION,
                    U.USU_ULTIMO_ACCESO
                FROM GCB_USUARIO U
                INNER JOIN GCB_ROL R
                    ON R.ROL_ROL = U.ROL_ROL
                WHERE LOWER(U.USU_EMAIL) = LOWER(:Email)";

            return await connection.QueryFirstOrDefaultAsync<UsuarioDTO>(
                sql,
                new { Email = email }
            );
        }

        public async Task<int> Crear(CrearUsuarioDTO dto, string passwordHash)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var id = await connection.ExecuteScalarAsync<int>(
                    "SELECT GCB_SEQ_USUARIO.NEXTVAL FROM DUAL",
                    transaction: transaction
                );

                var sql = @"
                    INSERT INTO GCB_USUARIO (
                        USU_USUARIO,
                        ROL_ROL,
                        USU_PRIMER_NOMBRE,
                        USU_SEGUNDO_NOMBRE,
                        USU_PRIMER_APELLIDO,
                        USU_SEGUNDO_APELLIDO,
                        USU_EMAIL,
                        USU_PASSWORD,
                        USU_ESTADO,
                        USU_FECHA_CREACION,
                        USU_ULTIMO_ACCESO
                    ) VALUES (
                        :USU_USUARIO,
                        :ROL_ROL,
                        :USU_PRIMER_NOMBRE,
                        :USU_SEGUNDO_NOMBRE,
                        :USU_PRIMER_APELLIDO,
                        :USU_SEGUNDO_APELLIDO,
                        :USU_EMAIL,
                        :USU_PASSWORD,
                        'A',
                        SYSDATE,
                        NULL
                    )";

                await connection.ExecuteAsync(sql, new
                {
                    USU_USUARIO = id,
                    dto.ROL_ROL,
                    dto.USU_PRIMER_NOMBRE,
                    dto.USU_SEGUNDO_NOMBRE,
                    dto.USU_PRIMER_APELLIDO,
                    dto.USU_SEGUNDO_APELLIDO,
                    dto.USU_EMAIL,
                    USU_PASSWORD = passwordHash
                }, transaction);

                transaction.Commit();
                return id;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> Actualizar(int id, ActualizarUsuarioDTO dto)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"
                    UPDATE GCB_USUARIO
                    SET
                        ROL_ROL = :ROL_ROL,
                        USU_PRIMER_NOMBRE = :USU_PRIMER_NOMBRE,
                        USU_SEGUNDO_NOMBRE = :USU_SEGUNDO_NOMBRE,
                        USU_PRIMER_APELLIDO = :USU_PRIMER_APELLIDO,
                        USU_SEGUNDO_APELLIDO = :USU_SEGUNDO_APELLIDO,
                        USU_EMAIL = :USU_EMAIL
                    WHERE USU_USUARIO = :Id";

                var rows = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    dto.ROL_ROL,
                    dto.USU_PRIMER_NOMBRE,
                    dto.USU_SEGUNDO_NOMBRE,
                    dto.USU_PRIMER_APELLIDO,
                    dto.USU_SEGUNDO_APELLIDO,
                    dto.USU_EMAIL
                }, transaction);

                transaction.Commit();
                return rows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> CambiarPassword(int id, string passwordHash)
        {
            using var connection = GetConnection();

            var sql = @"
                UPDATE GCB_USUARIO
                SET USU_PASSWORD = :PasswordHash
                WHERE USU_USUARIO = :Id";

            var rows = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                PasswordHash = passwordHash
            });

            return rows > 0;
        }

        public async Task<bool> Desactivar(int id)
        {
            using var connection = GetConnection();

            var sql = @"
                UPDATE GCB_USUARIO
                SET USU_ESTADO = 'I'
                WHERE USU_USUARIO = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }

        public async Task<bool> Reactivar(int id)
        {
            using var connection = GetConnection();

            var sql = @"
                UPDATE GCB_USUARIO
                SET USU_ESTADO = 'A'
                WHERE USU_USUARIO = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }

        public async Task<bool> ActualizarUltimoAcceso(int id)
        {
            using var connection = GetConnection();

            var sql = @"
                UPDATE GCB_USUARIO
                SET USU_ULTIMO_ACCESO = SYSDATE
                WHERE USU_USUARIO = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }
    }
}
