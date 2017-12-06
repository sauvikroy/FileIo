using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Read_C_Struct
{


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TestStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public int[] m_IntArray;
        public int m_Int;
    }

    public static class Program
    {
        public static void writeStructsToFile(TestStruct[] structures, int n, string fileName)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, 
                FileMode.OpenOrCreate, FileAccess.Write)))
            {
                var sz = Marshal.SizeOf(typeof(TestStruct));
                var buffer = new byte[sz];
                //Buffer.BlockCopy(structures, 0, buffer, 0, buffer.Length);

                for (int i = 0; i < n; ++i)
                {
                    TestStruct ms = (TestStruct)structures.GetValue(i);
                    IntPtr ptr = Marshal.AllocHGlobal(sz);
                    Marshal.StructureToPtr(ms, ptr, true);
                    Marshal.Copy(ptr, buffer, 0, sz);
                    writer.Write(buffer, 0, sz);
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        public static TestStruct[] readStructsFromFile(string fileName, int n)
        {
            TestStruct[] structures = null;

            if (File.Exists(fileName))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    var sz = Marshal.SizeOf(typeof(TestStruct));
                    var buffer = new byte[sz * n];
                    reader.Read(buffer, 0, sz * n);
                    var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    structures = new TestStruct[n];

                    IntPtr data = pinnedBuffer.AddrOfPinnedObject();
                    data -= sz;
                    for (int i = 0; i < n; ++i)
                    {
                        data += sz;
                        TestStruct ms = (TestStruct)Marshal.PtrToStructure(data, typeof(TestStruct));
                        structures.SetValue(ms, i);
                    }

                    pinnedBuffer.Free();
                }
            }

            return structures;
        }

        public static void printTestStruct(TestStruct iTestStruct)
        {
            int i;

            Console.Write(@"{");
            Console.Write(@"{");
            for (i = 0; i < 10; i++)
            {
                Console.Write("{0}{1}", iTestStruct.m_IntArray[i], (i == 9 ? "" : ", "));
            }
            Console.Write(@"}, ");
            Console.Write("{0}", iTestStruct.m_Int);
            Console.Write(@"}");
        }

        static void Main(string[] args)
        {
            TestStruct[] s = readStructsFromFile(@"C:\Users\SRY8\Desktop\structWrite.bin", 5);
            Console.Write("\n{0}\n", "Reading structs from file...");
            for (int i = 0; i < 5; ++i)
            {
                TestStruct ms = (TestStruct)s.GetValue(i);
                printTestStruct(ms);
                Console.WriteLine();
            }

            // Write...
            {
                TestStruct[] pTestStruct = new TestStruct[5];
                int j;

                // Perform Struct Assignment...
                for (j = 0; j < 5; ++j)
                {
                    int i;
                    int st = 0;

                    pTestStruct[j].m_IntArray = new int[10];
                    for (i = 0; i < 10; i++)
                    {
                        st += i * (j + 1);
                        pTestStruct[j].m_IntArray[i] = i * (j + 1);
                    }
                    pTestStruct[j].m_Int = st;
                }
                writeStructsToFile(pTestStruct, 5, @"C:\Users\SRY8\Desktop\structWrite.bin");
            }
        }
    }
}