using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    [Conditional("UNITY_EDITOR")]
    public class FoldoutAttribute : StructuralAttribute
    {
        private readonly string _name;

        public FoldoutAttribute(string name)
        {
            _name = name;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            if (memberElement is null)
                return;
            
            var group = rootElement.Q<Foldout>(_name);

            if (group == null)
            {
                group = new Foldout
                {
                    name = _name
                };

                group.text = _name;
                var index= memberElement.parent.IndexOf(memberElement);
                memberElement.parent.Insert(index, group);
                //rootElement.Add(group);
            }

            group.Add(memberElement.parent != rootElement ? memberElement.parent : memberElement);
        }
    }
}