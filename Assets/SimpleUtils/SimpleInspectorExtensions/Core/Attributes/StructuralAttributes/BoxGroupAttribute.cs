using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes
{
    public class BoxGroupAttribute : BaseExtensionAttribute
    {
        private readonly InspectorColor _labelColor;
        private readonly int _margin;
        private readonly int _padding;
        private readonly int _borderWidth;
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
            this(name, backgroundColor, labelColor, 0)
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
            int borderRadius = 0)
        {
            _name = name;
            _backgroundColor = backgroundColor;
            _labelColor = labelColor;
            _margin = margin;
            _padding = padding;
            _borderWidth = borderWidth;
            _borderColor = borderColor;
            _borderRadius = borderRadius;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            var group = rootElement.Q<Box>(_name);

            if (group == null)
            {
                group = new Box
                {
                    name = _name,
                    style =
                    {
                        marginTop = _margin,
                        marginBottom = _margin,
                        marginLeft = _margin,
                        marginRight = _margin,
                        paddingTop = _padding + 20,
                        paddingBottom = _padding,
                        paddingLeft = _padding,
                        paddingRight = _padding,
                        borderTopWidth = _borderWidth,
                        borderBottomWidth = _borderWidth,
                        borderLeftWidth = _borderWidth,
                        borderRightWidth = _borderWidth,
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
                rootElement.Add(group);
            }
            
            group.Add(memberElement);
        }
    }
}