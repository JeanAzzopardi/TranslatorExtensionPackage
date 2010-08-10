// Guids.cs
// MUST match guids.h
using System;

namespace JeanAzzopardi.TranslatorExtensionPackage
{
    static class GuidList
    {
        public const string guidTranslatorExtensionPackagePkgString = "c8648406-28cd-45c4-9d2f-ca0914d787f6";
        public const string guidTranslatorExtensionPackageCmdSetString = "4e7db2cd-1168-4bfe-99e5-60622349e60a";

        public static readonly Guid guidTranslatorExtensionPackageCmdSet = new Guid(guidTranslatorExtensionPackageCmdSetString);
    };
}