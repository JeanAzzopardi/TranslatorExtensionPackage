using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeanAzzopardi.TranslatorExtensionPackage
{
    public static class ExtensionMethods
    {
        public static bool IsLowerCase(this char c)
        {
            return c.ToString().Equals(c.ToString().ToLowerInvariant());
        }

        public static bool IsUpperCase(this char c)
        {
            return c.ToString().Equals(c.ToString().ToUpperInvariant());

        }
    }
}
