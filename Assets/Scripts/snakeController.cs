using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class snakeController : MonoBehaviour {
	//prefab dos elementso para se adicionar na calda
	public GameObject tailPrefab;
	//lista de objetos que é a calda
    private List<Transform> tail;
	//variavel para controlar se uma comida foi pega pela cabeca
    bool ate;
	//direcao incial da cobra
	private Vector2 dir = Vector2.right;
	//Ui text para exibir os bpontos
    private Text text;
    private bool gameOver;

    public Text PointsLabel
    {
        get { return text; }
        set { text = value; }
    }
    public bool GameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }

    // Use this for initialization
    void Start () {
        gameOver = false;
        tail = new List<Transform>();
        ate = false;
		//Registra o calbeck para a funcao de movimentacao da cobra 
        InvokeRepeating("move", 1, 0.2f);
        Instantiate(text);
        text.text = "Points: " + tail.Count;

    }    

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.name.StartsWith ("FoodPrefab")) {//se colidiu com uma comida destroi a comida e seta a variavel ate para true
            ate = true;
			Destroy (coll.gameObject);
		} else {//se colidiu com algo que não e uma comida game over
            gameOver = true;
        }
	}
	
	//Funcao serve somente para pegar as novas direcoes da cobra
	void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (verifyDirection(dir, Vector2.right)){
                dir = Vector2.right;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (verifyDirection(dir,- Vector2.up))
            {

                dir = -Vector2.up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (verifyDirection(dir, -Vector2.right))
            {

                dir = -Vector2.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (verifyDirection(dir, Vector2.up))
            {

                dir = Vector2.up;
            }
        }
        
       
    }
	//verifica se a cobra pode voltar (caso tenha ao menos um elemento na cauda ela não pode mais inverter a direcao atual)
    private bool verifyDirection(Vector2 direction, Vector2 newDirection)
    {
        if ((tail.Count > 0)&& newDirection == -direction)
        {
            return false;
        }
        return true;
    }
   

    public void move()
    {
        if (!gameOver)
        { 	
            Vector2 v = transform.position;
            transform.Translate(dir);
            if (ate)//instancia um novo elemento na cauda
            {
                GameObject g;
                g = Instantiate(tailPrefab, v, Quaternion.identity);
                tail.Insert(0, g.transform);
                text.text = "Points: " + tail.Count;
                ate = false;
            }
          
			//acerta a posicao dos elementos da cauda
            if (tail.Count > 0)
            {
                Vector2 v2 = tail[0].position;
                tail[0].position = v;
                for (int i = 1; i < tail.Count; i++)
                {
                    Vector2 aux = tail[i].position;
                    tail[i].position = v2;
                    v2 = aux;
                }
            }
        }
    }
}
