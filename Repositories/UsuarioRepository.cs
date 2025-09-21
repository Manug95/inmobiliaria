using MySql.Data.MySqlClient;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Interfaces;

namespace InmobiliariaGutierrezManuel.Repositories;

public class UsuarioRepository : BaseRepository, IUsuarioRepository
{
    private readonly string[] campos = ["Id", "Nombre", "Apellido", "Dni", "Telefono", "Email"];

    public UsuarioRepository() : base()
    {
    }

    public IList<Usuario> ListarUsuarios(int? offset, int? limit)
    {
        var usuarios = new List<Usuario>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(Usuario.Id)}, 
                    {nameof(Usuario.Nombre)}, 
                    {nameof(Usuario.Apellido)}, 
                    {nameof(Usuario.Email)},  
                    {nameof(Usuario.Avatar)}, 
                    {nameof(Usuario.Rol)}, 
                    {nameof(Usuario.Activo)} 
                FROM usuarios 
                WHERE {nameof(Usuario.Activo)} = 1"
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
                        usuarios.Add(new Usuario
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader[nameof(Usuario.Nombre)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader[nameof(Usuario.Apellido)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Activo = reader.GetBoolean(nameof(Inquilino.Activo)),
                            Rol = reader.GetString(nameof(Usuario.Rol)),
                            Avatar = reader[nameof(Usuario.Avatar)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Avatar))
                        });
                    }
                }
            }
        }

        return usuarios;
    }

    public Usuario? ObtenerUsuario(int id)
    {
        Usuario? usuario = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(Usuario.Id)}, 
                    {nameof(Usuario.Nombre)}, 
                    {nameof(Usuario.Apellido)}, 
                    pass, 
                    {nameof(Usuario.Rol)}, 
                    {nameof(Usuario.Avatar)}, 
                    {nameof(Usuario.Email)}, 
                    {nameof(Usuario.Activo)} 
                FROM usuarios 
                WHERE {nameof(Usuario.Activo)} = 1 
                    AND {nameof(Usuario.Id)} = @{nameof(Usuario.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Usuario.Id)}", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader[nameof(Usuario.Nombre)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader[nameof(Usuario.Apellido)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Activo = reader.GetBoolean(nameof(Inquilino.Activo)),
                            Rol = reader.GetString(nameof(Usuario.Rol)),
                            Password = reader.GetString("pass"),
                            Avatar = reader[nameof(Usuario.Avatar)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Avatar))
                        };
                    }
                }
            }
        }

        return usuario;
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        Usuario? usuario = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT 
                    {nameof(Usuario.Id)}, 
                    {nameof(Usuario.Nombre)}, 
                    {nameof(Usuario.Apellido)}, 
                    {nameof(Usuario.Avatar)}, 
                    {nameof(Usuario.Rol)}, 
                    pass, 
                    {nameof(Usuario.Email)}, 
                    {nameof(Usuario.Activo)} 
                FROM usuarios 
                WHERE {nameof(Usuario.Activo)} = 1 
                    AND {nameof(Usuario.Email)} = @{nameof(Usuario.Email)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Usuario.Email)}", email);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32(nameof(Usuario.Id)),
                            Nombre = reader[nameof(Usuario.Nombre)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader[nameof(Usuario.Apellido)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Apellido)),
                            Avatar = reader[nameof(Usuario.Avatar)] == DBNull.Value ? "" : reader.GetString(nameof(Usuario.Avatar)),
                            Rol = reader.GetString(nameof(Usuario.Rol)),
                            Password = reader.GetString("pass"),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Activo = reader.GetBoolean(nameof(Usuario.Activo))
                        };
                    }
                }
            }
        }

        return usuario;
    }

    public int InsertarUsuario(Usuario usuario)
    {
        int id = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                INSERT INTO usuarios 
                (
                    {nameof(Usuario.Nombre)}, 
                    {nameof(Usuario.Apellido)}, 
                    pass, 
                    {nameof(Usuario.Avatar)}, 
                    {nameof(Usuario.Email)}, 
                    {nameof(Usuario.Rol)}
                )
                VALUES 
                (
                    @{nameof(Usuario.Nombre)}, 
                    @{nameof(Usuario.Apellido)}, 
                    @{nameof(Usuario.Password)}, 
                    @{nameof(Usuario.Avatar)}, 
                    @{nameof(Usuario.Email)}, 
                    @{nameof(Usuario.Rol)}
                );
                
                SELECT LAST_INSERT_ID();"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Usuario.Nombre)}", usuario.Nombre);
                command.Parameters.AddWithValue($"{nameof(Usuario.Apellido)}", usuario.Apellido);
                command.Parameters.AddWithValue($"{nameof(Usuario.Password)}", usuario.Password);
                command.Parameters.AddWithValue($"{nameof(Usuario.Avatar)}", usuario.Avatar?.Length == 0 ? null : usuario.Avatar);
                command.Parameters.AddWithValue($"{nameof(Usuario.Email)}", usuario.Email);
                command.Parameters.AddWithValue($"{nameof(Usuario.Rol)}", usuario.Rol);

                try
                {
                    connection.Open();

                    id = Convert.ToInt32(command.ExecuteScalar());
                    usuario.Id = id;
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

    public bool ActualizarUsuario(Usuario usuario)
    {
        bool modificado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE usuarios 
                SET 
                {nameof(Usuario.Nombre)} = @{nameof(Usuario.Nombre)},
                {nameof(Usuario.Apellido)} = @{nameof(Usuario.Apellido)},
                {nameof(Usuario.Rol)} = @{nameof(Usuario.Rol)},
                {nameof(Usuario.Email)} = @{nameof(Usuario.Email)}, 
                pass = @{nameof(Usuario.Password)}, 
                {nameof(Usuario.Avatar)} = @{nameof(Usuario.Avatar)} 
                WHERE {nameof(Usuario.Id)} = @{nameof(Usuario.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Usuario.Nombre)}", usuario.Nombre);
                command.Parameters.AddWithValue($"{nameof(Usuario.Apellido)}", usuario.Apellido);
                command.Parameters.AddWithValue($"{nameof(Usuario.Rol)}", usuario.Rol);
                command.Parameters.AddWithValue($"{nameof(Usuario.Email)}", usuario.Email);
                command.Parameters.AddWithValue($"{nameof(Usuario.Password)}", usuario.Password);
                command.Parameters.AddWithValue($"{nameof(Usuario.Avatar)}", usuario.Avatar);
                command.Parameters.AddWithValue($"{nameof(Usuario.Id)}", usuario.Id);

                connection.Open();

                modificado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return modificado;
    }


    public bool EliminarUsuario(int id)
    {
        bool borrado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE usuarios 
                SET {nameof(Usuario.Activo)} = 0 
                WHERE {nameof(Usuario.Id)} = @{nameof(Usuario.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Usuario.Id)}", id);

                connection.Open();

                borrado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return borrado;
    }

    public int ContarUsuarios()
    {
        int cantidadUsuarios = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                SELECT COUNT({nameof(Usuario.Id)}) AS cantidad 
                FROM usuarios 
                WHERE {nameof(Inquilino.Activo)} = 1;"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {

                connection.Open();

                cantidadUsuarios = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
        }

        return cantidadUsuarios;
    }

    public bool ActualizarContraseña(int id, string password)
    {
        bool estaACtualizada = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE usuarios 
                SET 
                pass = @{nameof(Usuario.Password)} 
                WHERE {nameof(Usuario.Id)} = @{nameof(Usuario.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Usuario.Password)}", password);
                command.Parameters.AddWithValue($"{nameof(Usuario.Id)}", id);

                connection.Open();

                estaACtualizada = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return estaACtualizada;
    }
}