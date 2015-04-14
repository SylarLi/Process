using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TakeHarmBox : MonoBehaviour, ITakeHarmBox
{
    private long mid;

    public long id
    {
        get { return mid; }
        set { mid = value; }
    }

    /// <summary>
    /// 触发时受到伤害区块的表现
    /// </summary>
    public virtual void Trigger(ITriggerBox input)
    {
        Debug.Log("I'm hurt!!!");
    }
}
