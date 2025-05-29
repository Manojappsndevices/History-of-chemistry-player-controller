using UnityEngine;

public class ObjectInteractionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject uiElementToActivate;
    [SerializeField] private bool deactivateOnExit = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uiElementToActivate.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (deactivateOnExit && other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}