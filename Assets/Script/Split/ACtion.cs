using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACtion : MonoBehaviour
{
    public static ACtion instance;
    public GameObject clone;
    public  void Awake()
    {
        instance = this;
    }
    public void Split()
    {
        if (transform.localScale.x <= 2)
        {
            return;
        }
        clone =  Instantiate(gameObject, transform.position, Quaternion.identity);
        clone.GetComponent<SplitForce>().enabled = true;
        clone.GetComponent<SplitForce>().SplitForceMethod();
    }
}
