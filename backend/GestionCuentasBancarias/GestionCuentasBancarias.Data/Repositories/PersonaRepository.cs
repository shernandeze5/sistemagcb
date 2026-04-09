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
        SELECT 
            P.PER_PERSONA,
            P.TIP_TIPO_PERSONA,
            TP.TIP_DESCRIPCION,
            P.PER_PRIMER_NOMBRE,
            P.PER_SEGUNDO_NOMBRE,
            P.PER_PRIMER_APELLIDO,
            P.PER_SEGUNDO_APELLIDO,
            P.PER_RAZON_SOCIAL,
            P.PER_NIT,
            P.PER_DPI,
            P.PER_ESTADO,
            P.PER_FECHA_CREACION
        FROM GCB_PERSONA P
        INNER JOIN GCB_TIPO_PERSONA TP
            ON P.TIP_TIPO_PERSONA = TP.TIP_TIPO_PERSONA
        ORDER BY P.PER_PERSONA";

            using var cmd = new OracleCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var primerNombre = GetString(reader["PER_PRIMER_NOMBRE"]);
                var segundoNombre = GetString(reader["PER_SEGUNDO_NOMBRE"]);
                var primerApellido = GetString(reader["PER_PRIMER_APELLIDO"]);
                var segundoApellido = GetString(reader["PER_SEGUNDO_APELLIDO"]);
                var razonSocial = GetString(reader["PER_RAZON_SOCIAL"]);

                string nombreCompleto = !string.IsNullOrWhiteSpace(razonSocial)
                    ? razonSocial
                    : $"{primerNombre} {segundoNombre} {primerApellido} {segundoApellido}"
                        .Replace("  ", " ")
                        .Trim();

                lista.Add(new ResponsePersonaDTO
                {
                    PER_Persona = GetInt(reader["PER_PERSONA"]),
                    TIP_Tipo_Persona = GetInt(reader["TIP_TIPO_PERSONA"]),
                    TipoPersonaDescripcion = GetString(reader["TIP_DESCRIPCION"]),
                    PER_Primer_Nombre = primerNombre,
                    PER_Segundo_Nombre = segundoNombre,
                    PER_Primer_Apellido = primerApellido,
                    PER_Segundo_Apellido = segundoApellido,
                    PER_Razon_Social = razonSocial,
                    PER_NIT = GetString(reader["PER_NIT"]),
                    PER_DPI = GetString(reader["PER_DPI"]),
                    PER_Estado = GetString(reader["PER_ESTADO"]),
                    PER_Fecha_Creacion = reader["PER_FECHA_CREACION"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(reader["PER_FECHA_CREACION"]),
                    NombreCompleto = nombreCompleto
                });
            }

            return lista;
        }
        public async Task<ResponsePersonaDTO?> ObtenerPorIdAsync(int id)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
        SELECT 
            P.PER_PERSONA,
            P.TIP_TIPO_PERSONA,
            TP.TIP_DESCRIPCION,
            P.PER_PRIMER_NOMBRE,
            P.PER_SEGUNDO_NOMBRE,
            P.PER_PRIMER_APELLIDO,
            P.PER_SEGUNDO_APELLIDO,
            P.PER_RAZON_SOCIAL,
            P.PER_NIT,
            P.PER_DPI,
            P.PER_ESTADO,
            P.PER_FECHA_CREACION
        FROM GCB_PERSONA P
        INNER JOIN GCB_TIPO_PERSONA TP
            ON P.TIP_TIPO_PERSONA = TP.TIP_TIPO_PERSONA
        WHERE P.PER_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var primerNombre = GetString(reader["PER_PRIMER_NOMBRE"]);
                var segundoNombre = GetString(reader["PER_SEGUNDO_NOMBRE"]);
                var primerApellido = GetString(reader["PER_PRIMER_APELLIDO"]);
                var segundoApellido = GetString(reader["PER_SEGUNDO_APELLIDO"]);
                var razonSocial = GetString(reader["PER_RAZON_SOCIAL"]);

                string nombreCompleto = !string.IsNullOrWhiteSpace(razonSocial)
                    ? razonSocial
                    : $"{primerNombre} {segundoNombre} {primerApellido} {segundoApellido}"
                        .Replace("  ", " ")
                        .Trim();

                return new ResponsePersonaDTO
                {
                    PER_Persona = GetInt(reader["PER_PERSONA"]),
                    TIP_Tipo_Persona = GetInt(reader["TIP_TIPO_PERSONA"]),
                    TipoPersonaDescripcion = GetString(reader["TIP_DESCRIPCION"]),
                    PER_Primer_Nombre = primerNombre,
                    PER_Segundo_Nombre = segundoNombre,
                    PER_Primer_Apellido = primerApellido,
                    PER_Segundo_Apellido = segundoApellido,
                    PER_Razon_Social = razonSocial,
                    PER_NIT = GetString(reader["PER_NIT"]),
                    PER_DPI = GetString(reader["PER_DPI"]),
                    PER_Estado = GetString(reader["PER_ESTADO"]),
                    PER_Fecha_Creacion = reader["PER_FECHA_CREACION"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(reader["PER_FECHA_CREACION"]),
                    NombreCompleto = nombreCompleto
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

            string query = @"
        SELECT 
            P.PER_PERSONA,
            P.TIP_TIPO_PERSONA,
            TP.TIP_DESCRIPCION,
            P.PER_PRIMER_NOMBRE,
            P.PER_SEGUNDO_NOMBRE,
            P.PER_PRIMER_APELLIDO,
            P.PER_SEGUNDO_APELLIDO,
            P.PER_RAZON_SOCIAL,
            P.PER_NIT,
            P.PER_DPI,
            P.PER_ESTADO,
            P.PER_FECHA_CREACION
        FROM GCB_PERSONA P
        INNER JOIN GCB_TIPO_PERSONA TP
            ON P.TIP_TIPO_PERSONA = TP.TIP_TIPO_PERSONA
        WHERE P.PER_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            persona.PER_Persona = GetInt(reader["PER_PERSONA"]);
            persona.TIP_Tipo_Persona = GetInt(reader["TIP_TIPO_PERSONA"]);
            persona.TipoPersonaDescripcion = GetString(reader["TIP_DESCRIPCION"]);
            persona.PER_Primer_Nombre = GetString(reader["PER_PRIMER_NOMBRE"]);
            persona.PER_Segundo_Nombre = GetString(reader["PER_SEGUNDO_NOMBRE"]);
            persona.PER_Primer_Apellido = GetString(reader["PER_PRIMER_APELLIDO"]);
            persona.PER_Segundo_Apellido = GetString(reader["PER_SEGUNDO_APELLIDO"]);
            persona.PER_Razon_Social = GetString(reader["PER_RAZON_SOCIAL"]);
            persona.PER_NIT = GetString(reader["PER_NIT"]);
            persona.PER_DPI = GetString(reader["PER_DPI"]);
            persona.PER_Estado = GetString(reader["PER_ESTADO"]);
            persona.PER_Fecha_Creacion = reader["PER_FECHA_CREACION"] == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(reader["PER_FECHA_CREACION"]);

            persona.NombreCompleto = !string.IsNullOrWhiteSpace(persona.PER_Razon_Social)
                ? persona.PER_Razon_Social
                : $"{persona.PER_Primer_Nombre} {persona.PER_Segundo_Nombre} {persona.PER_Primer_Apellido} {persona.PER_Segundo_Apellido}"
                    .Replace("  ", " ")
                    .Trim();

            // ===== TELEFONOS =====
            string telQuery = @"
        SELECT 
            T.TEP_TELEFONO_PERSONA,
            T.PER_PERSONA,
            T.TIT_TIPO_TELEFONO,
            TT.TIT_DESCRIPCION,
            T.TEP_NUMERO,
            T.TEP_PRINCIPAL,
            T.TEP_ESTADO,
            T.TEP_FECHA_CREACION
        FROM GCB_TELEFONO_PERSONA T
        LEFT JOIN GCB_TIPO_TELEFONO TT
            ON T.TIT_TIPO_TELEFONO = TT.TIT_TIPO_TELEFONO
        WHERE T.PER_PERSONA = :ID
        ORDER BY T.TEP_TELEFONO_PERSONA";

            using (var cmdTel = new OracleCommand(telQuery, connection))
            {
                cmdTel.Parameters.Add("ID", OracleDbType.Int32).Value = id;

                using var rTel = await cmdTel.ExecuteReaderAsync();

                while (await rTel.ReadAsync())
                {
                    persona.Telefonos.Add(new ResponseTelefonoPersonaDTO
                    {
                        TEL_Telefono = GetInt(rTel["TEP_TELEFONO_PERSONA"]),
                        PER_Persona = GetInt(rTel["PER_PERSONA"]),
                        TIT_Tipo_Telefono = GetInt(rTel["TIT_TIPO_TELEFONO"]),
                        TipoTelefonoDescripcion = GetString(rTel["TIT_DESCRIPCION"]),
                        TEP_Numero = GetString(rTel["TEP_NUMERO"]),
                        TEP_Principal = GetString(rTel["TEP_PRINCIPAL"]),
                        TEP_Estado = GetString(rTel["TEP_ESTADO"]),
                        TEP_Fecha_Creacion = rTel["TEP_FECHA_CREACION"] == DBNull.Value
                            ? DateTime.MinValue
                            : Convert.ToDateTime(rTel["TEP_FECHA_CREACION"])
                    });
                }
            }

            // ===== DIRECCIONES =====
            string dirQuery = @"
        SELECT 
            D.DIR_DIRECCION,
            D.PER_PERSONA,
            D.TDI_TIPO_DIRECCION,
            TD.TDI_DESCRIPCION,
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
        LEFT JOIN GCB_TIPO_DIRECCION TD
            ON D.TDI_TIPO_DIRECCION = TD.TDI_TIPO_DIRECCION
        WHERE D.PER_PERSONA = :ID
        ORDER BY D.DIR_DIRECCION";

            using (var cmdDir = new OracleCommand(dirQuery, connection))
            {
                cmdDir.Parameters.Add("ID", OracleDbType.Int32).Value = id;

                using var rDir = await cmdDir.ExecuteReaderAsync();

                while (await rDir.ReadAsync())
                {
                    persona.Direcciones.Add(new ResponseDireccionPersonaDTO
                    {
                        DIR_Direccion = GetInt(rDir["DIR_DIRECCION"]),
                        PER_Persona = GetInt(rDir["PER_PERSONA"]),
                        TDI_Tipo_Direccion = GetInt(rDir["TDI_TIPO_DIRECCION"]),
                        TipoDireccionDescripcion = GetString(rDir["TDI_DESCRIPCION"]),
                        DIR_Departamento = GetString(rDir["DIR_DEPARTAMENTO"]),
                        DIR_Municipio = GetString(rDir["DIR_MUNICIPIO"]),
                        DIR_Colonia = GetString(rDir["DIR_COLONIA"]),
                        DIR_Zona = GetString(rDir["DIR_ZONA"]),
                        DIR_Numero_Casa = GetString(rDir["DIR_NUMERO_CASA"]),
                        DIR_Detalle = GetString(rDir["DIR_DETALLE"]),
                        DIR_Principal = GetString(rDir["DIR_PRINCIPAL"]),
                        DIR_Estado = GetString(rDir["DIR_ESTADO"]),
                        DIR_Fecha_Creacion = rDir["DIR_FECHA_CREACION"] == DBNull.Value
                            ? DateTime.MinValue
                            : Convert.ToDateTime(rDir["DIR_FECHA_CREACION"])
                    });
                }
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
        SET 
            TIP_TIPO_PERSONA = :TIP,
            PER_PRIMER_NOMBRE = :PNOMBRE,
            PER_SEGUNDO_NOMBRE = :SNOMBRE,
            PER_PRIMER_APELLIDO = :PAPELLIDO,
            PER_SEGUNDO_APELLIDO = :SAPELLIDO,
            PER_RAZON_SOCIAL = :RAZON,
            PER_NIT = :NIT,
            PER_DPI = :DPI,
            PER_ESTADO = :ESTADO
        WHERE PER_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);

            cmd.Parameters.Add("TIP", OracleDbType.Int32).Value = dto.TIP_Tipo_Persona;
            cmd.Parameters.Add("PNOMBRE", OracleDbType.Varchar2).Value = DbValue(dto.PER_Primer_Nombre);
            cmd.Parameters.Add("SNOMBRE", OracleDbType.Varchar2).Value = DbValue(dto.PER_Segundo_Nombre);
            cmd.Parameters.Add("PAPELLIDO", OracleDbType.Varchar2).Value = DbValue(dto.PER_Primer_Apellido);
            cmd.Parameters.Add("SAPELLIDO", OracleDbType.Varchar2).Value = DbValue(dto.PER_Segundo_Apellido);
            cmd.Parameters.Add("RAZON", OracleDbType.Varchar2).Value = DbValue(dto.PER_Razon_Social);
            cmd.Parameters.Add("NIT", OracleDbType.Varchar2).Value = DbValue(dto.PER_NIT);
            cmd.Parameters.Add("DPI", OracleDbType.Varchar2).Value = DbValue(dto.PER_DPI);
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

        public async Task<bool> AgregarTelefonoAsync(int personaId, CreateTelefonoPersonaExistenteDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
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
            :TEP_ESTADO,
            SYSDATE
        )";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = personaId;
            cmd.Parameters.Add("TIT_TIPO_TELEFONO", OracleDbType.Int32).Value = dto.TIT_Tipo_Telefono;
            cmd.Parameters.Add("TEP_NUMERO", OracleDbType.Varchar2).Value = DbValue(dto.TEP_Numero);
            cmd.Parameters.Add("TEP_PRINCIPAL", OracleDbType.Char).Value = dto.TEP_Principal;
            cmd.Parameters.Add("TEP_ESTADO", OracleDbType.Char).Value = dto.TEP_Estado;

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> AgregarDireccionAsync(int personaId, CreateDireccionPersonaExistenteDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
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

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("PER_PERSONA", OracleDbType.Int32).Value = personaId;
            cmd.Parameters.Add("TDI_TIPO_DIRECCION", OracleDbType.Int32).Value = dto.TDI_Tipo_Direccion;
            cmd.Parameters.Add("DIR_DEPARTAMENTO", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Departamento);
            cmd.Parameters.Add("DIR_MUNICIPIO", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Municipio);
            cmd.Parameters.Add("DIR_COLONIA", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Colonia);
            cmd.Parameters.Add("DIR_ZONA", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Zona);
            cmd.Parameters.Add("DIR_NUMERO_CASA", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Numero_Casa);
            cmd.Parameters.Add("DIR_DETALLE", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Detalle);
            cmd.Parameters.Add("DIR_PRINCIPAL", OracleDbType.Char).Value = dto.DIR_Principal;

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ActualizarTelefonoAsync(int telefonoId, UpdateTelefonoPersonaDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
        UPDATE GCB_TELEFONO_PERSONA
        SET
            TIT_TIPO_TELEFONO = :TIT_TIPO_TELEFONO,
            TEP_NUMERO = :TEP_NUMERO,
            TEP_PRINCIPAL = :TEP_PRINCIPAL,
            TEP_ESTADO = :TEP_ESTADO
        WHERE TEP_TELEFONO_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("TIT_TIPO_TELEFONO", OracleDbType.Int32).Value = dto.TIT_Tipo_Telefono;
            cmd.Parameters.Add("TEP_NUMERO", OracleDbType.Varchar2).Value = DbValue(dto.TEP_Numero);
            cmd.Parameters.Add("TEP_PRINCIPAL", OracleDbType.Char).Value = dto.TEP_Principal;
            cmd.Parameters.Add("TEP_ESTADO", OracleDbType.Char).Value = dto.TEP_Estado;
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = telefonoId;

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> EliminarTelefonoLogicoAsync(int telefonoId)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
        UPDATE GCB_TELEFONO_PERSONA
        SET TEP_ESTADO = 'I'
        WHERE TEP_TELEFONO_PERSONA = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = telefonoId;

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ActualizarDireccionAsync(int direccionId, UpdateDireccionPersonaDTO dto)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
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
        WHERE DIR_DIRECCION = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("TDI_TIPO_DIRECCION", OracleDbType.Int32).Value = dto.TDI_Tipo_Direccion;
            cmd.Parameters.Add("DIR_DEPARTAMENTO", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Departamento);
            cmd.Parameters.Add("DIR_MUNICIPIO", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Municipio);
            cmd.Parameters.Add("DIR_COLONIA", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Colonia);
            cmd.Parameters.Add("DIR_ZONA", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Zona);
            cmd.Parameters.Add("DIR_NUMERO_CASA", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Numero_Casa);
            cmd.Parameters.Add("DIR_DETALLE", OracleDbType.Varchar2).Value = DbValue(dto.DIR_Detalle);
            cmd.Parameters.Add("DIR_PRINCIPAL", OracleDbType.Char).Value = dto.DIR_Principal;
            cmd.Parameters.Add("DIR_ESTADO", OracleDbType.Char).Value = dto.DIR_Estado;
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = direccionId;

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> EliminarDireccionLogicoAsync(int direccionId)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();

            string query = @"
        UPDATE GCB_DIRECCION_PERSONA
        SET DIR_ESTADO = 'I'
        WHERE DIR_DIRECCION = :ID";

            using var cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add("ID", OracleDbType.Int32).Value = direccionId;

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}