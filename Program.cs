
using Microsoft.EntityFrameworkCore;
using WorkshopHubAPI.Data;
using WorkshopHubAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Connection
builder.Services.AddDbContext<WorkshopHubDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Register Services - Dependency Injection
builder.Services.AddScoped<TrainerService>();
builder.Services.AddScoped<TraineeService>();
builder.Services.AddScoped<WorkshopService>();
builder.Services.AddScoped<WorkshopSessionService>();
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<TraineeProfileService>();
builder.Services.AddScoped<DashboardService>();

// CORS - Allow Angular to access Web API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://127.0.0.1:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTP Request Pipeline
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AngularPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
