using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;


namespace Booth.PortfolioManager.RestApi.Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class GenericTestCaseAttribute : TestCaseAttribute, ITestBuilder
    {
        private readonly Type _type;
        public GenericTestCaseAttribute(Type type, params object[] arguments) : base(arguments)
        {
            _type = type;
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite)
        {
            if (method.IsGenericMethodDefinition && _type != null)
            {
                var gm = method.MakeGenericMethod(_type);
                return BuildFrom(gm, suite);
            }
            return BuildFrom(method, suite);
        }
    } 

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class GenericTestCaseSourceAttribute : TestCaseAttribute, ITestBuilder
    {
        private readonly string _SourceName;
        private readonly NUnitTestCaseBuilder _builder = new NUnitTestCaseBuilder();

        public GenericTestCaseSourceAttribute(string sourceName) : base()
        {
            _SourceName = sourceName;
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite)
        {
            var genericCount = method.GetGenericArguments().Length;

            foreach (var testData in GetTestCasesFor(method))
            {
                var typeArguments = testData.Arguments.Take(genericCount).Select(x => (Type)x).ToArray();
                var remainingArguments = testData.Arguments.Skip(genericCount).ToArray();

                var genericMethod = method.MakeGenericMethod(typeArguments);

                var parms = new TestCaseParameters(remainingArguments);
                parms.ExpectedResult = testData.ExpectedResult;
                parms.HasExpectedResult = testData.HasExpectedResult;
                parms.TestName = testData.TestName;

                yield return _builder.BuildTestMethod(genericMethod, suite, parms);
            }
        }

        private IEnumerable<ITestCaseData> GetTestCasesFor(IMethodInfo method)
        {
            Type sourceType = method.TypeInfo.Type;

            var members = sourceType.GetMember(_SourceName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            var m = members[0] as MethodInfo;

            var result = m.Invoke(null, new object[] { });

            return result as IEnumerable<ITestCaseData>;
        }
    }
}
