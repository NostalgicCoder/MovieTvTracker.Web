using Microsoft.EntityFrameworkCore;
using MovieTvTracker.Web.Data;
using TmdbApi.Lib;
using TmdbApi.Lib.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Register with dependancy injection container
builder.Services.AddSingleton<ITmdb>(x => new Tmdb("YOUR_TMDB_API_READACCESS_TOKEN_GOES_HERE"));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movie}/{action=MoviesNowPlaying}/{id?}");

app.Run();
