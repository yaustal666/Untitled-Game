using Reflex.Attributes;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractible
{
    [Inject] DialogueChannel _channel;
    [SerializeField] private string _name;

    [ContextMenu("Dialogue")]
    public void Interact()
    {
        Debug.Log(name + " Send Request");
        _channel.StartDialog(_name);
    }
}