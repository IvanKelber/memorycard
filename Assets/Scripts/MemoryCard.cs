using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{


    private TableController controller;
    [SerializeField] private GameObject cardBack;
    [SerializeField] private GameObject cardFront;

    private int _id;
    
    void OnMouseDown() {
        if(this.cardBack.activeSelf && controller.CanReveal()) {
            this.cardBack.SetActive(false);
            controller.RevealCard(this);
        }
    }

    public int GetID() {
        return _id;
    }

    public void SetCard(int id, Sprite cardImage) {
        this._id = id;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = cardImage;
        // renderer.size = new Vector2(renderer.size.x * 0.5f, renderer.size.y * 0.3f);
    }

    public void Unreveal() {
        this.cardBack.SetActive(true);
    }

    public void Destroy() {
        Destroy(cardBack);
        Destroy(cardFront);
    }

    public void SetController(TableController controller) {
        this.controller = controller;
    }

}
