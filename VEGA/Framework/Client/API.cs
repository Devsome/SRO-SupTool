using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VEGA.Framework.Client
{
    public static class API
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, uint dwAddress, ref byte[] lpBuffer, int nSize, out int lpBytesRead);

        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, uint dwAddress, IntPtr lpBuffer, int nSize, out IntPtr iBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheiritHandle, IntPtr dwProcessId);

        public static uint Rights = 0x0010 | 0x0020 | 0x0008;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            UInt32 dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 CloseHandle(
            IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(
            IntPtr hModule,
            string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(
            string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            IntPtr dwSize,
            uint flAllocationType,
            uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] buffer,
            uint size,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(
            IntPtr hProcess,
            IntPtr lpThreadAttribute,
            IntPtr dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);


        public static class VAE_Enums
        {
            public enum AllocationType
            {
                MEM_COMMIT = 0x1000,
                MEM_RESERVE = 0x2000,
                MEM_RESET = 0x80000,
            }

            public enum ProtectionConstants
            {
                PAGE_EXECUTE = 0X10,
                PAGE_EXECUTE_READ = 0X20,
                PAGE_EXECUTE_READWRITE = 0X40,
                PAGE_EXECUTE_WRITECOPY = 0X80,
                PAGE_NOACCESS = 0X01
            }
        }

    }
}
