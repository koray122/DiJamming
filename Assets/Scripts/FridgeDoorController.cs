using UnityEngine;

public class FridgeDoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        animator.SetTrigger("Open");
        isOpen = true;
    }

    private void CloseDoor()
    {
        animator.SetTrigger("Close");
        isOpen = false;
    }
}