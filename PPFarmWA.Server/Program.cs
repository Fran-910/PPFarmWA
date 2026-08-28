using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos;
using PPFarmWA.Repositorio;
using PPFarmWA.Repositorio.Repositorios;
using PPFramWA.Client;

var builder = WebApplication.CreateBuilder(args);

#region Servicios

string connectionString = builder.Configuration.GetConnectionString("ConnSqlServer")
    ?? throw new InvalidOperationException("No existe la conexión con la base de datos.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IItemRepositorio, ItemRepositorio>();
builder.Services.AddScoped<IRecursoRepositorio, RecursoRepositorio>();
builder.Services.AddScoped<IJugadorRepositorio, JugadorRepositorio>();
builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region MiddleWare

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();

}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


#endregion

app.Run();
