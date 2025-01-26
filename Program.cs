using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
    });
});

// Register DbContext with Dependency Injection
builder.Services.AddDbContext<DataContextEF>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository services
builder.Services.AddScoped<interfaceRepastory, UserRepository>();
builder.Services.AddScoped<ICompanyInterFaceRepositry, CompanyRepositry>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarRentRepository, CarRentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IBookTripRepositry, BookTripRepositry>();
builder.Services.AddScoped<IBuyItemRepositry, BuyItemRepositry>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepositry>();

// JWT Bearer Authentication
string? tokenKeyString = builder.Configuration.GetValue<string>("AppSettings:TokenKey");
if (string.IsNullOrEmpty(tokenKeyString))
{
    throw new InvalidOperationException("TokenKey is not set in the configuration.");
}

var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString));
var credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

var tokenValidationParameters = new TokenValidationParameters
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = true,
    ValidateAudience = false
};

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
    });

var app = builder.Build();

// Configure CORS
app.UseCors("DevCors");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
