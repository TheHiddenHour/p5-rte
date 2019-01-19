# P5-RTE-Tool
A tool made using PS3Lib that allows for the real time editing of Persona 5 for jailbroken PS3 consoles. This tool allows for the editing for various aspects of the protagonist as well as their personas in every slot. Within this project are classes for both the protagonist and personas with methods to make this process more simple and easily adaptable to other projects. It also includes the ability to set specific personas and skills through an expansive, scrollable list.
## Protagonist Editor
This tool allows for the editing of the protagonist's social stats, money, and level. All attributes listed can be found in *Protagonist.cs*.

```
void SetStat(Stat stat, short value) - Sets the protagonist's stat
short GetStat(Stat stat) - Returns the protagonist's stat
```

The protagonist's money can also be retrieved and set through the int `Money`.

The protagonist's level can be get/set through the byte `Level`.

## Persona Editor
This tool also has options for persona editing for every possible slot between 1 and 12. These edits include persona, skills, level, and stats. All attributes listed can be found in *Persona.cs*.

```
void SetPersona(int slot, short value) - Sets the persona in the designated slot
short GetPersona(int slot) - Returns the persona at the designated slot
```
```
void SetLevel(int slot, byte value) - Set persona's level at the designated slot
byte GetLevel(int slot) - Returns persona's level at the designated slot
```
```
void SetStat(int slot, Stat stat, byte value) - Set's the persona's stat at the designated slot
byte GetStat(int slot, Stat stat) - Returns the persona's stat at the designated slot
```

### Credits
zarroboogs
TGE
ShrineFox
