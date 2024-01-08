using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class LabelColorAttribute : BaseExtensionAttribute
    {
        private readonly InspectorColor _color;

        public LabelColorAttribute(InspectorColor color)
        {
            _color = color;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            memberElement.AddToClassList(_color.Style());
        }
    }
}