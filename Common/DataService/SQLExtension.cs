

namespace DataService;
public static partial class SQLExtensions
{
    private static string[] sqloperators = {"=",">", "<", ">=" , "<=", "<>", "!=", "between" , "like"};
    private static string[] sqllogical = {"and","or"};
    public static string isParameterized(this string where)
    {     
        var logics = where.ToLower().Split(sqllogical, StringSplitOptions.None);
        foreach(var logic in logics){
            var conditions =  logic.Split(sqloperators, StringSplitOptions.None);
            bool isnotvalid = false;
            if(conditions.Length == 2) if(!conditions[1].Replace(" ","").StartsWith("@")) isnotvalid = true;
            if(conditions.Length == 1)if(!conditions[0].Replace(" ","").StartsWith("@")) isnotvalid = true;
            
            if(isnotvalid) throw new Exception("Where clause is not parameterized");
        }
        return where;
    }

}

