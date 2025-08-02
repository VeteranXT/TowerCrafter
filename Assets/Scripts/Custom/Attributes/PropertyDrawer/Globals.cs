using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public class Globals : MonoBehaviour
{
    private static Dictionary<string, MemberInfo> _memberNameDictionary;
    private static List<string> _memberNames;
    private static bool _isDirty = true;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _memberNameDictionary = null;
        _memberNames = null;
        _isDirty = true;
    }

    public static Dictionary<string, MemberInfo> GetMemberNameDictionary
    {
        get
        {
            if (_isDirty || _memberNameDictionary == null)
                GetData();
            return _memberNameDictionary;
        }
    }

    public static List<string> GetMemberNames
    {
        get
        {
            if (_isDirty || _memberNames == null)
                GetData();
            return _memberNames;
        }
    }

    private static void GetData()
    {
        if (!_isDirty && _memberNameDictionary != null)
            return;

        var newDictionary = new Dictionary<string, MemberInfo>();
        var newNames = new List<string>();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Skip system assemblies for performance
            if (assembly.FullName.StartsWith("System.") ||
                assembly.FullName.StartsWith("UnityEngine.") ||
                assembly.FullName.StartsWith("Unity."))
                continue;

            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        var attribute = field.GetCustomAttribute<ExposeAttribute>();
                        if (attribute != null)
                        {
                            string key = $"{type.Name}/{attribute.displayName ?? field.Name}";
                            newDictionary[key] = field;
                            //newNames.Add($"{type.Name}/{attribute.displayName ?? field.Name}");
                            newNames.Add(key);
                            Debug.Log($"Adding field key: {key}");
                        }
                    }
                    foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                    {
                        var attr = property.GetCustomAttribute<ExposeAttribute>();
                        if (attr != null)
                        {
                            string key = $"{type.Name}/{attr.displayName ?? property.Name}";
                            newDictionary[key] = property;
                            //newNames.Add($"{type.Name}/{attr.displayName ?? property.Name}");
                            newNames.Add(key);
                            Debug.Log($"Adding field key: {key}");
                        }
                    }

                }
            }
            catch (ReflectionTypeLoadException)
            {
                // Skip types that fail to load
                continue;
            }
        }

        _memberNameDictionary = newDictionary;
        _memberNames = newNames;
        _isDirty = false;
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        _isDirty = true;
    }
}