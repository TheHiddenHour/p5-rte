using PS3Lib;

namespace Persona_5_RTE
{
    class Protagonist
    {
        private static PS3API PS3 = Form1.PS3;

        // Social stats
        public enum Stat
        {
            Knowledge,
            Charm,
            Proficiency,
            Guts,
            Kindness
        }
        
        // Set a social stat value
        public static void SetStat(Stat stat, short value)
        {
            uint address = 0x10C1870 + (0x02 * (uint)stat); // Calculate stat address
            PS3.Extension.WriteInt16(address, value); // Write short to address
        }

        // Get a social stat value
        public static short GetStat(Stat stat)
        {
            uint address = 0x10C1870 + (0x02 * (uint)stat); // Calculate stat address
            return PS3.Extension.ReadInt16(address); // Get short from address
        }

        // Protagonist money
        public static int Money
        {
            get
            {
                return PS3.Extension.ReadInt32(0x010B21C4);
            }
            set
            {
                PS3.Extension.WriteInt32(0x010B21C4, value);
            }
        }

        // Protagonist level
        public static byte Level
        {
            get
            {
                return PS3.Extension.ReadByte(0x010af289);
            }
            set
            {
                PS3.Extension.WriteByte(0x010af289, value);
            }
        }
    }
}
