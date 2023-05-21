using contact.Filters;
using contact.Services;
using contact.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// Add contact to the container.

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
