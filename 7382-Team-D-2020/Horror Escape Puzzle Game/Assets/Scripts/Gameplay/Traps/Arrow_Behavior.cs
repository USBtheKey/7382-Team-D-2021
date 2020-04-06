﻿
using UnityEngine;

public class Arrow_Behavior : MonoBehaviour
{
    [HideInInspector, SerializeField]private float speed;
    [HideInInspector, SerializeField]private bool homing = false;

    private Transform trackedObjPos;
    private Rigidbody2D rigid;

    public float Speed { set => speed = value; }
    public bool Homing { get => homing;  set => homing = value; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (homing) trackedObjPos = GameObject.FindGameObjectWithTag("Player").transform;
        
        transform.parent = null;
    }

    private void FixedUpdate()
    {
        if (homing) HomingToTarget();
        else if (rigid != null)
        {
            rigid.velocity = this.gameObject.transform.up * this.speed * 100 * Time.deltaTime;
        } 
    }


    private void HomingToTarget()
    {
        Vector2 direction = trackedObjPos.position - this.transform.position;
      
        float rotation = Vector3.Cross(direction.normalized, transform.up).z;
        rigid.angularVelocity = -rotation * 360f ;
        rigid.velocity = direction.normalized * this.speed * 100f * Time.deltaTime;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Trap"))
        {
            Invoke("DeleteGameobject", 3f);
            Destroy(rigid);
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<TrailRenderer>());
        }
    }

    private void DeleteGameobject()
    {
        Destroy(this.gameObject);
    }
}
