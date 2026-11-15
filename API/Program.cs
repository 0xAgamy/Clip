using Amazon.S3;
using Clips_api.Data;
using Clips_api.Models;
using Clips_api.Options;
using Clips_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
builder.Services.AddScoped<IvideoServices, VideoServices>();

//JWT
builder.Services.AddScoped<JWTservice>();
builder.Services.AddOptions<JwtOptions>()
	.BindConfiguration(JwtOptions.SectionName)
	.ValidateDataAnnotations()
	.ValidateOnStart();
// S3

builder.Services.AddOptions<S3Options>()
        .BindConfiguration(S3Options.SectionName)
        .ValidateDataAnnotations()
        .ValidateOnStart();

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var s3 = sp.GetRequiredService<IOptions<S3Options>>().Value;
    return new AmazonS3Client(s3.AccessKey, s3.SecretKey, new AmazonS3Config
    {
        ServiceURL = s3.ServiceUrl,
        ForcePathStyle = s3.ForcePathStyle,
        AuthenticationRegion = s3.Region ?? "us-east-1",
        UseHttp = s3.UseHttp
    });
});
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
        var jwt = builder.Configuration
            .GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;

        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.Key)),
            ValidIssuer   = jwt.Issuer,
            ValidAudience = jwt.Audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.FromMinutes(1),
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


var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
app.UseCors(a => a.AllowAnyHeader().AllowAnyMethod().WithOrigins(origins));

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
