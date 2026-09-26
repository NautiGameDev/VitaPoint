using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VitaPoint.Server.Data;
using VitaPoint.Server.Interfaces;
using VitaPoint.Server.Models;
using VitaPoint.Server.Repos;
using VitaPoint.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<Account, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedAccount = false; //Update in future when email services are available
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["VPAuth"];
            //Console.WriteLine($"Token found in cookie: {token != null}");
            context.Token = token;
            return Task.CompletedTask;
        }
    };
});

//Cors set-up to allow for desktop/mobile access of server during development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
        .WithOrigins("https://localhost:64711", "https://10.160.0.246:64711")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

//Add Services to system
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientRepo, PatientRepo>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepo, MessageRepo>();

builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorRepo, DoctorRepo>();

builder.Services.AddScoped<IPatientDoctorService, PatientDoctorService>();
builder.Services.AddScoped<IPatientDoctorRepo, PatientDoctorRepo>();

builder.Services.AddScoped<ILabResultService, LabResultService>();
builder.Services.AddScoped<ILabResultRepo, LabResultRepo>();

builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPrescriptionRepo, PrescriptionRepo>();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    if (await userManager.FindByNameAsync(builder.Configuration["AdminConfig:Username"]) == null)
    {
        var adminUser = new Account
        {
            UserName = builder.Configuration["AdminConfig:Username"],
            Email = builder.Configuration["AdminConfig:Email"]
        };

        var result = await userManager.CreateAsync(adminUser, builder.Configuration["AdminConfig:Password"]);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    if (await userManager.FindByNameAsync("mayalin@vitapoint.com") == null)
    {
        var testDoctor = new Account
        {
            UserName = "mayalin@vitapoint.com",
            Email = "mayalin@vitapoint.com"
        };

        var result = await userManager.CreateAsync(testDoctor, "VitaPoint1!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(testDoctor, "Moderator");
        }
    }
}

app.MapFallbackToFile("/index.html");

app.Run();
