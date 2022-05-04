using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Imas
{
    internal static class Common
    {
        public static object TypeTreeToType<T>(object typeTree)
        {
            T targetObject = (T)Activator.CreateInstance(typeof(T));

            foreach (DictionaryEntry dataProperty in (OrderedDictionary)typeTree)
            {
                PropertyInfo matchingProperty = typeof(T).GetProperties()
                    .FirstOrDefault(p => p.Name.ToLower().Equals(dataProperty.Key.ToString().ToLower()));

                if (matchingProperty != null)
                {
                    object value = null;
                    if (dataProperty.Value.GetType() == typeof(List<object>))
                    {
                        List<object> list = (List<object>)dataProperty.Value;

                        Type elementType;
                        if (list?.Count > 0)
                        {
                            elementType = list[0].GetType();

                            Array newArray = Array.CreateInstance(elementType, list.Count);
                            list.ToArray().CopyTo(newArray, 0);

                            value = newArray;
                        }
                    }
                    else
                    {
                        value = dataProperty.Value;
                    }

                    matchingProperty.SetValue(targetObject, value);
                }
            }

            return targetObject;
        }
    }
}
