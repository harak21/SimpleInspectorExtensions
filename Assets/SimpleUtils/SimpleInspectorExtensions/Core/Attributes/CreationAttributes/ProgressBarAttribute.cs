using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class ProgressBarAttribute : CreationAttribute
    {
        private readonly float _min;
        private readonly float _max;

        public ProgressBarAttribute(float min = 0f, float max = 100f)
        {
            _min = min;
            _max = max;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            if (ReflectionUtility.GetMemberType(memberInfo) != typeof(float))
                return;
            

            var ve = new VisualElement
            {
                style =
                {
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row)
                }
            };

            var parent = memberElement.parent;
            var indexOf = parent.IndexOf(memberElement);
            parent.Remove(memberElement);

            var progressBar = new ProgressBar()
            {
                name = memberInfo.Name,
                title = GetFormattedName(memberInfo.Name),
                lowValue = _min,
                highValue = _max
            };
            progressBar.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<float>(target, memberElement.name));
            progressBar.style.overflow = new StyleEnum<Overflow>(Overflow.Hidden);
            progressBar.style.flexGrow = 1;

            ve.Add(progressBar);
            switch (memberElement)
            {
                case FloatField floatField:
                    floatField.RegisterValueChangedCallback(evt =>
                    {
                        var value = Mathf.Clamp(evt.newValue, _min, _max);
                        floatField.SetValueWithoutNotify(value);
                        ReflectionUtility.SetMemberValue(target, value, memberElement.name);
                        progressBar.SetValueWithoutNotify(value);
                    });
                    ve.Add(floatField);
                    break;
                case PropertyField propertyField:
                    propertyField.RegisterValueChangeCallback(evt =>
                    {
                        var value = Mathf.Clamp(evt.changedProperty.floatValue, _min, _max);
                        ReflectionUtility.SetMemberValue(target, value, memberElement.name);
                        progressBar.SetValueWithoutNotify(value);
                    });
                    ve.Add(propertyField);
                    break;
            }
            parent.Insert(indexOf, ve);
        }
    }
}