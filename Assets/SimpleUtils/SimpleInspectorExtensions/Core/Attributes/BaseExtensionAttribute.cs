using System;
using System.Diagnostics;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes
{
    [Conditional("UNITY_EDITOR")]
    public abstract class BaseExtensionAttribute : Attribute
    {
        public abstract void Execute(VisualElement rootElement, Object target, VisualElement memberElement);
    }
}