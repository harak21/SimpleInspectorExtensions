using System.Text;
using SimpleUtils.SimpleInspectorExtensions.Editor.ReflectionData;

namespace SimpleUtils.SimpleInspectorExtensions.Editor.InspectorBuilder
{
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