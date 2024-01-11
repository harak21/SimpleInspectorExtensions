using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class OrderAttribute : StyleExtensionAttribute
    {
        private readonly int _order;
        private readonly bool _parent;

        public OrderAttribute(int order, bool parent = false)
        {
            _order = order;
            _parent = parent;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            VisualElement parent;
            VisualElement targetElement;
            if (memberElement.parent.parent == null)
            {
                parent = memberElement.parent;
                targetElement = memberElement;
            }
            else
            {
                targetElement = _parent ? memberElement.parent : memberElement;
                parent = _parent ? memberElement.parent.parent : memberElement.parent;
            }
            
            if (parent.childCount-1 < _order)
            {
                parent.Remove(targetElement);
                parent.Add(targetElement);
            }
            else
            {
                parent.Insert(_order, targetElement);
            }
        }
    }
}