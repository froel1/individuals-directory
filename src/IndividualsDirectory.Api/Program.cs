using FluentValidation;
using IndividualsDirectory.Api.Middleware;
using IndividualsDirectory.Data;
using IndividualsDirectory.Service;
using IndividualsDirectory.Service.Images;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.Filters.Add<IndividualsDirectory.Api.Filters.ValidationFilter>(); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.Configure<ImageStorageOptions>(builder.Configuration.GetSection("ImageStorage"));

builder.Services
    .AddDataLayer(builder.Configuration.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Connection string 'Default' not configured."))
    .AddServiceLayer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LocalizationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();