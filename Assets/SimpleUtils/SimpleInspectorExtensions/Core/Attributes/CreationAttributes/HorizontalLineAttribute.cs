using System;
using System.Diagnostics;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class HorizontalLineAttribute : CreationAttribute
    {
        private readonly int _width;
        private readonly int _spaceWidth;
        private readonly InspectorColor _lineColor;

        public HorizontalLineAttribute(int width = 3, int spaceWidth = 15, InspectorColor lineColor = InspectorColor.Black)
        {
            _width = width;
            _spaceWidth = spaceWidth;
            _lineColor = lineColor;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            var indexOf = memberElement.parent.IndexOf(memberElement);
            var ve = new VisualElement()
            {
                style =
                {
                    borderBottomWidth = _width,
                    borderBottomColor = _lineColor.Color(),
                    paddingTop = _spaceWidth,
                    marginBottom = _spaceWidth
                }
            };
            memberElement.parent.Insert(indexOf, ve);
        }
    }
}