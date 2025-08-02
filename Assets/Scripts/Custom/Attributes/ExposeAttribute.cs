using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.ReturnValue | AttributeTargets.Enum)]
public class ExposeAttribute : PropertyAttribute
{
    public string displayName;
    public ExposeAttribute(string exposeName)
    {
        displayName = exposeName;
    }
}

