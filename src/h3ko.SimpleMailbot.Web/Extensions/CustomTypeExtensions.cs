using System.Reflection;
using System;

namespace h3ko.SimpleMailbot.Web.Extensions{

    public static class CustomTypeExtensions
    {
        public static string GetAssemblyNameString(this Type t)
        {
            return t.GetTypeInfo().Assembly.GetName().Name;
        }
        public static string GetAssemblyVersion(this Type t)
        {
            return t.GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public static string GetAssemblyInformationalVersion(this Type t)
        {
            var attr = t.GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return attr?.InformationalVersion;
        }

        public static string GetAssemblyCopyright(this Type t)
        {
            var attr = t.GetTypeInfo().Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            return attr?.Copyright;
        }

        public static string GetAssemblyCompany(this Type t)
        {
            var attr = t.GetTypeInfo().Assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            return attr?.Company;
        }

    }
}