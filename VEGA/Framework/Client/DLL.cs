using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace VEGA.Framework.Client
{
    class DLL
    {
        private Process proc = null;
        private string dllFilePath = null;

        public DLL(Process proc)
        {
            this.proc = proc;
            this.dllFilePath = Framework.Global.Global.DllFilePath;
        }
        
        public bool Inject()
        {
            IntPtr hndProc = API.OpenProcess(0x2 | 0x8 | 0x10 | 0x20 | 0x400, 1, (uint)proc.Id);

            if (hndProc == IntPtr.Zero)
                return false;

            IntPtr lpLLAddress = API.GetProcAddress(API.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (lpLLAddress == IntPtr.Zero)
                return false;

            IntPtr lpAddress = API.VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)dllFilePath.Length,
                (uint)API.VAE_Enums.AllocationType.MEM_COMMIT |
                (uint)API.VAE_Enums.AllocationType.MEM_RESERVE,
                (uint)API.VAE_Enums.ProtectionConstants.PAGE_EXECUTE_READWRITE);

            if (lpAddress == IntPtr.Zero)
                return false;

            byte[] bytes = CalcBytes(dllFilePath);
            IntPtr ipTmp = IntPtr.Zero;
            int wpm = API.WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, out ipTmp);

            IntPtr ipThread = API.CreateRemoteThread(hndProc, (IntPtr)null, IntPtr.Zero, lpLLAddress, lpAddress, 0, (IntPtr)null);

            if (ipThread == IntPtr.Zero)
                return false;

            uint Result = API.WaitForSingleObject(ipThread, (uint)10000);
            if (Result == 0x00000080L || Result == 0x00000102L || Result == 0xFFFFFFFF)
            {
                if (hndProc != IntPtr.Zero)
                    API.CloseHandle(hndProc);

                return false;
            }

            if (hndProc != IntPtr.Zero)
                API.CloseHandle(hndProc);

            return true;
        }
        private byte[] CalcBytes(string sToConvert)
        {
            byte[] bRet = System.Text.Encoding.ASCII.GetBytes(sToConvert);

            return bRet;
        }
        private bool WriteBytes(IntPtr hProcess, uint dwAdress, byte[] lpByteBuffer, int nSize)
        {
            IntPtr iBytesWritten = IntPtr.Zero;
            IntPtr lpBuffer = GCHandle.Alloc(lpByteBuffer, GCHandleType.Pinned).AddrOfPinnedObject();

            return API.WriteProcessMemory(hProcess, dwAdress, lpBuffer, nSize, out iBytesWritten);
        }
    }
}
