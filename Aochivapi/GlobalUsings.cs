// Global using directives

#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
global using reg = Iced.Intel.AssemblerRegister64;
global using mem = Iced.Intel.AssemblerMemoryOperand;
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
global using static Iced.Intel.AssemblerRegisters;
global using Ioe = System.InvalidOperationException;
global using Aofre = System.ArgumentOutOfRangeException;
global using Unreachable = System.Diagnostics.UnreachableException;
global using static Aochivapi.RegIntMem.RegIntMemType;