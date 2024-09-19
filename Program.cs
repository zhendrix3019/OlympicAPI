using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure HTTPS port explicitly if needed
builder.WebHost.UseUrls("http://localhost:5104;https://localhost:7189");

// Add services to the container.
builder.Services.AddDbContext<MedalsContext>(options =>
    options.UseSqlite("Data Source=medals.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Olympic Medals API V1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at the root
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Olympic Medals API V1");
    });
}

app.UseHttpsRedirection();


app.MapGet("/api/country/{id}", async (int id, MedalsContext context) =>
{
    var country = await context.Countries.FindAsync(id);
    return country is not null ? Results.Ok(country) : Results.NotFound();
});

app.MapGet("/api/country", async (MedalsContext context) =>
{
    var countries = await context.Countries.ToListAsync();
    return Results.Ok(countries);
});

app.MapPost("/api/country", async (Country country, MedalsContext context) =>
{
    context.Countries.Add(country);
    await context.SaveChangesAsync();
    return Results.Created($"/api/country/{country.Id}", country);
});

app.MapDelete("/api/country/{id}", async (int id, MedalsContext context) =>
{
    var country = await context.Countries.FindAsync(id);
    if (country is null) return Results.NotFound();

    context.Countries.Remove(country);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
