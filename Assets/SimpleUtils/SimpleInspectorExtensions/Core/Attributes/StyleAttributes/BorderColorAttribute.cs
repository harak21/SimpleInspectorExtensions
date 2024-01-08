using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class BorderColorAttribute : BaseExtensionAttribute
    {
        private readonly InspectorColor _inspectorColor;

        public BorderColorAttribute(InspectorColor inspectorColor)
        {
            _inspectorColor = inspectorColor;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            memberElement.style.borderTopColor = _inspectorColor.Color();
            memberElement.style.borderBottomColor = _inspectorColor.Color();
            memberElement.style.borderLeftColor = _inspectorColor.Color();
            memberElement.style.borderRightColor = _inspectorColor.Color();
        }
    }
}