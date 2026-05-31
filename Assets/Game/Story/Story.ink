VAR introduced_bob = false
VAR introduced_margaret = false

INCLUDE Bob.ink

// ==========================================
//                МАРГАРЕТ
// ==========================================
=== npc_margaret ===
{ not introduced_margaret:
    Margaret: Ah, a fresh face! Welcome. I am Margaret, the herbalist. How can I help you today?
    ~ introduced_margaret = true
- else:
    Margaret: Do you have any other questions for me?
}

// Меню вопросов к Маргарет
+ [Ask about potions]
    Margaret: I brew standard healing salves and remedies for winter fevers.
    Margaret: My supply of Moonflower is low right now, though, so don't get hurt just yet!
    -> npc_margaret

+ [Ask about the forest]
    Margaret: The woods surrounding us are ancient and dangerous.
    Margaret: Stay on the main path. The deeper thickets hide things that don't like uninvited guests.
    -> npc_margaret

+ [Say goodbye]
    Margaret: May the wilds guide your steps. Goodbye.
    -> END
