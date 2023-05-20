using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACtion : MonoBehaviour
{
    public static ACtion instance;
  
    private Vector3 tempScale;
    public  void Awake()
    {
        instance = this;
    }
    public void Split()
    {
        //if (transform.localScale.x <= 2)
        //{
        //    return;
        //}
        //if (clone == null)
        //{
        //    clone = Instantiate(gameObject, transform.position, Quaternion.identity);
        //    clone.gameObject.tag = "SplitClone";
        //}
        instance = this;
        GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity);
        clone.gameObject.tag = "SplitClone";
        tempScale = transform.localScale;
        float biggerScaleX = tempScale.x - .5f;
        float biggerScaleY = tempScale.y - .5f;
        float biggerScaleZ = tempScale.z - .5f;

        tempScale.Set(biggerScaleX, biggerScaleY, biggerScaleZ);
        transform.localScale = tempScale;
        clone.GetComponent<SplitForce>().enabled = true;
        clone.GetComponent<SplitForce>().SplitForceMethod();
        
    }
}
