using App.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Servisleri konteynýra ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ayarlarý
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Herhangi bir kaynaða izin ver
              .AllowAnyMethod() // Herhangi bir HTTP metoduna izin ver
              .AllowAnyHeader(); // Herhangi bir baþlýða izin ver
    });
});

// Veritabaný baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(connectionString); // SQL Server kullanýmý
});

var app = builder.Build();

// Veritabanýný oluþtur (eðer varsa dokunma, yoksa oluþtur)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Eðer veritabaný zaten varsa iþlem yapma, yoksa oluþtur
    await dbContext.Database.EnsureCreatedAsync();

    // Eðer öðrenciler zaten varsa veri ekleme, yoksa 20 sahte veri ekle
    if (!dbContext.Students.Any())
    {
        BogusFakeData.SeedStudents(dbContext); // Sahte veri ekleme
    }
}

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger dokümantasyonu
    app.UseSwaggerUI(); // Swagger kullanýcý arayüzü
}
app.UseCors(); // CORS middleware
app.UseHttpsRedirection(); // HTTPS yönlendirmesi
app.UseAuthorization(); // Yetkilendirme middleware
app.MapControllers(); // Controller'larý haritalama

app.Run(); // Uygulamayý çalýþtýr
