using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class BorderWidthAttribute : StyleExtensionAttribute
    {
        private readonly bool _parent;
        private readonly Vector4 _borderWidth;
        
        public BorderWidthAttribute(int width, bool parent = false) : this(width, width, width, width, parent)
        {
        }

        public BorderWidthAttribute(int top, int bottom, int left, int right, bool parent = false)
        {
            _parent = parent;
            _borderWidth = new Vector4(top, bottom, left, right);
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var targetElement = _parent ? memberElement.parent : memberElement;
            targetElement.style.borderTopWidth = _borderWidth.x;
            targetElement.style.borderBottomWidth = _borderWidth.y;
            targetElement.style.borderLeftWidth = _borderWidth.z;
            targetElement.style.borderTopWidth = _borderWidth.w;
        }
    }
}