using System;
using System.Diagnostics;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class FoldoutAttribute : BaseExtensionAttribute
    {
        private readonly string _name;

        public FoldoutAttribute(string name)
        {
            _name = name;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            var group = rootElement.Q<Foldout>(_name);
            
            bool hasLabel = !string.IsNullOrEmpty(_name);

            if (group == null)
            {
                group = new Foldout
                {
                    name = _name
                };

                group.text = _name;
                rootElement.Add(group);
            }

            group.Add(memberElement.parent != rootElement ? memberElement.parent : memberElement);
        }
    }
}