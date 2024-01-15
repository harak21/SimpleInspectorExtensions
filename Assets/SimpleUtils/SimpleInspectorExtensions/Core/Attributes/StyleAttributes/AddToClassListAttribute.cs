using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    public class AddToClassListAttribute : StyleExtensionAttribute
    {
        private readonly string _className;
        private readonly bool _parent;

        public AddToClassListAttribute(string className, bool parent = false)
        {
            _className = className;
            _parent = parent;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement, MemberInfo memberInfo)
        {
            var targetElement = _parent ? memberElement.parent : memberElement;
            targetElement.AddToClassList(_className);
        }
    }
}