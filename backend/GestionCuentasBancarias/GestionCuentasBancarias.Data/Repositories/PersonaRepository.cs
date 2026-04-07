using GestionCuentasBancarias.Domain.DTOS.Persona;
using GestionCuentasBancarias.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GestionCuentasBancarias.Data.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly IConfiguration configuration;

        public PersonaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(configuration.GetConnectionString("OracleConnection"));
        }

        // ================= HELPERS =================

        private string GetString(object value)
            => value == DBNull.Value ? string.Empty : value.ToString()!;

        private int GetInt(object value)
        {
            if (value == DBNull.Value) return 0;

            if (value is OracleDecimal od)
                return od.ToInt32();

            return Convert.ToInt32(value);
        }

        private object DbValue(string? value)
            => string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;

        // ==========================================

        public async Task<IEnumerable<ResponsePersonaDTO>> ObtenerTodosAsync()
        {
            List<ResponsePersonaDTO> lista = new();

            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
                SELECT P.*, TP.TIP_DESCRIPCION
                FROM GCB_PERSONA P
                INNER JOIN GCB_TIPO_PERSONA TP
                    ON P.TIP_TIPO_PERSONA = TP.TIP_TIPO_PERSONA";

            using var cmd = new OracleCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new ResponsePersonaDTO
                {
                    PER_Persona = GetInt(reader["PER_PERSONA"]),
                    TIP_Tipo_Persona = GetInt(reader["TIP_TIPO_PERSONA"]),
                    TipoPersonaDescripcion = GetString(reader["TIP_DESCRIPCION"]),
                    PER_Primer_Nombre = GetString(reader["PER_PRIMER_NOMBRE"]),
                    PER_Primer_Apellido = GetString(reader["PER_PRIMER_APELLIDO"]),
                    PER_Estado = GetString(reader["PER_ESTADO"])
                });
            }

            return lista;
        }

        public async Task<ResponsePersonaDTO?> ObtenerPorIdAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"SELECT * FROM GCB_PERSONA WHERE PER_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ResponsePersonaDTO
                {
                    PER_Persona = GetInt(reader["PER_PERSONA"]),
                    PER_Estado = GetString(reader["PER_ESTADO"])
                };
            }

            return null;
        }

        public async Task<ResponsePersonaDetalleDTO?> ObtenerDetallePorIdAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            ResponsePersonaDetalleDTO persona = new()
            {
                Telefonos = new(),
                Direcciones = new()
            };

            string query = @"SELECT * FROM GCB_PERSONA WHERE PER_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            persona.PER_Persona = GetInt(reader["PER_PERSONA"]);
            persona.PER_Estado = GetString(reader["PER_ESTADO"]);

            // ===== TELEFONOS =====
            string telQuery = @"SELECT * FROM GCB_TELEFONO_PERSONA WHERE PER_PERSONA = :ID";

            using var cmdTel = new OracleCommand(telQuery, connection);
            cmdTel.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            using var rTel = await cmdTel.ExecuteReaderAsync();

            while (await rTel.ReadAsync())
            {
                persona.Telefonos.Add(new ResponseTelefonoPersonaDTO
                {
                    TEL_Telefono = GetInt(rTel["TEL_TELEFONO"]),
                    PER_Persona = GetInt(rTel["PER_PERSONA"]),
                    TEP_Numero = GetString(rTel["TEP_NUMERO"]),
                    TEP_Principal = GetString(rTel["TEP_PRINCIPAL"]),
                    TEP_Estado = GetString(rTel["TEP_ESTADO"])
                });
            }

            // ===== DIRECCIONES =====
            string dirQuery = @"SELECT * FROM GCB_DIRECCION_PERSONA WHERE PER_PERSONA = :ID";

            using var cmdDir = new OracleCommand(dirQuery, connection);
            cmdDir.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            using var rDir = await cmdDir.ExecuteReaderAsync();

            while (await rDir.ReadAsync())
            {
                persona.Direcciones.Add(new ResponseDireccionPersonaDTO
                {
                    DIR_Direccion = GetInt(rDir["DIR_DIRECCION"]),
                    PER_Persona = GetInt(rDir["PER_PERSONA"]),
                    DIR_Principal = GetString(rDir["DIR_PRINCIPAL"]),
                    DIR_Estado = GetString(rDir["DIR_ESTADO"])
                });
            }

            return persona;
        }

        public async Task<ResponseCreatePersonaDTO> CrearAsync(CreatePersonaDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                // ================= PERSONA =================
                string insertPersona = @"
        INSERT INTO GCB_PERSONA 
        (
            TIP_TIPO_PERSONA,
            PER_PRIMER_NOMBRE,
            PER_SEGUNDO_NOMBRE,
            PER_PRIMER_APELLIDO,
            PER_SEGUNDO_APELLIDO,
            PER_RAZON_SOCIAL,
            PER_NIT,
            PER_DPI,
            PER_ESTADO
        )
        VALUES
        (
            :TIP,
            :PNOMBRE,
            :SNOMBRE,
            :PAPELLIDO,
            :SAPELLIDO,
            :RAZON,
            :NIT,
            :DPI,
            :ESTADO
        )
        RETURNING PER_PERSONA INTO :ID";

                using var cmdPersona = new OracleCommand(insertPersona, connection);
                cmdPersona.Transaction = transaction;

                cmdPersona.Parameters.Add("TIP", OracleDbType.Int32).Value = dto.TIP_Tipo_Persona;
                cmdPersona.Parameters.Add("PNOMBRE", OracleDbType.Varchar2).Value = DbValue(dto.PER_Primer_Nombre);
                cmdPersona.Parameters.Add("SNOMBRE", OracleDbType.Varchar2).Value = DbValue(dto.PER_Segundo_Nombre);
                cmdPersona.Parameters.Add("PAPELLIDO", OracleDbType.Varchar2).Value = DbValue(dto.PER_Primer_Apellido);
                cmdPersona.Parameters.Add("SAPELLIDO", OracleDbType.Varchar2).Value = DbValue(dto.PER_Segundo_Apellido);
                cmdPersona.Parameters.Add("RAZON", OracleDbType.Varchar2).Value = DbValue(dto.PER_Razon_Social);
                cmdPersona.Parameters.Add("NIT", OracleDbType.Varchar2).Value = DbValue(dto.PER_NIT);
                cmdPersona.Parameters.Add("DPI", OracleDbType.Varchar2).Value = DbValue(dto.PER_DPI);
                cmdPersona.Parameters.Add("ESTADO", OracleDbType.Char).Value = dto.PER_Estado;

                var outId = new OracleParameter("ID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };

                cmdPersona.Parameters.Add(outId);

                await cmdPersona.ExecuteNonQueryAsync();

                int personaId = ((Oracle.ManagedDataAccess.Types.OracleDecimal)outId.Value).ToInt32();

                // ================= TELEFONOS =================
                if (dto.Telefonos != null && dto.Telefonos.Any())
                {
                    foreach (var tel in dto.Telefonos)
                    {
                        string insertTel = @"
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
                    :TIPO,
                    :NUMERO,
                    :PRINCIPAL,
                    :ESTADO,
                    SYSDATE
                )";

                        using var cmdTel = new OracleCommand(insertTel, connection);
                        cmdTel.Transaction = transaction;

                        cmdTel.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = personaId;
                        cmdTel.Parameters.Add("TIPO", OracleDbType.Int32).Value = tel.TIT_Tipo_Telefono;
                        cmdTel.Parameters.Add("NUMERO", OracleDbType.Varchar2).Value = tel.TEP_Numero;

                        cmdTel.Parameters.Add("PRINCIPAL", OracleDbType.Char).Value =
                            tel.TEP_Principal == "S" ? "S" : "N";

                        cmdTel.Parameters.Add("ESTADO", OracleDbType.Char).Value =
                            tel.TEP_Estado == "A" ? "A" : "I";

                        await cmdTel.ExecuteNonQueryAsync();
                    }
                }

                // ================= DIRECCIONES =================
                if (dto.Direcciones != null && dto.Direcciones.Any())
                {
                    foreach (var dir in dto.Direcciones)
                    {
                        string insertDir = @"
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
                    :TIPO,
                    :DEPTO,
                    :MUNI,
                    :COLONIA,
                    :ZONA,
                    :CASA,
                    :DETALLE,
                    :PRINCIPAL,
                    'A',
                    SYSDATE
                )";

                        using var cmdDir = new OracleCommand(insertDir, connection);
                        cmdDir.Transaction = transaction;

                        cmdDir.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = personaId;
                        cmdDir.Parameters.Add("TIPO", OracleDbType.Int32).Value = dir.TDI_Tipo_Direccion;
                        cmdDir.Parameters.Add("DEPTO", OracleDbType.Varchar2).Value = DbValue(dir.DIR_Departamento);
                        cmdDir.Parameters.Add("MUNI", OracleDbType.Varchar2).Value = DbValue(dir.DIR_Municipio);
                        cmdDir.Parameters.Add("COLONIA", OracleDbType.Varchar2).Value = DbValue(dir.DIR_Colonia);
                        cmdDir.Parameters.Add("ZONA", OracleDbType.Varchar2).Value = DbValue(dir.DIR_Zona);
                        cmdDir.Parameters.Add("CASA", OracleDbType.Varchar2).Value = DbValue(dir.DIR_Numero_Casa);
                        cmdDir.Parameters.Add("DETALLE", OracleDbType.Varchar2).Value = DbValue(dir.DIR_Detalle);

                        cmdDir.Parameters.Add("PRINCIPAL", OracleDbType.Char).Value =
                            dir.DIR_Principal == "S" ? "S" : "N";

                        await cmdDir.ExecuteNonQueryAsync();
                    }
                }

                transaction.Commit();

                return new ResponseCreatePersonaDTO
                {
                    PER_Persona = personaId,
                    Mensaje = "Persona creada con teléfonos y direcciones 🔥"
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> ActualizarAsync(int id, UpdatePersonaDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
                UPDATE GCB_PERSONA 
                SET PER_ESTADO = :ESTADO
                WHERE PER_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);

            cmd.Parameters.Add("ESTADO", OracleDbType.Char).Value = dto.PER_Estado;
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"UPDATE GCB_PERSONA SET PER_ESTADO = 'I' WHERE PER_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}