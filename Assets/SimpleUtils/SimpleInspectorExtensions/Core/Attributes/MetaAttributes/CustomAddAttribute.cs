using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.MetaAttributes
{
    public class CustomAddAttribute : BaseExtensionAttribute
    {
        private readonly string _addFunction;

        public CustomAddAttribute(string addFunction)
        {
            _addFunction = addFunction;
        }
        
        public override int Order => 5;
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement, MemberInfo memberInfo)
        {
            switch (memberElement)
            {
                case PropertyField propertyField:
                    propertyField.schedule.Execute(() =>
                    {
                        ((ListViewData)propertyField.Q<ListView>().userData).CustomAddFunction = _addFunction;
                    }).ExecuteLater(500L);
                    break;
                case ListView listView:
                    ((ListViewData)listView.userData).CustomAddFunction = _addFunction;
                    break;
            }
        }
    }
}