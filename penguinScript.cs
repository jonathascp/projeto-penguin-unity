using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class penguinScript : MonoBehaviour {

	[Header("Componentes do Personagem")]

	private Animator animator;
	private Rigidbody2D rgbody;
	public Transform groundCheck;
	public bool grounded;
	public Transform colisor,colisor2;
	private Vector3 direcaoUp = Vector3.up;
	private Vector3 direcaoDown = Vector3.down;
	public LayerMask layerMask;

	[Header("Configuração do Personagem")]

	public float forcajump;
	public bool running;


	[Header("Controlador do Game")]

	public int item;
	public bool die;
	public GameObject platform,platform2;
	public gameController gameController;
	private AudioSource audioPlayer;

	// Use this for initialization
	void Start () {

		die = false;
		animator = GetComponent<Animator> (); // Animador do Personagem
		rgbody = GetComponent<Rigidbody2D> (); // Corpo Rigido do Personagem
		gameController = FindObjectOfType(typeof(gameController)) as gameController;
		audioPlayer = GetComponent<AudioSource>();

	}	
	
	// Update is called once per frame
	void Update () {


		grounded = Physics2D.OverlapCircle (groundCheck.position, 0.02f, layerMask);

		RaycastHit2D hit = Physics2D.Raycast(colisor.position,direcaoUp,0.1f); // CRIA UM RAYCAST PARA CIMA , PARA DETECTAR A PLATAFORMA 
		Debug.DrawRay(colisor.position,direcaoUp * 0.1f,Color.red);

		if(hit == true && hit.transform.gameObject.tag == "Plataforma")    //	 DESATIVA O COLISOR AO DETECTAR 
		{																					
			platform = hit.transform.gameObject;			
			platform.GetComponent<BoxCollider2D>().enabled = false;
			StartCoroutine("ativarColisor");								// LIGA O DETECTOR DEPOIS DE UM TEMPO
		}
			

		/*ANIMADOR */
		animator.SetBool ("grounded", grounded);
		animator.SetBool ("jump", true);

		if(Input.GetKeyDown(KeyCode.UpArrow) && grounded == true && running == true)
		{
			rgbody.AddForce(Vector2.up * forcajump);
			audioPlayer.Play();
		}


		RaycastHit2D hit2 = Physics2D.Raycast(colisor2.position,direcaoDown,0.1f); // CRIA OUTRO RAYCAST PARA BAIXO
		Debug.DrawRay(colisor2.position,direcaoDown * 0.1f,Color.red);

		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			if(hit2 == true && hit2.transform.gameObject.tag == "Plataforma" && grounded == true) // SE O RAYCAST2 ESTIVER DETECTANDO COLISAO E ESTIVER NO CHAO
			{

				platform2 = hit2.transform.gameObject;
				platform2.GetComponent<BoxCollider2D>().enabled = false;
				StartCoroutine("ativarColisor2");
			}
		}


	} //

	/*COLISOR COM OS OBSTACULOS*/
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.tag == "obstaculo" )  // DETECTA COLISAO PARA A MORTE 
		{
			die = true;
			running = false;
			animator.SetBool("die",true);
			forcajump = 0;

		}
	}


	/*
	public void Jump()       // FUNÇÃO DE PULAR
	{
		if (grounded == true)  
		{
			rgbody.AddForce (new Vector2(0, forcajump));
		}
	}
*//*
	public void Down()     // FUNÇÃO DE DESCER
	{
		RaycastHit2D hit2 = Physics2D.Raycast(colisor2.position,direcaoDown,0.1f); // CRIA OUTRO RAYCAST PARA BAIXO
		Debug.DrawRay(colisor2.position,direcaoDown * 0.1f,Color.red);

		if(hit2 == true && hit2.transform.gameObject.tag == "Plataforma" && grounded == true) // SE O RAYCAST2 ESTIVER DETECTANDO COLISAO E ESTIVER NO CHAO
		{

				platform2 = hit2.transform.gameObject;
				platform2.GetComponent<BoxCollider2D>().enabled = false;
				StartCoroutine("ativarColisor2");
		}
	}
	*/
	void OnTriggerEnter2D(Collider2D col)       // COLETAVEL
	{
		Destroy (col.gameObject);
		item += 1;
	}

	IEnumerator ativarColisor()
	{
		yield return new WaitForSeconds(0.5f);
		platform.GetComponent<BoxCollider2D>().enabled = true;

	}

	IEnumerator ativarColisor2()
	{
		yield return new WaitForSeconds(0.5f);
		platform2.GetComponent<BoxCollider2D>().enabled = true;

	}
		
}
