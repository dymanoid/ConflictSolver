// <copyright file="MethodCatalog.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// A catalog of <see cref="MethodInfo"/> instances that need to be patched to collect
    /// Reflection requests.
    /// </summary>
    internal sealed class MethodCatalog : IMethodCatalog
    {
        private readonly Type _type = typeof(Type);
        private readonly Type _monoType = Type.GetType("System.MonoType");
        private readonly Type _property = typeof(PropertyInfo);
        private readonly Type _monoProperty = Type.GetType("System.Reflection.MonoProperty");
        private readonly Type _field = typeof(FieldInfo);
        private readonly Type _monoField = Type.GetType("System.Reflection.MonoField");

        /// <inheritdoc/>
        public IEnumerable<MethodInfo> GetAllMethods()
            => GetMethodsForMethodQuery()
                .Concat(GetMethodsForMethodsQuery())
                .Concat(GetMethodsForFieldQuery())
                .Concat(GetMethodsForFieldsQuery())
                .Concat(GetMethodsForFieldRead())
                .Concat(GetMethodsForFieldWrite())
                .Concat(GetMethodsForPropertyQuery())
                .Concat(GetMethodsForPropertiesQuery())
                .Concat(GetMethodsForPropertyRead())
                .Concat(GetMethodsForPropertyWrite());

        /// <inheritdoc/>
        public IEnumerable<MethodInfo> GetMethods(ReflectionData reflectionData)
        {
            switch (reflectionData)
            {
                case ReflectionData.MethodInfo:
                    return GetMethodsForMethodQuery();

                case ReflectionData.MethodInfoMultiple:
                    return GetMethodsForMethodsQuery();

                case ReflectionData.FieldInfo:
                    return GetMethodsForFieldQuery();

                case ReflectionData.FieldInfoMultiple:
                    return GetMethodsForFieldsQuery();

                case ReflectionData.FieldInfoValueRead:
                    return GetMethodsForFieldRead();

                case ReflectionData.FieldInfoValueWrite:
                    return GetMethodsForFieldWrite();

                case ReflectionData.PropertyInfo:
                    return GetMethodsForPropertyQuery();

                case ReflectionData.PropertyInfoMultiple:
                    return GetMethodsForPropertiesQuery();

                case ReflectionData.PropertyInfoValueRead:
                    return GetMethodsForPropertyRead();

                case ReflectionData.PropertyInfoValueWrite:
                    return GetMethodsForPropertyWrite();

                default:
                    throw new ArgumentOutOfRangeException(nameof(reflectionData), "Unsupported reflection data type");
            }
        }

        private IEnumerable<MethodInfo> GetMethodsForMethodQuery()
        {
            yield return _type.GetMethod(nameof(Type.GetMethod), new[] { typeof(string) });
            yield return _type.GetMethod(nameof(Type.GetMethod), new[] { typeof(string), typeof(Type[]) });
            yield return _type.GetMethod(nameof(Type.GetMethod), new[] { typeof(string), typeof(BindingFlags) });

            yield return _type.GetMethod(
                nameof(Type.GetMethod),
                new[]
                {
                    typeof(string),
                    typeof(Type[]),
                    typeof(ParameterModifier[]),
                });

            yield return _type.GetMethod(
                nameof(Type.GetMethod),
                new[]
                {
                    typeof(string),
                    typeof(BindingFlags),
                    typeof(Binder),
                    typeof(Type[]),
                    typeof(ParameterModifier[]),
                });

            yield return _type.GetMethod(
                nameof(Type.GetMethod),
                new[]
                {
                    typeof(string),
                    typeof(BindingFlags),
                    typeof(Binder),
                    typeof(CallingConventions),
                    typeof(Type[]),
                    typeof(ParameterModifier[]),
                });
        }

        private IEnumerable<MethodInfo> GetMethodsForMethodsQuery()
        {
            yield return _type.GetMethod(nameof(Type.GetMethods), new Type[0]);
            yield return _monoType.GetMethod(nameof(Type.GetMethods), new[] { typeof(BindingFlags) });
        }

        private IEnumerable<MethodInfo> GetMethodsForFieldQuery()
        {
            // Note: MonoType.GetField(string, BindingFlags) is an external intrinsic method and cannot be patched.
            // The method of the base class (Type.GetField) is abstract.
            // So, only the GetField(string) overload will be patched.
            yield return _type.GetMethod(nameof(Type.GetField), new[] { typeof(string) });
        }

        private IEnumerable<MethodInfo> GetMethodsForFieldsQuery()
        {
            yield return _type.GetMethod(nameof(Type.GetFields), new Type[0]);
            yield return _monoType.GetMethod(nameof(Type.GetFields), new[] { typeof(BindingFlags) });
        }

        private IEnumerable<MethodInfo> GetMethodsForPropertyQuery()
        {
            yield return _type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string) });
            yield return _type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(Type) });
            yield return _type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(BindingFlags) });
            yield return _type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(Type[]) });
            yield return _type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(Type), typeof(Type[]) });

            yield return _type.GetMethod(
                nameof(Type.GetProperty),
                new[]
                {
                    typeof(string),
                    typeof(Type),
                    typeof(Type[]),
                    typeof(ParameterModifier[]),
                });

            yield return _type.GetMethod(
                nameof(Type.GetProperty),
                new[]
                {
                    typeof(string),
                    typeof(BindingFlags),
                    typeof(Binder),
                    typeof(Type),
                    typeof(Type[]),
                    typeof(ParameterModifier[]),
                });
        }

        private IEnumerable<MethodInfo> GetMethodsForPropertiesQuery()
        {
            yield return _type.GetMethod(nameof(Type.GetProperties), new Type[0]);
            yield return _monoType.GetMethod(nameof(Type.GetProperties), new[] { typeof(BindingFlags) });
        }

        private IEnumerable<MethodInfo> GetMethodsForPropertyRead()
        {
            yield return _monoProperty.GetMethod(nameof(PropertyInfo.GetValue), new[] { typeof(object), typeof(object[]) });

            yield return _monoProperty.GetMethod(
                nameof(PropertyInfo.GetValue),
                new[]
                {
                    typeof(object),
                    typeof(BindingFlags),
                    typeof(Binder),
                    typeof(object[]),
                    typeof(CultureInfo),
                });
        }

        private IEnumerable<MethodInfo> GetMethodsForPropertyWrite()
        {
            yield return _property.GetMethod(nameof(PropertyInfo.SetValue), new[] { typeof(object), typeof(object), typeof(object[]) });

            yield return _monoProperty.GetMethod(
                nameof(PropertyInfo.SetValue),
                new[]
                {
                    typeof(object),
                    typeof(object),
                    typeof(BindingFlags),
                    typeof(Binder),
                    typeof(object[]),
                    typeof(CultureInfo),
                });
        }

        private IEnumerable<MethodInfo> GetMethodsForFieldRead()
        {
            yield return _monoField.GetMethod(nameof(FieldInfo.GetValue), new[] { typeof(object) });
        }

        private IEnumerable<MethodInfo> GetMethodsForFieldWrite()
        {
            yield return _field.GetMethod(nameof(FieldInfo.SetValue), new[] { typeof(object), typeof(object) });

            yield return _monoField.GetMethod(
                nameof(FieldInfo.SetValue),
                new[]
                {
                    typeof(object),
                    typeof(object),
                    typeof(BindingFlags),
                    typeof(Binder),
                    typeof(CultureInfo),
                });
        }
    }
}
