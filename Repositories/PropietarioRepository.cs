using System.Data;
using MySql.Data.MySqlClient;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Interfaces;

namespace InmobiliariaGutierrezManuel.Repositories;

public class PropietarioRepository : BaseRepository, IPropietarioRepository
{
    // readonly string connectionString = "server=127.0.0.1;uid=root;pwd=root;database=inmobiliaria";
    private readonly string[] campos = ["Id", "Nombre", "Apellido", "Dni", "Telefono", "Email"];

    public PropietarioRepository() : base()
    {

    }

    public IList<Propietario> ListarPropietarios(
        string? nomApe = null,
        string? orderBy = null,
        string? order = "ASC",
        int? offset = 1,
        int? limit = 10
    )
    {
        var propietarios = new List<Propietario>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(Propietario.Id)}, 
                    {nameof(Propietario.Nombre)}, 
                    {nameof(Propietario.Apellido)}, 
                    {nameof(Propietario.Dni)}, 
                    {nameof(Propietario.Telefono)}, 
                    {nameof(Propietario.Email)} 
                FROM propietarios 
                WHERE {nameof(Propietario.Activo)} = 1"
            ;

            if (!string.IsNullOrWhiteSpace(nomApe))
                sql += $" AND ({nameof(Propietario.Nombre)} LIKE @nomApe OR {nameof(Propietario.Apellido)} LIKE @nomApe)";
            if (!string.IsNullOrWhiteSpace(orderBy) && campos.Contains(orderBy, StringComparer.OrdinalIgnoreCase))
                sql += $" ORDER BY @orderBy {order}";
            if (offset.HasValue && limit.HasValue)
                sql += $" LIMIT @limit OFFSET @offset";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (!string.IsNullOrWhiteSpace(nomApe)) command.Parameters.AddWithValue($"{nameof(Propietario.Nombre)}", nomApe);
                if (!string.IsNullOrWhiteSpace(orderBy) && campos.Contains(orderBy, StringComparer.OrdinalIgnoreCase)) command.Parameters.AddWithValue($"orderBy", orderBy);
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
                        propietarios.Add(new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Email = reader.GetString(nameof(Propietario.Email))
                        });
                    }
                }
            }
        }

        return propietarios;
    }

    public Propietario? ObtenerPropietario(int? id, string? dni)
    {
        if (!id.HasValue) id = 0;
        Propietario? propietario = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(Propietario.Id)}, 
                    {nameof(Propietario.Nombre)}, 
                    {nameof(Propietario.Apellido)}, 
                    {nameof(Propietario.Dni)}, 
                    {nameof(Propietario.Telefono)}, 
                    {nameof(Propietario.Email)} 
                FROM propietarios 
                WHERE {nameof(Propietario.Activo)} = 1"
            ;

            if (id > 0 && string.IsNullOrWhiteSpace(dni)) sql += $" AND {nameof(Propietario.Id)} = @{nameof(Propietario.Id)}";
            if (!string.IsNullOrWhiteSpace(dni) && id <= 0) sql += $" AND {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)}";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (id > 0 && string.IsNullOrWhiteSpace(dni)) command.Parameters.AddWithValue($"{nameof(Propietario.Id)}", id);
                if (!string.IsNullOrWhiteSpace(dni) && id <= 0) command.Parameters.AddWithValue($"{nameof(Propietario.Dni)}", dni);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        propietario = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                        };
                    }
                }
            }
        }

        return propietario;
    }

    public int InsertarPropietario(Propietario propietario)
    {
        int id = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                INSERT INTO propietarios 
                (
                    {nameof(Propietario.Nombre)}, 
                    {nameof(Propietario.Apellido)}, 
                    {nameof(Propietario.Dni)}, 
                    {nameof(Propietario.Telefono)}, 
                    {nameof(Propietario.Email)}
                )
                VALUES 
                (
                    @{nameof(Propietario.Nombre)}, 
                    @{nameof(Propietario.Apellido)}, 
                    @{nameof(Propietario.Dni)}, 
                    @{nameof(Propietario.Telefono)}, 
                    @{nameof(Propietario.Email)}
                );
                
                SELECT LAST_INSERT_ID();"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Propietario.Nombre)}", propietario.Nombre);
                command.Parameters.AddWithValue($"{nameof(Propietario.Apellido)}", propietario.Apellido);
                command.Parameters.AddWithValue($"{nameof(Propietario.Dni)}", propietario.Dni);
                command.Parameters.AddWithValue($"{nameof(Propietario.Telefono)}", propietario.Telefono);
                command.Parameters.AddWithValue($"{nameof(Propietario.Email)}", propietario.Email);

                try
                {
                    connection.Open();

                    id = Convert.ToInt32(command.ExecuteScalar());
                    propietario.Id = id;
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

    public bool ActualizarPropietario(Propietario propietario)
    {
        bool modificado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE propietarios 
                SET 
                {nameof(Propietario.Nombre)} = @{nameof(Propietario.Nombre)},
                {nameof(Propietario.Apellido)} = @{nameof(Propietario.Apellido)},
                {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)},
                {nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)},
                {nameof(Propietario.Email)} = @{nameof(Propietario.Email)}
                WHERE {nameof(Propietario.Id)} = @{nameof(Propietario.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Propietario.Nombre)}", propietario.Nombre);
                command.Parameters.AddWithValue($"{nameof(Propietario.Apellido)}", propietario.Apellido);
                command.Parameters.AddWithValue($"{nameof(Propietario.Dni)}", propietario.Dni);
                command.Parameters.AddWithValue($"{nameof(Propietario.Telefono)}", propietario.Telefono);
                command.Parameters.AddWithValue($"{nameof(Propietario.Email)}", propietario.Email);
                command.Parameters.AddWithValue($"{nameof(Propietario.Id)}", propietario.Id);

                connection.Open();

                modificado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return modificado;
    }


    public bool EliminarPropietario(int id)
    {
        bool borrado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE propietarios 
                SET {nameof(Propietario.Activo)} = @{nameof(Propietario.Activo)} 
                WHERE {nameof(Propietario.Id)} = @{nameof(Propietario.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Propietario.Activo)}", 0);
                command.Parameters.AddWithValue($"{nameof(Propietario.Id)}", id);

                connection.Open();

                borrado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return borrado;
    }

    public int ContarPropietarios()
    {
        int cantidadPropietarios = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT COUNT({nameof(Propietario.Id)}) AS cantidad 
                FROM propietarios 
                WHERE {nameof(Propietario.Activo)} = 1;"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {

                connection.Open();

                cantidadPropietarios = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
        }

        return cantidadPropietarios;
    }

    public bool BuscarPorEmail(string? email)
    {
        if (string.IsNullOrEmpty(email)) email = "";

        string sql = @$"
            SELECT COUNT({nameof(Propietario.Email)}) AS c 
            FROM propietarios 
            WHERE {nameof(Propietario.Email)} = @{nameof(Propietario.Email)};"
        ;

        return VerificarExistencia(sql, nameof(Propietario.Email), email);
    }

    public bool BuscarPorTelefono(string? telefono)
    {
        if (string.IsNullOrEmpty(telefono)) telefono = "";

        string sql = @$"
            SELECT COUNT({nameof(Propietario.Telefono)}) AS c 
            FROM propietarios 
            WHERE {nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)};"
        ;

        return VerificarExistencia(sql, nameof(Propietario.Telefono), telefono);
    }

    private bool VerificarExistencia(string sql, string campo, string valor)
    {
        bool esta = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{campo}", valor);

                connection.Open();

                esta = Convert.ToInt32(command.ExecuteScalar()) > 0;

                connection.Close();
            }
        }

        return esta;
    }

    public bool Verificar(string dni, string email, string telefono)
    {
        bool hay = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT COUNT(*) AS c 
                FROM propietarios 
                WHERE {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)}
                    OR {nameof(Propietario.Email)} = @{nameof(Propietario.Email)}
                    OR {nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Propietario.Dni)}", dni);
                command.Parameters.AddWithValue($"{nameof(Propietario.Email)}", email);
                command.Parameters.AddWithValue($"{nameof(Propietario.Telefono)}", telefono);

                connection.Open();

                hay = Convert.ToInt32(command.ExecuteScalar()) > 0;

                connection.Close();
            }
        }

        return hay;
    }
}