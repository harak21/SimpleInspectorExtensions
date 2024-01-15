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
            var attributesData = new List<AttributeData>();

            foreach (var type in TypeCache.GetTypesDerivedFrom<UnityEngine.Object>())       
            {
                if (type.IsAbstract || type.IsGenericTypeDefinition || type.IsGenericType)
                    continue;

                var members = type.GetMembers(BindingFlags.Instance
                                                  | BindingFlags.Public | BindingFlags.NonPublic);
                for (var i = 0; i < members.Length;i++)
                {
                    var memberInfo = members[i];
                    var customAttributes = memberInfo.GetCustomAttributes<BaseExtensionAttribute>().ToArray();
                    if (customAttributes.Length == 0)
                        continue;

                    foreach (var customAttribute in customAttributes)
                    {
                        var attributeData = new AttributeData(customAttribute, memberInfo, i);
                        attributesData.Add(attributeData);
                    }
                }

                if (attributesData.Count <= 0)
                    continue;

                attributesData.Sort();
                var componentInfo = new ComponentInfo(type, attributesData.ToArray());
                attributesData.Clear();
                yield return componentInfo;
            }
        }
    }
}