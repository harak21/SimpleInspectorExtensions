using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData;
using UnityEditor;
using UnityEngine;

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

    internal class EditorGenerator
    {
        public static IEnumerable<(Type InspectedType, Type EditorType)> GenerateForComponents(
            ComponentInfo[] componentInfos)
        {
            var editorNames = new List<string>();
            
            var sourceBuilder = new EditorSourceBuilder();

            foreach (var componentInfo in componentInfos)
            {
                sourceBuilder.AddEditor(componentInfo, out var editorName);
                editorNames.Add(editorName);
            }
            
            var source = sourceBuilder.Build();

            var componentAssemblies = componentInfos.Select(p => p.Type.Assembly).Distinct().ToList();
            var assembly = CompileAssemblyFromSource(source, componentAssemblies);
            for (var index = 0; index < componentInfos.Length; index++)
            {
                ComponentInfo componentType = componentInfos[index];
                string editorName = editorNames[index];

                var editor = assembly.GetType(editorName);
                if (editor == null) continue;

                yield return (componentType.Type, editor);
            }
        }
        
        private static Assembly CompileAssemblyFromSource(string sourceCode, IEnumerable<Assembly> assemblies)
        {
            var csProvider = new CSharpCodeProvider();
            var compParams = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                ReferencedAssemblies =
                {
                    typeof(UnityEngine.Object).Assembly.Location,
                    typeof(UnityEditor.Editor).Assembly.Location,
                    typeof(GUILayoutOption).Assembly.Location,
                    typeof(EditorSourceBuilder).Assembly.Location,
                    typeof(BaseExtensionAttribute).Assembly.Location,
                    typeof(UnityEngine.UIElements.VisualElement).Assembly.Location
                }
            };
            foreach (Assembly assembly in assemblies)
            {
                compParams.ReferencedAssemblies.Add(assembly.Location);
            }

            var ass = Assembly.Load("netstandard, Version=2.1.0.0");
            compParams.ReferencedAssemblies.Add(ass.Location);

            CompilerResults compilerResults = csProvider.CompileAssemblyFromSource(compParams, sourceCode);

            foreach (CompilerError error in compilerResults.Errors)
            {
                Debug.LogError(error);
            }

            return compilerResults.CompiledAssembly;
        }
    }

    internal class EditorSourceBuilder
    {
        private readonly StringBuilder _sb = new StringBuilder();
        
        public EditorSourceBuilder()
        {
            _sb.AppendLine("using System;");
            _sb.AppendLine("using UnityEngine;");
            _sb.AppendLine("using UnityEngine.UIElements;");
            _sb.AppendLine("using UnityEditor;");
            _sb.AppendLine("using UnityEditor.UIElements;");
            _sb.AppendLine("using System.Reflection;");
            _sb.AppendLine("using SimpleUtils.SimpleInspectorExtensions.Editor.InspectorBuilder;");
        }
        
        public string Build() => _sb.ToString();
        
        public void AddEditor(ComponentInfo componentInfo, out string editorName)
        {
            editorName = $"{componentInfo.Type.Name.Replace('+', '_')}Editor";
            _sb.AppendLine($"public class {editorName} : Editor {{");
            _sb.AppendLine("public override VisualElement CreateInspectorGUI() {");
            GenerateOnInspectorGUI(_sb, componentInfo);
            _sb.AppendLine("}}");
        }

        private static void GenerateOnInspectorGUI(StringBuilder sb, ComponentInfo componentInfo)
        {
            GUIBuilder.AddInspectedType(componentInfo);
            //sb.AppendLine("DrawDefaultInspector();");
            sb.AppendLine("return SimpleUtils.SimpleInspectorExtensions.Editor.InspectorBuilder.GUIBuilder.CreateInspectorGUI(serializedObject, this);");
        }
    }
}