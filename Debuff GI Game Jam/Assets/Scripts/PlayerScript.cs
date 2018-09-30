using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
	public PolygonCollider2D collisionDetection;
	public GameObject shadow;
	public GameObject[] shadows;
	public Camera mainCamera;
	public float baseSpeed = 1.0F;
	float speedMod = 0.5F;
	int numShadows = 30;
	public Image[] hearts; 
	ArrayList trail = new ArrayList();
	bool rewinding = false;
	public bool lockOnShadow = false;
	public Sprite fullHeart, halfHeart, noHeart;
	protected Level[] levels = new Level[]{
		new Level("Desert", new Vector2(-4, 2), new string[]{"Prefabs/jellyfish_prefab"}, new Vector2[]{new Vector2(14, 21)}),
		new Level("Oasis", new Vector2(-4, 2), new string[]{"Prefabs/gem_prefab 1"}, new Vector2[]{new Vector2(10, 10)})
	};
	public int level = 0;
	// Use this for initialization
	void Start () {
		shadows = new GameObject[numShadows];
		shadows [0] = shadow;
		for (int i = 1; i < numShadows; i++) {
			shadows [i] = Instantiate (shadow);
		}
		levels [level].load ();
	}
	float accelLimit = 1;

	Vector2 prevAccel;

	float momentum = 0;
	// Update is called once per frame
	void UpdateHealthBar(){
		int currentHealth = GetComponent<LivingEntity>().currentHealth;
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
	bool prePortalAnim = false;
	void levelUpdate(){
		bool portal = true;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		if (GetComponent<SpriteAnim> ().loops > 0 || lockOnShadow || (noTrail && trail.Count > 0))
			portal = false;
		foreach (GameObject enemy in enemies){
			if (enemy.activeSelf) {
				portal = false;
				break;
			}
		}
		GameObject[] waypoints = GameObject.FindGameObjectsWithTag ("Waypoint");
		foreach (GameObject enemy in waypoints){
			if (enemy.activeSelf) {
				portal = false;
				break;
			}
		}
		if (portal) {
			if (!noTrail) {
				

				noTrail = true;
				return;
			}
			if (noTrail && !prePortalAnim) {
				GetComponent<SpriteAnim> ().PlayTemp (4, 1);
				prePortalAnim = true;
				return;
			}

			++level;
			if (level >= levels.Length) {
				SceneManager.LoadScene ("MainMenu");
				return;
			}
			levels [level].load ();
			GetComponent<SpriteAnim> ().PlayTemp (2, 4);
		}
	}
	void Update () {
		levelUpdate ();
		if (rewinding) {
			if (trail.Count < 1) {
				rewinding = false;
				return;
			}
			TimeShadow shadow = (TimeShadow)trail [0];
			GetComponent<SpriteRenderer> ().flipX = shadow.flip;
			transform.position = shadow.pos;
			//trail.Remove (0);
			trail.RemoveRange (0, Mathf.Max(1, Mathf.Min(trail.Count - 1, 80)));
			if (trail.Count == 0)
				rewinding = false;
			return;
		}
		UpdateHealthBar ();
		if (lockOnShadow) {
			foreach (GameObject obj in shadows) {
				if (obj.GetComponent<SpriteAnim> ().loops > 0) {
					mainCamera.transform.localPosition = new Vector3 (obj.transform.position.x - transform.position.x, obj.transform.position.y - transform.position.y, mainCamera.transform.localPosition.z);
				}
			}
		} else {
			mainCamera.transform.localPosition = new Vector3 (0, 0, mainCamera.transform.localPosition.z);
		}
		//FixedUpdate ();
		Vector2 mouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
		if (noTrail)
			mouse = Vector2.zero;
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

		mouse *= ((baseSpeed) * speedMod) * (1 + Mathf.Sqrt(momentum * 0.1F))*2;

		GetComponent<Rigidbody2D> ().velocity = mouse;
		//transform.position = newVec;

		if (Input.GetButtonDown("Vertical")) {
			rewinding = true;
			//transform.position = ((TimeShadow)trail[trail.Count - 1]).pos;
			//trail.Clear ();
		}
		if (Input.GetMouseButtonDown (0)) {
			GetComponent<SpriteAnim> ().PlayTemp (1, 1);
			BasicAttack ();
		}
		if (Input.GetMouseButtonDown (1)) {
			ShadowAttack ();
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
	bool noTrail = false;
	void FixedUpdate () {
		
		if (rewinding)
			return;
		if (GetComponent<Rigidbody2D> ().velocity.magnitude < 0.5 || noTrail) {
			if (trail.Count > 0) {
				trail.RemoveRange (trail.Count - Mathf.Min(trail.Count, 7), Mathf.Min(trail.Count, 7));
			}
			return;
		}
		//timeSinceUpdate += Time.fixedDeltaTime;
		/*if (timeSinceUpdate < 0.05) {
			
			return;
		}*/
		//timeSinceUpdate = 0;
		trail.Insert (0, new TimeShadow(transform.position, ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).x > 0));
		if (trail.Count > 300) {
			trail.RemoveRange (300, trail.Count - 300);
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
	public void Hurt(){
		GetComponent<SpriteAnim> ().PlayTemp (2, 1);
		if (GetComponent<LivingEntity> ().currentHealth <= 0) {
			GetComponent<SpriteAnim> ().PlayAnimation (2);
		}
	}
	void ShadowAttack(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach (GameObject enemy in enemies){
			for(int i = shadows.Length - 1; i >= 0; i--) {
				GameObject shadow = shadows [i];
				if (Mathf.Abs ((enemy.transform.position - shadow.transform.position).magnitude) < 1) {
					momentum = 0;
					shadow.GetComponent<SpriteAnim> ().PlayTemp (1, 1);
					enemy.GetComponent<LivingEntity> ().Hurt();
					lockOnShadow = true;
					break;
				}
			}
		}

	}

	
	// the left click attack of player
	// hurts enemies within a certain distance of player
	void BasicAttack() {
		float range = 3.0f;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		
		foreach (GameObject enemy in enemies) {
			float distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
			if (distance <= range) {
				enemy.GetComponent<LivingEntity>().Hurt();
			}
		}
	}
	public class Level{
		public string levelName;
		public int year;
		public string[] objects;
		public Vector2 start;
		public Vector2[] locations;
		public Level(string levelName, Vector2 start, string[] objects, Vector2[] locations){
			this.levelName = levelName;
			this.objects = objects;
			this.locations = locations;
			this.start = start;
		}
		public void load(){
			GameObject[] objects = GameObject.FindObjectsOfType<GameObject> ();
			foreach (GameObject o in objects) {
				if (!o.tag.Equals ("MainCamera") && !o.tag.Equals ("Player") && !o.tag.Equals("Shadow") && o.layer != 5) {
					o.SetActive (false);
					Destroy (o);
				}
			}
			GameObject _prefab = Resources.Load <GameObject> ("Tiles/" + levelName);

			GameObject gridBgPrefab = (GameObject)Instantiate (_prefab, new Vector3(0f,0f,0f), Quaternion.identity);
			GameObject.Find ("Character").transform.position = start;
			int index = 0;
			if (GameObject.Find ("Character").GetComponent<PlayerScript> ().shadows [0].GetComponent<SpriteAnim> ().loops > 0)
				index = 1;
			foreach (GameObject shadow in GameObject.Find ("Character").GetComponent<PlayerScript> ().shadows) {
				shadow.GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("Character").GetComponent<PlayerScript> ().shadows [index].GetComponent<SpriteRenderer> ().sprite;

			}
			GameObject.Find ("Character").GetComponent<PlayerScript> ().trail.Clear ();
			for (int i = 0; i < this.objects.Length; i++){
				GameObject obj = Resources.Load <GameObject> (this.objects[i]);
				Instantiate (obj);
				obj.transform.position = locations[i];
			}

		}
	}
}
