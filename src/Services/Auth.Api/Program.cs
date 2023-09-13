using Auth.Api.Application.Commands.RegisterUser;
using Auth.Api.Data;
using Auth.Api.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Shared.Kafka.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddKafkaMessageBus();

builder.Services.AddKafkaProducer<string, User>(p =>
{
    p.Topic = "users";
    p.BootstrapServers = builder.Configuration["KafkaConfiguration:BootstrapServers"];
});

builder.Services.AddControllers();
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

DbInitializer.Initialize(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();