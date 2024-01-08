using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes
{
    public class HideIfAttribute : BaseExtensionAttribute
    {
        private readonly string _condition;
        private string _attributeTarget;

        public HideIfAttribute(string condition)
        {
            _condition = condition;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            var condition = ReflectionUtility.GetMemberValue<bool>(target, memberElement.name);

            memberElement.style.display = condition 
                ? new StyleEnum<DisplayStyle>(DisplayStyle.None) 
                : new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            
            (rootElement.Q<PropertyField>(_condition)).RegisterValueChangeCallback(
                evt =>
                {
                    memberElement.style.display = evt.changedProperty.boolValue
                        ? new StyleEnum<DisplayStyle>(DisplayStyle.None)
                        : new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                });
        }
    }
}