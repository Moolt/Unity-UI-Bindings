using System;
using System.Linq.Expressions;

namespace UiBinding.Core
{
    public static class ExpressionExtensions
    {
        public static string GetMemberName<T>(this Expression<T> expression)
        {
            switch (expression.Body)
            {
                case MemberExpression memberExpression:
                    return memberExpression.Member.Name;
                case UnaryExpression unaryExpression when unaryExpression.Operand is MemberExpression memberExpression:
                    return memberExpression.Member.Name;
                default:
                    throw new NotImplementedException(expression.GetType().ToString());
            }
        }
    }
}