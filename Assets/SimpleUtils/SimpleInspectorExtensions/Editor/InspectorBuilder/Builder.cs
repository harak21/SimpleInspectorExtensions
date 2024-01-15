﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.InspectorBuilder
{
    internal static class Builder
    {
        [InitializeOnLoadMethod] 
        private static void Init()
        {
            InitUnityEditorSystem();
            var components = ComponentsGatherer.Gather().ToArray();
            
            foreach ((Type inspectedType, Type editorType) in EditorGenerator.GenerateForComponents(components))
            {
                RegisterEditor(inspectedType, editorType, false);
            }
        }

        private static void InitUnityEditorSystem()
        {
            var editorAssembly = typeof(CustomEditor).Assembly;
            var customEditorAttributesType = editorAssembly.GetType("UnityEditor.CustomEditorAttributes");
            var findMethod = customEditorAttributesType.GetMethod("FindCustomEditorTypeByType",
                BindingFlags.Static | BindingFlags.NonPublic, null,
                new[] { typeof(Type), typeof(bool) }, null);
            findMethod.Invoke(null, new object[] { typeof(UtilityMonoBehaviour), false });
        }
        
        private static void RegisterEditor(Type inspectedType, Type inspectorType, bool isSupportMultiEditing)
        {
            var editorAssembly = typeof(CustomEditor).Assembly;
            var customEditorAttributesType = editorAssembly.GetType("UnityEditor.CustomEditorAttributes");
            var monoEditorType = editorAssembly.GetType("UnityEditor.CustomEditorAttributes+MonoEditorType");

            var editorsDictionary = customEditorAttributesType.GetField(
                isSupportMultiEditing ? "kSCustomMultiEditors" : "kSCustomEditors",
                BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

            Type editorsDictionaryType = editorsDictionary.GetType();
            var containsKeyMethod = editorsDictionaryType.GetMethod("ContainsKey", new[] { typeof(Type) });
            var indexerProperty = editorsDictionaryType.GetProperty("Item");

            object editorList = null;
            if ((bool)containsKeyMethod.Invoke(editorsDictionary, new object[] { inspectedType }))
            {
                editorList = indexerProperty.GetValue(editorsDictionary, new object[] { inspectedType });
            }
            else
            {
                editorList = Activator.CreateInstance(typeof(List<>).MakeGenericType(monoEditorType));
                indexerProperty.SetValue(editorsDictionary, editorList, new object[] { inspectedType });
            }

            var monoEditor = Activator.CreateInstance(monoEditorType);
            monoEditorType.GetField("m_InspectedType").SetValue(monoEditor, inspectedType);
            monoEditorType.GetField("m_InspectorType").SetValue(monoEditor, inspectorType);
            ((IList)editorList).Insert(0, monoEditor);
        }
    }
}