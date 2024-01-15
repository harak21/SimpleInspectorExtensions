using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class BorderColorAttribute : StyleExtensionAttribute
    {
        private readonly InspectorColor _inspectorColor;
        private readonly bool _parent;

        public BorderColorAttribute(InspectorColor inspectorColor, bool parent = false)
        {
            _inspectorColor = inspectorColor;
            _parent = parent;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var targetElement = _parent ? memberElement.parent : memberElement;
            targetElement.style.borderTopColor = _inspectorColor.Color();
            targetElement.style.borderBottomColor = _inspectorColor.Color();
            targetElement.style.borderLeftColor = _inspectorColor.Color();
            targetElement.style.borderRightColor = _inspectorColor.Color();
        }
    }
}