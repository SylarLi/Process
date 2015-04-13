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
        animator.SetBool("fire", Input.GetKey(KeyCode.J));
    }

    public void Fire()
    {
        GameObject bullet = GameObject.Instantiate(GameObject.Find("Bullet")) as GameObject;
        bullet.SetActive(true);
        float dir = Mathf.Sign(transform.localScale.x);
        bullet.transform.position = transform.position + new Vector3(dir * 0.85f, 0.5f, 0);
        bullet.rigidbody2D.AddForce(new Vector2(dir * 10, 0), ForceMode2D.Impulse);
        rigidbody2D.AddForce(new Vector2(-transform.localScale.x * 3, 0), ForceMode2D.Impulse);
    }
}
