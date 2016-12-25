using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_gun : MonoBehaviour {

    public bool rotating = false;
    public float[] rotating_values = new float[6];
    private int action_count = 0;
    private Vector3[] rotation_vector = new Vector3[3];

    private bool dragging = false;
    private Vector3 drag_last_position = new Vector3(0,0,0);


	// Use this for initialization
	void Start () {
        rotation_vector[0] = new Vector3(0, 1, 0);
        rotation_vector[1] = new Vector3(1,0,0);
        rotation_vector[2] = new Vector3(0, 0, 1);
    }
	
	// Update is called once per frame
	void Update () {

        mouse();

            if (rotating)
            {

                if (Mathf.Abs(rotating_values[0]) + Mathf.Abs(rotating_values[1]) + Mathf.Abs(rotating_values[2]) > 0)
                {
                    //preset vars
                    float rotation_left = 0;
                    float actual_rotation_degrees = 0;

                    //spin
                    for (int i =  0; i < 3; i++)
                    {
                        if (Mathf.Abs(rotating_values[i]) > 0)
                        {
                            //set vars
                            actual_rotation_degrees = rotating_values[i + 3] * Time.deltaTime;
                            rotation_left = rotating_values[i];
                            if(Mathf.Abs(rotation_left) > Mathf.Abs(actual_rotation_degrees)) {
                                rotating_values[i] -= actual_rotation_degrees;
                            } else
                            {
                                actual_rotation_degrees = rotation_left;
                                rotating_values[i] = 0;
                            }

                            rotation_left = rotating_values[i];
                            transform.Rotate(rotation_vector[i] * actual_rotation_degrees, Space.Self);
                        }
                    }
                
                } else
                {
                    rotating = false;
                }
            }

        
	}

    public void rotate(float x, float y, float z, float time)
    {
        
        if( Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z) > 0)
        {

            //Set vars
            float sign = 1;
            rotating = true;

            //set array
            if (Mathf.Abs(x) > 0)
            {
                sign = Mathf.Sign(x);
                rotating_values[0] = rotating_values[0] + x;
                rotating_values[3] = rotating_values[0] / time;
            }
            if (Mathf.Abs(y) > 0)
            {
                sign = Mathf.Sign(y);
                rotating_values[1] = rotating_values[1] + y;
                rotating_values[4] = rotating_values[1] / time;
            }
            if (Mathf.Abs(z) > 0)
            {
                sign = Mathf.Sign(z);
                rotating_values[2] = rotating_values[2] + z;
                rotating_values[5] = rotating_values[2] / time;
            }
            
        }
    }

    private void mouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            action_count++;
            if (action_count == 1)
            {
                rotate(-270, 0, 0, 0.15f);
            }
            else if (action_count > 1)
            {
                rotate(360, 0, 0, 0.2f);
            }
            dragging = true;
        }

        if(dragging)
        {
            //set basic vars
            Vector3 mouse = Input.mousePosition;
            mouse.z = 1f; 
            Vector3 vec = Camera.main.ScreenToWorldPoint(mouse);

            Vector3 difference = mouse - drag_last_position;
            Debug.Log(difference);
            drag_last_position = mouse;

            //Move
            transform.position = vec;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
        

        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 10);
        //draw invisible ray cast/vector
        Debug.DrawLine(ray.origin, hit.point);
        //log hit area to the console
        Debug.Log(hit.point);

    */




    }
}
