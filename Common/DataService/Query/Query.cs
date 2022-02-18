
using AplusExtension;
namespace DataService;

public class Query
{
    public string tables { get; set; }
    public string? where { get; set; }
    public List<Parameter>? parameters { get; set; }

    public Query Set(Object paras)
    {
        parameters = paras.ToDictionary().toParameterList();
        return this;
    }

    public Query Where(string _where)
    {
        where = _where.isParameterized();
        return this;
    }

    public Query(string _tables)
    {
        tables = _tables;
    }
  

    public QuerySelect Select(string fields)
    {

        return new QuerySelect(this, fields);
    }

    public QueryInsert Insert(object data)
    {
        return new QueryInsert(this, data.ToDictionary());
    }

    public QueryInsert Insert(Dictionary<string, object> data)
    {
        return new QueryInsert(this, data);
    }

    public QueryUpdate Update(Dictionary<string, object> data)
    {

        return new QueryUpdate(this, data);
    }
    public QueryUpdate Update(object data)
    {

        return new QueryUpdate(this, data.ToDictionary());
    }

    public QueryDelete Delete()
    {

        return new QueryDelete(this);
    }

}










