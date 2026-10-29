var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var apiUrl = builder.Configuration["GestionOrdinateursApiUrl"] ?? "https://localhost:7001";
builder.Services.AddHttpClient("GestionOrdinateursApi", client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowBlazorClient");
app.UseAuthorization();
app.MapControllers();

app.Run();