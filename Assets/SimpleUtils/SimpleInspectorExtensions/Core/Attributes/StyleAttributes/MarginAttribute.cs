using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class MarginAttribute : BaseExtensionAttribute
    {
        private readonly Vector4 _margin;
        
        public MarginAttribute(int margin)
        {
            _margin = new Vector4(margin, margin, margin, margin);
        }

        public MarginAttribute(int marginTop, int marginBottom, int marginLeft, int marginRight)
        {
            _margin = new Vector4(marginTop, marginBottom, marginLeft, marginRight);
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            memberElement.style.marginTop = _margin.x;
            memberElement.style.marginBottom = _margin.y;
            memberElement.style.marginLeft = _margin.z;
            memberElement.style.marginRight = _margin.w;
        }
    }
}