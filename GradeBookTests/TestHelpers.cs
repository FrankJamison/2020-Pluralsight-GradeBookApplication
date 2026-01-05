using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace GradeBookTests
{
    public static class TestHelpers
    {
        private static readonly string _projectName = "GradeBook";

        public static Type GetUserType(string fullName)
        {
            EnsureProjectAssemblyLoaded();

            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    where assembly.FullName.StartsWith(_projectName)
                    from type in SafeGetTypes(assembly)
                    where type.FullName == fullName
                    select type).FirstOrDefault();
        }

        private static void EnsureProjectAssemblyLoaded()
        {
            var alreadyLoaded = AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => string.Equals(a.GetName().Name, _projectName, StringComparison.OrdinalIgnoreCase));

            if (alreadyLoaded)
                return;

            try
            {
                Assembly.Load(new AssemblyName(_projectName));
            }
            catch
            {
                // Best-effort: tests will fail with a clear message if the assembly can't be loaded.
            }
        }

        private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null);
            }
        }
    }
}
