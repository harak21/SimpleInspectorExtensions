﻿using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    [Conditional("UNITY_EDITOR")]
    public class ButtonAttribute : CreationAttribute
    {
        private readonly string _name;

        public ButtonAttribute(string name)
        {
            _name = name;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var button = rootElement.Q<Button>(memberElement.name);
            button.text = _name;
            button.clicked += () => ReflectionUtility.InvokeMethod(target, memberElement.name);
        }
    }
}