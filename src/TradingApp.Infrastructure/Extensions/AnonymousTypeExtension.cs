using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Infrastructure.Extensions;

public static class AnonymousTypeExtension
{
    public static IEnumerable<ExpandoObject> ToExpandoObjectCollection(this IEnumerable<object> collection)
    {
        var joinData = new List<ExpandoObject>();
        foreach (var item in collection)
        {
            IDictionary<string, object> itemExpando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item.GetType()))
            {
                itemExpando.Add(property.Name, property.GetValue(item));
            }
            joinData.Add(itemExpando as ExpandoObject);
        }

        return joinData;
    }
}
