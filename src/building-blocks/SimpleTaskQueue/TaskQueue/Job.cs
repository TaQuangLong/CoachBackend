using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleTaskQueue.TaskQueue
{
    public class Job
    {
        public Job(Type type, MethodInfo methodInfo, IReadOnlyList<object> args)
        {
            Type = type;
            MethodInfo = methodInfo;
            Args = args;
        }

        public Type Type { get; }
        public MethodInfo MethodInfo { get; }
        public IReadOnlyList<object> Args { get; }
        public Dictionary<string, string> CustomData { get; private set; } = new Dictionary<string, string>();

        public void AddCustomData(string key, string value)
        {
            if (!CustomData.ContainsKey(key)) CustomData.Add(key, value);
        }

        public static Job FromExpression(Expression<Func<Task>> methodCall)
        {
            return FromExpression(methodCall, null);
        }

        public static Job FromExpression(Expression<Action> methodCall)
        {
            return FromExpression(methodCall, null);
        }

        public static Job FromExpression<T>(Expression<Func<T, Task>> methodCall)
        {
            return FromExpression(methodCall, typeof(T));
        }

        public static Job FromExpression<T>(Expression<Action<T>> methodCall)
        {
            return FromExpression(methodCall, typeof(T));
        }

        private static Job FromExpression(LambdaExpression methodCall, Type explicitType)
        {
            if (methodCall == null)
            {
                throw new ArgumentException("Method call is null or not lambda convertible");
            }

            if (!(methodCall.Body is MethodCallExpression body))
            {
                throw new ArgumentException("Expression body should be of type `MethodCallExpression`",
                    nameof(methodCall));
            }

            var methodInfo = body.Method;
            var type = explicitType;
            if (explicitType == null)
            {
                type = body.Method.DeclaringType;
            }

            return new Job(type, methodInfo, Evaluate(body.Arguments));
        }

        private static object[] Evaluate(IEnumerable<Expression> expressions)
        {
            return expressions.Select(Evaluate).ToArray();
        }

        // TODO: improve this process
        private static object Evaluate(Expression expression)
        {
            switch (expression)
            {
                case ConstantExpression constantExpression:
                    return constantExpression.Value;
                case MemberExpression memberExpression:
                    return Expression.Lambda(memberExpression).Compile().DynamicInvoke();
                case MethodCallExpression methodCallExpression:
                    return Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
                default:
                    throw new NotImplementedException($"This expression type is not supported, {expression.ToString()}");
            }
        }
    }
}
