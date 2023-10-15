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

var storageSection = builder.Configuration.GetSection(StorageSettings.Name);
builder.Services.Configure<StorageSettings>(storageSection);
var storage = storageSection.Get<StorageSettings>();

switch (storage.Type)
{
  case "file":
    builder.Services.AddSingleton<IContactStorage, FileStorage>();
    break;
  case "sqlite":
    builder.Services.AddSingleton<IContactStorage, SqliteStorage>();
    break;
  case "memory":
    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<IContactStorage, MemoryStorage>();
    break;
  default:
    throw new NotImplementedException("Storage type not implemented");
}

// Add memory caching.


// Configure custom contact
builder.Services.AddSingleton<IReCaptchaApiService, ReCaptchaApiService>();
builder.Services.AddSingleton<IContactService, ContactService>();
builder.Services.AddSingleton<IEmailService, EmailService>();

// Configure options.
var emailSettingsSection = builder.Configuration.GetSection(EmailSettings.Name);
builder.Services.Configure<EmailSettings>(emailSettingsSection);

var apiSettingsSection = builder.Configuration.GetSection(ApiSettings.Name);
builder.Services.Configure<ApiSettings>(apiSettingsSection);

var userSection = builder.Configuration.GetSection(UserData.Name);
builder.Services.Configure<UserData>(userSection);

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
