using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persona_5_Real_Time_Editor
{
    class Addresses
    {
        public static uint MoneyAddress = 0x010B21C4;

        /*personaSlot and skillSlot start at 1, not 0.*/

        public static API API = MainForm.API;

        public enum SocialStats
        {
            Knowledge = 0,
            Charm = 1,
            Proficiency = 2,
            Guts = 3,
            Kindness = 4
        }

        public enum PersonaStats
        {
            Strength = 26,
            Magic = 27,
            Endurance = 28,
            Agility = 29,
            Luck = 30
        }

        public static string SocialStatName(SocialStats socialStat, int socialStatValue)
        {
            int value = socialStatValue;

            if(socialStat == SocialStats.Knowledge)
            {
                if (value <= 33)
                    return "Oblivious";
                else if (value <= 81)
                    return "Learned";
                else if (value <= 125)
                    return "Scholarly";
                else if (value <= 191)
                    return "Encyclopedic";
                else
                    return "Erudite";
            }
            else if(socialStat == SocialStats.Charm)
            {
                if (value <= 5)
                    return "Existent";
                else if (value <= 51)
                    return "Head-Turning";
                else if (value <= 91)
                    return "Suave";
                else if (value <= 131)
                    return "Charismatic";
                else
                    return "Debonair";
            }
            else if(socialStat == SocialStats.Proficiency)
            {
                if (value <= 11)
                    return "Bumbling";
                else if (value <= 33)
                    return "Decent";
                else if (value <= 59)
                    return "Skilled";
                else if (value <= 86)
                    return "Masterful";
                else
                    return "Transcendent";
            }
            else if(socialStat == SocialStats.Guts)
            {
                if (value <= 10)
                    return "Milquetoast";
                else if (value <= 28)
                    return "Bold";
                else if (value <= 56)
                    return "Staunch";
                else if (value <= 112)
                    return "Dauntless";
                else
                    return "Lionhearted";
            }
            else if(socialStat == SocialStats.Kindness)
            {
                if (value <= 13)
                    return "Inoffensive";
                else if (value <= 43)
                    return "Considerate";
                else if (value <= 90)
                    return "Empathetic";
                else if (value <= 135)
                    return "Selfless";
                else
                    return "Angelic";
            }

            return "unknown";
        }

        /*---Address fiding methods---*/
		
		//Returns the address of a persona's bytes
        public static uint GetPersonaAddress(int personaSlot)
        {
            return 0x010af2b6 + ((uint)(personaSlot - 1) * 0x30);
        }
		//Returns the address of a persona's level
        public static uint GetPersonaLevelAddress(int personaSlot)
        {
            return GetPersonaAddress(personaSlot) + 2;
        }
		//Returns the address of a persona's statistic
        public static uint GetPersonaStatAddress(int personaSlot, PersonaStats personaStat)
        {
            return GetPersonaAddress(personaSlot) + (uint)personaStat;
        }
		//Returns the address of a persona's skill
        public static uint GetPersonaSkillAddress(int personaSlot, int skillSlot)
        {
            return (GetPersonaAddress(personaSlot) + 10) + ((uint)(skillSlot - 1) * 2);
        }
		//Returns the address of a protagonist's skill
        public static uint GetSocialStatAddress(SocialStats socialStat)
        {
            return 0x10C1870 + ((uint)socialStat * 0x02);
        }

        /*---Memory methods---*/
		
		//Set the bytes of a persona
        public static void SetPersona(int personaSlot, byte[] personaBytes)
        {
            uint address = GetPersonaAddress(personaSlot);
            API.WriteBytes(address, personaBytes);
        }
		//Get the bytes of a persona
        public static byte[] GetPersona(int personaSlot)
        {
            uint address = GetPersonaAddress(personaSlot);
            return API.ReadBytes(address, 2);
        }
		//Set the bytes of a persona's skill
        public static void SetPersonaSkill(int personaSlot, int skillSlot, byte[] skillBytes)
        {
            uint address = GetPersonaSkillAddress(personaSlot, skillSlot);
            API.WriteBytes(address, skillBytes);
        }
		//Get the bytes of a persona's skill
        public static byte[] GetPersonaSkill(int personaSlot, int skillSlot)
        {
            uint address = GetPersonaSkillAddress(personaSlot, skillSlot);
            return API.ReadBytes(address, 2);
        }
		//Set the value of a persona's level
        public static void SetPersonaLevel(int personaSlot, int levelValue)
        {
            uint address = GetPersonaLevelAddress(personaSlot);
            byte level = Convert.ToByte(levelValue);
            API.WriteByte(address, level);
        }
		//Get the value of a persona's level
        public static int GetPersonaLevel(int personaSlot)
        {
            uint address = GetPersonaLevelAddress(personaSlot);
            byte level = API.ReadByte(address);
            return Convert.ToInt16(level);
        }
		//Set the value of a persona's statistic
        public static void SetPersonaStat(int personaSlot, PersonaStats personaStat, int personaStatValue)
        {
            uint address = GetPersonaStatAddress(personaSlot, personaStat);
            byte value = Convert.ToByte(personaStatValue);
            API.WriteByte(address, value);
        }
		//Get the value of a persona's statistic
        public static int GetPersonaStat(int personaSlot, PersonaStats personaStat)
        {
            uint address = GetPersonaStatAddress(personaSlot, personaStat);
            byte value = API.ReadByte(address);
            return Convert.ToInt16(value);
        }
		//Set the value of a protagonist's social statistic
        public static void SetSocialStat(SocialStats socialStat, int socialStatValue)
        {
            uint address = GetSocialStatAddress(socialStat);
            short value = (short)socialStatValue;
            API.WriteShort(address, value);
        }
		//Get the value of a protagonist's social statistic
        public static int GetSocialStat(SocialStats socialStat)
        {
            uint address = GetSocialStatAddress(socialStat);
            short value = API.ReadShort(address);
            return Convert.ToInt16(value);
        }
		//Set the value of the protagonist's money
        public static void SetMoney(int moneyValue)
        {
            uint address = MoneyAddress;
            byte[] value = BitConverter.GetBytes(moneyValue);
            Array.Reverse(value);
            API.WriteBytes(address, value);
        }
		//Get the value of the protagonist's money
        public static int GetMoney()
        {
            uint address = MoneyAddress;
            byte[] value = API.ReadBytes(address, 4);
            Array.Reverse(value);
            return BitConverter.ToInt32(value, 0);
        }
    }
}
