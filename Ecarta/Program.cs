using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

var parameters = new Ecarta.InitialisationParameters(app.Configuration);


try
{
    if (parameters.Rabbithost != null && parameters.RabbitUser != null && parameters.RabbitPass != null)
    {
        Lizard.Monitor.LizardMessageClient.Initialise(
            parameters.Rabbithost, 
            parameters.RabbitUser, 
            parameters.RabbitPass);
    }

    Lizard.Monitor.LizardMessageClient.Instance.SendLog(new Lizard.Models.LogEntryAddOptions()
    {
        Message = "Ecarta started",
        Occurred = DateTime.UtcNow
    });
}
catch { }

Ecarta.Listener.Initialise(parameters);

//app.UseAuthorization();

//app.MapControllers();

var name = Assembly.GetExecutingAssembly().GetName();

app.MapGet("/metadata", async (context) => await context.Response.WriteAsJsonAsync(new { name.Name, name.Version, Author = "Daniel James Thorne" }));

app.Run();
