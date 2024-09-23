using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    float flip;
    public Transform map;
    Vector3 target;
    float speed = 5;
    Vector3 test;
    float singlestep;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            flip = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            flip = 0;
        }
        Debug.Log(flip);

        if (flip == 1)
        {
            target.z = map.rotation.z + 90;
            singlestep = speed * Time.deltaTime;
            Debug.Log(target);
            test = Vector3.RotateTowards(transform.forward, target, singlestep, 1f);

            map.rotation = Quaternion.LookRotation(test);
        }
        else if(flip == -1)
        {
            map.Rotate(0, 0, -90);
        }
    }
}
