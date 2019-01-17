using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;

namespace Persona_5_RTE
{
    class Persona
    {
        private static PS3API PS3 = Form1.PS3;

        public enum Stat
        {
            Strength,
            Magic,
            Endurance,
            Agility,
            Luck
        }

        public static void SetSkill(int slot, int skill, short value)
        {
            uint address = 0x10AF2C0 + (uint)(slot * 0x30) + (uint)(skill * 0x02);
            PS3.Extension.WriteInt16(address, value);
        }

        public static short GetSkill(int slot, int skill)
        {
            uint address = 0x10AF2C0 + (uint)(slot * 0x30) + (uint)(skill * 0x02);
            return PS3.Extension.ReadInt16(address);
        }

        public static void SetPersona(int slot, short value)
        {
            uint address = 0x010af2b6 + (uint)(slot * 0x30); // Persona 0 address + slot * 0x30
            PS3.Extension.WriteInt16(address, value);
        }

        public static short GetPersona(int slot)
        {
            uint address = 0x010af2b6 + (uint)(slot * 0x30); // Persona 0 address + slot * 0x30
            return PS3.Extension.ReadInt16(address);
        }

        public static void SetLevel(int slot, byte value)
        {
            uint address = 0x10AF2B8 + (uint)(slot * 0x30); // Persona 0 level address + slot * 0x30
            PS3.Extension.WriteByte(address, value);
        }

        public static byte GetLevel(int slot)
        {
            uint address = 0x10AF2B8 + (uint)(slot * 0x30); // Persona 0 level address + slot * 0x30
            return PS3.Extension.ReadByte(address);
        }

        public static void SetStat(int slot, Stat stat, byte value)
        {
            uint address = 0x10AF2D0 + (uint)(slot * 0x30) + (uint)stat; // Persona 0 str address + slot * 0x30 + stat
            PS3.Extension.WriteByte(address, value);
        }

        public static byte GetStat(int slot, Stat stat)
        {
            uint address = 0x10AF2D0 + (uint)(slot * 0x30) + (uint)stat; // Persona 0 str address + slot * 0x30 + stat
            return PS3.Extension.ReadByte(address);
        }
    }
}
