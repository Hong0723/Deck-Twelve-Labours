using UnityEngine;

public class Interactions : Singleton<Interactions>
{
    public bool PlayerIsDragging { get; set; } = false;
    public bool InputLocked { get; private set; } = false;

    public void SetInputLocked(bool locked)
    {
        InputLocked = locked;
        if (locked)
        {
            PlayerIsDragging = false;
        }
    }

    public bool PlayerCanInteract()
    {
        if (InputLocked) return false;
        if (!ActionSystem.Instance.IsPerforming) return true;
        else return false;
    }

    public bool PlayerCanHover()
    {
        if (InputLocked) return false;
        if (PlayerIsDragging) return false;
        return true;
    }
}
