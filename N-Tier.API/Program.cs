using N_Tier.Application.Exceptions;
using N_Tier.Application.Hub;

var builder = WebApplication.CreateBuilder(args);

builder.AddProjectConfiguration();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddValidationServices();
builder.Services.AddSignalR();
builder.Services.AddCorsPolicy();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
//------------------------------------------------------------------------------------------
var app = builder.Build();

//Auto update database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await AutomatedMigration.MigrateAsync(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapHub<NotificationHub>("/notificationHub");
app.UseStaticFiles();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();