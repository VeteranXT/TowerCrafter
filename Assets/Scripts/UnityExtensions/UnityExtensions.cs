using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityEngine.Extensions
{
    public static class UnityExtensions
    {
        public static T GetComponentInChildrenWithTag<T>(this GameObject parent, string tag, bool includeInactive = false) where T : Component
        {
            T[] components = parent.GetComponentsInChildren<T>(includeInactive);
            foreach (T comp in components)
            {
                if (comp.CompareTag(tag))
                    return comp;
            }
            return null;
        }
   
        public static List<T> GetComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool includeInactive = false) where T : Component
        {
            List<T> results = new List<T>();
            T[] components = parent.GetComponentsInChildren<T>(includeInactive);
            foreach (T comp in components)
            {
                if (comp.CompareTag(tag))
                    results.Add(comp);
            }
            return results;
        }
        public static T GetComponentInChildrenWithTag<T>(this MonoBehaviour mb, string tag, bool includeInactive = false) where T : Component
        {
            return mb.gameObject.GetComponentInChildrenWithTag<T>(tag, includeInactive);
        }
        public static List<T> GetComponentsInChildrenWithTag<T>(this MonoBehaviour mb, string tag, bool includeInactive = false) where T : Component
        {
            return mb.gameObject.GetComponentsInChildrenWithTag<T>(tag, includeInactive);
        }
   


    }
}

