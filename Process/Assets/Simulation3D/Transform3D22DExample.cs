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

    public float linearDrag = 5f;

    public float moveDrag = 6f;

    private Vector3 mPosition;

    private Vector3 mLocalScale;

    private GameObject shadow;

    void Start()
    {
        mPosition = transform.position;
        mLocalScale = transform.localScale;
        projectY = mPosition.y;
        shadow = transform.Find("Shadow").gameObject;
        rigidbody2D.drag = linearDrag;
    }

    private void FixedUpdate()
    {
        if (airTime > 0)
        {
            rigidbody2D.AddForce(new Vector2(0, gravity * rigidbody2D.mass), ForceMode2D.Force);
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                localScale = new Vector3(Mathf.Abs(localScale.x), 1, 1);
                rigidbody2D.AddForce(new Vector2(moveDrag, 0), ForceMode2D.Force);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                localScale = new Vector3(-Mathf.Abs(localScale.x), 1, 1);
                rigidbody2D.AddForce(new Vector2(-moveDrag, 0), ForceMode2D.Force);
            }
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
            if ((airTime -= Time.deltaTime) <= 0)
            {
                rigidbody2D.velocity -= new Vector2(airSlideH * dirSpeedH, rigidbody2D.velocity.y);
                rigidbody2D.drag = linearDrag;
                position = new Vector3(position.x, position.y, 0);
                airTime = 0;
                airSlideH = 0;
                airSlideV = 0;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.K))
            {
                rigidbody2D.velocity = new Vector2(0, jumpStartSpeed);
                rigidbody2D.drag = 0;
                airTime = Mathf.Abs(jumpStartSpeed / gravity) * 2;
                projectY = position.y;
            }
        }
        position = TransformUtil.Position2DY23DZ(transform.position, projectY);
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

    public Vector3 localScale
    {
        get
        {
            return mLocalScale;
        }
        set
        {
            if (mLocalScale != value)
            {
                mLocalScale = value;
                transform.localScale = mLocalScale;
            }
        }
    }

}
