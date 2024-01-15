using System;
using System.Diagnostics;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class StyleSheetAttribute : StyleExtensionAttribute
    {
        private readonly string _styleSheetName;
        private readonly bool _parent;

        public StyleSheetAttribute(string styleSheetName, bool parent = false)
        {
            _styleSheetName = styleSheetName;
            _parent = parent;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var findAssets = AssetDatabase.FindAssets(_styleSheetName);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                AssetDatabase.GUIDToAssetPath(findAssets[0]));

            if (styleSheet != null)
            {
                var targetElement = _parent ? memberElement.parent : memberElement;
                targetElement.styleSheets.Add(styleSheet);
            }
        }
    }
}