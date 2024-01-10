using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class RadioButtonGroupAttribute : BaseExtensionAttribute
    {
        private readonly string _name;

        public RadioButtonGroupAttribute(string name)
        {
            _name = name;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            if (memberElement is not Toggle toggle)
                return;
            var label = Regex.Replace(memberElement.name.TrimStart('_'), "^[a-z]", c => c.Value.ToUpper());

            var parent = memberElement.parent;
            var indexOf = parent.IndexOf(memberElement);
            parent.Remove(memberElement);

            var radioButtonGroup = rootElement.Q<RadioButtonGroup>(_name);
            if (radioButtonGroup == null)
            {
                CreateGroup(target, memberElement, parent, indexOf, label, toggle);
            }
            else
            {
                var choices = radioButtonGroup.choices.ToList();
                choices.Add(label);
                radioButtonGroup.choices = choices;
                var userData = (List<string>)radioButtonGroup.userData;
                userData.Add(memberElement.name);
            
                if (toggle.value)
                {
                    radioButtonGroup.SetValueWithoutNotify(choices.IndexOf(label));
                }
            }
            
        }

        private void CreateGroup(Object target, VisualElement memberElement,VisualElement parent,int indexOf, string label, Toggle toggle)
        {
            var radioButtonGroup = new RadioButtonGroup
            {
                name = _name,
                label = _name
            };

            radioButtonGroup.RegisterValueChangedCallback(evt =>
            {
                var fields = (List<string>)radioButtonGroup.userData;
                for (int i = 0; i < fields.Count; i++)
                {
                    ReflectionUtility.SetMemberValue(target, i == evt.newValue, fields[i]);
                }
            });
            parent.Insert(indexOf, radioButtonGroup);
            radioButtonGroup.choices = new List<string>() { label };
            radioButtonGroup.userData = new List<string>() { memberElement.name };

            if (toggle.value)
            {
                radioButtonGroup.SetValueWithoutNotify(0);
            }
        }
    }
}