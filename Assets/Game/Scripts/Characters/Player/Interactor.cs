using Reflex.Attributes;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Inject] private InputReader _inputReader;
    public IInteractible _interactible;

    private void Start()
    {
        _inputReader.InteractPressed += OnInteract;
    }

    public void OnInteract()
    {
        _interactible?.Interact();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractible>(out var interactible))
        {
            _interactible = interactible;
        }
    }

    private void OnDestroy()
    {
        _inputReader.InteractPressed -= OnInteract;
    }
}