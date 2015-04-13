using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TakeHarmBox : MonoBehaviour, ITakeHarmBox
{
    /// <summary>
    /// 触发时受到伤害区块的表现
    /// </summary>
    public void Trigger()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HarmBox box = collider.GetComponent<HarmBox>();
        if (box != null)
        {
            Trigger();
        }
    }
}
