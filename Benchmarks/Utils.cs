using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Chessar.Benchmarks
{
    internal static class Utils
    {
        internal static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            if (toCheck.IsAbstract)
                return false;
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        internal static Delegate MakeDelegate(this MethodInfo method, Type targetType = null)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));
            if (method.IsGenericMethod)
                throw new ArgumentException("The provided method must not be generic.", nameof(method));
            if (!method.IsStatic && targetType is null)
                throw new ArgumentNullException(nameof(targetType));
            Contract.EndContractBlock();

            return method.CreateDelegate(GetDelegateType(method, targetType));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Type GetDelegateType(MethodInfo method, Type targetType)
        {
            var args = method.IsStatic ? Enumerable.Empty<Type>() : new[] { targetType };
            return Expression.GetDelegateType(args.Concat(from parameter in method.GetParameters() select parameter.ParameterType)
                .Concat(new[] { method.ReturnType }).ToArray());
        }
    }
}
