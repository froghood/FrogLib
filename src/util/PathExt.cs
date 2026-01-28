using System.Text.RegularExpressions;

namespace FrogLib;

internal static partial class PathExt {
    public static string ToUnixPath(string path) {
        return path.Replace('\\', '/');
    }

    public static string RemoveRelativePath(string path) {
        return MyRegex().Replace(path.Trim(), string.Empty);
    }

    [GeneratedRegex(@"^[.][\/]")]
    private static partial Regex MyRegex();
}