using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
public class Descriptor
{

    [TextArea(8, 16),SerializeField] 
    private string description;
    [SerializeField]
    private ExposedSelector[] exposedValues;
    private List<string> exposedValuesList;
    public string FormatString(object[] obj)
    {
        exposedValuesList = new List<string>();
        for (int i = 0; i < exposedValues.Length; i++)
        {
            Debug.Log($"Trying to lookup key: '{exposedValues[i]._fieldName}'");
            string key = exposedValues[i]._fieldName.Trim();

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("ExposedSelector._fieldName is null or empty!");
                exposedValuesList.Add("Missing");
                continue;
            }
            if (!Globals.GetMemberNameDictionary.TryGetValue(key, out MemberInfo info))
            {
                Debug.LogError($"Key '{key}' not found in Globals dictionary.");
                exposedValuesList.Add("Missing");
                continue;
            }

            string value = GetValue(info, obj);
            exposedValuesList.Add(value);
        }

        return string.Format(description, exposedValuesList.ToArray());
    }
    private string GetValue(MemberInfo info, object[] obj)
    {
        foreach (var objs in obj)
        {
            if (info.ReflectedType == objs.GetType())
            {
                object val = null;
                if (info is FieldInfo field)
                {
                    val = field.GetValue(objs);
                }
                else if (info is PropertyInfo prop)
                {
                    val = prop.GetValue(objs);
                }
                else
                {
                    return "Unsupported member type";
                }

                if (val == null)
                    return "null";

                // If value is a list or enumerable (but exclude string, which is IEnumerable<char>)
                if (val is System.Collections.IEnumerable enumerable && !(val is string))
                {
                    List<string> items = new List<string>();
                    foreach (var item in enumerable)
                    {
                        items.Add(item?.ToString() ?? "null");
                    }
                    return string.Join(", ", items);
                }

                return val.ToString();
            }
        }
        return "Missing";
    }

}

