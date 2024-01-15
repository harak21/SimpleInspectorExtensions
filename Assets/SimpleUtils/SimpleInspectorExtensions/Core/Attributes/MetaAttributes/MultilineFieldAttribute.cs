using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.MetaAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class MultilineFieldAttribute : BaseExtensionAttribute
    {
        public override int Order => 5;
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            if (memberElement is TextField textField)
            {
                textField.multiline = true;
            }
        }
    }
}