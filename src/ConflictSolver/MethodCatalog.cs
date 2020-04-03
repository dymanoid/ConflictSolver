// <copyright file="MethodCatalog.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConflictSolver
{
    /// <summary>
    /// A catalog of <see cref="MethodInfo"/> instances that need to be patched to collect
    /// Reflection requests.
    /// </summary>
    internal sealed class MethodCatalog : IMethodCatalog
    {
        /// <inheritdoc/>
        public IEnumerable<MethodInfo> GetMethodsForMethodQuery()
        {
            var type = typeof(Type);
            yield return type.GetMethod(nameof(Type.GetMethod), new[] { typeof(string) });
            yield return type.GetMethod(nameof(Type.GetMethod), new[] { typeof(string), typeof(Type[]) });
            yield return type.GetMethod(nameof(Type.GetMethod), new[] { typeof(string), typeof(BindingFlags) });

            yield return type.GetMethod(
                nameof(Type.GetMethod),
                new[]
                {
                    typeof(string),
                    typeof(Type[]),
                    typeof(ParameterModifier[]),
                });

            yield return type.GetMethod(
                nameof(Type.GetMethod),
                new[]
                {
                    typeof(string),
                    typeof(BindingFlags),
                    typeof(Binder),
                    typeof(Type[]),
                    typeof(ParameterModifier[]),
                });

            yield return type.GetMethod(
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

        /// <inheritdoc/>
        public IEnumerable<MethodInfo> GetMethodsForFieldQuery()
        {
            var type = typeof(Type);
            yield return type.GetMethod(nameof(Type.GetField), new[] { typeof(string) });

            /*var runtimeType = type.GetType();
            yield return runtimeType.GetMethod(nameof(Type.GetField), new[] { typeof(string), typeof(BindingFlags) });*/
        }

        /// <inheritdoc/>
        public IEnumerable<MethodInfo> GetMethodsForPropertyQuery()
        {
            var type = typeof(Type);
            yield return type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string) });
            yield return type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(Type) });
            yield return type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(BindingFlags) });
            yield return type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(Type[]) });
            yield return type.GetMethod(nameof(Type.GetProperty), new[] { typeof(string), typeof(Type), typeof(Type[]) });

            yield return type.GetMethod(
                nameof(Type.GetProperty),
                new[]
                {
                    typeof(string),
                    typeof(Type),
                    typeof(Type[]),
                    typeof(ParameterModifier[]),
                });

            yield return type.GetMethod(
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

        /// <inheritdoc/>
        public IEnumerable<MethodInfo> GetAllMethods()
            => GetMethodsForMethodQuery().Concat(GetMethodsForFieldQuery()).Concat(GetMethodsForPropertyQuery());
    }
}
