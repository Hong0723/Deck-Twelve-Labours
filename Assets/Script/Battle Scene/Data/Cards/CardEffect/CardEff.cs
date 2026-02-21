using UnityEngine;

[CreateAssetMenu(fileName = "CardEff", menuName = "Scriptable Objects/CardEff")]
public abstract class CardEff : ScriptableObject
{
    public abstract void ExecuteEff();
}
