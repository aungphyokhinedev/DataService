
namespace DataService;
using AplusDbContext;
public interface GetWeatherForecasts
{
}

public interface WeatherForecasts
{
    WeatherForecast[] Forecasts {  get; }
}

public interface GetList{
    GetRequest request {get;}
    
}

public interface ListData{
    ListResponse response {get;}
}

public interface AddData{
    CreateRequest request {get;}
    
}

public interface UpdateData{
    UpdateRequest request {get;}
    
}

public interface RemoveData{
    RemoveRequest request {get;}
    
}

public interface ResultData{
    Response response {get;}
}