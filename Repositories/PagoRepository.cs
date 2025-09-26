using MySql.Data.MySqlClient;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Interfaces;

namespace InmobiliariaGutierrezManuel.Repositories;

public class PagoRepository : BaseRepository, IPagoRepository
{
    public bool ActualizarPago(Pago pago)
    {
        bool modificado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE pagos 
                SET 
                {nameof(Pago.IdContrato)} = @{nameof(Pago.IdContrato)}, 
                {nameof(Pago.Fecha)} = @{nameof(Pago.Fecha)}, 
                {nameof(Pago.Importe)} = @{nameof(Pago.Importe)}, 
                {nameof(Pago.Detalle)} = @{nameof(Pago.Detalle)} 
                WHERE {nameof(Pago.Id)} = @{nameof(Pago.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Pago.IdContrato)}", pago.IdContrato);
                command.Parameters.AddWithValue($"{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"{nameof(Pago.Detalle)}", pago.Detalle);
                command.Parameters.AddWithValue($"{nameof(Pago.Id)}", pago.Id);

                connection.Open();

                modificado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return modificado;
    }

    public int ContarPagos(int? idCon = null)
    {
        int cantidadPagos = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT COUNT({nameof(Pago.Id)}) AS cantidad 
                FROM pagos"
            ;

            if (idCon.HasValue)
                sql += $" WHERE {nameof(Pago.IdContrato)} = @{nameof(Pago.IdContrato)}";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (idCon.HasValue) command.Parameters.AddWithValue($"{nameof(Pago.IdContrato)}", idCon.Value);

                connection.Open();

                cantidadPagos = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
        }

        return cantidadPagos;
    }

