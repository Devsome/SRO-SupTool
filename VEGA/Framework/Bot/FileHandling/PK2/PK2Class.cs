using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace VEGA.Framework.Bot.FileHandling.PK2
{
    class PK2Class
    {
        private Framework.Shared.Blowfish blowfish = new Framework.Shared.Blowfish();
        private byte[] bKey = new byte[] { 0x32, 0xCE, 0xDD, 0x7C, 0xBC, 0xA8 };

        private Framework.Bot.Structs.PK2File.pk2Header header;
        private Framework.Bot.Structs.PK2File.pk2Folder mainFolder;
        private Framework.Bot.Structs.PK2File.pk2Folder currentFolder;

        private List<Framework.Bot.Structs.PK2File.pk2EntryBlock> EntryBlocks = new List<Framework.Bot.Structs.PK2File.pk2EntryBlock>();
        private List<Framework.Bot.Structs.PK2File.pk2File> Files = new List<Framework.Bot.Structs.PK2File.pk2File>();
        private List<Framework.Bot.Structs.PK2File.pk2Folder> Folders = new List<Framework.Bot.Structs.PK2File.pk2Folder>();

        FileStream fileStream;

        public PK2Class(string pk2FilePath)
        {
            if (File.Exists(pk2FilePath))
            {
                fileStream = new FileStream(pk2FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                blowfish.Initialize(bKey);
                BinaryReader reader = new BinaryReader(fileStream);
                header = (Framework.Bot.Structs.PK2File.pk2Header)BufferToStruct(reader.ReadBytes(256), typeof(Framework.Bot.Structs.PK2File.pk2Header));
                currentFolder = new Framework.Bot.Structs.PK2File.pk2Folder();
                currentFolder.name = pk2FilePath;
                currentFolder.files = new List<Framework.Bot.Structs.PK2File.pk2File>();
                currentFolder.subfolders = new List<Framework.Bot.Structs.PK2File.pk2Folder>();

                mainFolder = currentFolder;
                read(reader.BaseStream.Position);
            }

        }

        public bool fileExists(string name)
        {
            Framework.Bot.Structs.PK2File.pk2File file = Files.Find(item => item.name.ToLower() == name.ToLower());
            
            if (file.position != 0)
                return true;
            else
                return false;
        }
        public byte[] getFile(string name)
        {
            if (fileExists(name))
            {
                BinaryReader reader = new BinaryReader(fileStream);
                Framework.Bot.Structs.PK2File.pk2File file = Files.Find(item => item.name.ToLower() == name.ToLower());
                reader.BaseStream.Position = file.position;

                return reader.ReadBytes((int)file.size);
            }
            else
            {
                return null;
            }
        }
        public List<string> getFileNames()
        {
            List<string> tmpList = new List<string>();

            foreach (Framework.Bot.Structs.PK2File.pk2File file in Files)
                tmpList.Add(file.name);

            return tmpList;
        }

        public void read(Int64 position)
        {
            BinaryReader reader = new BinaryReader(fileStream);
            reader.BaseStream.Position = position;
            List<Framework.Bot.Structs.PK2File.pk2Folder> tmpFolders = new List<Framework.Bot.Structs.PK2File.pk2Folder>();
            Framework.Bot.Structs.PK2File.pk2EntryBlock entryBlock = (Framework.Bot.Structs.PK2File.pk2EntryBlock)BufferToStruct(blowfish.Decode(reader.ReadBytes(Marshal.SizeOf(typeof(Framework.Bot.Structs.PK2File.pk2EntryBlock)))), typeof(Framework.Bot.Structs.PK2File.pk2EntryBlock));

            for (int i = 0; i < 20; i++)
            {
                Framework.Bot.Structs.PK2File.pk2Entry entry = entryBlock.entries[i];

                switch (entry.type)
                {
                    case 0:
                        break;
                    case 1:
                        if (entry.name != "." && entry.name != "..")
                        {
                            Framework.Bot.Structs.PK2File.pk2Folder tmpFolder = new Framework.Bot.Structs.PK2File.pk2Folder();
                            tmpFolder.name = entry.name;
                            tmpFolder.position = BitConverter.ToInt64(entry.position, 0);
                            tmpFolders.Add(tmpFolder);
                            Folders.Add(tmpFolder);

                            if(tmpFolder != null && currentFolder.subfolders == null)
                                currentFolder.subfolders = new List<Framework.Bot.Structs.PK2File.pk2Folder>();

                            currentFolder.subfolders.Add(tmpFolder);
                        }
                        break;
                    case 2:
                        {
                            Framework.Bot.Structs.PK2File.pk2File tmpFile = new Framework.Bot.Structs.PK2File.pk2File();
                            tmpFile.position = entry.Position;
                            tmpFile.name = entry.name;
                            tmpFile.size = entry.Size;
                            tmpFile.parentFolder = currentFolder;
                            Files.Add(tmpFile);

                            currentFolder.files.Add(tmpFile);
                        }
                        break;
                }

            }

            if (entryBlock.entries[19].nChain != 0)
                read(entryBlock.entries[19].nChain);

            foreach (Framework.Bot.Structs.PK2File.pk2Folder folder in tmpFolders)
            {
                currentFolder = folder;

                if (folder.files == null)
                    folder.files = new List<Framework.Bot.Structs.PK2File.pk2File>();
                else if (folder.subfolders == null)
                    folder.subfolders = new List<Framework.Bot.Structs.PK2File.pk2Folder>();

                read(folder.position);
            }

        }

        public object BufferToStruct(byte[] buffer, Type returnStruct)
        {
            IntPtr pointer = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, pointer, buffer.Length);

            return Marshal.PtrToStructure(pointer, returnStruct);
        }   
    }
}
