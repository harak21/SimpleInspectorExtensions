using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class PaddingAttribute : BaseExtensionAttribute
    {
        private readonly Vector4 _padding;
        
        public PaddingAttribute(int padding)
        {
            _padding = new Vector4(padding, padding, padding, padding);
        }

        public PaddingAttribute(int paddingTop, int paddingBottom, int paddingLeft, int paddingRight)
        {
            _padding = new Vector4(paddingTop, paddingBottom, paddingLeft, paddingRight);
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            memberElement.style.paddingTop = _padding.x;
            memberElement.style.paddingBottom = _padding.y;
            memberElement.style.paddingLeft = _padding.z;
            memberElement.style.paddingRight = _padding.w;
        }
    }
}