using System;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData
{
    internal class AttributeData : IComparable<AttributeData>
    {
        public readonly int MemberOrder;
        public readonly BaseExtensionAttribute Attribute;
        public readonly MemberInfo MemberInfo;

        public AttributeData(BaseExtensionAttribute attribute, MemberInfo memberInfo, int memberOrder)
        {
            Attribute = attribute;
            MemberInfo = memberInfo;
            MemberOrder = memberOrder;
        }

        public int CompareTo(AttributeData other)
        {
            if (Attribute.Order == other.Attribute.Order)
            {
                return MemberOrder - other.MemberOrder;
            }
            
            return Attribute.Order - other.Attribute.Order;
        }
    }
}