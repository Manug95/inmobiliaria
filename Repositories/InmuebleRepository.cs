using MySql.Data.MySqlClient;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Interfaces;

namespace InmobiliariaGutierrezManuel.Repositories;

public class InmuebleRepository : BaseRepository, IInmuebleRepository
{
    public bool ActualizarInmueble(Inmueble inmueble)
    {
        bool modificado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE inmuebles 
                SET 
                {nameof(Inmueble.IdPropietario)} = @{nameof(Inmueble.IdPropietario)},
                {nameof(Inmueble.IdTipoInmueble)} = @{nameof(Inmueble.IdTipoInmueble)},
                {nameof(Inmueble.Uso)} = @{nameof(Inmueble.Uso)},
                {nameof(Inmueble.CantidadAmbientes)} = @{nameof(Inmueble.CantidadAmbientes)},
                {nameof(Inmueble.Calle)} = @{nameof(Inmueble.Calle)}, 
                {nameof(Inmueble.NroCalle)} = @{nameof(Inmueble.NroCalle)}, 
                {nameof(Inmueble.Precio)} = @{nameof(Inmueble.Precio)}, 
                {nameof(Inmueble.Disponible)} = @{nameof(Inmueble.Disponible)} 
                WHERE {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", inmueble.IdPropietario);
                command.Parameters.AddWithValue($"{nameof(Inmueble.IdTipoInmueble)}", inmueble.IdTipoInmueble);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"{nameof(Inmueble.CantidadAmbientes)}", inmueble.CantidadAmbientes);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Calle)}", inmueble.Calle);
                command.Parameters.AddWithValue($"{nameof(Inmueble.NroCalle)}", inmueble.NroCalle);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Precio)}", inmueble.Precio);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Disponible)}", inmueble.Disponible);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Id)}", inmueble.Id);

                connection.Open();

                modificado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return modificado;
    }

    public int ContarInmuebles()
    {
        int cantidadInmuebles = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT COUNT({nameof(Inmueble.Id)}) AS cantidad 
                FROM inmuebles 
                WHERE {nameof(Inmueble.Borrado)} = 0;"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {

                connection.Open();

                cantidadInmuebles = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
        }

        return cantidadInmuebles;
    }

    public bool EliminarInmueble(int id)
    {
        bool borrado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE inmuebles 
                SET {nameof(Inmueble.Borrado)} = 1, {nameof(Inmueble.Disponible)} = 0 
                WHERE {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Inmueble.Id)}", id);

                connection.Open();

                borrado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return borrado;
    }

    public int InsertarInmueble(Inmueble inmueble)
    {
        int id = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                INSERT INTO inmuebles 
                (
                    {nameof(Inmueble.IdPropietario)}, 
                    {nameof(Inmueble.IdTipoInmueble)}, 
                    {nameof(Inmueble.Uso)}, 
                    {nameof(Inmueble.CantidadAmbientes)}, 
                    {nameof(Inmueble.Calle)}, 
                    {nameof(Inmueble.NroCalle)}, 
                    {nameof(Inmueble.Latitud)}, 
                    {nameof(Inmueble.Longitud)}, 
                    {nameof(Inmueble.Precio)} 
                )
                VALUES 
                (
                    @{nameof(Inmueble.IdPropietario)}, 
                    @{nameof(Inmueble.IdTipoInmueble)}, 
                    @{nameof(Inmueble.Uso)}, 
                    @{nameof(Inmueble.CantidadAmbientes)}, 
                    @{nameof(Inmueble.Calle)}, 
                    @{nameof(Inmueble.NroCalle)}, 
                    @{nameof(Inmueble.Latitud)}, 
                    @{nameof(Inmueble.Longitud)}, 
                    @{nameof(Inmueble.Precio)} 
                );
                
                SELECT LAST_INSERT_ID();"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", inmueble.IdPropietario);
                command.Parameters.AddWithValue($"{nameof(Inmueble.IdTipoInmueble)}", inmueble.IdTipoInmueble);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"{nameof(Inmueble.CantidadAmbientes)}", inmueble.CantidadAmbientes);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Calle)}", inmueble.Calle);
                command.Parameters.AddWithValue($"{nameof(Inmueble.NroCalle)}", inmueble.NroCalle);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Latitud)}", inmueble.Latitud);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Longitud)}", inmueble.Longitud);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Precio)}", inmueble.Precio);

                try
                {
                    connection.Open();

                    id = Convert.ToInt32(command.ExecuteScalar());
                    inmueble.Id = id;
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

    public IList<Inmueble> ListarInmuebles(int? offset = null, int? limit = null)
    {
        var inmuebles = new List<Inmueble>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    i.{nameof(Inmueble.Id)}, 
                    i.{nameof(Inmueble.IdPropietario)}, 
                    i.{nameof(Inmueble.IdTipoInmueble)}, 
                    i.{nameof(Inmueble.Uso)}, 
                    i.{nameof(Inmueble.CantidadAmbientes)}, 
                    i.{nameof(Inmueble.Calle)}, 
                    i.{nameof(Inmueble.NroCalle)}, 
                    i.{nameof(Inmueble.Latitud)}, 
                    i.{nameof(Inmueble.Longitud)}, 
                    i.{nameof(Inmueble.Precio)}, 
                    i.{nameof(Inmueble.Disponible)}, 
                    ti.{nameof(TipoInmueble.Tipo)}, 
                    p.{nameof(Propietario.Nombre)}, 
                    p.{nameof(Propietario.Apellido)}, 
                    p.{nameof(Propietario.Dni)} 
                FROM inmuebles AS i 
                INNER JOIN tipos_inmueble AS ti 
                    ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                INNER JOIN propietarios AS p 
                    ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                WHERE {nameof(Inmueble.Borrado)} = 0"
            ;

            if (offset.HasValue && limit.HasValue)
                sql += $" LIMIT @limit OFFSET @offset";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
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
                        inmuebles.Add(new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                            Calle = reader.GetString(nameof(Inmueble.Calle)),
                            NroCalle = reader.GetUInt32(nameof(Inmueble.NroCalle)),
                            Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni))
                            },
                            Tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                            }
                        });
                    }
                }
            }
        }

        return inmuebles;
    }

    public IList<Inmueble> ListarInmueblesPorPropietario(
        string? nomApePropietario = null,
        string? orderBy = null,
        string? order = "ASC",
        int? offset = null,
        int? limit = null
    )
    {
        var inmuebles = new List<Inmueble>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(Inmueble.Id)}, 
                    {nameof(Inmueble.IdPropietario)}, 
                    {nameof(Inmueble.IdTipoInmueble)}, 
                    {nameof(Inmueble.Uso)}, 
                    {nameof(Inmueble.CantidadAmbientes)}, 
                    {nameof(Inmueble.Calle)}, 
                    {nameof(Inmueble.NroCalle)}, 
                    {nameof(Inmueble.Latitud)}, 
                    {nameof(Inmueble.Longitud)}, 
                    {nameof(Inmueble.Precio)}, 
                    {nameof(Inmueble.Disponible)}, 
                    ti.{nameof(TipoInmueble.Tipo)}, 
                    p.{nameof(Propietario.Nombre)}, 
                    p.{nameof(Propietario.Apellido)}, 
                    p.{nameof(Propietario.Dni)} 
                FROM inmuebles AS i 
                INNER JOIN tipos_inmueble AS ti 
                    ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                INNER JOIN propietarios AS p 
                    ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                WHERE {nameof(Inmueble.Borrado)} = 0"
            ;

            if (!string.IsNullOrWhiteSpace(nomApePropietario))
                sql += $" AND i.({nameof(Propietario.Nombre)} LIKE '@nomApe' OR i.{nameof(Propietario.Apellido)} LIKE '@nomApe')";
            if (!string.IsNullOrWhiteSpace(orderBy))
                sql += $" ORDER BY p.@orderBy {order}";
            if (offset.HasValue && limit.HasValue)
                sql += $" LIMIT @limit OFFSET @offset";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (!string.IsNullOrWhiteSpace(nomApePropietario)) command.Parameters.AddWithValue("nomApe", nomApePropietario);
                if (!string.IsNullOrWhiteSpace(orderBy)) command.Parameters.AddWithValue($"orderBy", orderBy);
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
                        inmuebles.Add(new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                            Calle = reader.GetString(nameof(Inmueble.Calle)),
                            NroCalle = reader.GetUInt32(nameof(Inmueble.NroCalle)),
                            Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni))
                            },
                            Tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                            }
                        });
                    }
                }
            }
        }

        return inmuebles;
    }

    public Inmueble? ObtenerInmueble(int id)
    {
        Inmueble? inmueble = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    i.{nameof(Inmueble.Id)}, 
                    i.{nameof(Inmueble.IdPropietario)}, 
                    i.{nameof(Inmueble.IdTipoInmueble)}, 
                    i.{nameof(Inmueble.Uso)}, 
                    i.{nameof(Inmueble.CantidadAmbientes)}, 
                    i.{nameof(Inmueble.Calle)}, 
                    i.{nameof(Inmueble.NroCalle)}, 
                    i.{nameof(Inmueble.Latitud)}, 
                    i.{nameof(Inmueble.Longitud)}, 
                    i.{nameof(Inmueble.Precio)}, 
                    i.{nameof(Inmueble.Disponible)}, 
                    ti.{nameof(TipoInmueble.Tipo)}, 
                    p.{nameof(Propietario.Nombre)}, 
                    p.{nameof(Propietario.Apellido)}, 
                    p.{nameof(Propietario.Dni)} 
                FROM inmuebles AS i 
                INNER JOIN tipos_inmueble AS ti 
                    ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                INNER JOIN propietarios AS p 
                    ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                WHERE i.{nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)} AND {nameof(Inmueble.Borrado)} = 0;"
            ;

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                command.Parameters.AddWithValue($"{nameof(Inmueble.Id)}", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                            Calle = reader.GetString(nameof(Inmueble.Calle)),
                            NroCalle = reader.GetUInt32(nameof(Inmueble.NroCalle)),
                            Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni))
                            },
                            Tipo = new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                            }
                        };
                    }
                }
            }
        }

        return inmueble;
    }
}