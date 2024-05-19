namespace Aochivapi;

using System.Runtime.InteropServices;

public partial class NativeFunction(nint ptr) : IDisposable
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        VirtualFree(ptr, 0, 0x8000);
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial void VirtualFree(nint lpAddress, uint dwSize, uint dwFreeType);


    public unsafe TReturn Call<TReturn>() =>
        ((delegate*<TReturn>)ptr)();

    public unsafe TReturn Call<TP1, TReturn>(TP1 p1) =>
        ((delegate*<TP1, TReturn>)ptr)(p1);

    public unsafe TReturn Call<TP1, TP2, TReturn>(TP1 a, TP2 b) =>
        ((delegate*<TP1, TP2, TReturn>)ptr)(a, b);

    public unsafe TReturn Call<TP1, TP2, TP3, TReturn>(TP1 a, TP2 b, TP3 c) =>
        ((delegate*<TP1, TP2, TP3, TReturn>)ptr)(a, b, c);

    public unsafe TReturn Call<TP1, TP2, TP3, TP4, TReturn>(TP1 a, TP2 b, TP3 c, TP4 d) =>
        ((delegate*<TP1, TP2, TP3, TP4, TReturn>)ptr)(a, b, c, d);

    ~NativeFunction()
    {
        Dispose();
    }
}