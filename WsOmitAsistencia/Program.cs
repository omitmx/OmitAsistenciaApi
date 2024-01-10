
/*
 *****************************************************
 ***********************PRODUCCION OMIT ASISTENCIA
 *****************************************************/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using WsOmitAsistencia.Models;
using WsOmitAsistencia.Services;
using WsOmitAsistencia.Services.Mongo;
using WsOmitAsistencia.Utils.Db;
using WsOmitAsistencia.Utils.Jwt;
using WsOmitAsistencia.Utils.Smtp;

string MiCors = "MiCors";

var builder = WebApplication.CreateBuilder(args);
//cors
builder.Services.AddCors(op =>
{
    op.AddPolicy(name: MiCors, builder =>
    {
        builder.WithOrigins("*");
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});



//Conexion MongoDB
var MongoDB = builder.Configuration.GetSection("MongoDB");
builder.Services.Configure<MongoDBConn>(MongoDB);
//SMTP 
var smtpCitesoft = builder.Configuration.GetSection("Smtp");
builder.Services.Configure<Smtp>(smtpCitesoft);


//conexion DB
var fbCnn = builder.Configuration.GetSection("FBCnn");
builder.Services.Configure<FBCnn>(fbCnn);

//JWT secreto
var appSettSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettSection);
//JWT
var appSettings = appSettSection.Get<AppSettings>();
var llave = Encoding.ASCII.GetBytes(appSettings.Secreto);

 
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 5001;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Net6v1", new OpenApiInfo
    {
        Title = "OmitWs",
        Version = "Net6v1",
        Contact = new OpenApiContact
        {
            Name = "Omit",
            Email = "emtimesystem@email.com",
            // Url = new Uri("#")
        }
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
    // Ruta al archivo XML de comentarios generados
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresar token de sesion!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                            {
                                                { jwtSecurityScheme, Array.Empty<string>() }
                                            });
});






builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(d =>
  {
      d.RequireHttpsMetadata = false;
      d.SaveToken = true;
      d.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(llave),
          ValidateIssuer = false,
          ValidateAudience = false
      };
  });

builder.Services.AddScoped<IUsuServicio, cUsuServicio>();
builder.Services.AddMvc(s =>
{
    s.EnableEndpointRouting = false;
});//.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

//mongo add singleton 
builder.Services.AddSingleton<IMongoDB>(d => d.GetRequiredService<IOptions<MongoDBConn>>().Value);
//inyectamos
builder.Services.AddSingleton<cOmitLogServicio>();
builder.Services.AddScoped<IUsuServicio, cUsuServicio>();



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("Net6v1/swagger.json", "OmitWs Net6v1"));

    var forwardOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        RequireHeaderSymmetry = false
    };
    forwardOptions.KnownNetworks.Clear();
    forwardOptions.KnownProxies.Clear();
    app.UseForwardedHeaders(forwardOptions);
}
else
{
    app.UseExceptionHandler("/Error");
    var forwardOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        RequireHeaderSymmetry = false
    };
    forwardOptions.KnownNetworks.Clear();
    forwardOptions.KnownProxies.Clear();
    app.UseForwardedHeaders(forwardOptions);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(MiCors);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=ComprasOc}/{action=Index}/{id?}");

});
app.Run();
