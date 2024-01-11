using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;
using SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData;
using UnityEngine;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.InspectorBuilder
{
    internal static class EditorGenerator
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
}