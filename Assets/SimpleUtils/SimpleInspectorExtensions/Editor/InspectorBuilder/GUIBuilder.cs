using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.InspectorBuilder
{
    public static class GUIBuilder
    {
        private static Dictionary<Type, ComponentInfo> _cashedTypes = new();
        
        internal static void AddInspectedType(ComponentInfo componentInfo)
        {
            _cashedTypes.Add(componentInfo.Type, componentInfo);
        }

        public static VisualElement CreateInspectorGUI(SerializedObject serializedObject, UnityEditor.Editor editor)
        {
            var componentInfo = _cashedTypes[serializedObject.targetObject.GetType()];
            var changedMembers = componentInfo.MemberData.Select(m => m.MemberInfo.Name).ToList();

            var root = new VisualElement();
            
            CreateDefaultInspector(serializedObject, root);
            CreateCustomInspector(componentInfo, root, serializedObject.targetObject);

            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("SimpleInspectorLabelColors")[0])));
            return root;
        }

        private static void CreateDefaultInspector(SerializedObject serializedObject, VisualElement root)
        {
            SerializedProperty iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                var propertyField = new PropertyField(iterator)
                {
                    name = iterator.name
                };
                root.Add(propertyField);
            }
        }
        private static void CreateCustomInspector(ComponentInfo componentInfo, VisualElement root, Object target)
        {
            foreach (var member in componentInfo.MemberData)
            {
                var memberInfo = member.MemberInfo;
                foreach (var attribute in member.Attributes)
                {
                    if (attribute is ButtonAttribute)
                    {
                        CreateButton(member, root);
                    }
                    
                    var element = root.Q<VisualElement>(memberInfo.Name);
                    if (element == null)
                    {
                        CreateElementView(member, root, target);
                        Debug.LogError($"Element {memberInfo.Name} not found");
                        continue;
                    }
                    
                    attribute.Execute(root, target, element);
                }
            }
        }

        private static void CreateElementView(MemberData member, VisualElement root, Object target)
        {
            
            
            var type = ((FieldInfo)member.MemberInfo).FieldType;
            var name = member.MemberInfo.Name;
            if (type == typeof(string))
            {
                var text = new TextField(name);
                text.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<string>(target, name));
                text.RegisterValueChangedCallback(evt => ReflectionUtility.SetMemberValue(target, evt.newValue, name));
                root.Add(text);
            }
        }

        private static void CreateButton(MemberData memberData, VisualElement root)
        {
            root.Add(new Button()
            {
                name = memberData.MemberInfo.Name
            });
        }
    }
}