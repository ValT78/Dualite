using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScripter    : MonoBehaviour
{
    public static DialogueScripter Instance;

    public Dialogue dialogue = new Dialogue("Name", "hello world");
    public float speed;
    public GameObject npcPrefab; 
    public GameObject npcObject;
    private bool facingRight = true;
    public List<Sprite> allSprites;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        Instance = this;
    }

    void Start()
    {
        ChangeSprite();
        StartCoroutine(IsComing());

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    // changer l'orientation du personnage
    public void Flip() 
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    // déclencher le dialogue en appelant DialogueManager
    public void TriggerDialogue() 
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }

    public IEnumerator IsReturning()
    {
        Flip(); // changer l'orientation du NPC, faire demi-tour
        while (npcObject.transform.localPosition.x > 0)
        {
            npcObject.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            yield return null;
        }

        ResetSprite();


    }
    private IEnumerator IsComing() 
    {
        float endPosition = 6f; // position cible

        // déplacement du NPC jusqu'à la position cible
        while(npcObject.transform.localPosition.x < endPosition) {
            npcObject.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            yield return null;
        }

        // quand le NPC atteint la position cible
        TriggerDialogue(); // lancer le dialogue
         
    } 
    // lancer le prochain NPC 
    void ChangeSprite()
    {
        int randomIndex = Random.Range(0, allSprites.Count); // générer un index aléatoire
        spriteRenderer.sprite = allSprites[randomIndex];   // appliquer le sprite choisi
    }

    private void ResetSprite()
    {
        npcObject.transform.localPosition = new Vector3 (0f, 0f, 0f); 
        spriteRenderer.flipX = false;
        // Reset dialogue 
        ChangeSprite();
        StartCoroutine(IsComing());

    }

}


