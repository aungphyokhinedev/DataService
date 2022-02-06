
public class PostgresConnection : IDbConnection {
   
    public PostgresConnection(string conn){
        connection = conn;
    }
}