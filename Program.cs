using contact.Filters;
using contact.Models.Settings;
using contact.Services;
using contact.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// Configure all services.

builder.Services.AddControllers(options =>
{
  options.Filters.Add<ApiExceptionFilterAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add memory caching.
builder.Services.AddMemoryCache();

// Configure custom contact
builder.Services.AddSingleton<IReCaptchaApiService, ReCaptchaApiService>();
builder.Services.AddSingleton<IContactService, ContactService>();
builder.Services.AddSingleton<IEmailService, EmailService>();


// Configure options.
var emailSettingsSection = builder.Configuration.GetSection("EmailSettings");
builder.Services.Configure<EmailSettings>(emailSettingsSection);

var apiSettingsSection = builder.Configuration.GetSection("ApiSettings");
builder.Services.Configure<ApiSettings>(apiSettingsSection);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseCors(opts =>
  {
      opts.AllowAnyOrigin();
      opts.AllowAnyHeader();
      opts.AllowAnyMethod();
  });
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
