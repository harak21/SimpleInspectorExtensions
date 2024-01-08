using System.Collections.Generic;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes;
using UnityEngine;

namespace Scenes
{
    public class Test : MonoBehaviour
    {
        [Dropdown("choices")]public string Fooo;

        private List<string> choices = new List<string>() { "a", "b" };
        
        
        [BoxGroup("sadds"), BackgroundColor(InspectorColor.Red), Padding(25)] public bool notHide;
        [BorderWidth(2), BorderColor(InspectorColor.Black), BorderRadius(4), Margin(0,0,0,3)] public bool hide;

        [LabelColor(InspectorColor.Gold)] public List<GameObject> List;
        [BoxGroup("sfs", InspectorColor.Black, InspectorColor.Red), LabelColor(InspectorColor.Gold)] public NestedTest nestedTest;

        [BoxGroup("box", margin: 15, padding:15, borderWidth: 1, borderColor: InspectorColor.Brown, borderRadius:3)] public Vector3 vector3;
        
        [Button("Invoke me")]
        private void ButtonTest()
        {
            Debug.Log("Invoked");
        }

        [BoxGroup("Hidden")] private string s = "hiddenStr";
    }
}