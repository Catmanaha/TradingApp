using System.Text;

namespace Extensions;

public static class EnumerableExtensions
{
    public static string GetHtml<T>(this IEnumerable<T> elements)
    {
        Type type = typeof(T);

        var props = type.GetProperties();

        StringBuilder sb = new StringBuilder(100);
        sb.Append("<ul>");

        foreach (var element in elements)
        {
            foreach (var prop in props)
            {
                sb.Append($"<li><span>{prop.Name}: </span>{prop.GetValue(element)}</li>");
            }
            sb.Append("<br/>");
        }
        sb.Append("</ul>");

        return sb.ToString();
    }
}