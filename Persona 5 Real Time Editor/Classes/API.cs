using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;
using System.Windows.Forms;

namespace Persona_5_Real_Time_Editor
{
    public class API
    {
		/*THIS IS A POORLY IMPLEMENTED WAY OF COMBINING TGE's RPCS3 MEMORY EDITOR AND PS3Lib*/

        public SelectAPI CurrentAPI = SelectAPI.PS3Lib;
        private PS3API PS3API;
        private ProcessMemoryAccessor RPCS3;
		
		public enum SelectAPI
        {
            PS3Lib,
            RPCS3
        }
		
        public API()
        {
            PS3API = new PS3API();
            PS3API.ChangeAPI(PS3Lib.SelectAPI.TargetManager);
        }

        public void ChangeToolAPI(SelectAPI newAPI)
        {
            if (newAPI == SelectAPI.PS3Lib)
                CurrentAPI = SelectAPI.PS3Lib;
            else
                CurrentAPI = SelectAPI.RPCS3;
        }

        public void ChangePS3API(PS3Lib.SelectAPI newAPI)
        {
            PS3API.ChangeAPI(newAPI);
        }

        /*---Connection methods---*/
		
		//Connects the PS3 to the tool using the current API (TMAPI or CCAPI). Will throw an error if using RPCS3 API instead.
        public bool PS3ConnectTarget()
        {
            //A check that the current API is PS3 just in case.
            if (CurrentAPI == SelectAPI.PS3Lib)
                return PS3API.ConnectTarget();
            else
                MessageBox.Show("The current API must be set to PS3 to use this method!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
		//Attaches to the game process (PS3 or PC). If using the PS3 API, make sure the target is connected first.
        public bool AttachProcess()
        {
            if (CurrentAPI == SelectAPI.PS3Lib)
                return PS3API.AttachProcess();
            else
            {
                try { RPCS3 = new ProcessMemoryAccessor("rpcs3", 0x100000000); }
                catch { return false; }

                return true;
            }
        }

        /*---Memory methods---*/
		
		//Read a byte from address
        public byte ReadByte(uint address)
        {
            if (CurrentAPI == SelectAPI.PS3Lib)
                return PS3API.Extension.ReadByte(address);
            else
                return RPCS3.ReadByte(address);
        }
		//Write a byte to address
        public void WriteByte(uint address, byte value)
        {
            if (CurrentAPI == SelectAPI.PS3Lib)
                PS3API.Extension.WriteByte(address, value);
            else
                RPCS3.WriteByte(address, value);
        }
		//Read bytes from address
        public byte[] ReadBytes(uint address, int length)
        {
            if (CurrentAPI == SelectAPI.PS3Lib)
                return PS3API.Extension.ReadBytes(address, length);
            else
                return RPCS3.ReadBytes(address, length);
        }
		//Write bytes to address
        public void WriteBytes(uint address, byte[] values)
        {
            if (CurrentAPI == SelectAPI.PS3Lib)
                PS3API.Extension.WriteBytes(address, values);
            else
                RPCS3.WriteBytes(address, values);
        }
		//Read short from address
        public short ReadShort(uint address)
        {
            if (CurrentAPI == SelectAPI.PS3Lib)
                return PS3API.Extension.ReadInt16(address);
            else
                return RPCS3.ReadBeShort(address);
        }
		//Write short to address
        public void WriteShort(uint address, short value)
        {
            if (CurrentAPI == SelectAPI.PS3Lib)
                PS3API.Extension.WriteInt16(address, value);
            else
                RPCS3.WriteBeShort(address, value);
        }
    }
}
