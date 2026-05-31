using Reflex.Attributes;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Inject] private Game _game;
    public string Destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _game.SceneTransition(Destination);
        }
    }
}
