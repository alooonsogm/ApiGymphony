using ApiGymphony.Data;
using ApiGymphony.Helpers;
using ApiGymphony.Repositories;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

string azureKeys = builder.Configuration.GetValue<string>("AzureKeys:StoredAccount");
BlobServiceClient blobServiceClient = new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HelperUsuarioToken>();
HelperCifrado.Initialize(builder.Configuration);
HelperActionOAuthService helper = new HelperActionOAuthService(builder.Configuration);
builder.Services.AddSingleton<HelperActionOAuthService>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticationSchema()).AddJwtBearer(helper.GetJWtBearerOptions());

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
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
