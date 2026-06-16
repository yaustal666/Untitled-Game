VAR wizard_first_dialogue = false
VAR introduced_wizard = false

=== npc_wizard ===
{ not introduced_wizard:
    Creature: Woke up? 
    
- else:
    Creature: Is there anything else you would like to know?
}

+ [Ask about the village]
    Creature: Well, it's a quiet place. Mostly farmers and craftspeople.
    Creature: Nothing exciting ever happens here, and honestly, that's exactly how we like it.
    ~ startQuest("quest_basic_quest")
    -> npc_wizard
    
    ~ introduced_wizard = true
    + [Ask about forest]
        Creature: Scary place, but rich on some stuff. And monsters/
        Creature: Bring me 2 sticks. 
        ~startQuest("quest_basic_quest")
        -> npc_wizard
    
    + [Say goodbye]
        Creature: Safe travels, friend. Watch your back on the roads!
        ~ introduced_wizard = false
        -> END