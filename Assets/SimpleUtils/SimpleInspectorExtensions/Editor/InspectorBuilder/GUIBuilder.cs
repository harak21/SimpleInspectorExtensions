using System;
using System.Collections.Generic;
using System.Linq;
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
                        element = CreateElementView(member, root, target);
                        element.name = memberInfo.Name;
                        root.Add(element);
                    }
                    
                    attribute.Execute(root, target, element);
                }
            }
        }

        private static void CreateButton(MemberData memberData, VisualElement root)
        {
            root.Add(new Button()
            {
                name = memberData.MemberInfo.Name
            });
        }

        private static VisualElement CreateElementView(MemberData member, VisualElement root, Object target)
        {
            var type = ((FieldInfo)member.MemberInfo).FieldType;
            var name = member.MemberInfo.Name;

            //if (type.IsGenericType)
            //{
            //    //if (type.IsArray)
            //    {
            //        var source = ReflectionUtility.GetMemberValue<List<object>>(target, name);
            //        return ElementsFactory.CreateListView(member, target, source);
            //    }
            //}

            if (type == typeof(int))
            {
               return ElementsFactory.CreateField<IntegerField, int>(member, target);
            }

            if (type == typeof(long))
            {
                return ElementsFactory.CreateField<LongField, long>(member, target);
            }

            if (type == typeof(bool))
            {
                return ElementsFactory.CreateField<Toggle, bool>(member, target);
            }

            if (type == typeof(double))
            {
                return ElementsFactory.CreateField<DoubleField, double>(member, target);
            }

            if (type == typeof(float))
            {
                return ElementsFactory.CreateField<FloatField, float>(member, target);
            }

            if (type == typeof(string))
            {
                return ElementsFactory.CreateField<TextField, string>(member, target);
            }

            if (type == typeof(Color))
            {
                return ElementsFactory.CreateField<ColorField, Color>(member, target);
            }

            if (type == typeof(LayerMask))
            {
                return ElementsFactory.CreateLayerMask(member, target);
            }

            if (type.BaseType == typeof(Enum))
            {
                //if (type.IsDefined(typeof (FlagsAttribute), false))
                {
                    return ElementsFactory.CreateEnumField(member, target);
                }
            }

            if (type == typeof(Vector2))
            {
                return ElementsFactory.CreateField<Vector2Field, Vector2>(member, target);
            }

            if (type == typeof(Vector3))
            {
                return ElementsFactory.CreateField<Vector3Field, Vector3>(member, target);
            }

            if (type == typeof(Vector4))
            {
                return ElementsFactory.CreateField<Vector4Field, Vector4>(member, target);
            }
            
            if (type == typeof(Rect))
            {
                return ElementsFactory.CreateField<RectField, Rect>(member, target);
            }

            if (type == typeof(char))
            {
                return ElementsFactory.CreateCharField(member, target);
            }

            if (type == typeof(AnimationCurve))
            {
                return ElementsFactory.CreateField<CurveField, AnimationCurve>(member, target);
            }

            if (type == typeof(Bounds))
            {
                return ElementsFactory.CreateField<BoundsField, Bounds>(member, target);
            }

            if (type == typeof(Gradient))
            {
                return ElementsFactory.CreateField<GradientField, Gradient>(member, target);
            }

            if (type == typeof(Vector2Int))
            {
                return ElementsFactory.CreateField<Vector2IntField, Vector2Int>(member, target);
            }

            if (type == typeof(Vector3Int))
            {
                return ElementsFactory.CreateField<Vector3IntField, Vector3Int>(member, target);
            }

            if (type == typeof(RectInt))
            {
                return ElementsFactory.CreateField<RectIntField, RectInt>(member, target);
            }

            if (type == typeof(BoundsInt))
            {
                return ElementsFactory.CreateField<BoundsIntField, BoundsInt>(member, target);
            }

            if (typeof(Object).IsAssignableFrom(type))
            {
                return ElementsFactory.CreateObjectField(member, target);
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
            
            ve.Add(new Label($"Simple inspector doesn't support {type} types"){
                style =
                {
                    color = Color.white,
                }});
            
            return ve;
        }
    }
}