using System;
using System.Drawing;
using System.Windows.Forms;
using PS3Lib;

namespace Persona_5_RTE
{
    public partial class Form1 : Form
    {
        private static readonly short[] knowledgeValues = new short[] { 33, 81, 125, 191, 192 };
        private static readonly short[] charmValues = new short[] { 5, 51, 91, 131, 132 };
        private static readonly short[] proficiencyValues = new short[] { 11, 33, 59, 86, 87 };
        private static readonly short[] gutsValues = new short[] { 10, 28, 56, 112, 113 };
        private static readonly short[] kindnessValues = new short[] { 13, 43, 90, 135, 136 };

        private static int personaSlot = 0;
        private static int skillSlot = 0;

        public static PS3API PS3 = new PS3API(SelectAPI.TargetManager);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set persona name to default index of 0
            personaNameInput.SelectedIndex = 0;
            // Set persona skill to default index of 0
            skillNameInput.SelectedIndex = 0;
            // Set protagonist social stat boxes to max values
            knowledgeInput.SelectedIndex = knowledgeInput.Items.Count - 1;
            charmInput.SelectedIndex = charmInput.Items.Count - 1;
            proficiencyInput.SelectedIndex = proficiencyInput.Items.Count - 1;
            gutsInput.SelectedIndex = gutsInput.Items.Count - 1;
            kindnessInput.SelectedIndex = kindnessInput.Items.Count - 1;
        }

        // Set toolstrip status text
        private void statusPrint(string message, Color? color = null)
        {
            // If color parameter is not defined, default it to black
            if (color == null)
                color = Color.Black;
            // Change color and text
            toolStripStatusLabel1.ForeColor = (Color)color;
            toolStripStatusLabel1.Text = message;
        }

        // Set a social stat combobox's index based on retrieved protagonist stat value
        private void RetrieveSocialStat(Protagonist.Stat stat, short[] levelValues, ComboBox statbox)
        {
            short value = Protagonist.GetStat(stat); // Protagonist's value
            int index = 0; // Index of the selected combobox item

            // Iterate through each entry in the values array
            for (int i = 0; i < levelValues.Length; i++)
            {
                // If the protagonist's value is more than or equal to the current entry, increment the combobox index
                if (value >= levelValues[i])
                    index++;
                else // If not, then it is safe to exit the loop prematurely
                    break;
            }
            // Set the combobox's item to index
            statbox.SelectedIndex = (index > 0) ? index - 1 : index;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Select API
            SelectAPI API;
            if (tMAPIToolStripMenuItem.Checked)
                API = SelectAPI.TargetManager;
            else if (cCAPIToolStripMenuItem.Checked)
                API = SelectAPI.ControlConsole;
            else
                API = SelectAPI.TargetManager;
            PS3.ChangeAPI(API);

            // Attempt to connect to target
            if (!PS3.ConnectTarget())
            {
                DialogResult dialogResult = MessageBox.Show("Could not connect to target", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                // If the user clicks the retry button
                if (dialogResult == DialogResult.Retry)
                    toolStripButton1_Click(sender, e);

                return;
            }

            // Attempt to attach to process
            if (!PS3.AttachProcess())
            {
                DialogResult dialogResult = MessageBox.Show("Could not attach to process", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                // If the user clicks the retry button
                if (dialogResult == DialogResult.Retry)
                    toolStripButton1_Click(sender, e);

                return;
            }

            // If the user successfully connects and attaches
            tabControl1.Enabled = true; // Enable features of the program
            statusPrint("Successfully connected and attached", Color.Green); // Print success at the bottom of the screen
        }

        private void tMAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check TMAPI and uncheck CCAPI
            tMAPIToolStripMenuItem.Checked = true;
            cCAPIToolStripMenuItem.Checked = false;
        }

        private void cCAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check CCAPI and uncheck TMAPI
            cCAPIToolStripMenuItem.Checked = true;
            tMAPIToolStripMenuItem.Checked = false;
        }

        private void moneySet_Click(object sender, EventArgs e)
        {
            Protagonist.Money = (int)moneyInput.Value;
        }

        private void moneyGet_Click(object sender, EventArgs e)
        {
            moneyInput.Value = Protagonist.Money;
        }

        private void levelSet_Click(object sender, EventArgs e)
        {
            Protagonist.Level = (byte)levelInput.Value;
        }

        private void levelget_Click(object sender, EventArgs e)
        {
            levelInput.Value = Protagonist.Level;
        }

        private void knowledgeSet_Click(object sender, EventArgs e)
        {
            Protagonist.SetStat(Protagonist.Stat.Knowledge, knowledgeValues[knowledgeInput.SelectedIndex]);
        }

        private void knowledgeGet_Click(object sender, EventArgs e)
        {
            RetrieveSocialStat(Protagonist.Stat.Knowledge, knowledgeValues, knowledgeInput);
        }

        private void charmSet_Click(object sender, EventArgs e)
        {
            Protagonist.SetStat(Protagonist.Stat.Charm, charmValues[charmInput.SelectedIndex]);
        }

        private void charmGet_Click(object sender, EventArgs e)
        {
            RetrieveSocialStat(Protagonist.Stat.Charm, charmValues, charmInput);
        }

