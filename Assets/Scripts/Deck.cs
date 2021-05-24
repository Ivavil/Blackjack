using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        string[] list_name = new string[52];
        int name_card = 0;
        for(int i = 0; i<= values.Length - 1; i++)
        {
            list_name[i] = faces[i].name;
            
            int startPosition = list_name[i].IndexOf("_") + 1;
           
            name_card = int.Parse(list_name[i].Substring(startPosition));
            if(name_card >= 0 && name_card <= 12)
            {
                values[i] = name_card + 1;
            } else if (name_card >= 13 && name_card <= 25)
            {
                values[i] = name_card - 13 + 1;
            } else if (name_card >= 26 && name_card <= 38)
            {
                values[i] = name_card - 26 + 1;
            } else if(name_card >= 39 && name_card <= 51)
            {
                values[i] = name_card - 39 + 1;
            } if(name_card == 10 || name_card == 11 || name_card == 12 || name_card == 23 || name_card == 24 || name_card == 25 || name_card == 36 || name_card == 37 || name_card == 38 || name_card == 49 || name_card == 50 || name_card == 51)
            {
                values[i] = 10;
            } if(name_card == 0 || name_card == 13 || name_card == 26 || name_card == 39)
            {
                values[i] = 11;
            }

            //Debug.Log(values[i]);
        }
    }

    private void ShuffleCards()
    {
         string[] list_name = new string[52];
        int name_card = 0;
        int aux = 0;
        Sprite aux_spr;
        for(int i = 0; i <= 51; i++)
        {
            int random = Random.Range(0, 51);
            aux = values[i];
            aux_spr = faces[i];
            values[i] = values[random];
            faces[i] = faces[random];
            values[random] = aux;
            faces[random] = aux_spr;

        }     
    }

    void StartGame()
    {
        finalMessage.text = "";
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();

            
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }

        if (values[0] + values[2] == 21)
        {
            
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "Player wins";
            

        }
        if (values[1] + values[3] == 21)
        {
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "Dealer wins";
            

        }
    }

    private void CalculateProbabilities()
    {
         int b_card = 0;
        int c_card = 0;
        int d_card = 0;

        for (int i = 4; i <= 51; i++){
            if(values[i] > (values[0] + values[2] - values[3]))
            {
                b_card++;
            }
        }

        for (int i = 4; i <= 51; i++)
        {
            if ((player.GetComponent<CardHand>().points + values[i]) >= 17 && (player.GetComponent<CardHand>().points + values[i]) <= 21)
            {
                c_card++;
            }
        }

        for (int i = 4; i <= 51; i++)
        {
            if ((player.GetComponent<CardHand>().points + values[i]) > 21)
            {
                d_card++;
            }
        }

        double probab = (float)b_card / 48;
        double probab2 = (float)c_card / 48;
        double probab3 = (float)d_card / 48;

        int probab_1 = (int)(probab * 100);
        int probab_2 = (int)(probab2 * 100);
        int probab_3 = (int)(probab3 * 100);

        probMessage.text = probab_1.ToString() + "%" + " - " + probab_2.ToString() + "%"+ " - " + probab_3.ToString() + "%";
        
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
