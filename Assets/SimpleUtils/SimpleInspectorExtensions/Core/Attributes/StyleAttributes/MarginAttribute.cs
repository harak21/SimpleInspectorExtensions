using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class MarginAttribute : BaseExtensionAttribute
    {
        private readonly bool _parent;
        private readonly Vector4 _margin;
        
        public MarginAttribute(int margin, bool parent = false) 
            : this(margin,margin,margin,margin, parent)
        {
        }

        public MarginAttribute(int marginTop, int marginBottom, int marginLeft, int marginRight, bool parent = false)
        {
            _parent = parent;
            _margin = new Vector4(marginTop, marginBottom, marginLeft, marginRight);
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            var targetElement = _parent ? memberElement.parent : memberElement;
            targetElement.style.marginTop = _margin.x;
            targetElement.style.marginBottom = _margin.y;
            targetElement.style.marginLeft = _margin.z;
            targetElement.style.marginRight = _margin.w;
        }
    }
}