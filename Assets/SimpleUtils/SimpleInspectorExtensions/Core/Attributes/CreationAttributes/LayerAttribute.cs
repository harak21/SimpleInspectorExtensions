using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class LayerAttribute : CreationAttribute
    {
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            if (ReflectionUtility.GetMemberType(memberInfo) != typeof(int)) 
                return;
            
            var parent = memberElement.parent;
            var indexOf = parent.IndexOf(memberElement);
            parent.Remove(memberElement);
            var tagField = new LayerField()
            {
                name = memberElement.name,
                label = GetFormattedName(memberElement.name)
            };
            tagField.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<int>(target, memberElement.name));
            tagField.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, memberElement.name);
            });
            parent.Insert(indexOf, tagField);
        }
    }
}