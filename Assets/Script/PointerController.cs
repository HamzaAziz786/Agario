using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    public float speed;


    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed * moveHorizontal * Time.deltaTime,
            0, speed * moveVertical * Time.deltaTime);
        transform.Translate(movement);
    }








}
