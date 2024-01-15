using System;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Utility
{
    public class ListViewData
    {
        public Type[] GenericArgs;
        public object Target;
        public Type FieldType;
        public string CustomAddFunction;
    }
}