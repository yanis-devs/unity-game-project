using UnityEngine;

public class testing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.position += new Vector3(10, 0, 0);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += new Vector3(1, 0, 0) * 10 * Time.deltaTime;
            }
            transform.position += new Vector3(1, 0, 0) * 5 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.position += new Vector3(-10, 0, 0);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += new Vector3(-1, 0, 0) * 50 * Time.deltaTime;
            }
            transform.position += new Vector3(-1, 0, 0) * 5 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.position += new Vector3(0, 10, 0);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += new Vector3(0, 1, 0) * 10 * Time.deltaTime;
            }
            transform.position += new Vector3(0, 1, 0) * 5 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.position += new Vector3(0, -10, 0);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += new Vector3(0, -1, 0) * 10 * Time.deltaTime;
            }
            transform.position += new Vector3(0, -1, 0) * 5 * Time.deltaTime;
        }
    }
}
