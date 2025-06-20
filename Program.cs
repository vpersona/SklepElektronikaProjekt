using ProjektSklepElektronika.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// swagger:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICartService, CartService>();

var app = builder.Build();

// dodawanie swagger ui do œrodowiska developerskiego
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
