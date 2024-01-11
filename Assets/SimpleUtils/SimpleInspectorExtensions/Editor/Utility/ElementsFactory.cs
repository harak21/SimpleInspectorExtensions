using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.Utility
{
    internal static class ElementsFactory
    {
        public static ListView CreateListView<T>(MemberData memberData, Object target, List<T> source)
        {
            var listView = new ListView();
            listView.reorderMode = ListViewReorderMode.Animated;
            listView.showBorder = true;
            listView.showAddRemoveFooter = true;
            listView.showBoundCollectionSize = true;
            listView.showFoldoutHeader = true;
            listView.headerTitle = memberData.MemberInfo.Name;
            listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            listView.userData = source;
            listView.showAlternatingRowBackgrounds = AlternatingRowBackground.None;
            listView.viewDataKey = $"{target.name}-{memberData.MemberInfo.Name}";
            listView.name = memberData.MemberInfo.Name;
            return listView;
        }

        public static TField CreateField<TField, TFieldValue>(string fieldName, Object target) 
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

        public static TextField CreateCharField(string fieldName, Object target)
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

        public static LayerMaskField CreateLayerMask(string fieldName, Object target)
        {
            var field = CreateBaseField<LayerMaskField, int>(fieldName);
            field.SetValueWithoutNotify(ReflectionUtility.GetMemberValue<LayerMask>(target, fieldName));
            field.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, (LayerMask)evt.newValue, fieldName);
            });
            return field;
        }

        public static EnumField CreateEnumField(string fieldName, Object target)
        {
            var field = CreateBaseField<EnumField, Enum>(fieldName);
            field.Init(ReflectionUtility.GetMemberValue<Enum>(target, fieldName));
            field.RegisterValueChangedCallback(evt =>
            {
                ReflectionUtility.SetMemberValue(target, evt.newValue, fieldName);
            });
            return field;
        }

        public static ObjectField CreateObjectField(string fieldName, Type objectType, Object target)
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

        private static TField CreateBaseField<TField, TFieldValue>(string fieldName) where TField : BaseField<TFieldValue>, new()
        {
            var field = new TField
            {
                name = fieldName,
                label = Regex.Replace(fieldName.TrimStart('_'), "^[a-z]", c => c.Value.ToUpper())
            };
            return field;
        }
    }
}