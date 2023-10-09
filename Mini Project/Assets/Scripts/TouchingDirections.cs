using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D touchingCol;
    Animator animator;
    public ContactFilter2D castFillter;
    public float groundDistance = 0.05f;

    private Vector2 wallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];


    [SerializeField] private bool _isGrounded = true;
    

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool("isGrounded", _isGrounded);

        }
    }

    

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFillter, groundHits, groundDistance) > 0;
       
    }
}
