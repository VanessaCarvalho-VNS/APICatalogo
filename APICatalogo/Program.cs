using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Models;
using APICatalogo.RateLimitOptions;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
})
.AddNewtonsoftJson();

var OrigensComAcessoPermitido = "_origensComAcessoPermitido";

builder.Services.AddCors(options =>
{
    options.AddPolicy("OrigensComAcessoPermitido",
     Policy =>
     {
         Policy.WithOrigins("https://localhost:7296")
         .WithMethods("GET", "POST")
         .AllowAnyHeader();

     });

});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "testev1", Version = "v1" });
    //c.SwaggerDoc("v2", new OpenApiInfo { Title = "testev2", Version = "v2" });
    c.SwaggerDoc("v1", new OpenApiInfo 
    {
        Version = "v1",
        Title = "APICatalogo",
        Description = "Catálogo de Produtos e Categorias",
        TermsOfService = new Uri("https://vanessa.net/terms"),
        Contact = new OpenApiContact
        {
            Name = "vanessa",
            Email = "vnscarvalh@gmail.com",
            Url = new Uri("https://www.vanessa.net"),
        },
        License = new OpenApiLicense
        {
            Name = "Usar sobre LICX",
            Url = new Uri("https://vanessa.net/license"),
        }
      
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT ",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

string mySqlConnection = builder.Configuration
                                .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));

var secretKey = builder.Configuration["JWT:SecretKey"]
                   ?? throw new ArgumentException("Invalid secret key!!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "vanessa"));

    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));

    options.AddPolicy("ExclusivePolicyOnly", policy =>
                      policy.RequireAssertion(context =>
                      context.User.HasClaim(Claim =>
                      Claim.Type == "id" && Claim.Value == "vanessa")
                      || context.User.IsInRole("SuperAdmin")));


});

var myOptions = new MyRateLimitOptions();

builder.Configuration.GetSection(MyRateLimitOptions.MyRatelimit).Bind(myOptions);

builder.Services.AddRateLimiter(rateLimiteroptions =>
{
    rateLimiteroptions.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
    {
        options.PermitLimit = myOptions.PermitLimit; //1;
        options.Window = TimeSpan.FromSeconds(myOptions.Window);
        options.QueueLimit = myOptions.QueueLimit; //2;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    rateLimiteroptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext => 
                            RateLimitPartition.GetFixedWindowLimiter(
                                               partitionKey: httpcontext.User.Identity?.Name ??
                                                             httpcontext.Request.Headers.Host.ToString(),
        factory: partition => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 5,
            QueueLimit = 0,
            Window = TimeSpan.FromSeconds(10)

        }));



});

builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader(),
        new UrlSegmentApiVersionReader());
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    // Habilita o middleware para servir o swagger
//    // gerado como um endpoint JSON
//    app.UseSwagger();

//    //Habilita o middleware de arquivos estaticos
//    //app.UseSwaggerUI();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json",
//            "APICatalogo");
//    });
//}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseRateLimiter();

app.UseCors(OrigensComAcessoPermitido);

app.UseAuthorization();
app.MapControllers();
app.Run();