using System;
using System.Diagnostics;
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
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            if (memberElement is not FloatField floatField)
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
                name = memberElement.name,
                title = Regex.Replace(memberElement.name.TrimStart('_'), "^[a-z]", c => c.Value.ToUpper()),
                lowValue = _min,
                highValue = _max
            };
            progressBar.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<float>(target, memberElement.name));
            floatField.RegisterValueChangedCallback(evt =>
            {
                var value = Mathf.Clamp(evt.newValue, _min, _max);
                floatField.SetValueWithoutNotify(value);
                ReflectionUtility.SetMemberValue(target, value, memberElement.name);
                progressBar.SetValueWithoutNotify(value);
            });
            //floatField.style.maxWidth = 200;
            //floatField.style.minWidth = 200;
            progressBar.style.overflow = new StyleEnum<Overflow>(Overflow.Hidden);
            progressBar.style.flexGrow = 1;

            //floatField.label = "";
            ve.Add(progressBar);
            ve.Add(floatField);
            parent.Insert(indexOf, ve);
        }
    }
}