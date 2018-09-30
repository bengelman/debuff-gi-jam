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
	public float baseSpeed = 0.5F;
	private bool stunned = false;
	float speedMod = 0.75F;
	int numShadows = 30;
	public Image[] hearts; 
	ArrayList trail = new ArrayList();
	public bool breakHourglass = false;
	public bool attacking = false;
	bool rewinding = false;
	public bool lockOnShadow = false;
	public Sprite fullHeart, halfHeart, noHeart;
	public bool noTrail = false;
	public int invulnerability = 0;
	/* *
	 * String: tilemap name (in Tiles/)
	 * Vector2: position where player spawns
	 * String[]: prefabs to be loaded (includes enemies, environmental objects, etc.)
	 * Vector2[]: positions of prefabs to be loaded
	 * */
	protected Level[] levels = new Level[]{



		//new Level("Oasis", new Vector2(0,0), new string[]{"Prefabs/laser"}, new Vector2[]{new Vector2(0f, 0f), new Vector2(0f, 0f)}, new Vector2[]{new Vector2(-2f, 0f), new Vector2(2f, 0f)}) , // test triangle spawning

		
		new Level("Oasis", new Vector2(-4, 1),
		new string[]{"Prefabs/gem_prefab 1", "Prefabs/jellyfish_prefab"}, // "Prefabs/wurm_prefab"},
		new Vector2[]{new Vector2(-1.4F, 4.3F), new Vector2(8.5F, 0.37F)}), // new Vector2(-3.0F, 0.5F)}),
			
		new Level("Level2", new Vector2(1.3F, -3.2F),
		new string[]{"Prefabs/hourglass", "Prefabs/hourglass", "Prefabs/wurm"},
			new Vector2[]{new Vector2(11F, 1F), new Vector2(-8F, 1F), new Vector2(0, 0)}),

		new Level("Desert", new Vector2(-4, 2),
		new string[]{"Prefabs/jellyfish_prefab"},
		new Vector2[]{new Vector2(14, 21)}),

		new Level("Oasis", new Vector2(0,-4), new string[]{"triangle pair"}, new Vector2[]{}, new Vector2[]{new Vector2(-2f, 0f), new Vector2(2f, 0f)}) , // test triangle spawning

		new Level("BossFight", new Vector2(-2, -7),
			new string[]{"Prefabs/jellyfish_prefab"},
			new Vector2[]{new Vector2(14, 21)})

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
	public void Stun(float time){
		stunned = true;
		StartCoroutine ("endStun", time);
	}
	IEnumerator endStun(float time){
		yield return new WaitForSeconds (time);
		stunned = false;
		yield break;
	}
	void levelUpdate(){
		bool portal = true;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		if (GetComponent<SpriteAnim> ().loops > 0 || lockOnShadow || (noTrail))
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
		GameObject[] hourglasses = GameObject.FindGameObjectsWithTag ("Hourglass");
		foreach (GameObject enemy in hourglasses){
			if (enemy.GetComponent<SpriteAnim>().loops == 0 && !breakHourglass) {
				portal = false;
				break;
			}
		}
		if (portal) {
			if (!firstFlag) {
				
				firstFlag = true;
				noTrail = true;
				return;
			}
			if (firstFlag && !prePortalAnim) {
				GetComponent<SpriteAnim> ().PlayTemp (4, 1);
				prePortalAnim = true;
				return;
			}
			prePortalAnim = false;
			firstFlag = false;
			++level;
			if (level >= levels.Length) {
				SceneManager.LoadScene ("MainMenu");
				return;
			}
			levels [level].load ();
			stunned = false;
			breakHourglass = false;
			GetComponent<SpriteAnim> ().PlayTemp (2, 4);
		}
	}
	public bool firstFlag = false;
	void Update () {
		invulnerability -= 1;
		levelUpdate ();
		if (invulnerable > 0) {
			invulnerable = Mathf.Max (0, invulnerable - Time.deltaTime);
		}
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
		if (trail.Count < 1) {
			noTrail = false;
		}
		if (noTrail || firstFlag) {
			mouse.Normalize();
			mouse *= 0.7F;
		}
		GetComponent<SpriteRenderer> ().flipX = mouse.x > 0;
		float mag = mouse.magnitude;
		if (mouse.magnitude > 1.0f) {
			mouse.Normalize (); 
			mouse /= accelLimit;
			//mouse *= 1 + (mag / 100);
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
		if (level == levels.Length - 1)
			Debug.Log ("Moving");
		mouse *= ((baseSpeed) * speedMod);// * (1 + Mathf.Sqrt(momentum * 0.05F));

		GetComponent<Rigidbody2D> ().velocity = mouse;
		//transform.position = newVec;

		if (Input.GetButtonDown("Rewind")) {
			rewinding = true;
			//transform.position = ((TimeShadow)trail[trail.Count - 1]).pos;
			//trail.Clear ();
		}
		if (Input.GetMouseButtonDown (0) && !attacking && !lockOnShadow) {
			GetComponent<SpriteAnim> ().PlayTemp (1, 1);
			BasicAttack ();
		}
		if (Input.GetMouseButtonDown (1) && trail.Count > 290 && !attacking && !lockOnShadow) {
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

	void FixedUpdate () {
		
		if (rewinding)
			return;
		if (GetComponent<Rigidbody2D> ().velocity.magnitude < 0.5 || noTrail || firstFlag) {
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
	float invulnerable = 0;
	public void Hurt(){
		if (stunned || attacking || invulnerable > 0)
			return;
		GetComponent<LivingEntity> ().currentHealth--;
		GetComponent<SpriteAnim> ().PlayTemp (2, 1);
		invulnerable = 0.5f;
		if (GetComponent<LivingEntity> ().currentHealth <= 0) {
			GetComponent<SpriteAnim> ().PlayTemp (2, 1);
			if (GetComponent<LivingEntity> ().currentHealth <= 0) {
				GetComponent<SpriteAnim> ().PlayTemp (2, 1);
			}
		}
	}
	void ShadowAttack(){
		
		GameObject[] basicEnemies = GameObject.FindGameObjectsWithTag ("Enemy");

		List<GameObject> enemies = new List<GameObject> (GameObject.FindGameObjectsWithTag("Hourglass"));
		enemies.AddRange (basicEnemies);
		List<GameObject> hourglasses = new List<GameObject> ();

		foreach (GameObject enemy in enemies){
			for(int i = shadows.Length - 1; i >= 0; i--) {
				GameObject shadow = shadows [i];
				if (Mathf.Abs ((enemy.transform.position - shadow.transform.position).magnitude) < 1) {
					momentum = 0;
					shadow.GetComponent<SpriteAnim> ().PlayTemp (1, 1);
					if (enemy.tag.Equals ("Hourglass")) {
						enemy.GetComponent<SpriteAnim> ().PlayTemp (1, 1);
						hourglasses.Add (enemy);
					}
					else
						enemy.GetComponent<LivingEntity> ().Hurt();
					lockOnShadow = true;
					break;
				}
			}
		}

		if (hourglasses.Count >= GameObject.FindGameObjectsWithTag ("Hourglass").Length) {
			breakHourglass = true;
		}


	}

	// the left click attack of player
	// hurts enemies within a certain distance of player
	void BasicAttack() {
		StartCoroutine (attackAfterDelay(0.35F));
		attacking = true;
	}
	IEnumerator attackAfterDelay(float delay){
		yield return new WaitForSeconds (delay);
		float range = 3.0f;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach (GameObject enemy in enemies) {
			float distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
			if (distance <= range) {
				enemy.GetComponent<LivingEntity>().Hurt();
			}
		}
		attacking = false;
	}
	public class Level{
		public string levelName;
		public int year;
		public string[] objects;
		public Vector2 start;
		public Vector2[] locations;
		public Vector2[] triangle_locations;
		public Level(string levelName, Vector2 start, string[] objects, Vector2[] locations, Vector2[] triangle_locations= null ){
			this.levelName = levelName;
			this.objects = objects;
			this.locations = locations;
			this.start = start;
			this.triangle_locations = triangle_locations;
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
			Debug.Log(_prefab);
			GameObject gridBgPrefab = (GameObject)Instantiate (_prefab, new Vector3(0f,0f,0f), Quaternion.identity);
			GameObject.Find ("Character").transform.position = start;
			int index = 0;
			if (GameObject.Find ("Character").GetComponent<PlayerScript> ().shadows [0].GetComponent<SpriteAnim> ().loops > 0)
				index = 1;
			foreach (GameObject shadow in GameObject.Find ("Character").GetComponent<PlayerScript> ().shadows) {
				shadow.GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("Character").GetComponent<PlayerScript> ().shadows [index].GetComponent<SpriteRenderer> ().sprite;

			}
			GameObject.Find ("Character").GetComponent<PlayerScript> ().trail.Clear ();
			int triangle_head = 0;
			for (int i = 0; i < this.objects.Length; i++){
				if(this.objects[i] == "triangle pair"){
					//spawn the triangles
					GameObject obj = Resources.Load<GameObject>("Prefabs/triangle");
					GameObject obj2 = Resources.Load<GameObject>("Prefabs/triangle2");
					triangle a_triangle = Instantiate(obj, this.triangle_locations[triangle_head], new Quaternion(0,0,0,0)).gameObject.GetComponent<triangle>();
					triangle2 a_triangle2 = Instantiate(obj2,this.triangle_locations[triangle_head+1], new Quaternion(0,0,0,0)).gameObject.GetComponent<triangle2>();
					a_triangle.sibling = a_triangle2;
					a_triangle2.sibling = a_triangle;
					triangle_head+=2; // set the triangle head up
				}
				else{ // not instantiating a triangle
					GameObject obj = Resources.Load <GameObject> (this.objects[i]);
					Instantiate (obj);
					obj.transform.position = locations[i];
				}
			}

		}
	}
}
