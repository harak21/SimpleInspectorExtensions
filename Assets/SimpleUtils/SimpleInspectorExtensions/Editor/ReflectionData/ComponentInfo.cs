using System;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData
{
    internal class ComponentInfo
    {
        public readonly Type Type;
        public readonly AttributeData[] AttributesData; 

        public ComponentInfo(Type type, AttributeData[] attributesData)
        {
            Type = type;
            AttributesData = attributesData;
        }
    }
}