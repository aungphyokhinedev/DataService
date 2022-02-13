
using System.ComponentModel;
using System.Reflection;
using DataService;
using Newtonsoft.Json;

namespace AplusExtension;
public static partial class Extensions
{
    /// <summary>
    ///     A T extension method that query if '@this' is not null.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if not null, false if not.</returns>
    public static bool IsNotNullOrEmpty(this object? @this)
    {
        return @this != null && @this.ToString().Length > 0;
    }

    public static bool IsPositiveNumber(this int @this)
    {
        return @this > 0;
    }



    public static Dictionary<string, object> PropertiesFromInstance(this object @this)
    {
        if (@this == null) return null;
        Type TheType = @this.GetType();
        PropertyInfo[] Properties = TheType.GetProperties();
        Dictionary<string, object> PropertiesMap = new Dictionary<string, object>();
        foreach (PropertyInfo Prop in Properties)
        {
            try
            {
                var value = @this.GetType().GetProperty(Prop.Name).GetValue(@this, null);
                if (value != null)
                {
                    PropertiesMap.Add(Prop.Name, value);
                }
            }
            catch (Exception e)
            {

            }

        }
        return PropertiesMap;
    }

    public static List<Parameter> toParameterList(this Dictionary<string, object> dict)
    {

        return dict.Select(x=>new Parameter{
                key =  x.Key,
                value = x.Value,
                type = x.Value.GetType()
            }).ToList();
    }

    public static List<QueryRequest> toQueryRequest(this List<QueryContext> requests)
    {

        return requests.Select(x=>new QueryRequest{
                request = JsonConvert.SerializeObject(x),
                type = x.GetType()
            }).ToList();
    }

    public static IDictionary<string, T> ToDictionary<T>(this object source)
    {
        if (source == null)
           ThrowExceptionWhenSourceArgumentIsNull();

        var dictionary = new Dictionary<string, T>();
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            AddPropertyToDictionary<T>(property, source, dictionary);
        return dictionary;
    }

    private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
    {
        object value = property.GetValue(source);
        if (IsOfType<T>(value))
            dictionary.Add(property.Name, (T)value);
    }

    private static bool IsOfType<T>(object value)
    {
        return value is T;
    }

    private static void ThrowExceptionWhenSourceArgumentIsNull()
    {
        throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
    }


     public static Dictionary<string?, object?> toDictionaryList(this List<Parameter> list)
    {
        if(list == null) return null;

        return list.ToDictionary(x=>x.key,x=>{
                    
                    ///this is for message queue serialization on date data problem
                    if(x.type == typeof(DateTimeOffset)){
                        return DateTimeOffset.Parse(x.value.ToString());
                    }
                    return x.value;
                } );
    }
}

