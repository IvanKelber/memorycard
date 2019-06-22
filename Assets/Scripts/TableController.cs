using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{

    [SerializeField] private int grid_rows = 2;
    [SerializeField] private int grid_cols = 4;

    [SerializeField] private Sprite[] cardImages;
    [SerializeField] private MemoryCard cardPrefab;
    [SerializeField] private GameObject startButton;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private float offsetX = 2f;
    [SerializeField] private float offsetY = 2.5f;
    [SerializeField] private TextMesh label;

    private List<int> card_ids = new List<int>();
    private List<MemoryCard> cards = new List<MemoryCard>();

    private MemoryCard revealedCard = null;
    private bool _canReveal;
    private int _falseMatches;
    private int _score;
    // Start is called before the first frame update
    void Start()
    {
        // Error handling :)
        if(grid_rows * grid_cols % 2 == 1) {
            print("ERROR:  Make sure there are an even number of cards in the grid!  This is a matching game after all...");
            System.Environment.Exit(0);
        }
        activateStartButton(true);
    }

    public void RevealCard(MemoryCard card) {
        if(revealedCard == null) {
            revealedCard = card;
        } else {
            _canReveal = false;
            StartCoroutine(CheckMatch(card));
        }
    } 

    private IEnumerator CheckMatch(MemoryCard card) {
        if(card.GetID() != revealedCard.GetID()) {
            _falseMatches++;
            updateScoreLabel();
            yield return new WaitForSeconds(.5f);
            card.Unreveal();
            revealedCard.Unreveal();

        } else {
            _score++;
            if(_score == cardImages.Length) {
                activateStartButton(true);
            }
        }
        revealedCard = null;
        _canReveal = true;
    }

    public bool CanReveal() {
        return _canReveal;
    }

    private void updateScoreLabel() {
        label.text = "Incorrect Guesses:  " + _falseMatches;
    }

    private void activateStartButton(bool b) {
        this.startButton.SetActive(b);
    }

    public void StartGame() {
        init();
    }

    private void initializeCards() {
        // Randomize card ids
        card_ids.Clear();
        for(int i = 0; i < cardImages.Length; i++) {
            card_ids.Add(i);
            card_ids.Add(i);
        } 
        Utility.Shuffle(card_ids);

        //Destroy any existing cards
        for(int i = 0; i < cards.Count; i++) {
            cards[i].Destroy();
            print("Destroying card i: " + i + " with id " + cards[i].GetID());
        }
        cards.Clear();

        // Position Cards
        for(int i = 0; i < grid_cols; i++) {
            for(int j = 0; j < grid_rows; j++) {
                MemoryCard card = Instantiate(cardPrefab) as MemoryCard;
                card.SetController(this);
                int id = Utility.Pop(card_ids, card_ids.Count - 1);
                card.SetCard(id, cardImages[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = (-offsetY * j) - startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
                cards.Add(card);
            }
        }
    }

    private void init() {
        _score = 0;
        _falseMatches = 0;
        _canReveal = true;
        updateScoreLabel();
        activateStartButton(false);
        initializeCards();
    }
}
