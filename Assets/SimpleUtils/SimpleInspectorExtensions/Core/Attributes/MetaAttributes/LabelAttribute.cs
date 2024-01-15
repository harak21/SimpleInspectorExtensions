using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.MetaAttributes
{
    public class LabelAttribute : BaseExtensionAttribute
    {
        private readonly string _label;

        public LabelAttribute(string label)
        {
            _label = label;
        }
        
        public override int Order => 5;
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var memberInfos = memberElement.GetType().GetMember("label", BindingFlags.Instance | BindingFlags.Public);
            var hasLabel = 
                memberInfos.Length > 0;
            if (hasLabel)
            {
                ReflectionUtility.SetMemberValue(memberElement, _label,"label" );
            }
        }
    }
}