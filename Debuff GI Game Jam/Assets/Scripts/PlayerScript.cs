using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	public PolygonCollider2D collisionDetection;
	public GameObject shadow;
	public GameObject[] shadows;
	public float baseSpeed = 1.0F;
	float speedMod = 1;
	int numShadows = 10;
	public Image[] hearts; 
	ArrayList trail = new ArrayList();
	bool rewinding = false;
	public Sprite fullHeart, halfHeart, noHeart;
	// Use this for initialization
	void Start () {
		shadows = new GameObject[numShadows];
		shadows [0] = shadow;
		for (int i = 1; i < numShadows; i++) {
			shadows [i] = Instantiate (shadow);
		}

	}
	float accelLimit = 1;

	Vector2 prevAccel;

	float momentum = 0;
	// Update is called once per frame
	void UpdateHealthBar(){
		int currentHealth = GetComponent<LivingEntity> ().currentHealth;
		if (currentHealth > 1) {
			hearts [0].sprite = fullHeart;
		} else if (currentHealth == 1) {
			hearts [0].sprite = halfHeart;
		} else {
			hearts [0].sprite = noHeart;
		}
		if (currentHealth > 3) {
			hearts [1].sprite = fullHeart;
		} else if (currentHealth == 3) {
			hearts [1].sprite = halfHeart;
		} else {
			hearts [1].sprite = noHeart;
		}
		if (currentHealth > 5) {
			hearts [2].sprite = fullHeart;
		} else if (currentHealth == 5) {
			hearts [2].sprite = halfHeart;
		} else {
			hearts [2].sprite = noHeart;
		}
	}
	void Update () {
		if (rewinding) {
			if (trail.Count < 1) {
				rewinding = false;
				return;
			}
			TimeShadow shadow = (TimeShadow)trail [0];
			GetComponent<SpriteRenderer> ().flipX = shadow.flip;
			transform.position = shadow.pos;
			//trail.Remove (0);
			trail.RemoveRange (0, Mathf.Max(1, Mathf.Min(trail.Count - 1, 20)));
			if (trail.Count == 0)
				rewinding = false;
			return;
		}
		UpdateHealthBar ();
		//FixedUpdate ();
		Vector2 mouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;

		GetComponent<SpriteRenderer> ().flipX = mouse.x > 0;
		float mag = mouse.magnitude;
		if (mouse.magnitude > 1.0f) {
			mouse.Normalize (); 
			mouse /= accelLimit;
			mouse *= 1 + (mag / 100);
			momentum += Time.deltaTime;

			if (accelLimit > 1) {
				accelLimit -= Time.deltaTime;
			} else {
			}
		} else {
			momentum -= Time.deltaTime * 4;
			if (momentum < 0)
				momentum = 0;
			mouse /= accelLimit;
			if (accelLimit > 1) {
				momentum = 0;
			}
			if (accelLimit < 10) {
				accelLimit+=Time.deltaTime * 1;

			}
		}

		mouse *= ((baseSpeed) * speedMod) * (1 + Mathf.Sqrt(momentum * 0.1F));

		GetComponent<Rigidbody2D> ().velocity = mouse;
		//transform.position = newVec;

		if (Input.GetButtonDown("Vertical")) {
			rewinding = true;
			//transform.position = ((TimeShadow)trail[trail.Count - 1]).pos;
			//trail.Clear ();
		}
		if (Input.GetMouseButtonDown (1)) {
			GetComponent<SpriteAnim> ().PlayTemp (1, 1);
		}
		int i = trail.Count;
		for (int j = 1; j <= shadows.Length; j++) {
			if (i >= j) {
				shadows[j - 1].SetActive (true);
				Vector3 position = ((TimeShadow)trail [(((trail.Count - 1) * j) / shadows.Length)]).pos;
					shadows [j - 1].GetComponent<SpriteRenderer> ().flipX = ((TimeShadow)trail [(((trail.Count - 1) * j) / shadows.Length)]).flip;
				shadows[j - 1].transform.position = position;
			} else {
				shadows[j - 1].SetActive (false);
			}
		}
	}
	void FixedUpdate () {
		if (rewinding)
			return;
		if (GetComponent<Rigidbody2D> ().velocity.magnitude < 0.5) {
			if (trail.Count > 0) {
				trail.RemoveRange (trail.Count - Mathf.Min(trail.Count, 7), Mathf.Min(trail.Count, 7));
			}
			return;
		}
		trail.Insert (0, new TimeShadow(transform.position, ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).x > 0));
		if (trail.Count > 100) {
			trail.RemoveRange (100, trail.Count - 100);
		}
	}
	
	/*
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Enemy"){
			Destroy(coll.gameObject);
		}
	}*/
	
	void LateUpdate()
	{
		GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100);
		/*
		for (int i = 0; i < 5; i++)
			shadows[i].GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100) - i;
			*/
	}
	class TimeShadow{
		public TimeShadow(Vector3 pos, bool flip){
			this.pos = pos;
			this.flip = flip;
		}
		public Vector3 pos;
		public bool flip;

	}
	void Hurt(){
	}
}
