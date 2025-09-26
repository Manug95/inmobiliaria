using MySql.Data.MySqlClient;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Interfaces;

namespace InmobiliariaGutierrezManuel.Repositories;

public class ContratoRepository : BaseRepository, IContratoRepository
{
    public bool ActualizarContrato(Contrato contrato)
    {
        bool modificado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE contratos 
                SET 
                {nameof(Contrato.IdInmueble)} = @{nameof(Contrato.IdInmueble)}, 
                {nameof(Contrato.IdInquilino)} = @{nameof(Contrato.IdInquilino)}, 
                {nameof(Contrato.MontoMensual)} = @{nameof(Contrato.MontoMensual)}, 
                {nameof(Contrato.FechaInicio)} = @{nameof(Contrato.FechaInicio)}, 
                {nameof(Contrato.FechaFin)} = @{nameof(Contrato.FechaFin)}, 
                {nameof(Contrato.FechaTerminado)} = @{nameof(Contrato.FechaTerminado)} 
                WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Contrato.IdInmueble)}", contrato.IdInmueble);
                command.Parameters.AddWithValue($"{nameof(Contrato.IdInquilino)}", contrato.IdInquilino);
                command.Parameters.AddWithValue($"{nameof(Contrato.MontoMensual)}", contrato.MontoMensual);
                command.Parameters.AddWithValue($"{nameof(Contrato.FechaInicio)}", contrato.FechaInicio);
                command.Parameters.AddWithValue($"{nameof(Contrato.FechaFin)}", contrato.FechaFin);
                command.Parameters.AddWithValue($"{nameof(Contrato.FechaTerminado)}", contrato.FechaTerminado);
                command.Parameters.AddWithValue($"{nameof(Contrato.Id)}", contrato.Id);

                connection.Open();

                modificado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return modificado;
    }

    public int ContarContratos(int? idInm = null, string? desde = null, string? hasta = null, string? fechaAVencer = null)
    {
        int cantidadContratos = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT COUNT({nameof(Contrato.Id)}) AS cantidad 
                FROM contratos 
                WHERE {nameof(Contrato.Borrado)} = 0"
            ;

            if (idInm.HasValue)
                sql += $" AND {nameof(Contrato.IdInmueble)} = @idInm";
            
            if (!string.IsNullOrWhiteSpace(desde) && !string.IsNullOrWhiteSpace(hasta))
                sql += @$" AND ({nameof(Contrato.FechaInicio)} BETWEEN @desde AND @hasta 
                            OR {nameof(Contrato.FechaFin)} BETWEEN @desde AND @hasta
                            OR @desde BETWEEN {nameof(Contrato.FechaInicio)} AND {nameof(Contrato.FechaFin)} 
                            OR @hasta BETWEEN {nameof(Contrato.FechaInicio)} AND {nameof(Contrato.FechaFin)}) 
                            AND {nameof(Contrato.FechaTerminado)} IS NULL";

            if (!string.IsNullOrWhiteSpace(fechaAVencer))
                sql += $" AND {nameof(Contrato.FechaFin)} BETWEEN @hoy AND @fecha AND {nameof(Contrato.FechaTerminado)} IS NULL";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (idInm.HasValue) command.Parameters.AddWithValue($"idInm", idInm.Value);
                if (!string.IsNullOrWhiteSpace(desde) && !string.IsNullOrWhiteSpace(hasta))
                {
                    command.Parameters.AddWithValue("desde", desde);
                    command.Parameters.AddWithValue("hasta", hasta);
                }
                if (!string.IsNullOrWhiteSpace(fechaAVencer))
                {
                    command.Parameters.AddWithValue("hoy", DateTime.Today.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("fecha", fechaAVencer);
                }

                connection.Open();

                cantidadContratos = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
        }

        return cantidadContratos;
    }

    public bool EliminarContrato(int id)
    {
        bool borrado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE contratos 
                SET {nameof(Contrato.Borrado)} = 1 
                WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Contrato.Id)}", id);

                connection.Open();

                borrado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return borrado;
    }

    public int InsertarContrato(Contrato contrato)
    {
        int id = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                INSERT INTO contratos 
                (
                    {nameof(Contrato.IdInmueble)}, 
                    {nameof(Contrato.IdInquilino)}, 
                    {nameof(Contrato.IdUsuarioContratador)}, 
                    {nameof(Contrato.MontoMensual)}, 
                    {nameof(Contrato.FechaInicio)}, 
                    {nameof(Contrato.FechaFin)} 
                )
                VALUES 
                (
                    @{nameof(Contrato.IdInmueble)}, 
                    @{nameof(Contrato.IdInquilino)}, 
                    @{nameof(Contrato.IdUsuarioContratador)}, 
                    @{nameof(Contrato.MontoMensual)}, 
                    @{nameof(Contrato.FechaInicio)}, 
                    @{nameof(Contrato.FechaFin)} 
                ); 
                
                SELECT LAST_INSERT_ID();"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Contrato.IdInmueble)}", contrato.IdInmueble);
                command.Parameters.AddWithValue($"{nameof(Contrato.IdInquilino)}", contrato.IdInquilino);
                command.Parameters.AddWithValue($"{nameof(Contrato.IdUsuarioContratador)}", contrato.IdUsuarioContratador);
                command.Parameters.AddWithValue($"{nameof(Contrato.MontoMensual)}", contrato.MontoMensual);
                command.Parameters.AddWithValue($"{nameof(Contrato.FechaInicio)}", contrato.FechaInicio);
                command.Parameters.AddWithValue($"{nameof(Contrato.FechaFin)}", contrato.FechaFin);

                try
                {
                    connection.Open();

                    id = Convert.ToInt32(command.ExecuteScalar());
                    contrato.Id = id;
                }
                catch (MySqlException mye)
                {
                    Console.WriteLine(mye.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                connection.Close();
            }
        }

        return id;
    }

    public IList<Contrato> ListarContratos(int? offset = null, int? limit = null, int? idInm = null, string? desde = null, string? hasta = null, string? fechaAVencer = null)
    {
        var contratos = new List<Contrato>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    c.{nameof(Contrato.Id)}, 
                    c.{nameof(Contrato.IdInmueble)}, 
                    c.{nameof(Contrato.IdInquilino)}, 
                    c.{nameof(Contrato.MontoMensual)}, 
                    c.{nameof(Contrato.FechaInicio)}, 
                    c.{nameof(Contrato.FechaFin)}, 
                    c.{nameof(Contrato.FechaTerminado)}, 
                    c.{nameof(Contrato.IdUsuarioContratador)}, 
                    c.{nameof(Contrato.IdUsuarioTerminador)}, 
                    inm.{nameof(Inmueble.IdPropietario)}, 
                    inm.{nameof(Inmueble.Calle)}, 
                    inm.{nameof(Inmueble.NroCalle)}, 
                    inm.{nameof(Inmueble.IdTipoInmueble)}, 
                    ti.{nameof(Inmueble.Tipo)}, 
                    p.{nameof(Propietario.Nombre)}, 
                    p.{nameof(Propietario.Apellido)}, 
                    p.{nameof(Propietario.Dni)}, 
                    inq.{nameof(Inquilino.Nombre)}, 
                    inq.{nameof(Inquilino.Apellido)}, 
                    inq.{nameof(Inquilino.Dni)} 
                FROM contratos AS c 
                INNER JOIN inmuebles AS inm 
                    ON c.{nameof(Contrato.IdInmueble)} = inm.id 
                INNER JOIN tipos_inmueble AS ti 
                    ON inm.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                INNER JOIN propietarios AS p 
                    ON inm.{nameof(Inmueble.IdPropietario)} = p.id 
                INNER JOIN inquilinos AS inq 
                    ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                WHERE c.{nameof(Contrato.Borrado)} = 0"
            ;

            if (idInm.HasValue)
                sql += $" AND c.{nameof(Contrato.IdInmueble)} = @idInm";

            if (!string.IsNullOrWhiteSpace(desde) && !string.IsNullOrWhiteSpace(hasta))
                sql += @$" AND ((c.{nameof(Contrato.FechaInicio)} BETWEEN @desde AND @hasta) 
                            OR (c.{nameof(Contrato.FechaFin)} BETWEEN @desde AND @hasta)) 
                            OR @desde BETWEEN c.{nameof(Contrato.FechaInicio)} AND c.{nameof(Contrato.FechaFin)} 
                            OR @hasta BETWEEN c.{nameof(Contrato.FechaInicio)} AND c.{nameof(Contrato.FechaFin)} 
                            AND c.{nameof(Contrato.FechaTerminado)} IS NULL";

            if (!string.IsNullOrWhiteSpace(fechaAVencer))
                sql += $" AND c.{nameof(Contrato.FechaFin)} BETWEEN @hoy AND @fecha AND c.{nameof(Contrato.FechaTerminado)} IS NULL";

            if (offset.HasValue && limit.HasValue)
                    sql += $" LIMIT @limit OFFSET @offset";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (idInm.HasValue) command.Parameters.AddWithValue($"idInm", idInm.Value);

                if (!string.IsNullOrWhiteSpace(desde) && !string.IsNullOrWhiteSpace(hasta))
                {
                    command.Parameters.AddWithValue("desde", desde);
                    command.Parameters.AddWithValue("hasta", hasta);
                }

                if (!string.IsNullOrWhiteSpace(fechaAVencer))
                {
                    command.Parameters.AddWithValue("hoy", DateTime.Today.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("fecha", fechaAVencer);
                }

                if (offset.HasValue && limit.HasValue)
                {
                    command.Parameters.AddWithValue($"limit", limit.Value);
                    command.Parameters.AddWithValue($"offset", (offset.Value - 1) * limit.Value);
                }

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            IdUsuarioContratador = reader[nameof(Contrato.IdUsuarioContratador)] == DBNull.Value ? 0 : reader.GetInt32(nameof(Contrato.IdUsuarioContratador)),
                            IdUsuarioTerminador = reader[nameof(Contrato.IdUsuarioTerminador)] == DBNull.Value ? 0 : reader.GetInt32(nameof(Contrato.IdUsuarioTerminador)),
                            MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                            FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                            FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                            FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                Calle = reader.GetString(nameof(Inmueble.Calle)),
                                NroCalle = reader.GetUInt32(nameof(Inmueble.NroCalle)),
                                Tipo = new TipoInmueble
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                },
                                Duenio = new Propietario
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                    Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                    Dni = reader.GetString(nameof(Propietario.Dni))
                                }
                            },
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                                Dni = reader.GetString(nameof(Inquilino.Dni))
                            }
                        });
                    }
                }
            }
        }

        return contratos;
    }

    public Contrato? ObtenerContrato(int id)
    {
        Contrato? contrato = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    c.{nameof(Contrato.Id)}, 
                    c.{nameof(Contrato.IdInmueble)}, 
                    c.{nameof(Contrato.IdInquilino)}, 
                    c.{nameof(Contrato.MontoMensual)}, 
                    c.{nameof(Contrato.FechaInicio)}, 
                    c.{nameof(Contrato.FechaFin)}, 
                    c.{nameof(Contrato.FechaTerminado)}, 
                    c.{nameof(Contrato.IdUsuarioContratador)}, 
                    c.{nameof(Contrato.IdUsuarioTerminador)}, 
                    inm.{nameof(Inmueble.IdPropietario)}, 
                    inm.{nameof(Inmueble.Calle)}, 
                    inm.{nameof(Inmueble.NroCalle)}, 
                    inm.{nameof(Inmueble.Uso)}, 
                    inm.{nameof(Inmueble.IdTipoInmueble)}, 
                    ti.{nameof(Inmueble.Tipo)}, 
                    p.{nameof(Propietario.Nombre)} AS nombreProp, 
                    p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                    p.{nameof(Propietario.Dni)} AS dniProp, 
                    inq.{nameof(Inquilino.Nombre)} AS nombreInq, 
                    inq.{nameof(Inquilino.Apellido)} AS apellidoInq, 
                    inq.{nameof(Inquilino.Dni)} AS dniInq, 
                    uc.{nameof(Usuario.Nombre)} AS nombreContratador, 
                    uc.{nameof(Usuario.Apellido)} AS apellidoContratador, 
                    uc.{nameof(Usuario.Rol)} AS RolContratador, 
                    ut.{nameof(Usuario.Nombre)} AS nombreTerminador, 
                    ut.{nameof(Usuario.Apellido)} AS apellidoTerminador, 
                    ut.{nameof(Usuario.Rol)} AS RolTerminador 
                FROM contratos AS c 
                INNER JOIN usuarios AS uc 
                    ON c.{nameof(Contrato.IdUsuarioContratador)} = uc.id 
                LEFT JOIN usuarios AS ut 
                    ON c.{nameof(Contrato.IdUsuarioTerminador)} = ut.id 
                INNER JOIN inmuebles AS inm 
                    ON c.{nameof(Contrato.IdInmueble)} = inm.id 
                INNER JOIN tipos_inmueble AS ti 
                    ON inm.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                INNER JOIN propietarios AS p 
                    ON inm.{nameof(Inmueble.IdPropietario)} = p.id 
                INNER JOIN inquilinos AS inq 
                    ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                WHERE c.{nameof(Contrato.Borrado)} = 0 AND c.{nameof(Contrato.Id)} = @{nameof(Contrato.Id)}"
            ;

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                command.Parameters.AddWithValue($"{nameof(Contrato.Id)}", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            IdUsuarioContratador = reader[nameof(Contrato.IdUsuarioContratador)] == DBNull.Value ? 0 : reader.GetInt32(nameof(Contrato.IdUsuarioContratador)),
                            IdUsuarioTerminador = reader[nameof(Contrato.IdUsuarioTerminador)] == DBNull.Value ? 0 : reader.GetInt32(nameof(Contrato.IdUsuarioTerminador)),
                            MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                            FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                            FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                            FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                Calle = reader.GetString(nameof(Inmueble.Calle)),
                                NroCalle = reader.GetUInt32(nameof(Inmueble.NroCalle)),
                                Uso = reader.GetString(nameof(Inmueble.Uso)),
                                Tipo = new TipoInmueble
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                },
                                Duenio = new Propietario
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    Nombre = reader.GetString("nombreProp"),
                                    Apellido = reader.GetString("apellidoProp"),
                                    Dni = reader.GetString("dniProp")
                                }
                            },
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                Nombre = reader.GetString("nombreInq"),
                                Apellido = reader.GetString("apellidoInq"),
                                Dni = reader.GetString("dniInq")
                            },
                            UsuarioContratador = new Usuario
                            {
                                Id = reader.GetInt32(nameof(Contrato.IdUsuarioContratador)),
                                Nombre = reader["nombreContratador"] == DBNull.Value ? null : reader.GetString("nombreContratador"),
                                Apellido = reader["apellidoContratador"] == DBNull.Value ? null : reader.GetString("apellidoContratador"),
                                Rol = reader.GetString("RolContratador")
                            }
                        };
                        if (reader[nameof(Contrato.IdUsuarioTerminador)] != DBNull.Value)
                        {
                            contrato.UsuarioTerminador = new Usuario
                            {
                                Id = reader.GetInt32(nameof(Contrato.IdUsuarioTerminador)),
                                Nombre = reader["nombreTerminador"] == DBNull.Value ? null : reader.GetString("nombreTerminador"),
                                Apellido = reader["apellidoTerminador"] == DBNull.Value ? null : reader.GetString("apellidoTerminador"),
                                Rol = reader.GetString("RolTerminador")
                            };
                        }
                    }
                }
            }
        }

        return contrato;
    }

    public bool EstaDisponible(string desde, string hasta, int idInmueble, int idContrato)
    {
        bool disponible;
        int rowCount = 0;

        // string sql = @$"
        // SELECT {nameof(Contrato.Id)} 
        // FROM contratos 
        // WHERE 
        //     {nameof(Contrato.IdInmueble)} = @idInmueble 
        //     AND (@desde BETWEEN {nameof(Contrato.FechaInicio)} AND {nameof(Contrato.FechaFin)} 
        //     OR @hasta BETWEEN {nameof(Contrato.FechaInicio)} AND {nameof(Contrato.FechaFin)});"
        // ;

        string sql = @$"
            SELECT COUNT({nameof(Contrato.Id)}) 
            FROM contratos 
            WHERE {nameof(Contrato.IdInmueble)} = @idInmueble 
                AND (@desde BETWEEN {nameof(Contrato.FechaInicio)} AND {nameof(Contrato.FechaFin)} 
                OR @hasta BETWEEN {nameof(Contrato.FechaInicio)} AND {nameof(Contrato.FechaFin)});"
        ;

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("idInmueble", idInmueble);
                command.Parameters.AddWithValue("desde", desde);
                command.Parameters.AddWithValue("hasta", hasta);

                connection.Open();

                // using (var reader = command.ExecuteReader())
                // {
                    // while (reader.Read())
                    // {
                    //     rowCount++;
                    // }
                    rowCount = Convert.ToInt32(command.ExecuteScalar());
                    disponible = (idContrato == 0 && rowCount == 0) || (idContrato > 0 && rowCount == 1);
                // }
            }
        }

        return disponible;
    }

    public bool TerminarContrato(int id, string fecha, int idUsuario)
    {
        bool finalizado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE contratos 
                SET {nameof(Contrato.FechaTerminado)} = @{nameof(Contrato.FechaTerminado)}, 
                    {nameof(Contrato.IdUsuarioTerminador)} = @{nameof(Contrato.IdUsuarioTerminador)} 
                WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Contrato.FechaTerminado)}", fecha);
                command.Parameters.AddWithValue($"{nameof(Contrato.IdUsuarioTerminador)}", idUsuario);
                command.Parameters.AddWithValue($"{nameof(Contrato.Id)}", id);

                connection.Open();

                finalizado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return finalizado;
    }
}