using MySql.Data.MySqlClient;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Interfaces;

namespace InmobiliariaGutierrezManuel.Repositories;

public class TipoInmuebleRepository : BaseRepository, ITipoInmuebleRepository
{

    public TipoInmuebleRepository() : base()
    {
    }

    public IList<TipoInmueble> ListarTiposInmueble()
    {
        var tiposInmuebles = new List<TipoInmueble>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(TipoInmueble.Id)}, 
                    {nameof(TipoInmueble.Tipo)}, 
                    IFNULL({nameof(TipoInmueble.Descripcion)}, 'Sin Descripción') AS {nameof(TipoInmueble.Descripcion)} 
                FROM tipos_inmueble;"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tiposInmuebles.Add(new TipoInmueble
                        {
                            Id = reader.GetInt32(nameof(TipoInmueble.Id)),
                            Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),
                            Descripcion = reader.GetString(nameof(TipoInmueble.Descripcion))
                        });
                    }
                }
            }
        }

        return tiposInmuebles;
    }

    public TipoInmueble? ObtenerTipoInmueble(int id)
    {
        TipoInmueble? tipoInmueble = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(TipoInmueble.Id)}, 
                    {nameof(TipoInmueble.Tipo)}, 
                    IFNULL({nameof(TipoInmueble.Descripcion)}, 'Sin Descripción') AS {nameof(TipoInmueble.Descripcion)}
                FROM tipos_inmueble 
                WHERE {nameof(TipoInmueble.Id)} = {id}"
            ;

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tipoInmueble = new TipoInmueble
                        {
                            Id = reader.GetInt32(nameof(TipoInmueble.Id)),
                            Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),
                            Descripcion = reader.GetString(nameof(TipoInmueble.Descripcion))
                        };
                    }
                }
            }
        }

        return tipoInmueble;
    }

    public int InsertarTipoInmueble(TipoInmueble tipoInmueble)
    {
        int id = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                INSERT INTO tipos_inmueble 
                ({nameof(TipoInmueble.Tipo)}, {nameof(TipoInmueble.Descripcion)}) 
                VALUES 
                (@{nameof(TipoInmueble.Tipo)}, @{nameof(TipoInmueble.Descripcion)}); 
                SELECT LAST_INSERT_ID();"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(TipoInmueble.Tipo)}", tipoInmueble.Tipo?.ToUpper());
                command.Parameters.AddWithValue($"{nameof(TipoInmueble.Descripcion)}", tipoInmueble.Descripcion);

                try
                {
                    connection.Open();

                    id = Convert.ToInt32(command.ExecuteScalar());
                    tipoInmueble.Id = id;
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

    public bool ActualizarTipoInmueble(TipoInmueble tipoInmueble)
    {
        bool modificado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE tipos_inmueble 
                SET 
                {nameof(TipoInmueble.Tipo)} = @{nameof(TipoInmueble.Tipo)}, 
                {nameof(TipoInmueble.Descripcion)} = @{nameof(TipoInmueble.Descripcion)}
                WHERE {nameof(TipoInmueble.Id)} = @{nameof(TipoInmueble.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(TipoInmueble.Tipo)}", tipoInmueble.Tipo);
                command.Parameters.AddWithValue($"{nameof(TipoInmueble.Descripcion)}", tipoInmueble.Descripcion);
                command.Parameters.AddWithValue($"{nameof(TipoInmueble.Id)}", tipoInmueble.Id);

                connection.Open();

                modificado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return modificado;
    }


    public bool EliminarTipoInmueble(int id)
    {
        bool borrado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                DELETE FROM tipos_inmueble 
                WHERE {nameof(TipoInmueble.Id)} = @{nameof(TipoInmueble.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(TipoInmueble.Id)}", id);

                connection.Open();

                borrado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return borrado;
    }
}