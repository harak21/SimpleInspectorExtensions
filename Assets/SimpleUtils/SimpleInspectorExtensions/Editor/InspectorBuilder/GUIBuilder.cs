using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData;
using SimpleUtils.SimpleInspectorExtensions.Editor.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.InspectorBuilder
{
    public static class GUIBuilder
    {
        private static readonly Dictionary<Type, ComponentInfo> CashedTypes = new();
        private static StyleSheet _defaultStyleSheet;

        internal static void AddInspectedType(ComponentInfo componentInfo)
        {
            CashedTypes.Add(componentInfo.Type, componentInfo);
        }

        public static VisualElement CreateInspectorGUI(SerializedObject serializedObject)
        {
            var componentInfo = CashedTypes[serializedObject.targetObject.GetType()];

            var root = new VisualElement();
            root.AddToClassList("rootElement");
            
            _defaultStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("SimpleInspectorDefaultStyleSheet")[0]));
            root.styleSheets.Add(_defaultStyleSheet);
            
            CreateElements(serializedObject, root, componentInfo);
            return root;
        }

        private static void CreateElements(SerializedObject serializedObject, VisualElement root,
            ComponentInfo componentInfo)
        {
            CreateDefaultInspector(serializedObject, root, componentInfo);
            CreateCustomInspector(componentInfo, root, serializedObject.targetObject, serializedObject);
        }

        private static void CreateCustomInspector(ComponentInfo componentInfo, VisualElement root, Object target,
            SerializedObject serializedObject)
        {
            var members = ReflectionUtility.GetMembers(target);

            foreach (var attributeData in componentInfo.AttributesData)
            {
                if (attributeData.Attribute is ButtonAttribute)
                {
                    CreateButton(attributeData.MemberInfo.Name, root);
                }

                if (attributeData.Attribute is DisableDefaultStyleSheet)
                {
                    root.styleSheets.Remove(_defaultStyleSheet);
                }

                var element = root.Q<VisualElement>(attributeData.MemberInfo.Name);
                if (element == null && attributeData.MemberInfo is not MethodInfo)
                {
                    var type = ReflectionUtility.GetMemberType(attributeData.MemberInfo);
                    element = ElementsFactory.CreateElementView(type, attributeData.MemberInfo.Name, target);
                    element.name = attributeData.MemberInfo.Name;
                    var index = members.IndexOf(attributeData.MemberInfo);
                    bool hasOrder = true;
                    while (serializedObject.FindProperty(members[index].Name) == null)
                    {
                        if (index >= members.Count - 1)
                        {
                            hasOrder = false;
                            break;
                        }

                        index++;
                    }

                    if (hasOrder)
                    {
                        var order = root.IndexOf(root.Q<VisualElement>(members[index].Name));
                        if (order < root.childCount && order != -1)
                        {
                            root.Insert(order, element);
                        }
                        else
                        {
                            root.Add(element);
                        }
                    }
                    else
                    {
                        root.Add(element);
                    }
                }
                attributeData.Attribute.Execute(root, target, element, attributeData.MemberInfo);
            }
        }

        private static void CreateDefaultInspector(SerializedObject serializedObject, VisualElement root,
            ComponentInfo componentInfo)
        {
            SerializedProperty iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                //if (componentInfo.AttributesData.FirstOrDefault(d => d.MemberInfo.Name == iterator.name) != null)
                //    continue;

                var propertyField = new PropertyField(iterator)
                {
                    name = iterator.name
                };
                root.Add(propertyField);
            }
        }

        private static void CreateButton(string name, VisualElement root)
        {
            root.Add(new Button()
            {
                name = name
            });
        }
    }
}