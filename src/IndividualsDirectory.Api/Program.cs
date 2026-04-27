using FluentValidation;
using IndividualsDirectory.Api.Middleware;
using IndividualsDirectory.Data;
using IndividualsDirectory.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddLocalization();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services
    .AddDataLayer()
    .AddServiceLayer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LocalizationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
