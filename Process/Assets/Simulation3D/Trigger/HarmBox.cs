using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HarmBox : MonoBehaviour, IHarmBox
{
    private long mid;

    private List<ITriggerBox> inputs = new List<ITriggerBox>();

    public long id
    {
        get { return mid; }
        set { mid = value; }
    }

    /// <summary>
    /// 触发时伤害区块的表现
    /// </summary>
    public virtual void Trigger(ITriggerBox input)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        TakeHarmBox input = collider.GetComponent<TakeHarmBox>();
        if (input != null && !inputs.Contains(input))
        {
            Trigger(input);
            input.Trigger(this);
            inputs.Add(input);
        }
    }

    private void OnDisable()
    {
        inputs.Clear();
    }
}
