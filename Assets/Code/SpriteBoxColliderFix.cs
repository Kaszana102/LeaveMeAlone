using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBoxColliderFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider.size = spriteRenderer.bounds.size;
    }

}
