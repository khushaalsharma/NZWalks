using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Middlewares;
using NZWalks.API.Repositories;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Serilog service 
//it is added at top because we want it to start working as soon as APIs are live

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NZwalks_logs.txt", rollingInterval: RollingInterval.Day) //preferred is Day, this will create new logs file Minute in this case
    .MinimumLevel.Warning() //Information method shows Information and Debugs. Apart from this we have Debug and Warnign method as well
    .CreateLogger();

builder.Logging.ClearProviders(); //clears out any providers injected till now
builder.Logging.AddSerilog(logger);
//This code piece here states that builder should use Serilogs to create logs and write them in console showing information as minimum level of data shown


builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//will add options to integrate autentication to swagger
builder.Services.AddSwaggerGen(options =>
{
    //SwaggerDoc method allows to add meta data about the API
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "NZ Walks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, //This states that auth token should be sent in Headers anytime API is called
        Type = SecuritySchemeType.ApiKey, //this ensures that JWT is treated as a Key
        Scheme = JwtBearerDefaults.AuthenticationScheme //this is the default JWT Authentication Scheme - Bearer
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "OAuth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//injecting db context class
builder.Services.AddDbContext<NZWalksDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString"))); //DbContext
//db context for auth
builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

//Repo
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepositiry>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();   

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles)); //Auto Mapper

//Identity Solution
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>() //for role use IdentityRole
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();


//Identity Options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1; //configuartion for password limitations for Auth
});

//JWT addition
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//adding authentication script to the app
app.UseAuthentication();
app.UseAuthorization();

//global exception handling
app.UseMiddleware<ExceptionHandlerMiddleware>();

//new middleware to ensure that URLs for images on this server are available to user on the browser
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images", //this means for URL like https://localhost:1223/Images, it will point to the Images folder's physical location on server

});

app.MapControllers();

app.Run();
