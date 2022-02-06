using MassTransit;
using DataService.Consumers;
using DataService;

var builder = WebApplication.CreateBuilder(args);

//for datetime with time zone issue
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//for db context
builder.Services.AddScoped<IDbConnection>(db=>new PostgresConnection("Host=54.169.8.73;Username=postgres;Password=newpassword;Database=aplus"));
builder.Services.AddScoped<IDataContext,PostgresDataContext>();

// for message bus
builder.Services.AddMassTransit(x =>
            {
                //x.AddConsumers(Assembly.GetExecutingAssembly());
                x.AddConsumer<GetWeatherForecastConsumer>();
                 x.AddConsumer<AddDataConsumer>();
                 x.AddConsumer<UpdateDataConsumer>();
                  x.AddConsumer<RemoveDataConsumer>();
                x.AddConsumer<GetListDataConsumer>();
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("54.169.8.73","/",h=>{
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);
                });


                x.AddRequestClient<GetWeatherForecasts>();
                 x.AddRequestClient<AddData>();
                  x.AddRequestClient<UpdateData>();
                   x.AddRequestClient<RemoveData>();
                x.AddRequestClient<GetList>();

            }).AddMassTransitHostedService();




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
