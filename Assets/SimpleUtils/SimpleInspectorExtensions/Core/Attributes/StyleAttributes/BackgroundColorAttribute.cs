using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class BackgroundColorAttribute : BaseExtensionAttribute
    {
        private readonly InspectorColor _color;

        public BackgroundColorAttribute(InspectorColor color)
        {
            _color = color;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            memberElement.style.backgroundColor = new StyleColor(_color.Color());
        }
    }
}