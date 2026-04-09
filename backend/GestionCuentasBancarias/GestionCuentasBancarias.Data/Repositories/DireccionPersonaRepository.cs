using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class DireccionPersonaRepository : IDireccionPersonaRepository
    {
        private readonly IConfiguration configuration;

        public DireccionPersonaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(configuration.GetConnectionString("OracleConnection"));
        }

        public async Task<IEnumerable<ResponseDireccionPersonaDTO>> ObtenerPorPersonaAsync(int personaId)
        {
            List<ResponseDireccionPersonaDTO> lista = new();

            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
                SELECT 
                    D.DIR_DIRECCION,
                    D.PER_PERSONA,
                    D.TDI_TIPO_DIRECCION,
                    TD.TDI_DESCRIPCION AS TIPO_DIRECCION_DESCRIPCION,
                    D.DIR_DEPARTAMENTO,
                    D.DIR_MUNICIPIO,
                    D.DIR_COLONIA,
                    D.DIR_ZONA,
                    D.DIR_NUMERO_CASA,
                    D.DIR_DETALLE,
                    D.DIR_PRINCIPAL,
                    D.DIR_ESTADO,
                    D.DIR_FECHA_CREACION
                FROM GCB_DIRECCION_PERSONA D
                INNER JOIN GCB_TIPO_DIRECCION TD
                    ON D.TDI_TIPO_DIRECCION = TD.TDI_TIPO_DIRECCION
                WHERE D.PER_PERSONA = :PER_PERSONA
                  AND D.DIR_ESTADO = 'A'
                ORDER BY D.DIR_PRINCIPAL DESC, D.DIR_DIRECCION DESC";

            using var command = new OracleCommand(query, connection);
            command.BindByName = true;
            command.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = personaId;

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new ResponseDireccionPersonaDTO
                {
                    DIR_Direccion = Convert.ToInt32(reader["DIR_DIRECCION"]),
                    PER_Persona = Convert.ToInt32(reader["PER_PERSONA"]),
                    TDI_Tipo_Direccion = Convert.ToInt32(reader["TDI_TIPO_DIRECCION"]),
                    TipoDireccionDescripcion = reader["TIPO_DIRECCION_DESCRIPCION"]?.ToString() ?? "",
                    DIR_Departamento = reader["DIR_DEPARTAMENTO"]?.ToString() ?? "",
                    DIR_Municipio = reader["DIR_MUNICIPIO"]?.ToString() ?? "",
                    DIR_Colonia = reader["DIR_COLONIA"]?.ToString() ?? "",
                    DIR_Zona = reader["DIR_ZONA"]?.ToString() ?? "",
                    DIR_Numero_Casa = reader["DIR_NUMERO_CASA"]?.ToString() ?? "",
                    DIR_Detalle = reader["DIR_DETALLE"]?.ToString() ?? "",
                    DIR_Principal = reader["DIR_PRINCIPAL"]?.ToString() ?? "N",
                    DIR_Estado = reader["DIR_ESTADO"]?.ToString() ?? "A",
                    DIR_Fecha_Creacion = Convert.ToDateTime(reader["DIR_FECHA_CREACION"])
                });
            }

            return lista;
        }

        public async Task<ResponseDireccionPersonaDTO?> ObtenerPorIdAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
                SELECT 
                    D.*, TD.TDI_DESCRIPCION AS TIPO_DIRECCION_DESCRIPCION
                FROM GCB_DIRECCION_PERSONA D
                INNER JOIN GCB_TIPO_DIRECCION TD
                    ON D.TDI_TIPO_DIRECCION = TD.TDI_TIPO_DIRECCION
                WHERE D.DIR_DIRECCION = :DIR_DIRECCION";

            using var command = new OracleCommand(query, connection);
            command.BindByName = true;
            command.Parameters.Add("DIR_DIRECCION", OracleDbType.Int32).Value = id;

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            return new ResponseDireccionPersonaDTO
            {
                DIR_Direccion = Convert.ToInt32(reader["DIR_DIRECCION"]),
                PER_Persona = Convert.ToInt32(reader["PER_PERSONA"]),
                TDI_Tipo_Direccion = Convert.ToInt32(reader["TDI_TIPO_DIRECCION"]),
                TipoDireccionDescripcion = reader["TIPO_DIRECCION_DESCRIPCION"]?.ToString() ?? "",
                DIR_Departamento = reader["DIR_DEPARTAMENTO"]?.ToString() ?? "",
                DIR_Municipio = reader["DIR_MUNICIPIO"]?.ToString() ?? "",
                DIR_Colonia = reader["DIR_COLONIA"]?.ToString() ?? "",
                DIR_Zona = reader["DIR_ZONA"]?.ToString() ?? "",
                DIR_Numero_Casa = reader["DIR_NUMERO_CASA"]?.ToString() ?? "",
                DIR_Detalle = reader["DIR_DETALLE"]?.ToString() ?? "",
                DIR_Principal = reader["DIR_PRINCIPAL"]?.ToString() ?? "N",
                DIR_Estado = reader["DIR_ESTADO"]?.ToString() ?? "A",
                DIR_Fecha_Creacion = Convert.ToDateTime(reader["DIR_FECHA_CREACION"])
            };
        }

        public async Task<bool> CrearAsync(CreateDireccionPersonaDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                // 🔥 Si viene como principal → quitar otros
                if (dto.DIR_Principal == "S")
                {
                    string reset = @"
                        UPDATE GCB_DIRECCION_PERSONA
                        SET DIR_PRINCIPAL = 'N'
                        WHERE PER_PERSONA = :PER_PERSONA
                          AND DIR_ESTADO = 'A'";

                    using var resetCommand = new OracleCommand(reset, connection);
                    resetCommand.Transaction = transaction;
                    resetCommand.BindByName = true;
                    resetCommand.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = dto.PER_Persona;
                    await resetCommand.ExecuteNonQueryAsync();
                }

                string insert = @"
                    INSERT INTO GCB_DIRECCION_PERSONA
                    (
                        PER_PERSONA,
                        TDI_TIPO_DIRECCION,
                        DIR_DEPARTAMENTO,
                        DIR_MUNICIPIO,
                        DIR_COLONIA,
                        DIR_ZONA,
                        DIR_NUMERO_CASA,
                        DIR_DETALLE,
                        DIR_PRINCIPAL,
                        DIR_ESTADO,
                        DIR_FECHA_CREACION
                    )
                    VALUES
                    (
                        :PER_PERSONA,
                        :TDI_TIPO_DIRECCION,
                        :DIR_DEPARTAMENTO,
                        :DIR_MUNICIPIO,
                        :DIR_COLONIA,
                        :DIR_ZONA,
                        :DIR_NUMERO_CASA,
                        :DIR_DETALLE,
                        :DIR_PRINCIPAL,
                        'A',
                        SYSDATE
                    )";

                using var command = new OracleCommand(insert, connection);
                command.Transaction = transaction;
                command.BindByName = true;

                command.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = dto.PER_Persona;
                command.Parameters.Add("TDI_TIPO_DIRECCION", OracleDbType.Int32).Value = dto.TDI_Tipo_Direccion;
                command.Parameters.Add("DIR_DEPARTAMENTO", OracleDbType.Varchar2).Value = (object?)dto.DIR_Departamento ?? DBNull.Value;
                command.Parameters.Add("DIR_MUNICIPIO", OracleDbType.Varchar2).Value = (object?)dto.DIR_Municipio ?? DBNull.Value;
                command.Parameters.Add("DIR_COLONIA", OracleDbType.Varchar2).Value = (object?)dto.DIR_Colonia ?? DBNull.Value;
                command.Parameters.Add("DIR_ZONA", OracleDbType.Varchar2).Value = (object?)dto.DIR_Zona ?? DBNull.Value;
                command.Parameters.Add("DIR_NUMERO_CASA", OracleDbType.Varchar2).Value = (object?)dto.DIR_Numero_Casa ?? DBNull.Value;
                command.Parameters.Add("DIR_DETALLE", OracleDbType.Varchar2).Value = (object?)dto.DIR_Detalle ?? DBNull.Value;
                command.Parameters.Add("DIR_PRINCIPAL", OracleDbType.Char).Value = dto.DIR_Principal;

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

        public async Task<bool> ActualizarAsync(int id, UpdateDireccionPersonaDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                // 🔥 Si se vuelve principal → quitar otros
                if (dto.DIR_Principal == "S")
                {
                    string reset = @"
                        UPDATE GCB_DIRECCION_PERSONA
                        SET DIR_PRINCIPAL = 'N'
                        WHERE PER_PERSONA = (
                            SELECT PER_PERSONA 
                            FROM GCB_DIRECCION_PERSONA 
                            WHERE DIR_DIRECCION = :DIR_DIRECCION
                        )
                        AND DIR_ESTADO = 'A'
                        AND DIR_DIRECCION <> :DIR_DIRECCION";

                    using var resetCommand = new OracleCommand(reset, connection);
                    resetCommand.Transaction = transaction;
                    resetCommand.BindByName = true;
                    resetCommand.Parameters.Add("DIR_DIRECCION", OracleDbType.Int32).Value = id;
                    await resetCommand.ExecuteNonQueryAsync();
                }

                string update = @"
                    UPDATE GCB_DIRECCION_PERSONA
                    SET
                        TDI_TIPO_DIRECCION = :TDI_TIPO_DIRECCION,
                        DIR_DEPARTAMENTO = :DIR_DEPARTAMENTO,
                        DIR_MUNICIPIO = :DIR_MUNICIPIO,
                        DIR_COLONIA = :DIR_COLONIA,
                        DIR_ZONA = :DIR_ZONA,
                        DIR_NUMERO_CASA = :DIR_NUMERO_CASA,
                        DIR_DETALLE = :DIR_DETALLE,
                        DIR_PRINCIPAL = :DIR_PRINCIPAL,
                        DIR_ESTADO = :DIR_ESTADO
                    WHERE DIR_DIRECCION = :DIR_DIRECCION";

                using var command = new OracleCommand(update, connection);
                command.Transaction = transaction;
                command.BindByName = true;

                command.Parameters.Add("TDI_TIPO_DIRECCION", OracleDbType.Int32).Value = dto.TDI_Tipo_Direccion;
                command.Parameters.Add("DIR_DEPARTAMENTO", OracleDbType.Varchar2).Value = (object?)dto.DIR_Departamento ?? DBNull.Value;
                command.Parameters.Add("DIR_MUNICIPIO", OracleDbType.Varchar2).Value = (object?)dto.DIR_Municipio ?? DBNull.Value;
                command.Parameters.Add("DIR_COLONIA", OracleDbType.Varchar2).Value = (object?)dto.DIR_Colonia ?? DBNull.Value;
                command.Parameters.Add("DIR_ZONA", OracleDbType.Varchar2).Value = (object?)dto.DIR_Zona ?? DBNull.Value;
                command.Parameters.Add("DIR_NUMERO_CASA", OracleDbType.Varchar2).Value = (object?)dto.DIR_Numero_Casa ?? DBNull.Value;
                command.Parameters.Add("DIR_DETALLE", OracleDbType.Varchar2).Value = (object?)dto.DIR_Detalle ?? DBNull.Value;
                command.Parameters.Add("DIR_PRINCIPAL", OracleDbType.Char).Value = dto.DIR_Principal;
                command.Parameters.Add("DIR_ESTADO", OracleDbType.Char).Value = dto.DIR_Estado;
                command.Parameters.Add("DIR_DIRECCION", OracleDbType.Int32).Value = id;

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
                UPDATE GCB_DIRECCION_PERSONA
                SET DIR_ESTADO = 'I',
                    DIR_PRINCIPAL = 'N'
                WHERE DIR_DIRECCION = :ID";

            using var command = new OracleCommand(query, connection);
            command.BindByName = true;
            command.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
}