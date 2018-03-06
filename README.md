# P5-RTE-Tool
A tool made using PS3Lib that allows for the editing of persona slots, stats, and skills in Persona 5.
# Methods
### Get address methods
```
uint GetPersonaAddress(int personaSlot)
uint GetPersonaLevelAddress(int personaSlot)
uint GetPersonaStatAddress(int personaSlot, PersonaStats personaStat)
uint GetPersonaSkillAddress(int personaSlot, int skillSlot)
uint GetSocialStatAddress(SocialStats socialStat)
```
### Get memory methods
```
byte[] GetPersona(int personaSlot)
byte[] GetPersonaSkill(int personaSlot, int skillSlot)
int GetPersonaLevel(int personaSlot)
int GetPersonaStat(int personaSlot, PersonaStats personaStat)
int GetSocialStat(SocialStats socialStat)
int GetMoney()
```
### Set memory methods
```
void SetPersona(int personaSlot, byte[] personaBytes)
void SetPersonaSkill(int personaSlot, int skillSlot, byte[] skillBytes)
void SetPersonaLevel(int personaSlot, int levelValue)
void SetPersonaStat(int personaSlot, PersonaStats personaStat, int personaStatValue)
void SetSocialStat(SocialStats socialStat, int socialStatValue)
void SetMoney(int moneyValue)
```
# Parameters
**personaSlot** - 1-12. The persona slot to be modified.

**skillSlot** - 1-8. The skill of the persona to be modified.

**personaBytes** - {0x01, 0x02}, {0x0F, 0xAB}, etc. The bytes of the persona to be written.

**levelValue** - 1-99. The level of the persona to be written.

**personaStat** - Strength, Endurance, etc. The statistic of the persona to be modified.

**personaStatValue** - 1-99. The value of the persona's statistic to be written.

**skillBytes** - {0x01, 0x02}, {0x0F, 0xAB}, etc. The bytes of the skill to be written.

**socialStat** - Knowledge, Charm, etc. The social statistic of the protagonist to be modified.

**socialStatValue** - (Multiple ranges). The value of the protagonist's statistic to be written.

**moneyValue** - The value of the protagonist's money to be written.

# Credits
**TGE** - His code that enabled RPCS3 support for this tool. It wouldn't have been implemented without him.

**ShrineFox** - Calling my little tool "cool" and ultimately influencing its release. This thing would have never seen the light of day without him.

**Amicitia** - The discord group was supportive of my tool and helped me whenever possible.
