using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Repositories;
using InmobiliariaGutierrezManuel.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InmobiliariaGutierrezManuel.Models.ViewModels;

namespace InmobiliariaGutierrezManuel.Controllers;

public class UsuarioController : Controller
{
    private readonly IUsuarioRepository repo;
    private readonly IConfiguration config;
    private readonly IWebHostEnvironment env;
    public UsuarioController(IUsuarioRepository repo, IConfiguration config, IWebHostEnvironment env)
    {
        this.repo = repo;
        this.config = config;
        this.env = env;
    }

    [Authorize(Policy = "ADMIN")]
    public IActionResult Index(int offset = 1, int limit = 10)
    {
        IList<Usuario> usuarios = repo.ListarUsuarios(offset, limit);
        int cantidadUsuarios = repo.ContarUsuarios();

        ViewBag.cantPag = Math.Ceiling((decimal)cantidadUsuarios / limit);
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;
        ViewBag.usuarios = usuarios;
        ViewBag.mensajeError = "";

        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        ViewBag.mensajeError = TempData["MensajeError"];
        var loginViewModel = new LoginViewModel();
        string? email = TempData["Mail"] as string;
        if (!string.IsNullOrWhiteSpace(email))
            loginViewModel.Email = email;
        return View(loginViewModel);
    }

    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel login)
    {
        try
        {
            // var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
            if (ModelState.IsValid)
            {
                string hashed = HashearPassword(login.Password!);

                Usuario? user = repo.ObtenerPorEmail(login.Email!);
                if (user == null || user.Password != hashed)
                {
                    TempData["MensajeError"] = "El email o la clave son correctos incorrecto(s)";
                    TempData["Mail"] = login.Email;
                    // TempData["returnUrl"] = returnUrl;
                    return RedirectToAction("Login");
                    // return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nombre + " " + user.Apellido),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Uri, user.Avatar ?? ""),
                    new Claim(ClaimTypes.Role, user.Rol!),
                    new Claim("id", user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                // TempData.Remove("returnUrl");
                // return Redirect(returnUrl);
                if (User.IsInRole(Rol.ADMIN.ToString())) return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                TempData["MensajeError"] = "El email o la clave no son correctos";
                TempData["ViewModel"] = login;
                return View(nameof(Login));
            }
            // TempData["returnUrl"] = returnUrl;
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Authorize(Policy = "ADMIN")]
    public IActionResult Guardar(Usuario usuario)
    {
        if (!ModelState.IsValid)
        {
            string errorMsg = "";
            foreach (var estado in ModelState)
            {
                var campo = estado.Key;
                foreach (var error in estado.Value.Errors)
                {
                    errorMsg += $" - {error.ErrorMessage}";
                }
            }
            TempData["MensajeError"] = errorMsg;
            return RedirectToAction(nameof(Index));
        }
        try
        {
            string hashed = HashearPassword(usuario.Password!);
            usuario.Password = hashed;
            //var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
            repo.InsertarUsuario(usuario);
            if (usuario.AvatarFile != null && usuario.Id > 0)
            {
                GuardarAvatarDelUsuario(usuario);
                repo.ActualizarUsuario(usuario);
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            TempData["MensajeError"] = "No se pudo crear el usuario";
            return RedirectToAction(nameof(FormularioUsuario));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ActualizarDatos(Usuario usuario)
    {
        Usuario? usuarioDB = repo.ObtenerUsuario(usuario.Id);
        usuarioDB!.Nombre = usuario.Nombre;
        usuarioDB!.Apellido = usuario.Apellido;
        usuarioDB!.Rol = usuario.Rol;
        repo.ActualizarUsuario(usuarioDB);
        //actualizo la claim que tiene el nombre y el apelldio del usuario
        if (User.Claims.FirstOrDefault(c => c.Type == "id")?.Value == usuario.Id.ToString())
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimNombreApellido = identity.FindFirst(ClaimTypes.Name);
                if (claimNombreApellido != null)
                {
                    identity.RemoveClaim(claimNombreApellido);
                    var newClaim = new Claim(ClaimTypes.Name, usuarioDB.Nombre + " " + usuarioDB.Apellido);
                    identity.AddClaim(newClaim);
                }
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            }
        }

        if (User.IsInRole(Rol.ADMIN.ToString())) return RedirectToAction(nameof(Index));
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CambiarContrasenia(CambiarContraseniaViewModel vm)
    {
        Usuario? usuarioDB = repo.ObtenerUsuario(vm.Id);

        if (vm.PasswordNuevo != null)
        {
            if (vm.PasswordViejo != null && usuarioDB!.Password != HashearPassword(vm.PasswordViejo))
            {
                TempData["MensajeError"] = "La contraseña vieja es incorrecta";
                return RedirectToAction(nameof(FormularioEditarUsuario), new { id = vm.Id });
            }
            usuarioDB!.Password = HashearPassword(vm.PasswordNuevo!);
        }

        repo.ActualizarUsuario(usuarioDB!);

        if (User.Claims.FirstOrDefault(c => c.Type == "id")?.Value == vm.Id.ToString() || !User.IsInRole(Rol.ADMIN.ToString()))
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (User.IsInRole(Rol.ADMIN.ToString()))
            return RedirectToAction(nameof(Index));

        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ActualizarAvatar(Usuario usuario)
    {
        Usuario? usuarioDB = repo.ObtenerUsuario(usuario.Id)!;
        BorrarAvatar(usuario.Id, usuarioDB.Avatar ?? "");
        GuardarAvatarDelUsuario(usuario);
        usuarioDB.Avatar = usuario.Avatar;
        repo.ActualizarUsuario(usuarioDB);
        //actualizo la claim del email del usuario
        var identity = User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var claimAvatar = identity.FindFirst(ClaimTypes.Uri);
            if (claimAvatar != null)
            {
                identity.RemoveClaim(claimAvatar);
                var newClaim = new Claim(ClaimTypes.Uri, usuarioDB.Avatar!);
                identity.AddClaim(newClaim);
            }
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

        if (User.IsInRole(Rol.ADMIN.ToString())) return RedirectToAction(nameof(Index));
        return RedirectToAction(nameof(Index), "Home");
    }

    [Authorize]
    public IActionResult EliminarAvatar(int id, string avatar)
    {
        try
        {
            BorrarAvatar(id, avatar);
            Usuario? usuarioDB = repo.ObtenerUsuario(id);
            usuarioDB!.Avatar = null;
            repo.ActualizarUsuario(usuarioDB);
        }
        catch
        {
            TempData["MensajeError"] = "No se pudo eliminar el avatar";
        }
        if (User.IsInRole(Rol.ADMIN.ToString())) return RedirectToAction(nameof(Index));
        return RedirectToAction(nameof(Index), "Home");
    }

    // public IActionResult ActualizarPassword()
    // {
    //     return View();
    // }

    [Authorize(Policy = "ADMIN")]
    public IActionResult FormularioUsuario(int id = 0)
    {
        Usuario? usuario;

        if (id > 0)
        {
            usuario = repo.ObtenerUsuario(id);
        }
        else
        {
            usuario = new Usuario();
        }

        ViewBag.mensajeError = TempData["MensajeError"];

        return View(usuario);
    }

    [Authorize]
    public IActionResult FormularioEditarUsuario(int id)
    {
        if (!User.IsInRole(Rol.ADMIN.ToString()) && User.Claims.FirstOrDefault(c => c.Type == "id")?.Value != id.ToString())
            return RedirectToAction(nameof(Index), "Home");

        Usuario? usuario = repo.ObtenerUsuario(id);
        ViewBag.mensajeError = TempData["MensajeError"];
        return View(usuario);
    }

    [Authorize(Policy = "ADMIN")]
    public IActionResult Eliminar(int id)
    {
        repo.EliminarUsuario(id);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private string HashearPassword(string password)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]!),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 1000,
            numBytesRequested: 256 / 8))
        ;
    }

    private void GuardarAvatarDelUsuario(Usuario usuario)
    {
        string wwwPath = this.env.WebRootPath;
        string path = Path.Combine(wwwPath, "Uploads");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile!.FileName);
        string pathCompleto = Path.Combine(path, fileName);
        usuario.Avatar = Path.Combine("/Uploads", fileName);

        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
        {
            usuario.AvatarFile.CopyTo(stream);
        }
    }

    private void BorrarAvatar(int id, string avatar)
    {
        var ruta = Path.Combine(env.WebRootPath, "Uploads", $"avatar_{id}" + Path.GetExtension(avatar));
        if (System.IO.File.Exists(ruta))
            System.IO.File.Delete(ruta);
    }
}
