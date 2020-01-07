using System.Linq;

namespace ProtoPrimitives.NET.Strings
{
    internal static class Extensions
    {
        internal static bool IsWhiteSpaceOnly(this string source) => source.All(char.IsWhiteSpace);

        internal static bool HasLeadingWhiteSpace(this string source) => char.IsWhiteSpace(source, 0);
        
        internal static bool HasTrailingWhiteSpace(this string source) => char.IsWhiteSpace(source, source.Length - 1);
    }
}