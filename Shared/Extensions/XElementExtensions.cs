using System;
using System.Xml.Linq;

namespace Shared.Extensions;

public static class XElementExtensions
{
    public static string GetStringAttr(this XElement elem, string attrName, string defaultValue = "")
    {
        return elem.Attribute(attrName)?.Value ?? defaultValue;
    }

    public static int GetIntAttr(this XElement elem, string attrName, int defaultValue = 0)
    {
        return elem.Attribute(attrName)?.Value.SafeParseInt(defaultValue) ?? defaultValue;
    }
}
