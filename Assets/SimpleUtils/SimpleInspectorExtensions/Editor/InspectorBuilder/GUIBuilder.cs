using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes;
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
        private static Dictionary<Type, ComponentInfo> _cashedTypes = new();

        internal static void AddInspectedType(ComponentInfo componentInfo)
        {
            _cashedTypes.Add(componentInfo.Type, componentInfo);
        }

        public static VisualElement CreateInspectorGUI(SerializedObject serializedObject, UnityEditor.Editor editor)
        {
            var componentInfo = _cashedTypes[serializedObject.targetObject.GetType()];

            var root = new VisualElement();
            root.AddToClassList("rootElement");

            CreateElements(serializedObject, root, componentInfo);

            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("SimpleInspectorLabelColors")[0])));
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("SimpleInspectorDefaultStyleSheet")[0])));
            return root;
        }

        private static void CreateElements(SerializedObject serializedObject, VisualElement root,
            ComponentInfo componentInfo)
        {
            CreateDefaultInspector(serializedObject, root);
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

                var element = root.Q<VisualElement>(attributeData.MemberInfo.Name);
                if (element == null)
                {
                    var type = ((FieldInfo)attributeData.MemberInfo).FieldType;
                    element = CreateElementView(type, attributeData.MemberInfo.Name, target);
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
                attributeData.Attribute.Execute(root, target, element);
            }
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

        private static void CreateButton(string name, VisualElement root)
        {
            root.Add(new Button()
            {
                name = name
            });
        }

        private static VisualElement CreateElementView(Type type, string fieldName, Object target)
        {
            if (type == typeof(int))
            {
                return ElementsFactory.CreateField<IntegerField, int>(fieldName, target);
            }

            if (type == typeof(long))
            {
                return ElementsFactory.CreateField<LongField, long>(fieldName, target);
            }

            if (type == typeof(bool))
            {
                return ElementsFactory.CreateField<Toggle, bool>(fieldName, target);
            }

            if (type == typeof(double))
            {
                return ElementsFactory.CreateField<DoubleField, double>(fieldName, target);
            }

            if (type == typeof(float))
            {
                return ElementsFactory.CreateField<FloatField, float>(fieldName, target);
            }

            if (type == typeof(string))
            {
                return ElementsFactory.CreateField<TextField, string>(fieldName, target);
            }

            if (type == typeof(Color))
            {
                return ElementsFactory.CreateField<ColorField, Color>(fieldName, target);
            }

            if (type == typeof(LayerMask))
            {
                return ElementsFactory.CreateLayerMask(fieldName, target);
            }

            if (type.BaseType == typeof(Enum))
            {
                //if (type.IsDefined(typeof (FlagsAttribute), false))
                {
                    return ElementsFactory.CreateEnumField(fieldName, target);
                }
            }

            if (type == typeof(Vector2))
            {
                return ElementsFactory.CreateField<Vector2Field, Vector2>(fieldName, target);
            }

            if (type == typeof(Vector3))
            {
                return ElementsFactory.CreateField<Vector3Field, Vector3>(fieldName, target);
            }

            if (type == typeof(Vector4))
            {
                return ElementsFactory.CreateField<Vector4Field, Vector4>(fieldName, target);
            }

            if (type == typeof(Rect))
            {
                return ElementsFactory.CreateField<RectField, Rect>(fieldName, target);
            }

            if (type == typeof(char))
            {
                return ElementsFactory.CreateCharField(fieldName, target);
            }

            if (type == typeof(AnimationCurve))
            {
                return ElementsFactory.CreateField<CurveField, AnimationCurve>(fieldName, target);
            }

            if (type == typeof(Bounds))
            {
                return ElementsFactory.CreateField<BoundsField, Bounds>(fieldName, target);
            }

            if (type == typeof(Gradient))
            {
                return ElementsFactory.CreateField<GradientField, Gradient>(fieldName, target);
            }

            if (type == typeof(Vector2Int))
            {
                return ElementsFactory.CreateField<Vector2IntField, Vector2Int>(fieldName, target);
            }

            if (type == typeof(Vector3Int))
            {
                return ElementsFactory.CreateField<Vector3IntField, Vector3Int>(fieldName, target);
            }

            if (type == typeof(RectInt))
            {
                return ElementsFactory.CreateField<RectIntField, RectInt>(fieldName, target);
            }

            if (type == typeof(BoundsInt))
            {
                return ElementsFactory.CreateField<BoundsIntField, BoundsInt>(fieldName, target);
            }

            if (typeof(Object).IsAssignableFrom(type))
            {
                return ElementsFactory.CreateObjectField(fieldName, type, target);
            }

            var ve = new VisualElement()
            {
                style =
                {
                    color = Color.white,
                    backgroundColor = new StyleColor(new Color32(56, 56, 56, 255)),
                    paddingTop = 5,
                    paddingBottom = 5,
                    paddingLeft = 5,
                    paddingRight = 5,
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderBottomLeftRadius = 5,
                    borderBottomRightRadius = 5,
                    borderTopRightRadius = 5,
                    borderTopLeftRadius = 5,
                    marginTop = 10,
                    marginBottom = 10,
                    marginLeft = 10,
                    marginRight = 10,
                    alignItems = new StyleEnum<Align>(Align.Center),
                    justifyContent = new StyleEnum<Justify>(Justify.Center)
                }
            };

            ve.Add(new Label($"Simple inspector doesn't support {type} types")
            {
                style =
                {
                    color = Color.white,
                }
            });

            return ve;
        }
    }
}