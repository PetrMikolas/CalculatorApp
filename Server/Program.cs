using CalculatorApp.Server.Services;
using CalculatorApp.Server.Services.Database;

var builder = WebApplication.CreateBuilder(args);

// gRPC
builder.Services.AddGrpc();

builder.Services.AddCors(setupAction =>
{
	setupAction.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyHeader()
		.AllowAnyOrigin()
		.AllowAnyMethod()
		.WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
	});
});

builder.Services.AddGrpcReflection();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();

// Logging
builder.Logging.AddLog4Net();

var app = builder.Build();

app.UseCors();
app.UseRouting();
app.UseHttpsRedirection();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.UseEndpoints(endpoints =>
{
	endpoints.MapGrpcService<CalculatorService>().EnableGrpcWeb();
});

// Maps to gRPC reflexy service (command: grpcui localhost:5200)
app.MapGrpcReflectionService();

// Runs application
app.Run();