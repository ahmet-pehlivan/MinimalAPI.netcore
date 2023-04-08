using Microsoft.EntityFrameworkCore;
using MinimalAPI.netcore.Data;
using MinimalAPI.netcore.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DbContextClass>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
//get the list of product "urun listesini getir"
app.MapGet("/productlist", async (DbContextClass dbContext) =>
{
    var products = await dbContext.Product.ToListAsync();
    if (products == null)
    {
        return Results.NoContent();
    }
    return Results.Ok(products);
});
//get product by id "id'ye gore urun getir"
app.MapGet("/getproductbyid", async (int id, DbContextClass dbContext) =>
{
    var product = await dbContext.Product.FindAsync(id);
    if (product == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);
});
//create a new product
app.MapPost("/createproduct", async (Product product, DbContextClass dbContext) =>
{
    var result = dbContext.Product.Add(product);
    await dbContext.SaveChangesAsync();
    return Results.Ok(result.Entity);
});
//update the product "Urunu guncelle"
app.MapPut("/updateproduct", async (Product product, DbContextClass dbContext) =>
{
    var productDetail = await dbContext.Product.FindAsync(product.ProductId);
    if (product == null)
    {
        return Results.NotFound();
    }
    productDetail.ProductName = product.ProductName;
    productDetail.ProductDescription = product.ProductDescription;
    productDetail.ProductPrice = product.ProductPrice;
    productDetail.ProductStock = product.ProductStock;
    await dbContext.SaveChangesAsync();
    return Results.Ok(productDetail);
});
//delete the product by id "urunu sil"
app.MapDelete("/deleteproduct/{id}", async (int id, DbContextClass dbContext) =>
{
    var product = await dbContext.Product.FindAsync(id);
    if (product == null)
    {
        return Results.NoContent();
    }
    dbContext.Product.Remove(product);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});
app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}