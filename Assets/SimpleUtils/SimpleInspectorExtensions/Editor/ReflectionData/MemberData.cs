using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData
{
    internal class MemberData
    {
        public MemberInfo MemberInfo;
        public BaseExtensionAttribute[] Attributes;

        public MemberData(MemberInfo memberInfo)
        {
            MemberInfo = memberInfo;
        }
    }
}