using PS3Lib;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Persona_5_Real_Time_Editor
{
    public partial class MainForm : Form
    {
        public static API API = new API();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        /*---Form utility methods---*/
		
		//Sets the toolstriplabel text and color
        private void SetInformationBar(string text, Color? color = null)
        {
            toolStripLabel1.Text = text;
            toolStripLabel1.ForeColor = color ?? Color.Black;
        }
		//Converts a byte array to a string
        private string ByteArrayToString(byte[] array)
        {
            string arrayString = "";
            for (int byteIndex = 0; byteIndex < array.Length; byteIndex++)
                arrayString = arrayString + array[byteIndex].ToString("X");

            return arrayString;
        }
		//Converts a string into a byte array
        private byte[] StringToByteArray(string str)
        {
            return Enumerable.Range(0, str.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(str.Substring(x, 2), 16))
                .ToArray();
        }
		//Quickly change a socialstat combobox data
        private void SetSocialStatCombobox(ComboBox combobox, Addresses.SocialStats socialStat, int[] socialStatValues)
        {
            int value = socialStatValues[combobox.SelectedIndex];
            Addresses.SetSocialStat(socialStat, value);
            SetInformationBar(socialStat.ToString() + " set to: " + combobox.Text);
        }
		//Retrieve the data from a socialstat combobox
        private void GetSocialStatCombobox(ComboBox combobox, Addresses.SocialStats socialStat)
        {
            int value = Addresses.GetSocialStat(socialStat);
            combobox.Text = Addresses.SocialStatName(socialStat, value);
            SetInformationBar(socialStat.ToString() + " retrieved as: " + combobox.Text);
        }

        /*---API form methods---*/

        private void tmapiRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(tmapiRadioButton.Checked && !rpcs3RadioButton.Checked && !ccapiRadioButton.Checked)
            {
                if (API.CurrentAPI != API.SelectAPI.PS3Lib)
                {
                    API.ChangeToolAPI(API.SelectAPI.PS3Lib);
                    attachButton.Enabled = true;
                    connectAndAttachButton.Enabled = true;
                    connectButton.Enabled = true;
                }
                API.ChangePS3API(SelectAPI.TargetManager);
            }
        }

        private void rpcs3RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!tmapiRadioButton.Checked && rpcs3RadioButton.Checked && !ccapiRadioButton.Checked)
            {
                if (API.CurrentAPI != API.SelectAPI.RPCS3)
                {
                    API.ChangeToolAPI(API.SelectAPI.RPCS3);
                    attachButton.Enabled = true;
                    connectButton.Enabled = false;
                    connectAndAttachButton.Enabled = false;
                }
            }
        }

        private void ccapiRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!tmapiRadioButton.Checked && !rpcs3RadioButton.Checked && ccapiRadioButton.Checked)
            {
                if (API.CurrentAPI != API.SelectAPI.PS3Lib)
                {
                    API.ChangeToolAPI(API.SelectAPI.PS3Lib);
                    attachButton.Enabled = true;
                    connectAndAttachButton.Enabled = true;
                    connectButton.Enabled = true;
                }
                API.ChangePS3API(SelectAPI.ControlConsole);
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            //A check just in case to make sure that the current API is PS3.
            if(API.CurrentAPI == API.SelectAPI.PS3Lib)
            {
                if (API.PS3ConnectTarget())
                {
                    attachButton.Enabled = true;
                    SetInformationBar("PS3 successfully connected!", Color.Blue);
                }
                else
                    SetInformationBar("PS3 could not connect!", Color.Red);
            }
            else
                MessageBox.Show("The current API must be set to PS3 to use this button!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void attachButton_Click(object sender, EventArgs e)
        {
            if (API.AttachProcess())
            {
                tabControl1.Enabled = true;
                SetInformationBar("Process successfully attached!", Color.Green);
            }
            else
                SetInformationBar("Process could not be attached!", Color.Red);
        }

        private void connectAndAttachButton_Click(object sender, EventArgs e)
        {
            //Check that the API is PS3 just in case
            if (API.CurrentAPI == API.SelectAPI.PS3Lib)
            {
                if (API.PS3ConnectTarget() && API.AttachProcess())
                {
                    tabControl1.Enabled = true;
                    SetInformationBar("Successfully connected and attached!", Color.Green);
                }
                else
                    SetInformationBar("Could not connect and attach!", Color.Red);
            }
            else
                MessageBox.Show("The current API must be set to PS3 to use this button!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /*---Memory form methods---*/
        private void SearchPersonaButton_Click(object sender, EventArgs e)
        {
            using (SelectionForm personaSelector = new SelectionForm("Persona"))
            {
                if (personaSelector.ShowDialog() == DialogResult.Cancel && personaSelector.HasConfirmedSelection)
                    personaInput.Text = personaSelector.Selection;
            }
        }

        private void SetPersonaButton_Click(object sender, EventArgs e)
        {
			//Convert persona byte string into actual bytes
            byte[] personaBytes = StringToByteArray(personaInput.Text);
			//Set the new persona data
            Addresses.SetPersona((int)personaSlotInput.Value, personaBytes);
            //Change the information bar
            SetInformationBar("Persona " + personaSlotInput.Value + " set to: " + ByteArrayToString(personaBytes));
        }

        private void GetPersonaButton_Click(object sender, EventArgs e)
        {
            byte[] personaBytes = Addresses.GetPersona((int)personaSlotInput.Value);
            string personaBytesString = ByteArrayToString(personaBytes);

            if (personaBytesString.Length == 3)
                personaBytesString = "0" + personaBytesString;
            else if (personaBytesString.Length == 2)
                personaBytesString = "00" + personaBytesString;

            personaInput.Text = personaBytesString;
            SetInformationBar("Persona " + personaSlotInput.Value + " retrieved as: " + personaBytesString);
        }

        private void SearchSkillButton_Click(object sender, EventArgs e)
        {
            using (SelectionForm skillSelector = new SelectionForm("Skill"))
            {
                if (skillSelector.ShowDialog() == DialogResult.Cancel && skillSelector.HasConfirmedSelection)
                    skillInput.Text = skillSelector.Selection;
            }
        }

        private void SetPersonaSkillButton_Click(object sender, EventArgs e)
        {
            byte[] skillBytes = StringToByteArray(skillInput.Text);
            Addresses.SetPersonaSkill((int)personaSlotInput.Value, (int)skillSlotInput.Value, skillBytes);
            SetInformationBar("SKill " + skillSlotInput.Value + " set to: " + ByteArrayToString(skillBytes));
        }

        private void GetPersonaSkillButton_Click(object sender, EventArgs e)
        {
            byte[] skillBytes = Addresses.GetPersonaSkill((int)personaSlotInput.Value, (int)skillSlotInput.Value);
            string skillBytesString = ByteArrayToString(skillBytes);

            if (skillBytesString.Length == 3)
                skillBytesString = "0" + skillBytesString;
            else if (skillBytesString.Length == 2)
                skillBytesString = "00" + skillBytesString;

            skillInput.Text = skillBytesString;
            SetInformationBar("SKill " + skillSlotInput.Value + " retrieved as: " + skillBytesString);
        }

        private void SetPersonaLevelButton_Click(object sender, EventArgs e)
        {
            Addresses.SetPersonaLevel((int)personaSlotInput.Value, (int)levelInput.Value);
            SetInformationBar("Level set to: " + levelInput.Value);
        }

        private void GetPersonaLevelButton_Click(object sender, EventArgs e)
        {
            levelInput.Value = Addresses.GetPersonaLevel((int)personaSlotInput.Value);
            SetInformationBar("Level retrieved as: " + levelInput.Value);
        }

        private void SetStrengthButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Strength;
            Addresses.SetPersonaStat((int)personaSlotInput.Value, stat, (int)strengthInput.Value);
            SetInformationBar(stat.ToString() + " set to: " + strengthInput.Value);
        }

        private void GetStrengthButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Strength;
            strengthInput.Value =  Addresses.GetPersonaStat((int)personaSlotInput.Value, stat);
            SetInformationBar(stat.ToString() + " retrieved as: " + strengthInput.Value);
        }

        private void SetMagicButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Magic;
            Addresses.SetPersonaStat((int)personaSlotInput.Value, stat, (int)magicInput.Value);
            SetInformationBar(stat.ToString() + " set to: " + magicInput.Value);
        }

        private void GetMagicButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Magic;
            magicInput.Value = Addresses.GetPersonaStat((int)personaSlotInput.Value, stat);
            SetInformationBar(stat.ToString() + " retrieved as: " + magicInput.Value);
        }

        private void SetEnduranceButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Endurance;
            Addresses.SetPersonaStat((int)personaSlotInput.Value, stat, (int)enduranceInput.Value);
            SetInformationBar(stat.ToString() + " set to: " + enduranceInput.Value);
        }

        private void GetEnduranceButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Endurance;
            enduranceInput.Value = Addresses.GetPersonaStat((int)personaSlotInput.Value, stat);
            SetInformationBar(stat.ToString() + " retrieved as: " + enduranceInput.Value);
        }

        private void SetAgilityButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Agility;
            Addresses.SetPersonaStat((int)personaSlotInput.Value, stat, (int)agilityInput.Value);
            SetInformationBar(stat.ToString() + " set to: " + agilityInput.Value);
        }

        private void GetAgilityButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Agility;
            agilityInput.Value = Addresses.GetPersonaStat((int)personaSlotInput.Value, stat);
            SetInformationBar(stat.ToString() + " retrieved as: " + agilityInput.Value);
        }

        private void SetLuckButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Luck;
            Addresses.SetPersonaStat((int)personaSlotInput.Value, stat, (int)luckInput.Value);
            SetInformationBar(stat.ToString() + " set to: " + luckInput.Value);
        }

        private void GetLuckButton_Click(object sender, EventArgs e)
        {
            Addresses.PersonaStats stat = Addresses.PersonaStats.Luck;
            luckInput.Value = Addresses.GetPersonaStat((int)personaSlotInput.Value, stat);
            SetInformationBar(stat.ToString() + " retrieved as: " + luckInput.Value);
        }

        private void SetKnowledgeButton_Click(object sender, EventArgs e)
        {
            ComboBox combobox = knowledgeInput;
            Addresses.SocialStats stat = Addresses.SocialStats.Knowledge;
            int[] socialStatValues = new int[5] { 33, 81, 125, 191, 192 };

            SetSocialStatCombobox(combobox, stat, socialStatValues);
        }

        private void GetKnowledgeButton_Click(object sender, EventArgs e)
        {
            GetSocialStatCombobox(knowledgeInput, Addresses.SocialStats.Knowledge);
        }

        private void SetGutsButton_Click(object sender, EventArgs e)
        {
            ComboBox combobox = gutsInput;
            Addresses.SocialStats stat = Addresses.SocialStats.Guts;
            int[] socialStatValues = new int[5] { 10, 28, 56, 112, 113 };

            SetSocialStatCombobox(combobox, stat, socialStatValues);
        }

        private void GetGutsButton_Click(object sender, EventArgs e)
        {
            GetSocialStatCombobox(gutsInput, Addresses.SocialStats.Guts);
        }

        private void SetProficiencyButton_Click(object sender, EventArgs e)
        {
            ComboBox combobox = proficiencyInput;
            Addresses.SocialStats stat = Addresses.SocialStats.Proficiency;
            int[] socialStatValues = new int[5] { 11, 33, 59, 86, 87 };

            SetSocialStatCombobox(combobox, stat, socialStatValues);
        }

        private void GetProficiencyButton_Click(object sender, EventArgs e)
        {
            GetSocialStatCombobox(proficiencyInput, Addresses.SocialStats.Proficiency);
        }

        private void SetKindnessButton_Click(object sender, EventArgs e)
        {
            ComboBox combobox = kindnessInput;
            Addresses.SocialStats stat = Addresses.SocialStats.Kindness;
            int[] socialStatValues = new int[5] { 13, 43, 90, 135, 136 };

            SetSocialStatCombobox(combobox, stat, socialStatValues);
        }

        private void GetKindnessButton_Click(object sender, EventArgs e)
        {
            GetSocialStatCombobox(kindnessInput, Addresses.SocialStats.Kindness);
        }

        private void SetCharmButton_Click(object sender, EventArgs e)
        {
            ComboBox combobox = charmInput;
            Addresses.SocialStats stat = Addresses.SocialStats.Charm;
            int[] socialStatValues = new int[5] { 5, 51, 91, 131, 132 };

            SetSocialStatCombobox(combobox, stat, socialStatValues);
        }

        private void GetCharmButton_Click(object sender, EventArgs e)
        {
            GetSocialStatCombobox(charmInput, Addresses.SocialStats.Charm);
        }

        private void SetMoneyButton_Click(object sender, EventArgs e)
        {
            Addresses.SetMoney((int)moneyInput.Value);
            SetInformationBar("Money set to: " + moneyInput.Value);
        }

        private void GetMoneyButton_Click(object sender, EventArgs e)
        {
            moneyInput.Value = Addresses.GetMoney();
            SetInformationBar("Money retrieved as: " + moneyInput.Value);
        }

        private void dumpButton_Click(object sender, EventArgs e)
        {
            /*Personas
            Persona Skills
            Persona Stats
            Social Stats*/

            dumpOutput.Text = "";

            for(int personaSlot = 0; personaSlot < 12; personaSlot++)
            {
                //Prefix
                dumpOutput.Text = dumpOutput.Text + "---PERSONA " + (personaSlot + 1) + "---\n";
                //Persona
                if (dumpListBox.GetItemChecked(0))
                {
                    string personaBytesString = ByteArrayToString(Addresses.GetPersona(personaSlot + 1));
                    if (personaBytesString.Length == 3)
                        personaBytesString = "0" + personaBytesString;
                    else if (personaBytesString.Length == 2)
                        personaBytesString = "00" + personaBytesString;

                    dumpOutput.Text = dumpOutput.Text + "Persona - 0x" + Addresses.GetPersonaAddress(personaSlot + 1).ToString("X") + " - " + personaBytesString + "\n";
                }
                //Persona skills
                if (dumpListBox.GetItemChecked(1))
                {
                    for (int skillSlot = 0; skillSlot < 8; skillSlot++)
                    {
                        string skillBytesString = ByteArrayToString(Addresses.GetPersonaSkill(personaSlot+1, skillSlot+1));
                        if (skillBytesString.Length == 3)
                            skillBytesString = "0" + skillBytesString;
                        else if (skillBytesString.Length == 2)
                            skillBytesString = "00" + skillBytesString;

                        dumpOutput.Text = dumpOutput.Text + "Skill " + (skillSlot+1) + "- 0x" + Addresses.GetPersonaSkillAddress(personaSlot + 1, skillSlot + 1).ToString("X") + " - " + skillBytesString + "\n";
                    }
                }
                //Persona stats
                if (dumpListBox.GetItemChecked(2))
                {
                    dumpOutput.Text = dumpOutput.Text + "Strength - 0x" + Addresses.GetPersonaStatAddress(personaSlot+1, Addresses.PersonaStats.Strength).ToString("X") + " - " + Addresses.GetPersonaStat(personaSlot + 1, Addresses.PersonaStats.Strength) + "\n";
                    dumpOutput.Text = dumpOutput.Text + "Magic - 0x" + Addresses.GetPersonaStatAddress(personaSlot + 1, Addresses.PersonaStats.Magic).ToString("X") + " - " + Addresses.GetPersonaStat(personaSlot + 1, Addresses.PersonaStats.Magic) + "\n";
                    dumpOutput.Text = dumpOutput.Text + "Endurance - 0x" + Addresses.GetPersonaStatAddress(personaSlot + 1, Addresses.PersonaStats.Endurance).ToString("X") + " - " + Addresses.GetPersonaStat(personaSlot + 1, Addresses.PersonaStats.Endurance) + "\n";
                    dumpOutput.Text = dumpOutput.Text + "Agility - 0x" + Addresses.GetPersonaStatAddress(personaSlot + 1, Addresses.PersonaStats.Agility).ToString("X") + " - " + Addresses.GetPersonaStat(personaSlot + 1, Addresses.PersonaStats.Agility) + "\n";
                    dumpOutput.Text = dumpOutput.Text + "Luck - 0x" + Addresses.GetPersonaStatAddress(personaSlot + 1, Addresses.PersonaStats.Luck).ToString("X") + " - " + Addresses.GetPersonaStat(personaSlot + 1, Addresses.PersonaStats.Luck) + "\n";
                }
            }
            //Social stats
            if (dumpListBox.GetItemChecked(3))
            {
                dumpOutput.Text = dumpOutput.Text + "\n---SOCIAL STATS---\n";
                dumpOutput.Text = dumpOutput.Text + "Knowledge - 0x" + Addresses.GetSocialStatAddress(Addresses.SocialStats.Knowledge).ToString("X") + " - " + Addresses.GetSocialStat(Addresses.SocialStats.Knowledge) + "\n";
                dumpOutput.Text = dumpOutput.Text + "Charm - 0x" + Addresses.GetSocialStatAddress(Addresses.SocialStats.Charm).ToString("X") + " - " + Addresses.GetSocialStat(Addresses.SocialStats.Charm) + "\n";
                dumpOutput.Text = dumpOutput.Text + "Proficiency - 0x" + Addresses.GetSocialStatAddress(Addresses.SocialStats.Proficiency).ToString("X") + " - " + Addresses.GetSocialStat(Addresses.SocialStats.Proficiency) + "\n";
                dumpOutput.Text = dumpOutput.Text + "Guts - 0x" + Addresses.GetSocialStatAddress(Addresses.SocialStats.Guts).ToString("X") + " - " + Addresses.GetSocialStat(Addresses.SocialStats.Guts) + "\n";
                dumpOutput.Text = dumpOutput.Text + "Kindness - 0x" + Addresses.GetSocialStatAddress(Addresses.SocialStats.Kindness).ToString("X") + " - " + Addresses.GetSocialStat(Addresses.SocialStats.Kindness) + "\n";
            }
        }

        private void dumpClearButton_Click(object sender, EventArgs e)
        {
            dumpOutput.Text = "";
        }

        private void dumpCopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dumpOutput.Text);
        }
    }
}
