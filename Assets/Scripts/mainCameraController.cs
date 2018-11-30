using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class mainCameraController : MonoBehaviour {
	//tempo acumulado para controlar o tempo de criar uma nova comida
	private float timeSpaw=0f;
    //tempo para criar uma nova comida
    public float spawFoodInterval = 3;

	//prefab da comida
    public GameObject foodPrefab;
	//prefab de uma parede no mapa
    public GameObject cellPrefab;
    public GameObject headPrefab;

	//limites do cenario
    public Transform borderTop;
	public Transform borderBottom;
	public Transform borderLeft;
	public Transform borderRight;

	//UI Text para exibir os pontos
    public Text pointsLabel;
	//A cabeca da cobra
    private GameObject head;
	//A matriz que compoe o cenario com celulas vazias e ocupadas por paredes
    private int[,] mat;

    // Use this for initialization
    void Start () {
		//pega o nome do para carregar
        TextAsset mapConfig = Resources.Load("mapConfig") as TextAsset;
        

        string text = mapConfig.text;
        string nameMap = text.Split('.')[0];
		
		//Abre o arquivo do mapa (necessariamente o mapa tem que ter 40 celulas na horizontal por 20 celulas na vertical)
		//O é uma celula vazia e 1 é uma celula onde tem parede
        TextAsset map = Resources.Load(nameMap) as TextAsset;
         mat = new int[40,20];
        
		//carrega o mapa na matriz
        string textMapa = map.text;
        string[] lines = textMapa.Split('\n');
        for (int i = 0; i < 20; i++)
        {
           
            string[] col = lines[i].Split(',');
            for (int j = 0; j < 40; j++)
            {
                string value = col[j];
                if (value == "1")
                {
                    Instantiate(cellPrefab).transform.Translate(new Vector2(j, 19-i));
                }
                mat[j,19-i]= int.Parse(value);
            }
        }
		//instancia a cabeca da cobra no mapa
        head = Instantiate(headPrefab);
        head.GetComponent<snakeController>().PointsLabel = pointsLabel;
        head.transform.Translate(new Vector2(20, 10));
    }
	//Funcao que verifica se a posicao aonde vai spawnar a comida não é uma parede
    public bool isOccuped(int x, int y)
    {
        if ((x > 39 || x < 0) || (y > 19 || y < 0))
        {
            return true;
        }
        else if (mat[x, y] == 0)
        {
            return false;
        }
        return false;
    }
    private void Update()
    { 	//verifica se não ocorreu game over com a cobra
        if (!head.GetComponent<snakeController>().GameOver)
        {   //Acumula o tempo de spawn para a nova comida
            timeSpaw += Time.deltaTime;
            if (timeSpaw / (spawFoodInterval) > 1)//se o tempo de spawn acumulado for maior que o tempo de spawn então cria uma nova comida
            {
                timeSpaw = 0;
                bool ocuped = true;
                int x = 0, y = 0;
                while (ocuped)//enquanto a posição da comida for uma parede sorteia outra posicao
                {
                    x = (int)Random.Range(borderLeft.position.x, borderRight.position.x);
                    y = (int)Random.Range(borderTop.position.y, borderBottom.position.y);
                    ocuped = isOccuped(x, y);
                }
				//instancia a comida e coloca na posição sorteada
                Instantiate(foodPrefab).transform.Translate(new Vector2(x, y));
            }
        }
       
    }
}
