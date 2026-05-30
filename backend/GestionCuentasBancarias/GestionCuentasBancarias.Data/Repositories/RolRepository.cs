using GestionCuentasBancarias.Domain.DTOS.Rol;
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
    public class RolRepository : IRolRepository
    {
        private readonly string connectionString;

        public RolRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión OracleConnection.");
        }

        private IDbConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public async Task<IEnumerable<RolDTO>> ObtenerTodos()
        {
            using var connection = GetConnection();

            var sql = @"
                SELECT
                    ROL_ROL,
                    ROL_NOMBRE,
                    ROL_DESCRIPCION,
                    ROL_ESTADO,
                    ROL_FECHA_CREACION
                FROM GCB_ROL
                ORDER BY ROL_ROL DESC";

            return await connection.QueryAsync<RolDTO>(sql);
        }

        public async Task<RolDTO?> ObtenerPorId(int id)
        {
            using var connection = GetConnection();

            var sql = @"
                SELECT
                    ROL_ROL,
                    ROL_NOMBRE,
                    ROL_DESCRIPCION,
                    ROL_ESTADO,
                    ROL_FECHA_CREACION
                FROM GCB_ROL
                WHERE ROL_ROL = :Id";

            return await connection.QueryFirstOrDefaultAsync<RolDTO>(sql, new { Id = id });
        }

        public async Task<int> Crear(CrearRolDTO dto)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var id = await connection.ExecuteScalarAsync<int>(
                    "SELECT GCB_SEQ_ROL.NEXTVAL FROM DUAL",
                    transaction: transaction
                );

                var sql = @"
                    INSERT INTO GCB_ROL (
                        ROL_ROL,
                        ROL_NOMBRE,
                        ROL_DESCRIPCION,
                        ROL_ESTADO,
                        ROL_FECHA_CREACION
                    ) VALUES (
                        :ROL_ROL,
                        :ROL_NOMBRE,
                        :ROL_DESCRIPCION,
                        'A',
                        SYSDATE
                    )";

                await connection.ExecuteAsync(sql, new
                {
                    ROL_ROL = id,
                    dto.ROL_NOMBRE,
                    dto.ROL_DESCRIPCION
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

        public async Task<bool> Actualizar(int id, ActualizarRolDTO dto)
        {
            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"
                    UPDATE GCB_ROL
                    SET
                        ROL_NOMBRE = :ROL_NOMBRE,
                        ROL_DESCRIPCION = :ROL_DESCRIPCION
                    WHERE ROL_ROL = :Id";

                var rows = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    dto.ROL_NOMBRE,
                    dto.ROL_DESCRIPCION
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

        public async Task<bool> Desactivar(int id)
        {
            using var connection = GetConnection();

            var sql = @"
                UPDATE GCB_ROL
                SET ROL_ESTADO = 'I'
                WHERE ROL_ROL = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }

        public async Task<bool> Reactivar(int id)
        {
            using var connection = GetConnection();

            var sql = @"
                UPDATE GCB_ROL
                SET ROL_ESTADO = 'A'
                WHERE ROL_ROL = :Id";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }
    }
}
