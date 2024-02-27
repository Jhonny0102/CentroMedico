using CentroMedico.Data;
using CentroMedico.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//NECESITAMOS HABILITAR EL SERVICIO DE MEMORIA CACHE 
builder.Services.AddDistributedMemoryCache();
//SESSION PODEMOS CONFIGURARLO CON TIEMPO DE INACTIVIDAD 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

string connectionString = builder.Configuration.GetConnectionString("SqlCentroMedico");

builder.Services.AddTransient<RepositoryCentroMedicoSQL>();

builder.Services.AddDbContext<CentroMedicoContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Indicar la session.
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CentroMedico}/{action=Index}/{id?}");

app.Run();
