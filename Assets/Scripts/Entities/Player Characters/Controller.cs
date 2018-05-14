using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{

    public float moveSpeed = 6;
    public float sprintSpeed = 8;

    Rigidbody myRigidbody;
    Animator animation;
    public AudioClip footstep;
    private float tSoundLength = 0.563f;
    Vector3 velocity;
    private float tSoundEnd;
    private bool sprintCooldown;
    public int sprintCooldownAmount = 100;

    public bool canMove;

    private PlayerStats playerStats;

    void Start()

    {
        myRigidbody = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        animation = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove)
        {
            moveSpeed = 0;
        }
        var groundPlane = new Plane(Vector3.up, -transform.position.y);
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        float hitDistance;

        if (groundPlane.Raycast(mouseRay, out hitDistance))
        {
            var lookAtPosition = mouseRay.GetPoint(hitDistance);
            transform.LookAt(lookAtPosition, Vector3.up);
        }

        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;

        if (velocity != Vector3.zero && Time.time > tSoundEnd)
        {
            AudioSource.PlayClipAtPoint(footstep, transform.position);
            tSoundEnd = Time.time + tSoundLength;
        }

        /*/ BEGINNING SPRINT CODE + SPRINT COOLDOWN /*/
        //  Checks if player is holding the shift key pressed and the breath bar is filled up to a value more than 20, if so then the players movement speed will be increased to the sprintspeed
        {
            moveSpeed = sprintSpeed;
            if (Input.GetKey(KeyCode.LeftShift) && playerStats.breath >= 20 && sprintCooldown != true)
            {
                playerStats.breath--;
                if (playerStats.breath < 20)
                {
                    sprintCooldown = true;
                }
            }
            else
            {
                moveSpeed = 6;
            }

            if (playerStats.breath > sprintCooldownAmount)
            {
                sprintCooldown = false;
            }
            /*/ END SPRINT CODE + COOLDOWN /*/


        }
    }

        void FixedUpdate()
    {


            myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (h != 0.0f || v != 0.0f)
                animation.SetFloat("walking", 1.0f);
            else
                animation.SetFloat("walking", 0.0f);
        }
    }
