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
    public class MaxValueAttribute : ValidationAttribute
    {
        private readonly double _maxValue;

        public MaxValueAttribute(double maxValue)
        {
            _maxValue = maxValue;
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
                if (evt.newValue > _maxValue)
                {
                    longField.value = (long)Math.Floor(_maxValue);
                }
            });
        }

        private void HandleDoubleField(DoubleField doubleField)
        {
            doubleField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue > _maxValue)
                {
                    doubleField.value = _maxValue;
                }
            });
        }

        private void HandleFloatField(FloatField floatField)
        {
            floatField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue > _maxValue)
                {
                    floatField.value = (float)_maxValue;
                }
            });
        }

        private void HandleIntegerField(IntegerField integerField)
        {
            integerField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue > _maxValue)
                {
                    integerField.value = (int)Math.Floor(_maxValue);
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
                        if (intValue > _maxValue)
                        {
                            ReflectionUtility.SetMemberValue(target,
                                (int)Math.Floor(_maxValue),
                                memberElement.name);
                        }

                        break;
                    }
                    case float floatValue:
                    {
                        if (floatValue > _maxValue)
                        {
                            ReflectionUtility.SetMemberValue(target,
                                (float)_maxValue,
                                memberElement.name);
                        }

                        break;
                    }
                    case double doubleValue:
                    {
                        if (doubleValue > _maxValue)
                        {
                            ReflectionUtility.SetMemberValue(target,
                                _maxValue,
                                memberElement.name);
                        }

                        break;
                    }
                    case long longValue:
                    {
                        
                        if (longValue > _maxValue)
                        {
                            ReflectionUtility.SetMemberValue(target,
                                (long)Math.Floor(_maxValue),
                                memberElement.name);
                        }

                        break;
                    }
                }
            });
        }
    }
}