using UnityEngine;


public abstract class ItemUseAction : ScriptableObject
{
    public abstract void Execute(GameObject user);
}
