using Course4Me.Logic;
using Course4Me_ServerSide.Config;
using Course4Me_ServerSide.Data;
using Course4Me_ServerSide.InnerUtils;
using Course4Me_ServerSide.Interfaces;
using Course4Me_ServerSide.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Dev")));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICourseRepository, CourseRepository>();
builder.Services.AddTransient<IUtils, Utils>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<IVideoRepository, VideoRepository>();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddTransient<IJwtAuthenticationManager>(s => new JwtAuthenticationManager(builder.Configuration["Jwt:Key"]));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

});



builder.Services.AddCors(options => options.AddDefaultPolicy(x =>
{
    x.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
    
}));
var app = builder.Build();

app.UseCors();
app.UseAuthentication();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();

app.UseHttpsRedirection();


app.MapControllers();

ConfigActions.Run(builder,app.Environment);
app.Run();


