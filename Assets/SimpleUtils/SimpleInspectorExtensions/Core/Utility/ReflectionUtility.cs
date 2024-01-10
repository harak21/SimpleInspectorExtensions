using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Utility
{
    public static class ReflectionUtility
    {
        public static T GetMemberValue<T>(object target, string memberName)
        {
            var member = GetMember(target, memberName);
            
            if (member is null)
            {
                Debug.LogError($"Class member named {memberName} was not found");
                return default;
            }

            var value = member switch
            {
                MethodInfo methodInfo => (T)methodInfo.Invoke(target, Array.Empty<object>()),
                FieldInfo fieldInfo => (T)fieldInfo.GetValue(target),
                PropertyInfo propertyInfo => (T)propertyInfo.GetValue(target),
                _ => default
            };

            return value;
        }

        public static void SetMemberValue(object target, object value, string memberName)
        {
            var member = GetMember(target, memberName);

            if (member is null)
            {
                Debug.LogError($"Class member named {memberName} was not found");
                return;
            }

            if (member is MethodInfo methodInfo)
            {
                methodInfo.Invoke(target, new[] { value });
            }

            if (member is FieldInfo fieldInfo)
            {
                fieldInfo.SetValue(target, value);
            }

            if (member is PropertyInfo propertyInfo)
            {
                propertyInfo.SetValue(target, value);
            }

            if (target is Object o)
            {
                EditorUtility.SetDirty(o);
            }
            
        }

        public static void InvokeMethod(object target, string methodName)
        {
            var member = GetMember(target, methodName);
            
            if (member is null)
            {
                Debug.LogError($"Class member named {methodName} was not found");
                return;
            }

            if (member is MethodInfo methodInfo)
            {
                methodInfo.Invoke(target, Array.Empty<object>());
            }
        }

        private static MemberInfo GetMember(object target, string memberName)
        {
            return target.GetType().GetMember(memberName,
                BindingFlags.Instance | BindingFlags.Static
                                      | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly).FirstOrDefault();
        }
    }
}