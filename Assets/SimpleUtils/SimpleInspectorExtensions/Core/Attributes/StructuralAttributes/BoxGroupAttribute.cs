using System;
using System.Diagnostics;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class BoxGroupAttribute : StructuralAttribute
    {
        private readonly string _name;

        public BoxGroupAttribute(string name)
        {
            _name = name;
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
                };
                
                if (hasLabel)
                {
                    var label = new Label(_name);
                    label.AddToClassList("box-title-label");
                    group.Add(label);
                }

                var index= memberElement.parent.IndexOf(memberElement);
                memberElement.parent.Insert(index, group);
                //rootElement.Add(group);
            }
            
            group.Add(memberElement.parent != rootElement ? memberElement.parent : memberElement);
        }
    }
}