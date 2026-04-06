using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GestionCuentasBancarias.Data.Context;
using GestionCuentasBancarias.Domain.Entities;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class ChequeRepository : IChequeRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public ChequeRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Cheque>> ObtenerTodosAsync()
        {
            var sql = @"
                SELECT 
                    CHE_Cheque,
                    MOV_Movimiento,
                    CHE_Numero_Cheque,
                    CHE_Monto_Letras,
                    CHE_Fecha_Emision,
                    CHE_Fecha_Cobro,
                    CHE_Fecha_Vencimiento,
                    ESC_Estado_Cheque,
                    CHE_Fecha_Creacion,
                    CHQ_Chequera,
                    CHE_Beneficiario,
                    CHE_Concepto
                FROM GCB_CHEQUE
                ORDER BY CHE_Cheque";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Cheque>(sql);
        }

        public async Task<Cheque?> ObtenerPorIdAsync(int id)
        {
            var sql = @"
                SELECT 
                    CHE_Cheque,
                    MOV_Movimiento,
                    CHE_Numero_Cheque,
                    CHE_Monto_Letras,
                    CHE_Fecha_Emision,
                    CHE_Fecha_Cobro,
                    CHE_Fecha_Vencimiento,
                    ESC_Estado_Cheque,
                    CHE_Fecha_Creacion,
                    CHQ_Chequera,
                    CHE_Beneficiario,
                    CHE_Concepto
                FROM GCB_CHEQUE
                WHERE CHE_Cheque = :Id";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Cheque>(sql, new { Id = id });
        }

        public async Task<bool> CrearAsync(Cheque cheque)
        {
            var sql = @"
                INSERT INTO GCB_CHEQUE
                (
                    MOV_Movimiento,
                    CHE_Numero_Cheque,
                    CHE_Monto_Letras,
                    CHE_Fecha_Emision,
                    CHE_Fecha_Cobro,
                    CHE_Fecha_Vencimiento,
                    ESC_Estado_Cheque,
                    CHE_Fecha_Creacion,
                    CHQ_Chequera,
                    CHE_Beneficiario,
                    CHE_Concepto
                )
                VALUES
                (
                    :MOV_Movimiento,
                    :CHE_Numero_Cheque,
                    :CHE_Monto_Letras,
                    :CHE_Fecha_Emision,
                    :CHE_Fecha_Cobro,
                    :CHE_Fecha_Vencimiento,
                    :ESC_Estado_Cheque,
                    :CHE_Fecha_Creacion,
                    :CHQ_Chequera,
                    :CHE_Beneficiario,
                    :CHE_Concepto
                )";

             using var connection = _connectionFactory.CreateConnection();
             var existe = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM GCB_CHEQUE WHERE CHE_Numero_Cheque = :numero",
                 new { numero = cheque.CHE_Numero_Cheque }
             );

             if (existe > 0)
                return false;
             var filasAfectadas = await connection.ExecuteAsync(sql, cheque);
                return filasAfectadas > 0;
        }

        public async Task<bool> ActualizarAsync(Cheque cheque)
        {
            var sql = @"
                UPDATE GCB_CHEQUE
                SET
                    MOV_Movimiento = :MOV_Movimiento,
                    CHE_Numero_Cheque = :CHE_Numero_Cheque,
                    CHE_Monto_Letras = :CHE_Monto_Letras,
                    CHE_Fecha_Emision = :CHE_Fecha_Emision,
                    CHE_Fecha_Cobro = :CHE_Fecha_Cobro,
                    CHE_Fecha_Vencimiento = :CHE_Fecha_Vencimiento,
                    ESC_Estado_Cheque = :ESC_Estado_Cheque,
                    CHQ_Chequera = :CHQ_Chequera,
                    CHE_Beneficiario = :CHE_Beneficiario,
                    CHE_Concepto = :CHE_Concepto
                WHERE CHE_Cheque = :CHE_Cheque";

            using var connection = _connectionFactory.CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(sql, cheque);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var sql = @"
                DELETE FROM GCB_CHEQUE
                WHERE CHE_Cheque = :Id";

            using var connection = _connectionFactory.CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(sql, new { Id = id });
            return filasAfectadas > 0;
        }
    }
}