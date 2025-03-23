using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueScripter    : MonoBehaviour
{
    public static DialogueScripter Instance;

    public Dialogue dialogue;
    public float speed;
    public GameObject npcObject;
    public List<Sprite> allSprites;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Story story; 

    [SerializeField] TextMeshProUGUI proposion1;
    [SerializeField] TextMeshProUGUI proposion2;
    [SerializeField] TextMeshProUGUI proposion3;


    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        Instance = this;
    }

    void Start()
    {
        ResetSprite();
    }

    private string GetNameFromNode(StoryNode node)
    {
        if (node.HasTag("boss")) return "Boss";
        if (node.HasTag("maman")) return "Maman";
        if (node.HasTag("michel")) return "Michel";
        return "No Name";
    }

    // Création d'une liste avec les 3 propositions (good,bad,awful) 
    // dans laquelle on va piocher aléatoirement
    System.Random random = new System.Random();

    

    private List<int> choix = new List<int> { 1, 2, 3 };
    public List<int> shuffledList; 
    public void Shuffle()
    {
        // fct qui mélange une liste
        shuffledList = choix.OrderBy(x => random.Next()).ToList();
    }

    private void SetOptions(StoryNode node)
    {
        List<NextNode> nexts = node.GetNextNodes();
        // 
        Debug.Log(shuffledList.Count +" : " + shuffledList[0] + " " + shuffledList[1] + " " + shuffledList[2]);
        proposion1.text = nexts[shuffledList[0] - 1].display;
        proposion2.text = nexts[shuffledList[1] - 1].display;
        proposion3.text = nexts[shuffledList[2] - 1].display; 
    }

    // changer l'orientation du personnage
    public void Flip() 
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    // déclencher le dialogue en appelant DialogueManager
    public void TriggerDialogue() 
    {
        DialogueManager.instance.StartDialogue(dialogue, shuffledList);
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
        story.SetNextNode("boss01");
        StoryNode node = story.GetCurrentNode();
        string name = GetNameFromNode(node);
        dialogue = new Dialogue(name, story);
        Shuffle();
        SetOptions(node);
        DialogueManager.instance.panel.SetActive(false);

        npcObject.transform.localPosition = new Vector3 (0f, 0f, 0f); 
        spriteRenderer.flipX = false;
        // Reset dialogue 
        ChangeSprite();
        StartCoroutine(IsComing());

    }

}


