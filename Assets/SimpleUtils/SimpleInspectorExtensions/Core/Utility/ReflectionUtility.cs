using System;
using System.Collections.Generic;
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
                Debug.LogError($"Member named {memberName} was not found");
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

        public static void SetMemberValue(object target, object value, string memberName, bool skipDirtyFlag = false)
        {
            var member = GetMember(target, memberName); 

            switch (member)
            {
                case null:
                    Debug.LogError($"Member named {memberName} was not found");
                    return;
                case MethodInfo methodInfo:
                    methodInfo.Invoke(target, new[] { value });
                    break;
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(target, value);
                    break;
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(target, value);
                    break;
            }

            if (target is Object o && !skipDirtyFlag)
            {
                EditorUtility.SetDirty(o);
            }
            
        }

        public static object InvokeMethod(object target, string methodName)
        {
            var member = GetMember(target, methodName);
            
            switch (member)
            {
                case null:
                    Debug.LogError($"Class member named {methodName} was not found");
                    return null;
                case MethodInfo methodInfo:
                    return methodInfo.Invoke(target, Array.Empty<object>());
            }

            return null;
        }

        public static MemberInfo GetMember(object target, string memberName)
        {
            return target.GetType().GetMember(memberName,
                BindingFlags.Instance | BindingFlags.Static
                                      | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault();
        }

        public static List<MemberInfo> GetMembers(object target)
        {
            return GetMembers(target.GetType());
        }

        public static List<MemberInfo> GetMembers(Type type)
        {
            return type.GetMembers(BindingFlags.Instance | BindingFlags.Static
                                                         | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(m => m.MemberType is MemberTypes.Field or MemberTypes.Property or MemberTypes.NestedType or MemberTypes.Method).ToList();
        }

        public static List<MemberInfo> GetPublicFieldsAndProperties(Type type)
        {
            return type.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.MemberType is MemberTypes.Field or MemberTypes.Property).ToList();
        }

        public static Type GetMemberType(object target, string memberName)
        {
            var member = GetMember(target, memberName);
            return GetMemberType(member);
        }
        
        public static Type GetMemberType(MemberInfo member)
        {
            switch (member)
            {
                case FieldInfo fieldInfo:
                    return fieldInfo.FieldType;
                case PropertyInfo propertyInfo:
                    return propertyInfo.PropertyType;
                default:
                    return typeof(object);
            }
        }
    }
}