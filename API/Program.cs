using Amazon.S3;
using Clips_api.Data;
using Clips_api.Models;
using Clips_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("Env/config.json");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddCors();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<JWTservice>();
builder.Services.AddScoped<IvideoServices, VideoServices>();




// Add AWS S3 service and file storage service
var s3Config= new AmazonS3Config
{
    ServiceURL= builder.Configuration["S3:ServiceUrl"],
    ForcePathStyle = builder.Configuration.GetValue<bool>("S3:ForcePathStyle", true),
    AuthenticationRegion = builder.Configuration["S3:Region"] ?? "us-east-1",
    
    UseHttp=builder.Configuration.GetValue<bool>("S3:UseHttp", false)
    
};
builder.Services.AddSingleton<IAmazonS3>(sp => new AmazonS3Client(
    builder.Configuration["S3:AccessKey"],
    builder.Configuration["S3:SecretKey"],
    
    
    s3Config));
builder.Services.AddScoped<IFileStorageService, S3StorageService>();


builder.Services.AddIdentityCore<User>(o =>
{
    o.Password.RequiredLength = 6;
    o.Password.RequireDigit = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireNonAlphanumeric = false;

    o.SignIn.RequireConfirmedEmail = true;
}).AddRoles<IdentityRole>()
.AddRoleManager<RoleManager<IdentityRole>>()
.AddSignInManager<SignInManager<User>>()
.AddUserManager<UserManager<User>>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateIssuer = true,
            ValidateAudience = false,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(a => a.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
