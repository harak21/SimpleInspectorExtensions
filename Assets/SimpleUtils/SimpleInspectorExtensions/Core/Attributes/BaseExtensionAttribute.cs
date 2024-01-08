using System;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes
{
    public abstract class BaseExtensionAttribute : Attribute
    {
        public abstract void Execute(VisualElement rootElement, Object target, VisualElement memberElement);
    }
}