using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VEGA.Framework.Bot.Structs
{
    public class PK2File
    {
        [StructLayout(LayoutKind.Sequential, Size = 256)]
        public struct pk2Header
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public string name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] version;
            [MarshalAs(UnmanagedType.I1, SizeConst = 1)]
            public byte encryption;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] verify;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 205)]
            public byte[] reserved;
        }
        [StructLayout(LayoutKind.Sequential, Size = 128)]
        public struct pk2Entry
        {
            [MarshalAs(UnmanagedType.I1)]
            public byte type;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
            public string name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] accessTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] createTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] modifyTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] position;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] size;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] nextChain;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] padding;

            public long nChain { get { return BitConverter.ToInt64(nextChain, 0); } }
            public long Position { get { return BitConverter.ToInt64(position, 0); } }
            public uint Size { get { return BitConverter.ToUInt32(size, 0); } }
        }
        [StructLayout(LayoutKind.Sequential, Size = 2560)]
        public struct pk2EntryBlock
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public pk2Entry[] entries;
        }
        public struct pk2File
        {
            public string name;
            public long position;
            public uint size;
            public pk2Folder parentFolder;
        }
        public class pk2Folder
        {
            public string name;
            public long position;
            public List<pk2File> files;
            public List<pk2Folder> subfolders;
        } 
    }
}
