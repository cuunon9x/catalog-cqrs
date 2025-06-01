using Carter;
using CatalogCQRS.API.Filters;
using CatalogCQRS.API.Middleware;
using CatalogCQRS.Application.Common.Behaviors;
using CatalogCQRS.Application.Features.Products.Commands;
using CatalogCQRS.Infrastructure.Configuration;
using FluentValidation;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CatalogCorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination");
    });
});

// Add services to the container.
// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Catalog CQRS API", 
        Version = "v1",
        Description = "A CQRS-based product catalog API using Marten and PostgreSQL"
    });
    
    c.TagActionsBy(api => new[] { api.GroupName ?? "General" });
    c.UseInlineDefinitionsForEnums();
    
    // Add security definitions
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add MediatR and register handlers from the assembly containing our commands
builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);

// Add MediatR validation pipeline behavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);

// Add Carter for minimal APIs
builder.Services.AddCarter();

// Add Marten and Infrastructure services
builder.Services.AddMartenDb(builder.Configuration);

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 100;
        options.Window = TimeSpan.FromMinutes(1);
        options.AutoReplenishment = true;
    });
    
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog CQRS API V1");
        c.EnableFilter();
        c.EnableDeepLinking();
    });
}

// Global error handling
app.UseExceptionHandler("/error");

// Security headers
app.UseXContentTypeOptions();
app.UseReferrerPolicy(opts => opts.NoReferrer());
app.UseXXssProtection(options => options.EnabledWithBlockMode());
app.UseXfo(options => options.Deny());
app.UseCsp(opts => opts
    .BlockAllMixedContent()
    .StyleSources(s => s.Self())
    .StyleSources(s => s.UnsafeInline())
    .FontSources(s => s.Self())
    .FormActions(s => s.Self())
    .FrameAncestors(s => s.Self())
    .ImageSources(s => s.Self())
    .ScriptSources(s => s.Self())
);

// Use CORS before routing
app.UseCors("CatalogCorsPolicy");

app.UseHttpsRedirection();
app.UseRouting();

// Rate limiting
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

// Custom middleware for request logging
app.UseMiddleware<RequestLoggingMiddleware>();

// Map endpoints
app.MapCarter();

// Health checks endpoint
app.MapHealthChecks("/health", new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
