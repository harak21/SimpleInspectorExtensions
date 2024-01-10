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
    public class IntSliderAttribute : BaseExtensionAttribute
    {
        private readonly int _minValue;
        private readonly int _maxValue;

        public IntSliderAttribute(int minValue = 0, int maxValue = 100)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            if (memberElement is not IntegerField)
                return;

            var parent = memberElement.parent;
            var indexOf = parent.IndexOf(memberElement);
            parent.Remove(memberElement);

            var slider = new SliderInt()
            {
                name = memberElement.name,
                label = Regex.Replace(memberElement.name.TrimStart('_'), "^[a-z]", c => c.Value.ToUpper()),
                lowValue = _minValue,
                highValue = _maxValue
            };
            slider.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<int>(target, memberElement.name));
            slider.showInputField = true;
            slider.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, memberElement.name);
            });
            parent.Insert(indexOf, slider);
        }
    }
}