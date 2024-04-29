using AgendaCalendar.Application;
using AgendaCalendar.Infrastructure;
using AgendaCalendar.WEB_API;

var MyAllowSpecificOrigins = "AllowAll";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation()
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyHeader()
                  .AllowAnyOrigin()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
