using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class TelefonoPersonaRepository : ITelefonoPersonaRepository
    {
        private readonly IConfiguration configuration;

        public TelefonoPersonaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(configuration.GetConnectionString("OracleConnection"));
        }

        public async Task<IEnumerable<ResponseTelefonoPersonaDTO>> ObtenerPorPersonaAsync(int personaId)
        {
            List<ResponseTelefonoPersonaDTO> lista = new();

            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
                SELECT 
                    T.TEL_TELEFONO,
                    T.PER_PERSONA,
                    T.TIT_TIPO_TELEFONO,
                    TT.TIT_DESCRIPCION AS TIPO_TELEFONO_DESCRIPCION,
                    T.TEP_NUMERO,
                    T.TEP_PRINCIPAL,
                    T.TEP_ESTADO,
                    T.TEP_FECHA_CREACION
                FROM GCB_TELEFONO_PERSONA T
                INNER JOIN GCB_TIPO_TELEFONO TT
                    ON T.TIT_TIPO_TELEFONO = TT.TIT_TIPO_TELEFONO
                WHERE T.PER_PERSONA = :PER_PERSONA
                  AND T.TEP_ESTADO = 'A'
                ORDER BY T.TEP_PRINCIPAL DESC, T.TEL_TELEFONO DESC";

            using var command = new OracleCommand(query, connection);
            command.BindByName = true;
            command.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = personaId;

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new ResponseTelefonoPersonaDTO
                {
                    TEL_Telefono = Convert.ToInt32(reader["TEL_TELEFONO"]),
                    PER_Persona = Convert.ToInt32(reader["PER_PERSONA"]),
                    TIT_Tipo_Telefono = Convert.ToInt32(reader["TIT_TIPO_TELEFONO"]),
                    TipoTelefonoDescripcion = reader["TIPO_TELEFONO_DESCRIPCION"]?.ToString() ?? "",
                    TEP_Numero = reader["TEP_NUMERO"]?.ToString() ?? "",
                    TEP_Principal = reader["TEP_PRINCIPAL"]?.ToString() ?? "N",
                    TEP_Estado = reader["TEP_ESTADO"]?.ToString() ?? "A",
                    TEP_Fecha_Creacion = Convert.ToDateTime(reader["TEP_FECHA_CREACION"])
                });
            }

            return lista;
        }

        public async Task<ResponseTelefonoPersonaDTO?> ObtenerPorIdAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
                SELECT 
                    T.TEL_TELEFONO,
                    T.PER_PERSONA,
                    T.TIT_TIPO_TELEFONO,
                    TT.TIT_DESCRIPCION AS TIPO_TELEFONO_DESCRIPCION,
                    T.TEP_NUMERO,
                    T.TEP_PRINCIPAL,
                    T.TEP_ESTADO,
                    T.TEP_FECHA_CREACION
                FROM GCB_TELEFONO_PERSONA T
                INNER JOIN GCB_TIPO_TELEFONO TT
                    ON T.TIT_TIPO_TELEFONO = TT.TIT_TIPO_TELEFONO
                WHERE T.TEL_TELEFONO = :TEL_TELEFONO";

            using var command = new OracleCommand(query, connection);
            command.BindByName = true;
            command.Parameters.Add("TEL_TELEFONO", OracleDbType.Int32).Value = id;

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new ResponseTelefonoPersonaDTO
            {
                TEL_Telefono = Convert.ToInt32(reader["TEL_TELEFONO"]),
                PER_Persona = Convert.ToInt32(reader["PER_PERSONA"]),
                TIT_Tipo_Telefono = Convert.ToInt32(reader["TIT_TIPO_TELEFONO"]),
                TipoTelefonoDescripcion = reader["TIPO_TELEFONO_DESCRIPCION"]?.ToString() ?? "",
                TEP_Numero = reader["TEP_NUMERO"]?.ToString() ?? "",
                TEP_Principal = reader["TEP_PRINCIPAL"]?.ToString() ?? "N",
                TEP_Estado = reader["TEP_ESTADO"]?.ToString() ?? "A",
                TEP_Fecha_Creacion = Convert.ToDateTime(reader["TEP_FECHA_CREACION"])
            };
        }

        public async Task<bool> CrearAsync(CreateTelefonoPersonaDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                string principal = string.IsNullOrWhiteSpace(dto.TEP_Principal)
                ? "N"
                : dto.TEP_Principal.ToUpper();

                if (principal == "S")
                {
                    string reset = @"
                        UPDATE GCB_TELEFONO_PERSONA
                        SET TEP_PRINCIPAL = 'N'
                        WHERE PER_PERSONA = :PER_PERSONA
                          AND TEP_ESTADO = 'A'";

                    using var resetCommand = new OracleCommand(reset, connection);
                    resetCommand.Transaction = transaction;
                    resetCommand.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = dto.PER_Persona;
                    await resetCommand.ExecuteNonQueryAsync();
                }

                string insert = @"
                    INSERT INTO GCB_TELEFONO_PERSONA
                    (
                        PER_PERSONA,
                        TIT_TIPO_TELEFONO,
                        TEP_NUMERO,
                        TEP_PRINCIPAL,
                        TEP_ESTADO,
                        TEP_FECHA_CREACION
                    )
                    VALUES
                    (
                        :PER_PERSONA,
                        :TIT_TIPO_TELEFONO,
                        :TEP_NUMERO,
                        :TEP_PRINCIPAL,
                        'A',
                        SYSDATE
                    )";

                using var command = new OracleCommand(insert, connection);
                command.Transaction = transaction;

                command.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = dto.PER_Persona;
                command.Parameters.Add("TIT_TIPO_TELEFONO", OracleDbType.Int32).Value = dto.TIT_Tipo_Telefono;
                command.Parameters.Add("TEP_NUMERO", OracleDbType.Varchar2).Value = dto.TEP_Numero;
                command.Parameters.Add("TEP_PRINCIPAL", OracleDbType.Char).Value = principal;

                int rows = await command.ExecuteNonQueryAsync();
                transaction.Commit();

                return rows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> ActualizarAsync(int id, UpdateTelefonoPersonaDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                string principal = dto.TEP_Principal;
                string estado = dto.TEP_Estado;

                string update = @"
                    UPDATE GCB_TELEFONO_PERSONA
                    SET
                        TIT_TIPO_TELEFONO = :TIT_TIPO_TELEFONO,
                        TEP_NUMERO = :TEP_NUMERO,
                        TEP_PRINCIPAL = :TEP_PRINCIPAL,
                        TEP_ESTADO = :TEP_ESTADO
                    WHERE TEL_TELEFONO = :TEL_TELEFONO";

                using var command = new OracleCommand(update, connection);
                command.Transaction = transaction;

                command.Parameters.Add("TIT_TIPO_TELEFONO", OracleDbType.Int32).Value = dto.TIT_Tipo_Telefono;
                command.Parameters.Add("TEP_NUMERO", OracleDbType.Varchar2).Value = dto.TEP_Numero;
                command.Parameters.Add("TEP_PRINCIPAL", OracleDbType.Char).Value = principal;
                command.Parameters.Add("TEP_ESTADO", OracleDbType.Char).Value = estado;
                command.Parameters.Add("TEL_TELEFONO", OracleDbType.Int32).Value = id;

                int rows = await command.ExecuteNonQueryAsync();
                transaction.Commit();

                return rows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
                UPDATE GCB_TELEFONO_PERSONA
                SET TEP_ESTADO = 'I',
                    TEP_PRINCIPAL = 'N'
                WHERE TEL_TELEFONO = :TEL_TELEFONO";

            using var command = new OracleCommand(query, connection);
            command.Parameters.Add("TEL_TELEFONO", OracleDbType.Int32).Value = id;

            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
}