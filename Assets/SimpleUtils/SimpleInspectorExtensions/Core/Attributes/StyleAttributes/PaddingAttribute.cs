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
    public class PaddingAttribute : StyleExtensionAttribute
    {
        private readonly bool _parent;
        private readonly Vector4 _padding;
        
        public PaddingAttribute(int padding, bool parent = false)
            : this(padding,padding,padding,padding, parent)
        {
        }

        public PaddingAttribute(int paddingTop, int paddingBottom, int paddingLeft, int paddingRight, bool parent = false)
        {
            _parent = parent;
            _padding = new Vector4(paddingTop, paddingBottom, paddingLeft, paddingRight);
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var targetElement = _parent ? memberElement.parent : memberElement;
            targetElement.style.paddingTop = _padding.x;
            targetElement.style.paddingBottom = _padding.y;
            targetElement.style.paddingLeft = _padding.z;
            targetElement.style.paddingRight = _padding.w;
        }
    }
}