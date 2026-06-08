VAR introduced_bob = false

=== npc_bob ===
{ not introduced_bob:
    Bob: Hello, traveler! I'm Bob, the local blacksmith. What brings you to our village?
    ~ introduced_bob = true
- else:
    Bob: Is there anything else you would like to know?
}

+ [Ask about the village]
    Bob: Well, it's a quiet place. Mostly farmers and craftspeople.
    Bob: Nothing exciting ever happens here, and honestly, that's exactly how we like it.
    -> npc_bob

+ [Ask about his work]
    Bob: I mostly forge iron horseshoes and basic farming tools.
    Bob: Bring me 5 sticks. 
    ~ startQuest("quest_basic_quest")
    -> npc_bob

+ [Say goodbye]
    Bob: Safe travels, friend. Watch your back on the roads!
    ~ introduced_bob = false
    -> END