using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;
using SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData;
using UnityEditor;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.InspectorBuilder
{
    internal static class ComponentsGatherer
    {
        public static IEnumerable<ComponentInfo> Gather()
        {
            var memberData = new List<MemberData>();

            foreach (var type in TypeCache.GetTypesDerivedFrom<Object>())       
            {
                if (type.IsAbstract || type.IsGenericTypeDefinition || type.IsGenericType)
                    continue;

                foreach (var memberInfo in type.GetMembers(BindingFlags.Instance
                                                           | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var customAttributes = memberInfo.GetCustomAttributes<BaseExtensionAttribute>().ToArray();
                    if (customAttributes.Length == 0) 
                        continue;
                    
                    var data = new MemberData(memberInfo)
                    {
                        Attributes = customAttributes
                    };
                    memberData.Add(data);
                }

                if (memberData.Count <= 0)
                    continue;

                var componentInfo = new ComponentInfo(type, memberData.ToArray());
                yield return componentInfo;
                
                memberData.Clear();
            }
        }
    }
}