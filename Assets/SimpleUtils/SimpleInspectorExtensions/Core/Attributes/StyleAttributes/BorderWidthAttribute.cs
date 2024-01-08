using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class BorderWidthAttribute : BaseExtensionAttribute
    {
        private readonly Vector4 _borderWidth;
        
        public BorderWidthAttribute(int width) : this(width, width, width, width)
        {
        }

        public BorderWidthAttribute(int top, int bottom, int left, int right)
        {
            _borderWidth = new Vector4(top, bottom, left, right);
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            memberElement.style.borderTopWidth = _borderWidth.x;
            memberElement.style.borderBottomWidth = _borderWidth.y;
            memberElement.style.borderLeftWidth = _borderWidth.z;
            memberElement.style.borderTopWidth = _borderWidth.w;
        }
    }
}