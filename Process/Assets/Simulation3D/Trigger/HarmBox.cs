using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HarmBox : MonoBehaviour, IHarmBox
{
    /// <summary>
    /// 触发时伤害区块的表现
    /// </summary>
    public void Trigger()
    {
        GameObject.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        TakeHarmBox box = collider.GetComponent<TakeHarmBox>();
        if (box != null)
        {
            Trigger();
        }
    }
}
