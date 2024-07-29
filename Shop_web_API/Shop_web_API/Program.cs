


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shop_web_API.Data;
using Shop_web_API.Models;
using Shop_web_API.Profiles;
using Shop_web_API.Repositories.GenericRepository;
using Shop_web_API.Repositories.ProductRepository;
using Shop_web_API.Service.OrderService;
using Shop_web_API.Service.ProductService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddAutoMapper(typeof(OrderProfile));

// add injection 
builder.Services.AddScoped<IGenericRepository<Order>, GenericRepository<Order>>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtIssuer"],
        ValidAudience = builder.Configuration["JwtIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
    };
});





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop Web API", Version = "v1" });

    // Define the BearerAuth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// to add 2 type of roles if not exsistes 
SeedData(app);

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

//app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseRouting();

// Use CORS middleware
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



static void SeedData(WebApplication app)
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Create roles
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).Result)
            {
                var result = roleManager.CreateAsync(new IdentityRole(role)).Result;
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}