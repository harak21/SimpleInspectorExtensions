using System.Diagnostics;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes
{
    [Conditional("UNITY_EDITOR")]
    public abstract class StyleExtensionAttribute : BaseExtensionAttribute
    {
        public override int Order => int.MaxValue;
    }
}