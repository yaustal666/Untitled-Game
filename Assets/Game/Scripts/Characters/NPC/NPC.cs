using Reflex.Attributes;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractible
{
    [Inject] DialogueChannel _channel;
    [SerializeField] private string _name;

    public void Interact()
    {
        _channel.StartDialog(_name);
    }
}