using CareerCraft.Core.Services;
using CareerCraft.Infrastructure.Data;
using CareerCraft.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration d'Entity Framework avec SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<ITemplateService, RazorTemplateService>();
builder.Services.AddSingleton<IPdfGenerator, PuppeteerPdfGenerator>();
builder.Services.AddScoped<ISkillService, SkillService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Endpoints pour les Skills
app.MapGet("/api/skills", async (ISkillService skillService) => 
    Results.Ok(await skillService.GetAllAsync()))
    .WithName("GetAllSkills")
    .WithOpenApi();

app.MapGet("/api/skills/{id}", async (int id, ISkillService skillService) =>
{
    var skill = await skillService.GetByIdAsync(id);
    return skill != null ? Results.Ok(skill) : Results.NotFound();
})
    .WithName("GetSkillById")
    .WithOpenApi();

app.MapPost("/api/skills", async (CareerCraft.Core.Entities.Skill skill, ISkillService skillService) =>
{
    var createdSkill = await skillService.CreateAsync(skill);
    return Results.Created($"/api/skills/{createdSkill.Id}", createdSkill);
})
    .WithName("CreateSkill")
    .WithOpenApi();

app.MapPut("/api/skills/{id}", async (int id, CareerCraft.Core.Entities.Skill skill, ISkillService skillService) =>
{
    if (id != skill.Id) return Results.BadRequest();
    await skillService.UpdateAsync(skill);
    return Results.NoContent();
})
    .WithName("UpdateSkill")
    .WithOpenApi();

app.MapDelete("/api/skills/{id}", async (int id, ISkillService skillService) =>
{
    await skillService.DeleteAsync(id);
    return Results.NoContent();
})
    .WithName("DeleteSkill")
    .WithOpenApi();

// Abstractions métier
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
