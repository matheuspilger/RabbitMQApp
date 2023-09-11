using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQApp.Domain.Services;
using RabbitMQApp.Domain.Services.Interfaces;
using RabbitMQApp.Domain.Settings;
using RabbitMQApp.Domain.Tracings;
using RabbitMQApp.Domain.Workers;

var builder = WebApplication.CreateBuilder(args);

var jaegerSettings = builder.Configuration.GetSection(nameof(JaegerSettings)).Get<JaegerSettings>()!;
builder.Services.AddOpenTelemetry()
    .WithTracing(builder => builder.AddAspNetCoreInstrumentation()
        .AddSource(OpenTelemetryExtensions.ServiceName)
         .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: OpenTelemetryExtensions.ServiceName,
                            serviceVersion: OpenTelemetryExtensions.ServiceVersion))
    .AddHttpClientInstrumentation()
        .AddJaegerExporter(jaeger =>
        {
            jaeger.AgentHost = jaegerSettings.AgentHost;
            jaeger.AgentPort = jaegerSettings.AgentPort;
        }));

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection(nameof(RabbitMQSettings)));
builder.Services.AddScoped<ICreateMessageService, CreateMessageService>();
builder.Services.AddHostedService<ConsumerWorker>();

builder.Services.AddControllers();
builder.Services.AddMediatR(configuration => 
    configuration.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
