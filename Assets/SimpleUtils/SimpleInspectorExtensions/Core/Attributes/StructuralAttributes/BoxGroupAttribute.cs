using System;
using System.Diagnostics;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class BoxGroupAttribute : BaseExtensionAttribute
    {
        private readonly InspectorColor _labelColor;
        private readonly Vector4 _margin;
        private readonly Vector4 _padding;
        private readonly Vector4 _borderWidth;
        private readonly InspectorColor _borderColor;
        private readonly int _borderRadius;
        private readonly InspectorColor _backgroundColor;
        private readonly string _name;

        public BoxGroupAttribute(string name) : this (name, InspectorColor.Clear)
        {
            _name = name;
        }

        public BoxGroupAttribute(string name, InspectorColor backgroundColor) : this(name, backgroundColor, InspectorColor.Black)
        {
            _backgroundColor = backgroundColor;
        }

        public BoxGroupAttribute(string name, InspectorColor backgroundColor, InspectorColor labelColor) : 
            this(name, backgroundColor, labelColor, new Vector4())
        {
            _labelColor = labelColor;
        }

        public BoxGroupAttribute(string name, 
            InspectorColor backgroundColor = InspectorColor.Clear, 
            InspectorColor labelColor = InspectorColor.Black,
            int margin = 0,
            int padding = 0,
            int borderWidth = 0,
            InspectorColor borderColor = InspectorColor.Clear,
            int borderRadius = 0) : this(name, backgroundColor, labelColor, borderColor, borderRadius)
        {
            _margin = new Vector4(margin, margin, margin, margin);
            _padding = new Vector4(padding, padding, padding, padding);
            _borderWidth = new Vector4(borderWidth, borderWidth, borderWidth, borderWidth);
        }

        public BoxGroupAttribute(string name, 
            InspectorColor backgroundColor = InspectorColor.Clear, 
            InspectorColor labelColor = InspectorColor.Black,
            Vector4 margin = default,
            Vector4 padding = default,
            Vector4 borderWidth = default,
            InspectorColor borderColor = InspectorColor.Clear,
            int borderRadius = 0) : this(name, backgroundColor, labelColor, borderColor, borderRadius)
        {
            _margin = margin == default ? new Vector4(6,1,3,3) : margin;
            _padding = padding == default ? new Vector4(5,5,3,3) : padding;
            _borderWidth = borderWidth;
        }

        public BoxGroupAttribute(string name, 
            InspectorColor backgroundColor = InspectorColor.Clear, 
            InspectorColor labelColor = InspectorColor.Black,
            InspectorColor borderColor = InspectorColor.Clear,
            int borderRadius = 0)
        {
            _name = name;
            _backgroundColor = backgroundColor;
            _labelColor = labelColor;
            _borderColor = borderColor;
            _borderRadius = borderRadius;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            var group = rootElement.Q<Box>(_name);

            bool hasLabel = !string.IsNullOrEmpty(_name);

            if (group == null)
            {
                group = new Box
                {
                    name = _name,
                    style =
                    {
                        marginTop = _margin.x,
                        marginBottom = _margin.y,
                        marginLeft = _margin.z,
                        marginRight = _margin.w,
                        paddingTop = _padding.x + (hasLabel ? 20 : 0),
                        paddingBottom = _padding.y,
                        paddingLeft = _padding.z,
                        paddingRight = _padding.w,
                        borderTopWidth = _borderWidth.x,
                        borderBottomWidth = _borderWidth.y,
                        borderLeftWidth = _borderWidth.z,
                        borderRightWidth = _borderWidth.w,
                        borderTopColor = _borderColor.Color(),
                        borderBottomColor = _borderColor.Color(),
                        borderLeftColor = _borderColor.Color(),
                        borderRightColor = _borderColor.Color(),
                        borderTopLeftRadius = _borderRadius,
                        borderBottomRightRadius = _borderRadius,
                        borderTopRightRadius = _borderRadius,
                        borderBottomLeftRadius = _borderRadius,
                        backgroundColor = _backgroundColor.Color()
                    }
                };
                
                if (hasLabel)
                {
                    var label = new Label(_name)
                    {
                        style =
                        {
                            color = _labelColor.Color(),
                            position = new StyleEnum<Position>(Position.Absolute),
                            top = 2,
                            left = 2
                        }
                    };
                    group.Add(label);
                }
                
                rootElement.Add(group);
            }
            
            group.Add(memberElement.parent != rootElement ? memberElement.parent : memberElement);
        }
    }
}