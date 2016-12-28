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
    private Vector3 drag_Last_direction = new Vector3(0, 0, 0);
    private Vector3 drag_distance = new Vector3(0, 0, 0);


	// Use this for initialization
	void Start () {
        rotation_vector[0] = new Vector3(0, 1, 0);
        rotation_vector[1] = new Vector3(1,0,0);
        rotation_vector[2] = new Vector3(0, 0, 1);
    }
	
	// Update is called once per frame
	void Update () {

        mouse();
        rotate_motion();

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
        //Preset vars
        Vector3 drag_current_position = Input.mousePosition;

        

        //PRESS MOUSE BUTTON
        if (Input.GetMouseButtonDown(0))
        {
            
            //
            dragging = true;

            //
            if (action_count == 0)
            {
                rotate(-270, 0, 0, 0.15f);
                drag_last_position = drag_current_position;
                action_count++;
            }
            else if (action_count == 1)
            {
                
                
            }

            
        } else if(Input.GetMouseButtonUp(0)) { 
        //RELEASE DRAG
            dragging = false;
        }

        //DRAG
        if (dragging)
        {
            //set basic vars
            drag_current_position.z = 1f; 
            Vector3 vec = Camera.main.ScreenToWorldPoint(drag_current_position);
            Vector3 difference = drag_current_position - drag_last_position;

            //velocity y
            Vector3 drag_screen_current = new Vector3(0,0,0);
            drag_screen_current.y = drag_current_position.y / Screen.height;
            drag_screen_current.x = drag_current_position.x / Screen.width;
            //--
            Vector3 drag_screen_last = new Vector3(0, 0, 0);
            drag_screen_last.y = drag_last_position.y / Screen.height;
            drag_screen_last.x = drag_last_position.x / Screen.width;
            //--
            float magnitude_current = drag_screen_current.magnitude;
            float magnitude_last = drag_screen_last.magnitude;
            //-
            float drag_velocity = Mathf.Abs((magnitude_current - magnitude_last) / Time.deltaTime);
            


            if (difference.y != 0)
            {
                //DIRECTION CALCULATION
                float sign_x = Mathf.Sign(difference.x);
                float sign_y = Mathf.Sign(difference.y);
                if (drag_Last_direction.y != sign_y)
                {
                    
                    //save direction
                    drag_distance = Vector3.zero;
                    drag_Last_direction.y = sign_y;

                    if (drag_velocity > 0.3)
                    {
                        Debug.Log(drag_velocity);
                        rotate(360 * sign_y * -1f, 0, 0, 0.2f);
                    }

                } else
                {
                    
                    //save total drag distance in one direction
                    drag_distance.y += difference.y;

                   
                }
            }
            
            



            drag_last_position = drag_current_position;

            //Move
            transform.position = vec;
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

    private void rotate_motion ()
    {

        if (rotating)
        {

            if (Mathf.Abs(rotating_values[0]) + Mathf.Abs(rotating_values[1]) + Mathf.Abs(rotating_values[2]) > 0)
            {
                //preset vars
                float rotation_left = 0;
                float actual_rotation_degrees = 0;

                //spin
                for (int i = 0; i < 3; i++)
                {
                    if (Mathf.Abs(rotating_values[i]) > 0)
                    {
                        //set vars
                        actual_rotation_degrees = rotating_values[i + 3] * Time.deltaTime;
                        rotation_left = rotating_values[i];
                        if (Mathf.Abs(rotation_left) > Mathf.Abs(actual_rotation_degrees))
                        {
                            rotating_values[i] -= actual_rotation_degrees;
                        }
                        else
                        {
                            actual_rotation_degrees = rotation_left;
                            rotating_values[i] = 0;
                        }

                        rotation_left = rotating_values[i];
                        transform.Rotate(rotation_vector[i] * actual_rotation_degrees, Space.Self);
                    }
                }

            }
            else
            {
                rotating = false;
            }
        }

    }
}
