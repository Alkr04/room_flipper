using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    float flip;
    public Transform map;
    //public Transform target;
    //float speed = 5;
    //Vector3 test;
    //float singlestep;
    bool cool = false;
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

        if (!cool)
        {
            
            if (flip == 1)
            {
                /*Vector3 targetl = target.position - map.position;
                singlestep = speed * Time.deltaTime;
                //Debug.Log(map.rotation);
                //Debug.Log(target);
                test = Vector3.RotateTowards(transform.forward, targetl, singlestep, 0.0f);

                map.rotation = Quaternion.LookRotation(test);*/
                StartCoroutine(wait(flip));
            }
            else if(flip == -1)
            {
                StartCoroutine(wait(flip));
            }

        }
    }
    IEnumerator wait(float rotation)
    {
        cool = true;
        Vector3 temp = new Vector3 (0, 0, map.eulerAngles.z + (90 * rotation));
        if (temp.z < 0)
        {
            temp.z = 270;
            Debug.Log("270");
        }
        else if (temp.z > 359)
        {
            Debug.Log("359");
            temp.z = 0;
        }
        while (map.eulerAngles.z - temp.z != 0)
        {
            //Debug.Log(temp);
            //Debug.Log(map.eulerAngles - temp);
            yield return new WaitForSeconds(0.001f);
            map.Rotate(0, 0, rotation);
            Debug.Log(map.eulerAngles);
        }
        cool = false;
        yield return null;
    }
}
