using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitForce : MonoBehaviour
{
    public float Speed;
    public float LoseSpeed;
    public float DefaultSpeed;

    public bool ApplySplitForce = false;

    public void SplitForceMethod()
    {
        GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<PlayerController>().lockaction = true;
        Vector3 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2( dir.x , dir.z) * Mathf.Rad2Deg + 90f ;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Speed = DefaultSpeed;
        ApplySplitForce = true;


    }

    private void Update()
    {
        if (ApplySplitForce == false)
        {
            enabled = false;
            return;
        }
       // transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        Speed -= LoseSpeed * Time.deltaTime;
        if (Speed <= 0)
        {
            enabled = false;
            GetComponent<PlayerController>().lockaction = false;
            StartCoroutine(nameof(TriggerOn));
        }
    }
    IEnumerator TriggerOn()
    {
        yield return new WaitForSeconds(20f);
        GetComponent<BoxCollider>().isTrigger = true;
        PlayerController.CountSplit--;


    }
}
