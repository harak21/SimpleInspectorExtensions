using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.MetaAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class OnValueChangedAttribute : BaseExtensionAttribute
    {
        public override int Order => 5;

        private readonly string _callback;

        public OnValueChangedAttribute(string callback)
        {
            _callback = callback;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement)
        {
            var field = (FieldInfo)ReflectionUtility.GetMember(target, memberElement.name);
            var method = (MethodInfo)ReflectionUtility.GetMember(target, _callback);

            if (method == null || field == null)
                return;

            var parameterInfos = method.GetParameters();

            if (field.FieldType.IsGenericType)
            {
                var generic = field.FieldType.GetGenericArguments();
                if (parameterInfos.Length != generic.Length)
                {
                    Debug.LogError("The callback method must accept a field type argument");
                    return;
                }

                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    if (parameterInfos[i].ParameterType != generic[i])
                    {
                        Debug.LogError("The callback method must accept a field type argument");
                        return;
                    }
                }
            }
            else
            {
                if (parameterInfos.Length != 1 || parameterInfos[0].ParameterType != field.FieldType)
                {
                    Debug.LogError("The callback method must accept a field type argument");
                    return;
                }
                
            }
            

            if (memberElement is PropertyField propertyField)
            {
                propertyField.RegisterValueChangeCallback(evt =>
                {
                    var value = ReflectionUtility.GetMemberValue<object>(target, memberElement.name);
                    ReflectionUtility.SetMemberValue(target, value, _callback, true); 
                });
                return;
            }

            RegisterCallbacks(memberElement, target);
        }

        private void RegisterCallbacks(CallbackEventHandler memberElement, Object target)
        {
            memberElement.RegisterCallback<ChangeEvent<SerializedProperty>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<int>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<bool>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<float>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<double>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<string>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Color>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Object>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Enum>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Vector2>> (changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Vector3>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Vector4>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Rect>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<AnimationCurve>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Bounds>> (changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Gradient>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Quaternion>> (changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Vector2Int>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Vector3Int>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Vector3Int>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<RectInt>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<BoundsInt>>(changeEvent => OnValueChanged(changeEvent, target));
            memberElement.RegisterCallback<ChangeEvent<Hash128>>(changeEvent => OnValueChanged(changeEvent, target));
        }

        private void OnValueChanged<T>(ChangeEvent<T> changeEvent, Object target)
        {
            var methodInfo = (MethodInfo)ReflectionUtility.GetMember(target, _callback);
            if (!typeof(T).IsAssignableFrom(methodInfo.GetParameters()[0].ParameterType))
                return;
            ReflectionUtility.SetMemberValue(target, changeEvent.newValue, _callback);
        }
    }
}