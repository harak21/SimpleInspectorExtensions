using System;
using System.Diagnostics;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class LabelColorAttribute : BaseExtensionAttribute
    {
        private readonly InspectorColor _color;
        private readonly bool _parent;

        public LabelColorAttribute(InspectorColor color, bool parent = false)
        {
            _color = color;
            _parent = parent;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            var targetElement = _parent ? memberElement.parent : memberElement;
            targetElement.AddToClassList(_color.Style());
        }
    }
}