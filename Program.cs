using MassTransit;
using DataService;

var builder = WebApplication.CreateBuilder(args);

//for datetime with time zone issue
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//for db context

builder.Services.AddScoped<IDataContext, PostgresDataContext>();

// for message bus
builder.Services.AddMassTransit(x =>
            {
                //x.AddConsumers(Assembly.GetExecutingAssembly());
               
                x.AddConsumer<AddDataConsumer>();
                x.AddConsumer<UpdateDataConsumer>();
                x.AddConsumer<RemoveDataConsumer>();
                x.AddConsumer<GetListDataConsumer>();
                x.AddConsumer<TransactionConsumer>();
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMq:host"],builder.Configuration["RabbitMq:virtualHost"],h=>{
                        h.Username(builder.Configuration["RabbitMq:user"]);
                        h.Password(builder.Configuration["RabbitMq:password"]);
                    });
                    cfg.ConfigureEndpoints(context);
                });


               
                x.AddRequestClient<AddData>();
                x.AddRequestClient<UpdateData>();
                x.AddRequestClient<RemoveData>();
                x.AddRequestClient<GetList>();
                x.AddRequestClient<TransactionData>();

            }).AddMassTransitHostedService();




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
