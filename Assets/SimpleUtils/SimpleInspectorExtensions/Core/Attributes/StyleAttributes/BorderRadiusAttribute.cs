using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class BorderRadiusAttribute : BaseExtensionAttribute
    {
        private readonly int _radius;

        public BorderRadiusAttribute(int radius)
        {
            _radius = radius;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            memberElement.style.borderBottomLeftRadius = _radius;
            memberElement.style.borderBottomRightRadius = _radius;
            memberElement.style.borderTopLeftRadius = _radius;
            memberElement.style.borderTopRightRadius = _radius;
        }
    }
}