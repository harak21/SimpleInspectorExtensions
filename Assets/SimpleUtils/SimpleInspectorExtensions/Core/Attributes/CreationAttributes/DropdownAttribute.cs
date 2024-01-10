using System;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class DropdownAttribute : BaseExtensionAttribute
    {
        private readonly string _choicesSource;

        public DropdownAttribute(string choicesSource)
        {
            _choicesSource = choicesSource;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            memberElement.parent.Remove(memberElement);

            var choices = ReflectionUtility.GetMemberValue<List<string>>(target, _choicesSource);

            var dropdown = new DropdownField
            {
                choices = choices
            };
            
            dropdown.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<string>(target, memberElement.name));
            dropdown.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, memberElement.name);
            });
            
            rootElement.Add(dropdown);
        }
    }
}