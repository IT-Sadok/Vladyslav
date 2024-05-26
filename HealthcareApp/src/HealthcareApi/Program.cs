using Application.DTOs.Login;
using Application.DTOs.Register;
using FluentValidation;
using Healthcare.Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserDTOValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LoginUserDTOValidator).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, conf) =>
    conf.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();