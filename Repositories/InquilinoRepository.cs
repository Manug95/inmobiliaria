using MySql.Data.MySqlClient;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Interfaces;

namespace InmobiliariaGutierrezManuel.Repositories;

public class InquilinoRepository : BaseRepository, IInquilinoRepository
{
    private readonly string[] campos = ["Id", "Nombre", "Apellido", "Dni", "Telefono", "Email"];

    public InquilinoRepository() : base()
    {
    }

    public IList<Inquilino> ListarInquilinos(
        string? nomApe = null,
        string? orderBy = null,
        string? order = "ASC",
        int? offset = 1, //n° de l pagina actual
        int? limit = 10  //cantidad de resultados por pagina
    )
    {
        var inquilinos = new List<Inquilino>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(Inquilino.Id)}, 
                    {nameof(Inquilino.Nombre)}, 
                    {nameof(Inquilino.Apellido)}, 
                    {nameof(Inquilino.Dni)}, 
                    {nameof(Inquilino.Telefono)}, 
                    {nameof(Inquilino.Email)},  
                    {nameof(Inquilino.Activo)}  
                FROM inquilinos 
                WHERE {nameof(Inquilino.Activo)} = 1"
            ;

            if (!string.IsNullOrWhiteSpace(nomApe))
                sql += $" AND ({nameof(Inquilino.Nombre)} LIKE @nomApe OR {nameof(Inquilino.Apellido)} LIKE @nomApe)";
            if (!string.IsNullOrWhiteSpace(orderBy) && campos.Contains(orderBy, StringComparer.OrdinalIgnoreCase))
                sql += $" ORDER BY @orderBy {order}";
            if (offset.HasValue && limit.HasValue)
                sql += $" LIMIT @limit OFFSET @offset";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (!string.IsNullOrWhiteSpace(nomApe)) command.Parameters.AddWithValue($"nomApe", $"%{nomApe}%");
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
                        inquilinos.Add(new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Dni = reader.GetString(nameof(Inquilino.Dni)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Activo = reader.GetBoolean(nameof(Inquilino.Activo))
                        });
                    }
                }
            }
        }

        return inquilinos;
    }

    public Inquilino? ObtenerInquilino(int? id, string? dni)
    {
        if (!id.HasValue) id = 0;
        Inquilino? inquilino = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(Inquilino.Id)}, 
                    {nameof(Inquilino.Nombre)}, 
                    {nameof(Inquilino.Apellido)}, 
                    {nameof(Inquilino.Dni)}, 
                    {nameof(Inquilino.Telefono)}, 
                    {nameof(Inquilino.Email)}, 
                    {nameof(Inquilino.Activo)} 
                FROM inquilinos 
                WHERE {nameof(Inquilino.Activo)} = 1"
            ;

            if (id > 0 && string.IsNullOrWhiteSpace(dni)) sql += $" AND {nameof(Inquilino.Id)} = @{nameof(Inquilino.Id)}";
            if (!string.IsNullOrWhiteSpace(dni) && id <= 0) sql += $" AND {nameof(Inquilino.Dni)} = @{nameof(Inquilino.Dni)}";

            using (var command = new MySqlCommand(sql + ";", connection))
            {
                if (id > 0 && string.IsNullOrWhiteSpace(dni)) command.Parameters.AddWithValue($"{nameof(Inquilino.Id)}", id);
                if (!string.IsNullOrWhiteSpace(dni) && id <= 0) command.Parameters.AddWithValue($"{nameof(Inquilino.Dni)}", dni);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inquilino = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Dni = reader.GetString(nameof(Inquilino.Dni)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Activo = reader.GetBoolean(nameof(Inquilino.Activo))
                        };
                    }
                }
            }
        }

        return inquilino;
    }

    public int InsertarInquilino(Inquilino inquilino)
    {
        int id = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                INSERT INTO inquilinos 
                (
                    {nameof(Inquilino.Nombre)}, 
                    {nameof(Inquilino.Apellido)}, 
                    {nameof(Inquilino.Dni)}, 
                    {nameof(Inquilino.Telefono)}, 
                    {nameof(Inquilino.Email)}
                )
                VALUES 
                (
                    @{nameof(Inquilino.Nombre)}, 
                    @{nameof(Inquilino.Apellido)}, 
                    @{nameof(Inquilino.Dni)}, 
                    @{nameof(Inquilino.Telefono)}, 
                    @{nameof(Inquilino.Email)}
                );
                
                SELECT LAST_INSERT_ID();"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Inquilino.Nombre)}", inquilino.Nombre);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Apellido)}", inquilino.Apellido);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Dni)}", inquilino.Dni);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Telefono)}", inquilino.Telefono);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Email)}", inquilino.Email);

                try
                {
                    connection.Open();

                    id = Convert.ToInt32(command.ExecuteScalar());
                    inquilino.Id = id;
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

    public bool ActualizarInquilino(Inquilino inquilino)
    {
        bool modificado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE inquilinos 
                SET 
                {nameof(Inquilino.Nombre)} = @{nameof(Inquilino.Nombre)},
                {nameof(Inquilino.Apellido)} = @{nameof(Inquilino.Apellido)},
                {nameof(Inquilino.Dni)} = @{nameof(Inquilino.Dni)},
                {nameof(Inquilino.Telefono)} = @{nameof(Inquilino.Telefono)},
                {nameof(Inquilino.Email)} = @{nameof(Inquilino.Email)}
                WHERE {nameof(Inquilino.Id)} = @{nameof(Inquilino.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Inquilino.Nombre)}", inquilino.Nombre);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Apellido)}", inquilino.Apellido);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Dni)}", inquilino.Dni);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Telefono)}", inquilino.Telefono);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Email)}", inquilino.Email);
                command.Parameters.AddWithValue($"{nameof(Inquilino.Id)}", inquilino.Id);

                connection.Open();

                modificado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return modificado;
    }


    public bool EliminarInquilino(int id)
    {
        bool borrado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE inquilinos 
                SET {nameof(Inquilino.Activo)} = 0 
                WHERE {nameof(Inquilino.Id)} = @{nameof(Inquilino.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Inquilino.Id)}", id);

                connection.Open();

                borrado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return borrado;
    }

    public int ContarInquilinos()
    {
        int cantidadInquilinos = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT COUNT({nameof(Inquilino.Id)}) AS cantidad 
                FROM inquilinos 
                WHERE {nameof(Inquilino.Activo)} = 1;"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {

                connection.Open();

                cantidadInquilinos = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
        }

        return cantidadInquilinos;
    }

    public bool BuscarPorEmail(string? email)
    {
        if (string.IsNullOrEmpty(email)) email = "";

        string sql = @$"
            SELECT COUNT({nameof(Inquilino.Email)}) AS c 
            FROM inquilinos 
            WHERE {nameof(Inquilino.Email)} = @{nameof(Inquilino.Email)};"
        ;

        return VerificarExistencia(sql, nameof(Inquilino.Email), email);
    }

    public bool BuscarPorTelefono(string? telefono)
    {
        if (string.IsNullOrEmpty(telefono)) telefono = "";

        string sql = @$"
            SELECT COUNT({nameof(Inquilino.Telefono)}) AS c 
            FROM inquilinos 
            WHERE {nameof(Inquilino.Telefono)} = @{nameof(Inquilino.Telefono)};"
        ;

        return VerificarExistencia(sql, nameof(Inquilino.Telefono), telefono);
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
}