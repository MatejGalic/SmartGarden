using SmartBikeRental.Middleware;
using SmartGarden.Constants;
using SmartGarden.Hubs;
using SmartGarden.Services.Calculator;
using SmartGarden.Services.Garden;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMqttClientWrapper, MqttClientWrapper>();
builder.Services.AddSingleton<ICalculatorService, CalculatorService>();
builder.Services.AddSingleton<IGardenService, GardenService>();
builder.Services.AddSingleton<GardenHub>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // https://localhost:7288/swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapHub<GardenHub>("hub");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");


var mqttClient = app.Services.GetRequiredService<IMqttClientWrapper>();
mqttClient.Connect(Guid.NewGuid().ToString());
mqttClient.SubscribeToTopic(Topics.ReadingFromGarden);

app.Lifetime.ApplicationStopping.Register(() =>
{
    mqttClient.Disconnect();
});


app.MapFallbackToFile("index.html");

app.Run();
