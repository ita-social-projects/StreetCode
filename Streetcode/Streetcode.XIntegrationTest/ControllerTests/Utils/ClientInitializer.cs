using System.Linq.Expressions;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public static class ClientInitializer<T>
    {
        public static readonly Func<HttpClient, string, T> Initialize = CreateClientInitializerFunction();

        private static Func<HttpClient, string, T> CreateClientInitializerFunction()
        {
            ConstructorInfo TestClassConstructorCache = typeof(T).GetConstructor(new[] { typeof(HttpClient), typeof(string) })!;
            ParameterExpression clientParameter = Expression.Parameter(typeof(HttpClient), "client");
            ParameterExpression secondPartUrlParameter = Expression.Parameter(typeof(string), "secondPartUrl");

            NewExpression constructorExpression = Expression.New(
                TestClassConstructorCache,
                clientParameter,
                secondPartUrlParameter);

            Expression<Func<HttpClient, string, T>> lambdaExpression =
                Expression.Lambda<Func<HttpClient, string, T>>(
                    constructorExpression,
                    clientParameter,
                    secondPartUrlParameter);

            return lambdaExpression.Compile();
        }
    }
}
