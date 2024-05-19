namespace Aochivapi;

using System.Runtime.InteropServices;

public static partial class AsmFunctionMaker
{
    private const uint ExecuteReadWrite = 0x40;

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

    public static NativeFunction ToFunction(byte[] shellcode)
    {
        var functionPtr = VirtualAlloc(IntPtr.Zero, (uint)shellcode.Length, 0x1000, ExecuteReadWrite);
        Marshal.Copy(shellcode, 0, functionPtr, shellcode.Length);
        return new NativeFunction(functionPtr);
    }
}