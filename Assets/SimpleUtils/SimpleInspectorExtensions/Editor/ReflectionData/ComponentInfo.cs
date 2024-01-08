using System;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData
{
    internal class ComponentInfo
    {
        public readonly Type Type;
        public readonly MemberData[] MemberData;

        public ComponentInfo(Type type, MemberData[] memberData)
        {
            Type = type;
            MemberData = memberData;
        }
    }
}