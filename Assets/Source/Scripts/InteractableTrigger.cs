using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private Interactable _interactable;
    private void Start()
    {
        _interactable = GetComponentInParent<Interactable>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // assign the interactable to the player
        var player = other.GetComponent<Player>();
        player.interactable = _interactable;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // remove the interactable from the player
        var player = other.GetComponent<Player>();
        if(player.interactable == _interactable)
        {
            player.interactable = null;
        }
    }
}