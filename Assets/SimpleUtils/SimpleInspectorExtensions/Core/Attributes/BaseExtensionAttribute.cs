using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes
{
    [Conditional("UNITY_EDITOR")]
    public abstract class BaseExtensionAttribute : Attribute
    {
        public abstract int Order { get; }
        public abstract void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo);

        protected string GetFormattedName(string fieldName)
        {
            return Regex.Replace(fieldName.TrimStart('_'), "^[a-z]", c => c.Value.ToUpper());
        }
    }
}