using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Extension methods for the dynamic object.
/// </summary>
public static class XMLConverter
{
	/// <summary>
	/// Defines the simple types that is directly writeable to XML.
	/// </summary>
	private static readonly Type[] _writeTypes = new[] { typeof(string), typeof(DateTime), typeof(Enum), typeof(decimal), typeof(Guid) };

	/// <summary>
	/// Determines whether [is simple type] [the specified type].
	/// </summary>
	/// <param name="type">The type to check.</param>
	/// <returns>
	/// 	<c>true</c> if [is simple type] [the specified type]; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsSimpleType(this Type type)
	{
		return type.IsPrimitive || _writeTypes.Contains(type);
	}

	/// <summary>
	/// Converts an anonymous type to an XElement.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>Returns the object as it's XML representation in an XElement.</returns>
	public static XElement ToXml(object obj, bool includetype = false)
	{
		return ToXml(obj, null, includetype);
	}

	/// <summary>
	/// Converts an anonymous type to an XElement.
	/// </summary>
	/// <param name="obj">The input.</param>
	/// <param name="element">The element name.</param>
	/// <returns>Returns the object as it's XML representation in an XElement.</returns>
	public static XElement ToXml(object obj, string element, bool includetype = false)
	{
		if (obj == null)
		{
			return null;
		}

		if (String.IsNullOrEmpty(element))
		{
            element = obj.GetType().Name;
			//element = "object";
		}

		element = XmlConvert.EncodeName(element);
		var ret = new XElement(element);

		if (obj != null)
		{
			var type = obj.GetType();
			var props = type.GetProperties().OrderBy(x => x.Name);

            var elements = from prop in props where prop.PropertyType.IsArray == false
                           let name = XmlConvert.EncodeName(prop.Name)
                           let val = prop.GetValue(obj, null)
                           //let val = prop.PropertyType.IsArray ? "array" : prop.GetValue(input, null)
                           where val != null && val.GetType().IsGenericType != true
                           let value = (prop.PropertyType.IsSimpleType() ? (includetype ? CreateWithType(new XElement(name, val), prop) : new XElement(name, val)) : val.GetType().IsEnum ? (includetype ? CreateWithType(new XElement(name, Convert.ChangeType(val, Enum.GetUnderlyingType(prop.PropertyType.UnderlyingSystemType))), prop) : new XElement(name, Convert.ChangeType(val, Enum.GetUnderlyingType(prop.PropertyType.UnderlyingSystemType)))) : ToXml(val, name, includetype))
                           where value != null
                           select value;

			ret.Add(elements);
		}

		return ret;
	}

    /// <summary>
    /// Adds a Type attribute an element.
    /// </summary>
    /// <param name="el">The property info.</param>
    /// <param name="prop">The input object.</param>
    /// <returns>Returns an XElement with a Type object.</returns>
    private static XElement CreateWithType(XElement el, PropertyInfo prop)
    {
        XElement xel = new XElement(el);

        if (prop.PropertyType.IsEnum)
        {
            xel.Add(new XAttribute("Type", Enum.GetUnderlyingType(prop.PropertyType.UnderlyingSystemType).Name));
                    }
        else
        {
            xel.Add(new XAttribute("Type", prop.PropertyType.Name));
        }
        
        return xel;
    }

	/// <summary>
	/// Gets the array element.
	/// </summary>
	/// <param name="info">The property info.</param>
	/// <param name="input">The input object.</param>
	/// <returns>Returns an XElement with the array collection as child elements.</returns>
	private static XElement GetArrayElement(PropertyInfo info, Array input, bool includetype)
	{
		var name = XmlConvert.EncodeName(info.Name);

		XElement rootElement = new XElement(name);

		var arrayCount = input.GetLength(0);

		for (int i = 0; i < arrayCount; i++)
		{
			var val = input.GetValue(i);
            XElement childElement;
            
            if (val.GetType().IsSimpleType())
            {
                childElement =   new XElement(name + "Child", val);
            }
            else
            {
                childElement =  ToXml(val, includetype);
            }

			rootElement.Add(childElement);
		}

		return rootElement;
	}

}