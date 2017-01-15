using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMove : MonoBehaviour
{
    private List<GameObject> navpoints;
    public GameObject mapmanager;
    private GameObject spawnpoint;

    public GameObject healthobject;
    public Sprite[] healthsprites = new Sprite[4];

    public GameObject player;

	public int enemytype;
	private int health;
	private int spritecounter;
	private float speed;
	private int damage;

    private bool tooclose = true;

	private SpriteRenderer sr;

	public AudioSource audios;
	public AudioClip death;
	public AudioClip kamikaze;

	void Start ()
    {
		sr = GetComponent<SpriteRenderer> ();
		audios = GetComponent<AudioSource> ();

		if (enemytype == 0)
		{
			health = 4;
			spritecounter = 3;
			speed = 2;
			damage = 2;
		}
		else if (enemytype == 1)
		{
			health = 2;
			spritecounter = 1;
			speed = 3;
			damage = 1;
		}

		mapmanager = GameObject.Find ("mapmanager");
		player = GameObject.Find ("robot_prefab(Clone)");

        Physics2D.IgnoreLayerCollision(8,9);

        RandomMap mapscript = mapmanager.GetComponent<RandomMap>();
        navpoints = mapscript.returnSpawnpoints();

		healthobject.GetComponent<SpriteRenderer>().sprite = healthsprites[spritecounter];

        while (tooclose)
        {
            int rand = Random.Range(0, navpoints.Count);
            spawnpoint = navpoints[rand];
            if (Vector2.Distance(spawnpoint.transform.position, player.transform.position) > 10)
            {
                transform.position = spawnpoint.transform.position;
                tooclose = false;
            }
        }
    }
    
    void Update ()
    {
        if (player.transform.position.x >= transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

	void FixedUpdate ()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMove>().DealDamage(damage);
			mapmanager.GetComponent<GameManager> ().EnemyKilled();
			audios.PlayOneShot (kamikaze);
			GetComponent<Renderer> ().enabled = false;
			foreach (Renderer r in GetComponentsInChildren<Renderer>()) {r.enabled = false;}
			GetComponent<BoxCollider2D> ().enabled = false;
			Destroy (gameObject, kamikaze.length);
        }
    }

    public void DealDamage (int amount)
    {
        health = health - amount;
        spritecounter = spritecounter - amount;

        if (spritecounter > 0)
        {
            healthobject.GetComponent<SpriteRenderer>().sprite = healthsprites[spritecounter];
        }
        else if(health <= 0)
        {
			mapmanager.GetComponent<GameManager> ().EnemyKilled();

			audios.PlayOneShot (death);
			GetComponent<Renderer> ().enabled = false;
			foreach (Renderer r in GetComponentsInChildren<Renderer>()) {r.enabled = false;}
			GetComponent<BoxCollider2D> ().enabled = false;
			Destroy (gameObject, death.length);
        }
    }
}
