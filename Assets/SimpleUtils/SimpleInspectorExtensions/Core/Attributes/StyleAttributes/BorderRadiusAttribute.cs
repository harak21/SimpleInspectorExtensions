using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class BorderRadiusAttribute : StyleExtensionAttribute
    {
        private readonly int _radius;
        private readonly bool _parent;

        public BorderRadiusAttribute(int radius, bool parent = false)
        {
            _radius = radius;
            _parent = parent;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var targetElement = _parent ? memberElement.parent : memberElement;
            targetElement.style.borderBottomLeftRadius = _radius;
            targetElement.style.borderBottomRightRadius = _radius;
            targetElement.style.borderTopLeftRadius = _radius;
            targetElement.style.borderTopRightRadius = _radius;
        }
    }
}