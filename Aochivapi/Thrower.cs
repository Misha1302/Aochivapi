namespace Aochivapi;

using System.Diagnostics.CodeAnalysis;

public static class Thrower
{
    [DoesNotReturn]
    public static void Throw(Exception e) => throw e;

    [DoesNotReturn]
    public static T Throw<T>(Exception e) => throw e;
}