using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10;
	private float currentspeed;
    public int health = 10;

	private int speedboosts = 0;

	public GameObject mapmanager;

    public Text healthText;
	public Text speedText;

	private Rigidbody2D rgdbdy;

	public AudioSource audios;
	public AudioClip acceleration;

    void Start ()
    {
		mapmanager = GameObject.Find ("mapmanager");
		healthText = GameObject.Find ("health").GetComponent<Text> ();
		speedText = GameObject.Find ("speed").GetComponent<Text> ();
		healthText.text = ("Health: " + health);
		speedText.text = ("Speed-\nboosts: " + speedboosts);

		rgdbdy = GetComponent<Rigidbody2D> ();
		audios = GetComponent<AudioSource> ();
    }


	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space) && speedboosts != 0)
		{
			audios.PlayOneShot (acceleration);
			speedboosts--;
			speedText.text = ("Speed-\nboosts: " + speedboosts);
			speed = 5;
			StartCoroutine(ResetSpeed());
		}
	}


    void FixedUpdate()
    {
		if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5f)
		{
			currentspeed = Mathf.Lerp(currentspeed, speed * 0.7071f, 0.05f);
		}
		else
		{
			currentspeed = Mathf.Lerp(currentspeed, speed, 0.05f);
		}

		rgdbdy.velocity = new Vector2(Input.GetAxis("Horizontal") * currentspeed, Input.GetAxis("Vertical") * currentspeed);
    }

    public void DealDamage(int amount)
    {
        health = health - amount;
        healthText.text = "Health: " + health;

		if (health <= 0) 
		{
			mapmanager.GetComponent<GameManager> ().GameOver ();
		}
    }

	public void AddHealth(int amount)
	{
		health = health + amount;
		healthText.text = "Health: " + health;
	}

	public void AddBoost(int amount)
	{
		speedboosts = speedboosts + amount;
		speedText.text = ("Speed-\nboosts: " + speedboosts);
	}

	IEnumerator ResetSpeed()
	{
		yield return new WaitForSeconds(2);
		speed = 3;
	}
}