using Microsoft.EntityFrameworkCore;
using MyPortfolio.Authorization;
using MyPortfolio.Business.Mapping;
using MyPortfolio.Business.Services;
using MyPortfolio.Core.Interfaces;
using MyPortfolio.Data;
using MyPortfolio.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<ApiKeyAuthorizeFilter>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextApp",
        policy => policy.WithOrigins("http://localhost:3000") 
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// ==========================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowNextApp");

app.UseAuthorization();

app.MapControllers();

app.Run();