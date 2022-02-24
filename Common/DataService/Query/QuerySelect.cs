using AplusExtension;
using Newtonsoft.Json;

namespace DataService;
public class QuerySelect
{
   

    private string _fields { get; set; }
    private Query _query { get; set; }
    private string? _orderBy { get; set; }
    private string? _groupBy { get; set; }

    private int _limit { get; set; }

    private int _page { get; set; }

    private string _tag { get; set; }


    private string _where { get; set; }
    public List<Parameter>? _parameters { get; set; }

    public QuerySelect Where(string where, object parameter)
    {
        _where = where.isParameterized(true);
        _parameters = parameter.ToDictionary().toParameterList();
        return this;
    }

    public QuerySelect As(string tag)
    {
        _tag = tag;
        return this;
    }



    public QuerySelect(Query query, string fields = "*")
    {
        _fields = fields;
        _query = query;
    }

    public QuerySelect Order(string order)
    {
        _orderBy = order;
        return this;
    }

    public QuerySelect Group(string group)
    {
        _groupBy = group;
        return this;
    }

    public QuerySelect Page(int page)
    {
        _page = page;
        return this;
    }

    public QuerySelect Limit(int limit)
    {
        _limit = limit;
        return this;
    }

    public GetRequest Request()
    {
        return new GetRequest
        {
            page = this._page,
            pageSize = this._limit,
            tables = this._query._tables,
            fields = this._fields,
            groupBy = this._groupBy,
            orderBy = this._orderBy,
            filter = new Filter
            {
                where = _where,
                parameters = _parameters
            },
            tag = _tag,

        };
    }

    public QueryContract Contract(){
        return new QueryContract{
            type = QueryTypes.Listing,
            request = JsonConvert.SerializeObject(this.Request())
        };
    }

    public async Task<ListResponse> ExecuteAsync(IDataContext db){
        return await db.GetListAsync(this.Request().toSelect());
    }

}
