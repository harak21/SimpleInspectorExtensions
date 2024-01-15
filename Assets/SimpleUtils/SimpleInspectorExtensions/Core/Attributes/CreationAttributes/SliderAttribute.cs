using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class SliderAttribute : CreationAttribute
    {
        private readonly float _minValue;
        private readonly float _maxValue;

        public SliderAttribute(float minValue = 0f, float maxValue = 100f)
        {
            _minValue = minValue;
            _maxValue = maxValue; 
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            if (ReflectionUtility.GetMemberType(memberInfo) != typeof(float))
                return;

            var parent = memberElement.parent;
            var indexOf = parent.IndexOf(memberElement);
            parent.Remove(memberElement);

            var slider = new Slider
            {
                name = memberInfo.Name,
                label = GetFormattedName(memberInfo.Name),
                lowValue = _minValue,
                highValue = _maxValue
            };
            slider.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<float>(target, memberElement.name));
            slider.showInputField = true;
            slider.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, memberElement.name);
            });
            parent.Insert(indexOf, slider);
        }
    }
}