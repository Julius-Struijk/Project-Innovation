using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroplet : MonoBehaviour
{
    //Input handler reference
    InputHandler inputHandler;
    //GameManager reference
    GameManager gameManager;
    //Speed at which the droplets fall
    [SerializeField]
    float dropletSpeed;
    //List of different droplet sprites
    [SerializeField]
    List<Sprite> dropletVariants = new List<Sprite>();

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        inputHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputHandler>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (inputHandler == null)
        {
            Debug.LogError("Game Manager/Input Handler not found");
        }

        spriteRenderer.sprite = dropletVariants[Random.Range(0, dropletVariants.Count)];
    }

    //If a droplet collides with the pet, it destroys itself
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pet")
        {
            gameManager.cleaningGameHitAmount++;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position -= new Vector3(0, dropletSpeed * Time.deltaTime, 0);

        //Destroy if it goes out of the screen
        if (transform.position.y <= inputHandler.ScreenMinimumY() - 1)
        {
            Destroy(gameObject);
        }
    }
}
