using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float m_speed;           // The speed at which the character moves
    public static float m_idle;     // How long the character has been idle for

    private bool m_playing; 

    private Animator m_ani;         // The animator component, used to animate the player

    private AudioSource m_aud;      // The audio component, used to create sound

    public AudioClip m_pizza;       // "Pizza Time!" Player says this if they touch a customer (in a not dirty way)
    public AudioClip m_myBack;      // "My back!" Player shouts this when they fall in water

    private GameObject m_camera;    // The camera
    


    // Start is called before the first frame update
    void Start()
    {
        m_camera = GameObject.Find("Main Camera");

        m_ani = GetComponent<Animator>();
        m_ani.Play("Player_Idle");

        m_aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player falls in the water, they respawn
        if(transform.position.y <= -3)
        {
            transform.position = new Vector3(0, 0, -57);
            m_aud.clip = m_myBack;
            m_aud.Play();
        }

        // Vertical Rotation
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Euler(0, m_camera.transform.rotation.eulerAngles.y, 0);
            Move();
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Euler(0, m_camera.transform.rotation.eulerAngles.y + 180, 0);
            Move();
        }

        // Horizontal Rotation
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.rotation = Quaternion.Euler(0, m_camera.transform.rotation.eulerAngles.y + 45, 0).normalized;
                Move();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.rotation = Quaternion.Euler(0, m_camera.transform.rotation.eulerAngles.y + 135, 0).normalized;
                Move();
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, m_camera.transform.rotation.eulerAngles.y + 90, 0).normalized;
                Move();
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.rotation = Quaternion.Euler(0, m_camera.transform.rotation.eulerAngles.y + 315, 0).normalized;
                Move();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.rotation = Quaternion.Euler(0, m_camera.transform.rotation.eulerAngles.y + 225, 0).normalized;
                Move();
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, m_camera.transform.rotation.eulerAngles.y + 270, 0).normalized;
                Move();
            }
        }

        // Idle goes down
        m_idle -= 1 * Time.deltaTime;

        // Stop Running
        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            m_ani.Play("Player_Idle");
        }

    }

    void Move()
    {
        // Player always moves forward
        // As long as they're moving, idle is set to 1
        transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
        m_idle = 1;
        m_ani.Play("Player_Run");
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Customer")
        {
            m_aud.clip = m_pizza;
            m_aud.Play();
            collision.gameObject.tag = "Delivered";
        }
    }
}
