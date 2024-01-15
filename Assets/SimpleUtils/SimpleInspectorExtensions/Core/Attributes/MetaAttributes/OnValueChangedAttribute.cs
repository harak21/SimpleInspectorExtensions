using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
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
        private object _target;

        public OnValueChangedAttribute(string callback)
        {
            _callback = callback;
        }

        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            var field = (FieldInfo)ReflectionUtility.GetMember(target, memberElement.name);
            var method = (MethodInfo)ReflectionUtility.GetMember(target, _callback);

            if (method == null || field == null)
                return;

            var parameterInfos = method.GetParameters();

            if (typeof(IEnumerable).IsAssignableFrom(field.FieldType) && field.FieldType != typeof(string))
            {
                HandleCollectionTypeField(parameterInfos, memberElement, target);
                return;
            }

            HandleNonCollectionField(target, memberElement, parameterInfos, field);
        }

        private void HandleCollectionTypeField(ParameterInfo[] parameterInfos, VisualElement memberElement,
            Object target)
        {
            if (parameterInfos.Length != 0)
            {
                Debug.LogError("For collections callback should not contain input parameters ");
                return;
            }
            
            switch (memberElement)
            {
                case PropertyField propertyField:
                    propertyField.schedule.Execute((Action)(() =>
                    {
                        var listProperty = propertyField.Q<ListView>();
                        listProperty.itemsAdded += ints =>
                        {
                            ReflectionUtility.InvokeMethod(target,_callback);
                        };
                        listProperty.itemsRemoved += ints =>
                        {
                            ReflectionUtility.InvokeMethod(target,_callback);
                        };
                    })).ExecuteLater(1000L);
                    break;
                case ListView listView:
                    listView.itemsAdded += ints =>
                    {
                        ReflectionUtility.InvokeMethod(target,_callback);
                    };
                    listView.itemsRemoved += ints =>
                    {
                        ReflectionUtility.InvokeMethod(target,_callback);
                    };
                    break;
            }
        }

        private void HandleNonCollectionField(Object target, VisualElement memberElement, ParameterInfo[] parameterInfos,
            FieldInfo field)
        {
            if (parameterInfos.Length != 1 || parameterInfos[0].ParameterType != field.FieldType)
            {
                Debug.LogError("The callback method must accept a field type argument");
                return;
            }

            _target = target;
            var genericType = typeof(Object).IsAssignableFrom(field.FieldType) ? typeof(Object) : field.FieldType;
            var changeEventType = typeof(ChangeEvent<>).MakeGenericType(genericType);
            var eventCallbackType = typeof(EventCallback<>).MakeGenericType(changeEventType);
            var callbackMethodInfo = GetType()
                .GetMethod(nameof(Callback),
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .MakeGenericMethod(genericType);
            var del = Delegate.CreateDelegate(eventCallbackType, this, callbackMethodInfo);
            var eventHandlerMethod = typeof(CallbackEventHandler)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .First(m => m.Name == "RegisterCallback" && m.GetGenericArguments().Length == 1)
                .MakeGenericMethod(changeEventType);
            eventHandlerMethod.Invoke(memberElement, new object[] { del, TrickleDown.NoTrickleDown });
        }

        public void Callback<T>(ChangeEvent<T> changeEvent)
        {
            var methodInfo = (MethodInfo)ReflectionUtility.GetMember(_target, _callback);
            if (!typeof(T).IsAssignableFrom(methodInfo.GetParameters()[0].ParameterType))
                return;
            ReflectionUtility.SetMemberValue(_target, changeEvent.newValue, _callback);
        }
    }
}