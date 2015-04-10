using UnityEngine;
using System.Collections;

public class Transform3D22DExample : MonoBehaviour
{
    private float projectY = float.NaN;

    private float airTime = 0;

    private float airSlideH = 0;

    private float airSlideV = 0;

    public float gravity = -9.8f;

    public float jumpStartSpeed = 3f;

    public float dirSpeedH = 1f;

    public float dirSpeedV = 1f;

    private Vector3 mPosition;

    private GameObject shadow;

    void Start()
    {
        mPosition = transform.position;
        shadow = GameObject.Find("Shadow");
    }

    private void FixedUpdate()
    {
        if (airTime > 0)
        {
            rigidbody2D.AddForce(new Vector2(0, gravity * rigidbody2D.mass), ForceMode2D.Force);
        }
    }

    private void Update()
    {
        if (airTime > 0)
        {
            if (airSlideH == 0 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    airSlideH = -1;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    airSlideH = 1;
                }
                rigidbody2D.velocity += new Vector2(airSlideH * dirSpeedH, 0);
            }
            if (airSlideV == 0 && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
            {
                if (Input.GetKey(KeyCode.S))
                {
                    airSlideV = -1f;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    airSlideV = 1f;
                }
                rigidbody2D.velocity += new Vector2(0, airSlideV * dirSpeedV);
            }
            if (airSlideV != 0)
            {
                projectY += airSlideV * dirSpeedV * Time.deltaTime;
            }
            position = TransformUtil.Position2DY23DZ(transform.position, projectY);
            if ((airTime -= Time.deltaTime) <= 0)
            {
                airTime = 0;
                airSlideH = 0;
                airSlideV = 0;
                rigidbody2D.velocity = Vector2.zero;
                position = new Vector3(position.x, position.y, 0);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.J))
            {
                airTime = Mathf.Abs(jumpStartSpeed / gravity) * 2;
                projectY = position.y;
                rigidbody2D.velocity += new Vector2(0, jumpStartSpeed);
            }
        }
        shadow.transform.position = new Vector3(position.x, position.y, 0);
    }

    public Vector3 position
    {
        get
        {
            return mPosition;
        }
        set
        {
            if (mPosition != value)
            {
                mPosition = value;
                transform.position = TransformUtil.Position3DZ22DY(mPosition);
            }
        }
    }
}