        private void proficiencySet_Click(object sender, EventArgs e)
        {
            Protagonist.SetStat(Protagonist.Stat.Proficiency, proficiencyValues[proficiencyInput.SelectedIndex]);
        }

        private void proficiencyGet_Click(object sender, EventArgs e)
        {
            RetrieveSocialStat(Protagonist.Stat.Proficiency, proficiencyValues, proficiencyInput);
        }

        private void gutsSet_Click(object sender, EventArgs e)
        {
            Protagonist.SetStat(Protagonist.Stat.Guts, gutsValues[gutsInput.SelectedIndex]);
        }

        private void gutsGet_Click(object sender, EventArgs e)
        {
            RetrieveSocialStat(Protagonist.Stat.Guts, gutsValues, gutsInput);
        }

        private void kindnessSet_Click(object sender, EventArgs e)
        {
            Protagonist.SetStat(Protagonist.Stat.Kindness, kindnessValues[kindnessInput.SelectedIndex]);
        }

        private void kindnessGet_Click(object sender, EventArgs e)
        {
            RetrieveSocialStat(Protagonist.Stat.Kindness, kindnessValues, kindnessInput);
        }

        private void personaSlotInput_Leave(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(personaSlotInput.Text, out value)) // Parse text as an int
            {
                if (value >= 1 && value <= 12) // If the value is between 1 and 12
                {
                    // Text has passed checks, return method without proceeding to error
                    personaSlot = value - 1; // Subtract 1 just to remove the need to add it later in get / set methods
                    return;
                }
            }

            // If the text has not passed the previous checks
            personaSlotInput.Text = "1";
            MessageBox.Show("The persona slot input is not a valid number between 1 and 12", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        private void personaLevelSet_Click(object sender, EventArgs e)
        {
            Persona.SetLevel(personaSlot, (byte)personaLevelInput.Value);
        }

        private void personaLevelGet_Click(object sender, EventArgs e)
        {
            personaLevelInput.Value = Persona.GetLevel(personaSlot);
        }

        private void personaGetStrength_Click(object sender, EventArgs e)
        {
            personaStrengthInput.Value = Persona.GetStat(personaSlot, Persona.Stat.Strength);
        }

        private void personaStrengthSet_Click(object sender, EventArgs e)
        {
            Persona.SetStat(personaSlot, Persona.Stat.Strength, (byte)personaStrengthInput.Value);
        }

        private void personaMagicSet_Click(object sender, EventArgs e)
        {
            Persona.SetStat(personaSlot, Persona.Stat.Magic, (byte)personaMagicInput.Value);
        }

        private void personaMagicGet_Click(object sender, EventArgs e)
        {
            personaMagicInput.Value = Persona.GetStat(personaSlot, Persona.Stat.Magic);
        }

        private void personaEnduranceSet_Click(object sender, EventArgs e)
        {
            Persona.SetStat(personaSlot, Persona.Stat.Endurance, (byte)personaEnduranceInput.Value);
        }

        private void personaEnduranceGet_Click(object sender, EventArgs e)
        {
            personaEnduranceInput.Value = Persona.GetStat(personaSlot, Persona.Stat.Endurance);
        }

        private void personaAgilitySet_Click(object sender, EventArgs e)
        {
            Persona.SetStat(personaSlot, Persona.Stat.Agility, (byte)personaAgilityInput.Value);
        }

        private void personaAgilityGet_Click(object sender, EventArgs e)
        {
            personaAgilityInput.Value = Persona.GetStat(personaSlot, Persona.Stat.Agility);
        }

        private void personaLuckSet_Click(object sender, EventArgs e)
        {
            Persona.SetStat(personaSlot, Persona.Stat.Luck, (byte)personaLuckInput.Value);
        }

        private void personaLuckGet_Click(object sender, EventArgs e)
        {
            personaLuckInput.Value = Persona.GetStat(personaSlot, Persona.Stat.Luck);
        }

        private void personaGet_Click(object sender, EventArgs e)
        {
            personaInput.Value = Persona.GetPersona(personaSlot);
        }

        private void personaSet_Click(object sender, EventArgs e)
        {
            Persona.SetPersona(personaSlot, (short)personaInput.Value);
        }

        private void personaNameSet_Click(object sender, EventArgs e)
        {
            Persona.SetPersona(personaSlot, (short)personaNameInput.SelectedIndex);
        }

        private void personaNameGet_Click(object sender, EventArgs e)
        {
            personaNameInput.SelectedIndex = Persona.GetPersona(personaSlot);
        }

        private void skillInput_ValueChanged(object sender, EventArgs e)
        {
            skillNameInput.SelectedIndex = (int)skillInput.Value;
        }

        private void skillNameInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            skillInput.Value = skillNameInput.SelectedIndex;
        }

        private void skillSet_Click(object sender, EventArgs e)
        {
            Persona.SetSkill(personaSlot, skillSlot, (short)skillInput.Value);
        }

        private void skillGet_Click(object sender, EventArgs e)
        {
            skillInput.Value = Persona.GetSkill(personaSlot, skillSlot);
        }

        private void skillSlotInput_ValueChanged(object sender, EventArgs e)
        {
            skillSlot = (int)skillSlotInput.Value - 1;
        }
    }
}
