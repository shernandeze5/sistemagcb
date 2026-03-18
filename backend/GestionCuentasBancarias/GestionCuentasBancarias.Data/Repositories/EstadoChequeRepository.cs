using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class EstadoChequeRepository : IEstadoChequeRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public EstadoChequeRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<EstadoCheque>> ObtenerTodosAsync()
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT
                    ESC_ESTADO_CHEQUE AS ESC_Estado_Cheque,
                    ESC_DESCRIPCION AS ESC_Descripcion,
                    ESC_ESTADO AS ESC_Estado,
                    ESC_FECHA_CREACION AS ESC_Fecha_Creacion
                FROM GCB_ESTADO_CHEQUE
                WHERE ESC_ESTADO = 1
                ORDER BY ESC_ESTADO_CHEQUE";

            return await db.QueryAsync<EstadoCheque>(sql);
        }

        public async Task<EstadoCheque?> ObtenerPorIdAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                SELECT
                    ESC_ESTADO_CHEQUE AS ESC_Estado_Cheque,
                    ESC_DESCRIPCION AS ESC_Descripcion,
                    ESC_ESTADO AS ESC_Estado,
                    ESC_FECHA_CREACION AS ESC_Fecha_Creacion
                FROM GCB_ESTADO_CHEQUE
                WHERE ESC_ESTADO_CHEQUE = :Id";

            return await db.QueryFirstOrDefaultAsync<EstadoCheque>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(EstadoCheque entidad)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                INSERT INTO GCB_ESTADO_CHEQUE
                (ESC_DESCRIPCION, ESC_ESTADO, ESC_FECHA_CREACION)
                VALUES
                (:ESC_Descripcion, :ESC_Estado, :ESC_Fecha_Creacion)";

            int filas = await db.ExecuteAsync(sql, entidad);

            return filas > 0;
        }

        public async Task<bool> ActualizarAsync(EstadoCheque entidad)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                UPDATE GCB_ESTADO_CHEQUE
                SET
                    ESC_DESCRIPCION = :ESC_Descripcion,
                    ESC_ESTADO = :ESC_Estado
                WHERE ESC_ESTADO_CHEQUE = :ESC_Estado_Cheque";

            int filas = await db.ExecuteAsync(sql, entidad);

            return filas > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using IDbConnection db = _connectionFactory.CreateConnection();

            string sql = @"
                UPDATE GCB_ESTADO_CHEQUE
                SET ESC_ESTADO = 0
                WHERE ESC_ESTADO_CHEQUE = :Id";

            int filas = await db.ExecuteAsync(sql, new { Id = id });

            return filas > 0;
        }
    }
}