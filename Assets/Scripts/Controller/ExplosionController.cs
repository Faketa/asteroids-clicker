using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    const float AnimationTime  = 1.5f;
    void Start()
    {
        Destroy(this.gameObject, AnimationTime);
    }
}
