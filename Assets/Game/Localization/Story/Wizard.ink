VAR wizard_first_dialogue = false
VAR introduced_wizard = false

=== npc_wizard ===
{ not introduced_wizard:
    Creature: Woke up? 
    ~ introduced_wizard = true
- else:
    Creature: Is there anything else you would like to know?
}

+ [Ask about the village]
    Creature: Well, it's a quiet place. Mostly farmers and craftspeople.
    Creature: Nothing exciting ever happens here, and honestly, that's exactly how we like it.
    -> npc_wizard

+ [Ask about his work]
    Creature: I mostly forge iron horseshoes and basic farming tools.
    Creature: Bring me 5 sticks. 
    ~ startQuest("quest_basic_quest")
    -> npc_wizard

+ [Say goodbye]
    Creature: Safe travels, friend. Watch your back on the roads!
    ~ introduced_wizard = false
    -> END