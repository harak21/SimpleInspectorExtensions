using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.MetaAttributes
{
    public class InspectorTooltipAttribute : BaseExtensionAttribute
    {
        private readonly string _tooltip;

        public InspectorTooltipAttribute(string tooltip)
        {
            _tooltip = tooltip;
        }
        
        public override int Order => 5;
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            memberElement.tooltip = _tooltip;
        }
    }
}