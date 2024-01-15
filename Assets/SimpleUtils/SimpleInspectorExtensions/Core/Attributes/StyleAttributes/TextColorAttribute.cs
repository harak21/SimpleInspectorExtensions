using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class TextColorAttribute : StyleExtensionAttribute
    {
        private readonly InspectorColor _color;
        private readonly bool _parent;

        public TextColorAttribute(InspectorColor color, bool parent = false)
        {
            _color = color;
            _parent = parent;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var targetElement = _parent ? memberElement.parent : memberElement;

            if (targetElement is PropertyField propertyField)
            {
                propertyField.schedule.Execute(() =>
                {
                    propertyField.Query<Label>().ForEach(l => l.style.color = _color.Color());
                }).ExecuteLater(200L);
            }
            else
            {
                memberElement.Query<Label>().ForEach(l => l.style.color = _color.Color());
            }
            
            //targetElement.AddToClassList(_color.Style());
        }
    }
}