 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 25.0f;
    public float padding = 1.0f;
    public GameObject projectile;
    public float projectileSpeed = 5.0f;
    public float firingRate = 0.2f;
    public float health = 250.0f;
    public AudioClip fireSound;

    protected Joystick joystick;
    protected Joybutton joyButton;
    protected Rigidbody2D rigidBody;
    protected bool firing;

    float xmin;
    float xmax;
    float ymin;
    float ymax;

	// Use this for initialization
	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        Vector3 upMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance));
        Vector3 downMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));

        xmin = leftMost.x + padding;
        xmax = rightMost.x - padding;
        ymin = downMost.y + padding;
        ymax = upMost.y - padding;

        joystick = FindObjectOfType<Joystick>();
        joyButton = FindObjectOfType<Joybutton>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

	void fire() {
        Vector3 offset = new Vector3(0, 1, 0);
        GameObject beam = Instantiate(projectile, transform.position + offset, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space)) {
            InvokeRepeating("fire", 0.000001f, firingRate);
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke("fire");
        }

        if (!firing && joyButton.Pressed)
        {
            firing = true;
            InvokeRepeating("fire", 0.000001f, firingRate);
        }
        if (firing && !joyButton.Pressed)
        {
            firing = false;
            CancelInvoke("fire");
        }

        /*if (Input.GetButton("W") && Input.GetButton("A")) {
            transform.position += new Vector3(-speed * Time.deltaTime, speed * Time.deltaTime, 0);
        } else if (Input.GetButton("W") && Input.GetButton("D")) {
            transform.position += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
        } else if (Input.GetButton("A") && Input.GetButton("S")) {
            transform.position += new Vector3(-speed * Time.deltaTime, -speed * Time.deltaTime, 0);
        } else if (Input.GetButton("S") && Input.GetButton("D")) {
            transform.position += new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0);
        } else if (Input.GetKey(KeyCode.W)) {
            transform.position += Vector3.up * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.S)) {
            transform.position += Vector3.down * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }*/

        transform.position += new Vector3(joystick.Horizontal * speed * Time.deltaTime + Input.GetAxis("Horizontal")*0.5f,
                                          joystick.Vertical * speed * Time.deltaTime + Input.GetAxis("Vertical")*0.5f,
                                          0);

        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        float newY = Mathf.Clamp(transform.position.y, ymin, ymax);

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.getDamage();
            missile.hit();
            if (health <= 0)
            {
                die();
            }
        }
    }
    void die()
    {
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel("Lose Screen");
        Destroy(gameObject);
    }
}
