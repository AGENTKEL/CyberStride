using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject glassBreakEffect;

    public void DestroyGlass()
    {
        Instantiate(glassBreakEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
