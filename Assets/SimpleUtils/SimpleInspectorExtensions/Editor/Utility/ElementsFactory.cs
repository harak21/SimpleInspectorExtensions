using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.Utility
{
    internal static class ElementsFactory
    {
        public static VisualElement CreateElementView(Type type, string fieldName, object target)
        {
            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                return ElementsFactory.CreateListView(target, type, fieldName);
            }

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

        private static TField CreateField<TField, TFieldValue>(string fieldName, object target)
            where TField : BaseField<TFieldValue>, new()
        {
            var field = CreateBaseField<TField, TFieldValue>(fieldName);
            field.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<TFieldValue>(target, fieldName));
            field.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, fieldName);
            });
            return field;
        }

        private static TextField CreateCharField(string fieldName, object target)
        {
            var field = CreateBaseField<TextField, string>(fieldName);
            field.maxLength = 1;
            field.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<char>(target, fieldName).ToString());
            field.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue[0], fieldName);
            });
            return field;
        }

        private static LayerMaskField CreateLayerMask(string fieldName, object target)
        {
            var field = CreateBaseField<LayerMaskField, int>(fieldName);
            field.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<LayerMask>(target, fieldName));
            field.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, (LayerMask)evt.newValue, fieldName);
            });
            return field;
        }

        private static EnumField CreateEnumField(string fieldName, object target)
        {
            var field = CreateBaseField<EnumField, Enum>(fieldName);
            field.Init(ReflectionUtility.GetMemberValue<Enum>(target, fieldName));
            field.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, fieldName);
            });
            return field;
        }

        private static ObjectField CreateObjectField(string fieldName, Type objectType, object target)
        {
            var field = CreateBaseField<ObjectField, Object>(fieldName);
            field.objectType = objectType;
            field.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<Object>(target, fieldName));
            field.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, fieldName);
            });
            return field;
        }

        private static ListView CreateListView(object target, Type type, string fieldName)
        {
            var listView = new ListView
            {
                reorderMode = ListViewReorderMode.Animated,
                showBorder = true,
                showAddRemoveFooter = true,
                showBoundCollectionSize = true,
                showFoldoutHeader = true,
                headerTitle = GetLabel(fieldName),
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                showAlternatingRowBackgrounds = AlternatingRowBackground.None,
                viewDataKey = $"{((Object)target).name}-{fieldName}",
                name = fieldName,
                itemsSource = ReflectionUtility.GetMemberValue<IList>(target, fieldName),
                userData = new ListViewData()
                {
                    GenericArgs = type.GenericTypeArguments,
                    Target = target,
                    FieldType = type
                }
            };

            if (typeof(Object).IsAssignableFrom(type.GenericTypeArguments[0]))
            {
                listView.makeItem = MakeItem;
                listView.bindItem = BindItem;
            }
            else
            {
                //listView.reorderable = false;
                listView.makeItem = () =>
                {
                    var root = new VisualElement();
                    return root;
                };
                listView.bindItem = (ve, i) =>
                {
                    var memberType = ReflectionUtility.GetMemberType(target, fieldName);
                    var members = ReflectionUtility.GetPublicFieldsAndProperties(memberType.GenericTypeArguments[0]);

                    foreach (var member in members)
                    {
                        var element = ve.Q<VisualElement>(member.Name);
                        var memberType1 = ReflectionUtility.GetMemberType(member);
                        
                        if (memberType1 == memberType.GenericTypeArguments[0])
                            continue;

                        if (element is null)
                        {
                            element = CreateElementView(memberType1, member.Name, listView.itemsSource[i]);
                            ve.Add(element);
                        }

                        var value = ReflectionUtility.GetMemberValue<object>(listView.itemsSource[i], member.Name);
                        var m = (PropertyInfo)ReflectionUtility.GetMember(element, "value");
                        m.SetValue(element, value);
                    }
                };
            }

            listView.itemIndexChanged += (_, _) =>
            {
                if (target is Object o)
                {
                    EditorUtility.SetDirty(o);
                }
            };
            listView.itemsAdded += ints =>
            {
                var listViewData = (ListViewData)listView.userData;
                if (!string.IsNullOrEmpty(listViewData.CustomAddFunction))
                {
                    foreach (var i in ints)
                    {
                        var item = ReflectionUtility.InvokeMethod(target, listViewData.CustomAddFunction);
                        listView.itemsSource[i] = item;
                    }
                }
            };

            return listView;
        }

        private static void BindItem(VisualElement ve, int i)
        {
            var listView = ve.GetFirstAncestorOfType<ListView>();
            var listViewUserData = (ListViewData)listView.userData;
            var args = listViewUserData.GenericArgs;
            var objectField = (ObjectField)ve;
            objectField.objectType = args[0];
            objectField.userData = i;
            objectField.SetValueWithoutNotify((Object)listView.itemsSource[i]);
        }

        private static VisualElement MakeItem()
        {
            var objectField = new ObjectField();
            objectField.RegisterValueChangedCallback(evt =>
            {
                var listView = objectField.GetFirstAncestorOfType<ListView>();
                listView.itemsSource[(int)objectField.userData] = evt.newValue;
                var listViewData = ((ListViewData)listView.userData);
                var target = listViewData.Target;
                if (target is Object o)
                {
                    EditorUtility.SetDirty(o);
                }
            });
            return objectField;
        }

        private static TField CreateBaseField<TField, TFieldValue>(string fieldName)
            where TField : BaseField<TFieldValue>, new()
        {
            var field = new TField
            {
                name = fieldName,
                label = GetLabel(fieldName)
            };
            return field;
        }

        private static string GetLabel(string fieldName)
        {
            return Regex.Replace(fieldName.TrimStart('_'), "^[a-z]", c => c.Value.ToUpper());
        }
    }
}