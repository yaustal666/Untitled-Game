=== npc_bob ===
// Проверяем наш собственный флаг вместо счетчика кнота
{ not introduced_bob:
    Bob: Hello, traveler! I'm Bob, the local blacksmith. What brings you to our village?
    // Сразу меняем флаг, чтобы при возврате текст обновился
    ~ introduced_bob = true
- else:
    Bob: Is there anything else you would like to know?
}

// Меню вопросов к Бобу
+ [Ask about the village]
    Bob: Well, it's a quiet place. Mostly farmers and craftspeople.
    Bob: Nothing exciting ever happens here, and honestly, that's exactly how we like it.
    -> npc_bob // Теперь при возврате сработает ветка "else"

+ [Ask about his work]
    Bob: I mostly forge iron horseshoes and basic farming tools.
    Bob: If you need a weapon sharpened, come back when the forge is hot tomorrow.
    -> npc_bob

+ [Say goodbye]
    Bob: Safe travels, friend. Watch your back on the roads!
    ~ introduced_bob = false
    -> END