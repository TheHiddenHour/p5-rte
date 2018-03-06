using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Persona_5_Real_Time_Editor
{
    class ProcessMemoryAccessor
    {
        private readonly IntPtr mHandle;

        public long BaseAddress { get; set; }

        public ProcessMemoryAccessor(string processName, long baseAddress = 0)
        {
            BaseAddress = baseAddress;

            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null)
                throw new Exception("No such process exists");

            mHandle = OpenProcess(0x0008 | 0x0010 | 0x0020, false, process.Id);
        }

        public byte[] ReadBytes(long address, int count)
        {
            long effectiveAddress = BaseAddress + address;

            // Set custom memory protection
            Protection oldProtect = 0;
            if (!VirtualProtectEx(mHandle, (IntPtr)effectiveAddress, (IntPtr)count, Protection.ReadWrite, ref oldProtect))
                throw new Exception("VirtualProtectEx failed");

            var bytes = new byte[count];
            var numberOfBytesRead = IntPtr.Zero;

            if (!ReadProcessMemory(mHandle, (IntPtr)effectiveAddress, bytes, (IntPtr)count, ref numberOfBytesRead))
                throw new Exception("Failed to read process memory");

            // Reset memory protection
            if (!VirtualProtectEx(mHandle, (IntPtr)effectiveAddress, (IntPtr)count, oldProtect, ref oldProtect))
                throw new Exception("VirtualProtectEx failed");

            return bytes;
        }

        public byte ReadByte(long address)
        {
            return ReadBytes(address, 1)[0];
        }

        public short ReadBeShort(long address)
        {
            var bytes = ReadBytes(address, sizeof(short));
            return (short)(bytes[0] << 8 | bytes[1]);
        }

        public int ReadBeInt(long address)
        {
            var bytes = ReadBytes(address, sizeof(int));
            return bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
        }

        public void WriteBytes(long address, byte[] bytes)
        {
            long effectiveAddress = BaseAddress + address;

            // Set custom memory protection
            Protection oldProtect = 0;
            if (!VirtualProtectEx(mHandle, (IntPtr)effectiveAddress, (IntPtr)bytes.Length, Protection.ReadWrite, ref oldProtect))
                throw new Exception("VirtualProtectEx failed");

            IntPtr numberOfBytesWritten = IntPtr.Zero;
            if (!WriteProcessMemory(mHandle, (IntPtr)effectiveAddress, bytes, (IntPtr)bytes.Length, ref numberOfBytesWritten))
                throw new Exception("Failed to write process memory");

            // Reset memory protection
            if (!VirtualProtectEx(mHandle, (IntPtr)effectiveAddress, (IntPtr)bytes.Length, oldProtect, ref oldProtect))
                throw new Exception("VirtualProtectEx failed");
        }

        public void WriteByte(long address, byte value)
        {
            WriteBytes(address, new[] { value });
        }

        public void WriteBeShort(long address, short value)
        {
            WriteBytes(address, new[] { (byte)((value & 0xFF00) >> 8), (byte)(value & 0xFF) });
        }

        public void WriteBeInt(long address, int value)
        {
            WriteBytes(address, new[] { (byte)((value & 0xFF000000) >> 24), (byte)((value & 0xFF0000) >> 16), (byte)((value & 0xFF00) >> 8), (byte)(value & 0xFF) });
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr dwSize, ref IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr dwSize, ref IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, Protection flNewProtect, ref Protection lpflOldProtect);

        private enum Protection : uint
        {
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            Guard = 0x100,
            NoCache = 0x200,
            WriteCombine = 0x400
        }
    }
}
