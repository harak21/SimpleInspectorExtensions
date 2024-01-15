using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.MetaAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class HideIfAttribute : BaseExtensionAttribute
    {
        private readonly string _condition;
        private string _attributeTarget;

        public HideIfAttribute(string condition)
        {
            _condition = condition;
        }

        public override int Order => 5;

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var condition = ReflectionUtility.GetMemberValue<bool>(target, _condition);

            memberElement.style.display = condition 
                ? new StyleEnum<DisplayStyle>(DisplayStyle.None) 
                : new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            
            (rootElement.Q<PropertyField>(_condition))?.RegisterValueChangeCallback(
                evt =>
                {
                    memberElement.style.display = evt.changedProperty.boolValue
                        ? new StyleEnum<DisplayStyle>(DisplayStyle.None)
                        : new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                });
        }
    }
}