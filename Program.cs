using ApiGymphony.Data;
using ApiGymphony.Helpers;
using ApiGymphony.Repositories;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();

KeyVaultSecret secretsql = await secretClient.GetSecretAsync("secretogymphonysql");
string connectionString = secretsql.Value;

KeyVaultSecret secretstorage = await secretClient.GetSecretAsync("secretogymphonystorage");
string azureKeys = secretstorage.Value;

KeyVaultSecret secretCifrado = await secretClient.GetSecretAsync("secretogymphonycifrado");
string llaveCifrado = secretCifrado.Value;

KeyVaultSecret secretIssuer = await secretClient.GetSecretAsync("secretogymphonyissuer");
KeyVaultSecret secretAudience = await secretClient.GetSecretAsync("secretogymphonyaudience");
KeyVaultSecret secretKeyOAuth = await secretClient.GetSecretAsync("secretogymphonysecretkey");
string issuer = secretIssuer.Value;
string audience = secretAudience.Value;
string secretKey = secretKeyOAuth.Value;

BlobServiceClient blobServiceClient = new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HelperUsuarioToken>();
HelperCifrado.Initialize(llaveCifrado);
HelperActionOAuthService helper = new HelperActionOAuthService(issuer, audience, secretKey);
builder.Services.AddSingleton<HelperActionOAuthService>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticationSchema()).AddJwtBearer(helper.GetJWtBearerOptions());

// Add services to the container.
builder.Services.AddTransient<RepositoryGymphony>();
builder.Services.AddDbContext<GymphonyContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
