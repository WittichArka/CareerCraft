using CareerCraft.Core.Services;
using CareerCraft.Infrastructure.Data;
using CareerCraft.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using CareerCraft.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Configuration d'Entity Framework avec SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<ITemplateService, RazorTemplateService>();
builder.Services.AddSingleton<IPdfGenerator, PuppeteerPdfGenerator>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IUserService, UserService>();

// Ajout du CORS pour autoriser le projet Web
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebProject",
        builder => builder.WithOrigins("http://localhost:5283", "https://localhost:7234")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuration Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowWebProject");

// --- Endpoints pour les Skills ---
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

app.MapPost("/api/skills", async (Skill skill, ISkillService skillService) =>
{
    var createdSkill = await skillService.CreateAsync(skill);
    return Results.Created($"/api/skills/{createdSkill.Id}", createdSkill);
})
    .WithName("CreateSkill")
    .WithOpenApi();

app.MapPut("/api/skills/{id}", async (int id, Skill skill, ISkillService skillService) =>
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

// --- Endpoints pour les Users ---
app.MapGet("/api/users", async (IUserService userService) => 
    Results.Ok(await userService.GetAllAsync()))
    .WithName("GetAllUsers")
    .WithOpenApi();

app.MapGet("/api/users/{id}", async (int id, IUserService userService) =>
{
    var user = await userService.GetByIdAsync(id);
    return user != null ? Results.Ok(user) : Results.NotFound();
})
    .WithName("GetUserById")
    .WithOpenApi();

app.MapPost("/api/users", async (User user, IUserService userService) =>
{
    var createdUser = await userService.CreateAsync(user);
    return Results.Created($"/api/users/{createdUser.Id}", createdUser);
})
    .WithName("CreateUser")
    .WithOpenApi();

app.MapPut("/api/users/{id}", async (int id, User user, IUserService userService) =>
{
    if (id != user.Id) return Results.BadRequest();
    await userService.UpdateAsync(user);
    return Results.NoContent();
})
    .WithName("UpdateUser")
    .WithOpenApi();

app.MapDelete("/api/users/{id}", async (int id, IUserService userService) =>
{
    await userService.DeleteAsync(id);
    return Results.NoContent();
})
    .WithName("DeleteUser")
    .WithOpenApi();

// --- Endpoints pour les UserInfos ---
app.MapGet("/api/users/{userId}/infos", async (int userId, IUserService userService) => 
    Results.Ok(await userService.GetInfosByUserIdAsync(userId)))
    .WithName("GetUserInfos")
    .WithOpenApi();

app.MapPost("/api/users/{userId}/infos", async (int userId, UserInfo info, IUserService userService) =>
{
    var createdInfo = await userService.AddInfoAsync(userId, info);
    return Results.Created($"/api/users/{userId}/infos/{createdInfo.Id}", createdInfo);
})
    .WithName("AddUserInfo")
    .WithOpenApi();

app.MapPut("/api/users/{userId}/infos/{id}", async (int userId, int id, UserInfo info, IUserService userService) =>
{
    if (id != info.Id) return Results.BadRequest();
    await userService.UpdateInfoAsync(info);
    return Results.NoContent();
})
    .WithName("UpdateUserInfo")
    .WithOpenApi();

app.MapDelete("/api/users/{userId}/infos/{id}", async (int userId, int id, IUserService userService) =>
{
    await userService.DeleteInfoAsync(id);
    return Results.NoContent();
})
    .WithName("DeleteUserInfo")
    .WithOpenApi();

app.MapPatch("/api/users/{userId}/infos/reorder", async (int userId, List<int> infoIds, IUserService userService) =>
{
    await userService.ReorderInfosAsync(userId, infoIds);
    return Results.NoContent();
})
    .WithName("ReorderUserInfos")
    .WithOpenApi();

app.Run();
