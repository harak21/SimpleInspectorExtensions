using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;
using UnityEngine;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Utility
{
    internal static class InspectorColorExtensions
    {
        public static string Style(this InspectorColor inspectorColor)
        {
            return $"label-color-{inspectorColor.ToString().ToLower()}";
        }
        
        public static Color Color(this InspectorColor inspectorColor)
        {
            switch (inspectorColor)
            {
                case InspectorColor.White:
                    return UnityEngine.Color.white;
                case InspectorColor.Wheat:
                {
                    ColorUtility.TryParseHtmlString("#F5DEB3FF", out var color);
                    return color;
                }
                case InspectorColor.Whitesmoke:
                {
                    ColorUtility.TryParseHtmlString("#F5F5F5FF", out var color);
                    return color;
                }
                case InspectorColor.Black:
                    return UnityEngine.Color.black;
                case InspectorColor.Bisque:
                {
                    ColorUtility.TryParseHtmlString("#FFE4C4FF", out var color);
                    return color;
                }
                case InspectorColor.Beige:
                {
                    ColorUtility.TryParseHtmlString("#F5F5DCFF", out var color);
                    return color;
                }
                case InspectorColor.Blanchedalmond:
                {
                    ColorUtility.TryParseHtmlString("#FFEBCDFF", out var color);
                    return color;
                }
                case InspectorColor.Blueviolet:
                {
                    ColorUtility.TryParseHtmlString("#8A2BE2FF", out var color);
                    return color;
                }
                case InspectorColor.Brown:
                {
                    ColorUtility.TryParseHtmlString("#A52A2AFF", out var color);
                    return color;
                }
                case InspectorColor.Burlywood:
                {
                    ColorUtility.TryParseHtmlString("#DEB887FF", out var color);
                    return color;
                }
                case InspectorColor.Gray:
                    return UnityEngine.Color.gray;
                case InspectorColor.Gainsboro:
                {
                    ColorUtility.TryParseHtmlString("#DCDCDCFF", out var color);
                    return color;
                }
                case InspectorColor.Ghostwhite:
                {
                    ColorUtility.TryParseHtmlString("#F8F8FFFF", out var color);
                    return color;
                }
                case InspectorColor.Gold:
                {
                    ColorUtility.TryParseHtmlString("#FFD700FF", out var color);
                    return color;
                }
                case InspectorColor.Goldenrod:
                {
                    ColorUtility.TryParseHtmlString("#DAA520FF", out var color);
                    return color;
                }
                case InspectorColor.Greenyellow:
                {
                    ColorUtility.TryParseHtmlString("#ADFF2FFF", out var color);
                    return color;
                }
                case InspectorColor.Red:
                    return UnityEngine.Color.red;
                case InspectorColor.Rebeccapurple:
                {
                    ColorUtility.TryParseHtmlString("#663399FF", out var color);
                    return color;
                }
                case InspectorColor.Rosybrown:
                {
                    ColorUtility.TryParseHtmlString("#BC8F8FFF", out var color);
                    return color;
                }
                case InspectorColor.Royalblue:
                {
                    ColorUtility.TryParseHtmlString("#4169E1FF", out var color);
                    return color;
                }
                case InspectorColor.Palegoldenrod:
                {
                    ColorUtility.TryParseHtmlString("#EEE8AAFF", out var color);
                    return color;
                }
                case InspectorColor.Palegreen:
                {
                    ColorUtility.TryParseHtmlString("#98FB98FF", out var color);
                    return color;
                }
                case InspectorColor.Paleturquoise:
                {
                    ColorUtility.TryParseHtmlString("#AFEEEEFF", out var color);
                    return color;
                }
                case InspectorColor.Palevioletred:
                {
                    ColorUtility.TryParseHtmlString("#DB7093FF", out var color);
                    return color;
                }
                case InspectorColor.Papayawhip:
                {
                    ColorUtility.TryParseHtmlString("#FFEFD5FF", out var color);
                    return color;
                }
                case InspectorColor.Pink:
                {
                    ColorUtility.TryParseHtmlString("#FFC0CBFF", out var color);
                    return color;
                }
                case InspectorColor.Orange:
                {
                    ColorUtility.TryParseHtmlString("#FFA500FF", out var color);
                    return color;
                }
                case InspectorColor.Yellow:
                    return UnityEngine.Color.yellow;
                case InspectorColor.Green:
                    return UnityEngine.Color.green;
                case InspectorColor.Blue:
                    return UnityEngine.Color.blue;
                case InspectorColor.Indigo:
                {
                    ColorUtility.TryParseHtmlString("#4B0082FF", out var color);
                    return color;
                }
                case InspectorColor.Violet:
                {
                    ColorUtility.TryParseHtmlString("#EE82EEFF", out var color);
                    return color;
                }
                default:
                    return UnityEngine.Color.clear;
            }
        }
    }
}