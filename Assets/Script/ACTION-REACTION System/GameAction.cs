using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    // 실제 액션이 일어나기 직전에 실행될 효과
    public List<GameAction> PreReactions { get; private set; } = new(); 
    //액션이 실행되는 도중에 발생하는 효과(드물게 사용)
    public List<GameAction> PerformReactions { get; private set; } = new();
    //액션이 완전히 끝난 후 실행될 효과(데미지 입힌 후 스텟 오르는 등 효과)
    public List<GameAction> PostReactions { get; private set; } = new();
}
