using System;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class AnimatorParametersAttribute : CreationAttribute
    {
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            if (target is not Component gameObject)
                return;

            List<string> choices = new();
            var animator = gameObject.GetComponent<Animator>();
            foreach (var parameter in animator.parameters)
            {
                choices.Add(parameter.name);
            }

            var parent = memberElement.parent;
            var indexOf = parent.IndexOf(memberElement);
            parent.Remove(memberElement);
            var dropdown = new DropdownField
            {
                name = memberElement.name,
                label = memberElement.name,
                choices = choices
            };
            dropdown.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<string>(target, memberElement.name));

            dropdown.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, memberElement.name);
            });
            
            parent.Insert(indexOf, dropdown);
        }
    }
}