using JWT_ALL.Data;
using JWT_ALL.Data.Model;
using JWT_ALL.Helper;
using JWT_ALL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
// Add_Services
builder.Services.AddScoped<IAuth, AuthService>();
// Add ConnectionString

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
//{
//	options.Password.RequiredLength = 6;
//	options.Password.RequireNonAlphanumeric = false;
//	options.Password.RequireDigit = false;
//	options.Password.RequireUppercase = false;
//	options.Password.RequireLowercase = false;
//})
//	.AddEntityFrameworkStores<AppDbContext>()
//	.AddDefaultTokenProviders();

builder.Services.AddDbContext<AppDbContext>(op =>
op
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

              builder. Services.AddAuthentication(options =>
              {
              options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              options.DefaultChallengeScheme =    JwtBearerDefaults.AuthenticationScheme;
              options.DefaultScheme =             JwtBearerDefaults.AuthenticationScheme;
              })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience =builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
					    ClockSkew = TimeSpan.Zero
					};
                });

//builder.Services.AddAuthorization(options =>
//{
//	options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//	//options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(c=>c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.MapControllers();

app.Run();
