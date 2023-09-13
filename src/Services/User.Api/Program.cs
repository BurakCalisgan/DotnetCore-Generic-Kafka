using Microsoft.EntityFrameworkCore;
using Shared.Kafka.DependencyInjection;
using User.Api.Application.Queries.GetUsers;
using User.Api.Data;
using User.Api.Services.Kafka.Handlers;
using User.Api.Services.Kafka.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetUsersQueryHandler).Assembly));

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();

builder.Services.AddKafkaConsumer<string, UserMessage, UserCreatedHandler>(p =>
{
    p.Topic = "users";
    p.GroupId = "users_group";
    p.BootstrapServers = builder.Configuration["KafkaConfiguration:BootstrapServers"];
});

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