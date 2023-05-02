using HajurKoCarRental.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var services = new ServiceCollection();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    
});



builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add SeedData class
builder.Services.AddTransient<SeedData>();
builder.Services.AddScoped<EmailHelper>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();


// To Seed users and thier role the database
using var scope = app.Services.CreateScope();
var seedData = scope.ServiceProvider.GetRequiredService<SeedData>();
seedData.InitializeAsync();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("_myAllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
