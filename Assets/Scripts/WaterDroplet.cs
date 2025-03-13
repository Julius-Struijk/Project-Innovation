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

    //Audio
    public AudioSource audioSource;
    public AudioClip[] audioClipsHit;
    public AudioClip[] audioClipsMiss;
    [Range(0f, 1f)]
    public float clipVolume = 1.0f;

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

            //Play Audio
            //choose random clip
            //AudioClip randomClip = audioClipsHit[Random.Range(0, audioClipsHit.Length)];

            // Play sound through AudioManager
            AudioManager.Instance.PlaySound(audioClipsHit[Random.Range(0, audioClipsHit.Length)], clipVolume);

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position -= new Vector3(0, dropletSpeed * Time.deltaTime, 0);

        //Destroy if it goes out of the screen
        if (transform.position.y <= inputHandler.ScreenMinimumY() - 1)
        {
            //Play Audio
            //choose random clip
            //AudioClip randomClip = audioClipsMiss[Random.Range(0, audioClipsMiss.Length)];

            // Play sound through AudioManager
            AudioManager.Instance.PlaySound(audioClipsMiss[Random.Range(0, audioClipsMiss.Length)], clipVolume);
            //Debug.Log(randomClip);

            Destroy(gameObject);
        }
    }
}
