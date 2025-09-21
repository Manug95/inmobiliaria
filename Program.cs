using System.Security.Claims;
using InmobiliariaGutierrezManuel.Interfaces;
using InmobiliariaGutierrezManuel.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Home";
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);//Tiempo de expiración
    })
;
builder.Services.AddAuthorization(options =>
{
	//options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
	// options.AddPolicy("ADMIN", policy => policy.RequireRole("Administrador", "SuperAdministrador"));
	options.AddPolicy("ADMIN", policy => policy.RequireClaim(ClaimTypes.Role, "ADMIN"));
});

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPropietarioRepository, PropietarioRepository>();
builder.Services.AddScoped<IInquilinoRepository, InquilinoRepository>();
builder.Services.AddScoped<ITipoInmuebleRepository, TipoInmuebleRepository>();
builder.Services.AddScoped<IInmuebleRepository, InmuebleRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