    public int ContarPagosDeAlquileres(int idContrato)
    {
        int cantidadPagos = 0;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT COUNT({nameof(Pago.Id)}) AS cantidad 
                FROM pagos 
                WHERE {nameof(Pago.IdContrato)} = @{nameof(Pago.IdContrato)} 
                    AND {nameof(Pago.Tipo)} = 'MENSUALIZACION' 
                    AND {nameof(Pago.Estado)} = 1;"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Pago.IdContrato)}", idContrato);
                connection.Open();
                cantidadPagos = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return cantidadPagos;
    }

    public decimal ObtenerSumaMultaAlquiler(int idContrato)
    {
        decimal sumaMulta = 0;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT IFNULL(SUM({nameof(Pago.Importe)}), 0) AS suma 
                FROM pagos 
                WHERE {nameof(Pago.IdContrato)} = @{nameof(Pago.IdContrato)} 
                    AND {nameof(Pago.Tipo)} = 'MULTA'
                    AND {nameof(Pago.Estado)} = 1;"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Pago.IdContrato)}", idContrato);
                connection.Open();
                sumaMulta = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return sumaMulta;
    }

    public bool EliminarPago(int id, int idUsuario)
    {
        bool borrado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE pagos 
                SET {nameof(Pago.Estado)} = 0, 
                    {nameof(Pago.IdUsuarioAnulador)} = @{nameof(Pago.IdUsuarioAnulador)} 
                WHERE {nameof(Pago.Id)} = @{nameof(Pago.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Pago.IdUsuarioAnulador)}", idUsuario);
                command.Parameters.AddWithValue($"{nameof(Pago.Id)}", id);

                connection.Open();

                borrado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return borrado;
    }

    public int InsertarPago(Pago pago)
    {
        int id = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                INSERT INTO pagos 
                (
                    {nameof(Pago.IdContrato)}, 
                    {nameof(Pago.IdUsuarioCobrador)}, 
                    {nameof(Pago.Fecha)}, 
                    {nameof(Pago.Importe)}, 
                    {nameof(Pago.Detalle)}, 
                    {nameof(Pago.Tipo)}
                )
                VALUES 
                (
                    @{nameof(Pago.IdContrato)}, 
                    @{nameof(Pago.IdUsuarioCobrador)}, 
                    @{nameof(Pago.Fecha)}, 
                    @{nameof(Pago.Importe)}, 
                    @{nameof(Pago.Detalle)}, 
                    @{nameof(Pago.Tipo)} 
                ); 
                
                SELECT LAST_INSERT_ID();"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Pago.IdContrato)}", pago.IdContrato);
                command.Parameters.AddWithValue($"{nameof(Pago.IdUsuarioCobrador)}", pago.IdUsuarioCobrador);
                command.Parameters.AddWithValue($"{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"{nameof(Pago.Detalle)}", pago.Detalle);
                command.Parameters.AddWithValue($"{nameof(Pago.Tipo)}", pago.Tipo);

                try
                {
                    connection.Open();

                    id = Convert.ToInt32(command.ExecuteScalar());
                    pago.Id = id;
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

    public IList<Pago> ListarPagos(int? offset = null, int? limit = null, int? idCon = null)
    {
        var pagos = new List<Pago>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    p.{nameof(Pago.Id)},  
                    p.{nameof(Pago.IdContrato)},  
                    p.{nameof(Pago.Fecha)}, 
                    p.{nameof(Pago.Importe)}, 
                    p.{nameof(Pago.Tipo)} AS tipoPago, 
                    p.{nameof(Pago.Estado)}, 
                    IFNULL({nameof(Pago.Detalle)}, 'Sin Detalle') AS det, 
                    c.{nameof(Contrato.FechaInicio)}, 
                    c.{nameof(Contrato.FechaFin)}, 
                    c.{nameof(Contrato.FechaTerminado)}, 
                    c.{nameof(Contrato.IdInquilino)}, 
                    c.{nameof(Contrato.IdInmueble)}, 
                    c.{nameof(Contrato.MontoMensual)}, 
                    IFNULL(c.{nameof(Contrato.IdUsuarioContratador)}, 0) AS idUContratador, 
                    IFNULL(c.{nameof(Contrato.IdUsuarioTerminador)}, 0) AS idUTerminador 
                FROM pagos AS p 
                INNER JOIN contratos AS c 
                    ON p.{nameof(Pago.IdContrato)} = c.{nameof(Contrato.Id)}"
            ;

            if (idCon.HasValue)
                sql += $" WHERE p.{nameof(Pago.IdContrato)} = @{nameof(Pago.IdContrato)}";

            if (offset.HasValue && limit.HasValue)
                    sql += $" LIMIT @limit OFFSET @offset";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (idCon.HasValue) command.Parameters.AddWithValue($"{nameof(Pago.IdContrato)}", idCon.Value);

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
                        pagos.Add(new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            Tipo = reader.GetString("tipoPago"),
                            Detalle = reader.GetString("det"),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(nameof(Pago.IdContrato)),
                                FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                                FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                                FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdUsuarioContratador = reader.GetInt32("idUContratador"),
                                IdUsuarioTerminador = reader.GetInt32("idUTerminador"),
                            }
                        });
                    }
                }
            }
        }

        return pagos;
    }

    public Pago? ObtenerPago(int id)
    {
        Pago? pago = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    p.{nameof(Pago.Id)},  
                    p.{nameof(Pago.IdContrato)}, 
                    p.{nameof(Pago.IdUsuarioCobrador)}, 
                    p.{nameof(Pago.IdUsuarioAnulador)},   
                    p.{nameof(Pago.Fecha)}, 
                    p.{nameof(Pago.Importe)}, 
                    p.{nameof(Pago.Tipo)} AS tipoPago, 
                    p.{nameof(Pago.Estado)}, 
                    IFNULL({nameof(Pago.Detalle)}, 'Sin Detalle') AS det, 
                    c.{nameof(Contrato.FechaInicio)}, 
                    c.{nameof(Contrato.FechaFin)}, 
                    c.{nameof(Contrato.FechaTerminado)}, 
                    c.{nameof(Contrato.IdInquilino)}, 
                    c.{nameof(Contrato.IdInmueble)}, 
                    c.{nameof(Contrato.MontoMensual)}, 
                    IFNULL(c.{nameof(Contrato.IdUsuarioContratador)}, 0) AS idUContratador, 
                    IFNULL(c.{nameof(Contrato.IdUsuarioTerminador)}, 0) AS idUTerminador, 
                    i.{nameof(Inmueble.IdPropietario)}, 
                    i.{nameof(Inmueble.IdTipoInmueble)}, 
                    i.{nameof(Inmueble.Uso)}, 
                    i.{nameof(Inmueble.CantidadAmbientes)}, 
                    i.{nameof(Inmueble.Calle)}, 
                    i.{nameof(Inmueble.NroCalle)}, 
                    IFNULL(i.{nameof(Inmueble.Latitud)}, 0) AS latitud, 
                    IFNULL(i.{nameof(Inmueble.Longitud)}, 0) AS longitud, 
                    i.{nameof(Inmueble.Precio)}, 
                    i.{nameof(Inmueble.Disponible)}, 
                    ti.{nameof(TipoInmueble.Tipo)} AS tipoInmueble, 
                    pr.{nameof(Propietario.Nombre)} AS nombreProp, 
                    pr.{nameof(Propietario.Apellido)} AS apellidoProp, 
                    pr.{nameof(Propietario.Dni)} AS dniProp, 
                    inq.{nameof(Inquilino.Nombre)} AS nombreInq, 
                    inq.{nameof(Inquilino.Apellido)} AS apellidoInq, 
                    inq.{nameof(Inquilino.Dni)} AS dniInq, 
                    uc.{nameof(Usuario.Nombre)} AS nombreCobrador, 
                    uc.{nameof(Usuario.Apellido)} AS apellidoCobrador, 
                    uc.{nameof(Usuario.Rol)} AS RolCobrador, 
                    ua.{nameof(Usuario.Nombre)} AS nombreAnulador, 
                    ua.{nameof(Usuario.Apellido)} AS apellidoAnulador, 
                    ua.{nameof(Usuario.Rol)} AS RolAnulador 
                FROM pagos AS p 
                INNER JOIN usuarios AS uc 
                    ON p.{nameof(Pago.IdUsuarioCobrador)} = uc.id 
                LEFT JOIN usuarios AS ua 
                    ON p.{nameof(Pago.IdUsuarioAnulador)} = ua.id 
                INNER JOIN contratos AS c 
                    ON p.{nameof(Pago.IdContrato)} = c.{nameof(Contrato.Id)} 
                INNER JOIN inmuebles AS i 
                    ON c.{nameof(Contrato.IdInmueble)} = i.id 
                INNER JOIN tipos_inmueble AS ti 
                    ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                INNER JOIN propietarios AS pr 
                    ON i.{nameof(Inmueble.IdPropietario)} = pr.id 
                INNER JOIN inquilinos AS inq 
                    ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                WHERE p.{nameof(Pago.Id)} = @{nameof(Pago.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Pago.Id)}", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pago = new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            IdUsuarioCobrador = reader.GetInt32(nameof(Pago.IdUsuarioCobrador)),
                            IdUsuarioAnulador = reader[nameof(Pago.IdUsuarioAnulador)] == DBNull.Value ? 0 : reader.GetInt32(nameof(Pago.IdUsuarioAnulador)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            Tipo = reader.GetString("tipoPago"),
                            Detalle = reader.GetString("det"),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(nameof(Pago.IdContrato)),
                                FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                                FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                                FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                                MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdUsuarioContratador = reader.GetInt32("idUContratador"),
                                IdUsuarioTerminador = reader.GetInt32("idUTerminador"),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                    IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Calle = reader.GetString(nameof(Inmueble.Calle)),
                                    NroCalle = reader.GetUInt32(nameof(Inmueble.NroCalle)),
                                    Latitud = reader.GetDecimal("latitud"),
                                    Longitud = reader.GetDecimal("longitud"),
                                    Uso = reader.GetString(nameof(Inmueble.Uso)),
                                    Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                    CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                    Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                        Tipo = reader.GetString("tipoInmueble")
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
                                }
                            },
                            UsuarioCobrador = new Usuario
                            {
                                Id = reader.GetInt32(nameof(Pago.IdUsuarioCobrador)),
                                Nombre = reader["nombreCobrador"] == DBNull.Value ? null : reader.GetString("nombreCobrador"),
                                Apellido = reader["apellidoCobrador"] == DBNull.Value ? null : reader.GetString("apellidoCobrador"),
                                Rol = reader.GetString("rolCobrador")
                            }
                        };
                        if (reader[nameof(Pago.IdUsuarioAnulador)] != DBNull.Value)
                        {
                            pago.UsuarioAnulador = new Usuario
                            {
                                Id = reader.GetInt32(nameof(Pago.IdUsuarioAnulador)),
                                Nombre = reader["nombreAnulador"] == DBNull.Value ? null : reader.GetString("nombreAnulador"),
                                Apellido = reader["apellidoAnulador"] == DBNull.Value ? null : reader.GetString("apellidoAnulador"),
                                Rol = reader.GetString("rolAnulador")
                            };
                        }
                    }
                }
            }
        }

        return pago;
    }
}