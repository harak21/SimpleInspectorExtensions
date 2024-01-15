using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class MinMaxSliderAttribute : CreationAttribute
    {
        private readonly float _min;
        private readonly float _max;
        private readonly float _minValueRange;
        private readonly float _maxValueRange;

        public MinMaxSliderAttribute(float min = 0f, float max = 100f)
        {
            _min = min;
            _max = max;
            _minValueRange = 0;
            _maxValueRange = 0;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            if (ReflectionUtility.GetMemberType(memberInfo) != typeof(Vector2))
                return;

            var parent = memberElement.parent;
            var indexOf = parent.IndexOf(memberElement);
            parent.Remove(memberElement);

            var minMaxSlider = new MinMaxSlider()
            {
                name = memberInfo.Name,
                label = GetFormattedName(memberInfo.Name),
                lowLimit = _min,
                highLimit = _max,
                minValue = _minValueRange,
                maxValue = _maxValueRange
            };
            minMaxSlider.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<Vector2>(target, memberElement.name));
            minMaxSlider.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, memberElement.name);
            });
            parent.Insert(indexOf, minMaxSlider);
        }
    }
}