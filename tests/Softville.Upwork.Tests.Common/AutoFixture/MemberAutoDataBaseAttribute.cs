using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Xunit.Sdk;

namespace Softville.Upwork.Tests.Common.AutoFixture
{
    public class MemberAutoDataBaseAttribute : DataAttribute
    {
        private readonly string _propertyName;
        private readonly Func<IFixture> _createFixture;

        protected MemberAutoDataBaseAttribute(string propertyName, Func<IFixture> createFixture)
        {
            _propertyName = propertyName;
            _createFixture = createFixture;
        }

        public Type? PropertyHost { get; set; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            foreach (var values in GetAllParameterObjects(testMethod) ?? Enumerable.Empty<object[]>())
            {
                yield return GetObjects(values, testMethod.GetParameters(), _createFixture());
            }
        }

        private IEnumerable<object[]>? GetAllParameterObjects(MethodInfo methodUnderTest)
        {
            var type = PropertyHost ?? methodUnderTest.DeclaringType;
            var property = type!.GetProperty(_propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            if (property == null)
                throw new ArgumentException($"Could not find public static property {_propertyName} on {type.FullName}");
            var obj = property.GetValue(null, null);
            if (obj == null)
                return null;

            if (obj is IEnumerable<object[]> enumerable)
                return enumerable;

            if (obj is IEnumerable<object> singleEnumerable)
                return singleEnumerable.Select(x => new[] { x });

            throw new ArgumentException(
                $"Property {_propertyName} on {type.FullName} did not return IEnumerable<object[]>");
        }

        private static object[] GetObjects(object[] parameterized, ParameterInfo[] parameters, IFixture fixture)
        {
            var result = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (i < parameterized.Length)
                    result[i] = parameterized[i];
                else
                    result[i] = CustomizeAndCreate(fixture, parameters[i]);
            }

            return result;
        }

        private static object CustomizeAndCreate(IFixture fixture, ParameterInfo p)
        {
            var customizations = p.GetCustomAttributes(typeof(CustomizeAttribute), false)
                .OfType<CustomizeAttribute>()
                .Select(attr => attr.GetCustomization(p));

            foreach (var c in customizations)
            {
                fixture.Customize(c);
            }

            var context = new SpecimenContext(fixture);
            return context.Resolve(p);
        }
    }
}
