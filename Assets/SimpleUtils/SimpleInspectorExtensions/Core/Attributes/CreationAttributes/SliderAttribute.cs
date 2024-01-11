using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
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
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            if (memberElement is not FloatField)
                return;

            var parent = memberElement.parent;
            var indexOf = parent.IndexOf(memberElement);
            parent.Remove(memberElement);

            var slider = new Slider
            {
                name = memberElement.name,
                label = Regex.Replace(memberElement.name.TrimStart('_'), "^[a-z]", c => c.Value.ToUpper()),
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