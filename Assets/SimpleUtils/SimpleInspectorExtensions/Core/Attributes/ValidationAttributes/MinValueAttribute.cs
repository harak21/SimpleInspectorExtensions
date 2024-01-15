using System;
using System.Diagnostics;
using System.Reflection;
using SimpleUtils.SimpleInspectorExtensions.Core.Utility;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SimpleUtils.SimpleInspectorExtensions.Core.Attributes.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class MinValueAttribute : ValidationAttribute
    {
        private readonly double _minValue;

        public MinValueAttribute(double minValue)
        {
            _minValue = minValue;
        }
        
        public override void Execute(VisualElement rootElement, Object target, VisualElement memberElement,
            MemberInfo memberInfo)
        {
            switch (memberElement)
            {
                case PropertyField propertyField:
                    HandlePropertyField(target, memberElement, propertyField);
                    break;
                case IntegerField integerField:
                    HandleIntegerField(integerField);
                    break;
                case FloatField floatField:
                    HandleFloatField(floatField);
                    break;
                case DoubleField doubleField:
                    HandleDoubleField(doubleField);
                    break;
                case LongField longField:
                    HandleLongField(longField);
                    break;
            }
        }
        
        
        private void HandleLongField(LongField longField)
        {
            longField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue < _minValue)
                {
                    longField.value = (long)Math.Ceiling(_minValue);
                }
            });
        }

        private void HandleDoubleField(DoubleField doubleField)
        {
            doubleField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue < _minValue)
                {
                    doubleField.value = _minValue;
                }
            });
        }

        private void HandleFloatField(FloatField floatField)
        {
            floatField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue < _minValue)
                {
                    floatField.value = (float)_minValue;
                }
            });
        }

        private void HandleIntegerField(IntegerField integerField)
        {
            integerField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue < _minValue)
                {
                    integerField.value = (int)Math.Ceiling(_minValue);
                }
            });
        }

        private void HandlePropertyField(Object target, VisualElement memberElement, PropertyField propertyField)
        {
            propertyField.RegisterValueChangeCallback(evt =>
            {
                var value = ReflectionUtility.GetMemberValue<object>(target, memberElement.name);
                switch (value)
                {
                    case int intValue:
                    {
                        if (intValue < _minValue)
                        {
                            ReflectionUtility.SetMemberValue(target,
                                (int)Math.Ceiling(_minValue),
                                memberElement.name);
                        }

                        break;
                    }
                    case float floatValue:
                    {
                        if (floatValue < _minValue)
                        {
                            ReflectionUtility.SetMemberValue(target,
                                (float)_minValue,
                                memberElement.name);
                        }

                        break;
                    }
                    case double doubleValue:
                    {
                        if (doubleValue < _minValue)
                        {
                            ReflectionUtility.SetMemberValue(target,
                                _minValue,
                                memberElement.name);
                        }

                        break;
                    }
                    case long longValue:
                    {
                        if (longValue < _minValue)
                        {
                            ReflectionUtility.SetMemberValue(target,
                                (long)Math.Ceiling(_minValue),
                                memberElement.name);
                        }

                        break;
                    }
                }
            });
        }
    }
}