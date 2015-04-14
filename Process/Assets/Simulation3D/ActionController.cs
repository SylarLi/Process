using UnityEngine;

public class ActionController : MonoBehaviour
{
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        FireState(Input.GetKey(KeyCode.J));
        StabState(Input.GetKey(KeyCode.L));
    }

    public void FireState(bool state)
    {
        animator.SetBool("fire", state);
    }

    public void StabState(bool state)
    {
        animator.SetBool("stab", state);
    }

    #region Animation Events

    public void Fire(string bulletPath)
    {
        GameObject origin = transform.Find(bulletPath).gameObject;
        GameObject bullet = GameObject.Instantiate(origin) as GameObject;
        bullet.SetActive(true);
        float dir = Mathf.Sign(transform.localScale.x);
        bullet.transform.position = origin.transform.position;
        bullet.rigidbody2D.AddForce(new Vector2(dir * 10, 0), ForceMode2D.Impulse);
        rigidbody2D.AddForce(new Vector2(-transform.localScale.x * 3, 0), ForceMode2D.Impulse);
    }

    #endregion
}
