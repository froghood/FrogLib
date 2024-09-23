using System.Runtime.CompilerServices;

namespace FrogLib;

public static class Log {

    public static void Info(object message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) {
        Console.WriteLine($"{Path.GetRelativePath(Directory.GetCurrentDirectory(), file)}({line}): {message}");
    }

    public static void Warn(object message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) {
        Console.WriteLine($"{Path.GetRelativePath(Directory.GetCurrentDirectory(), file)}({line}): {message}");
    }
}