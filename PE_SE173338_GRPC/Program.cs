using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using PE_SE173338_GRPC.Protos; // Ensure this matches your proto namespace
using PE_SE173338_GRPC.Services;
using Repo; // Ensure this points to your service implementation

var builder = WebApplication.CreateBuilder(args);

// Add gRPC services to the container
builder.Services.AddGrpc();

builder.Services.AddScoped<ISilverJewelryRepo, SilverJewelryRepo>();

// Configure the gRPC client
builder.Services.AddGrpcClient<SilverJewelryService.SilverJewelryServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7046"); // Ensure this is the correct address for your gRPC service
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapGrpcService<SilverJewelryGrpcService>(); // Ensure this points to your service implementation

// Default endpoint for testing if needed
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

// Enable HTTPS Redirection and HSTS for security (optional but recommended)
app.UseHttpsRedirection();
app.UseHsts();

app.Run();