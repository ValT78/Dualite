using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public float speed;
    public GameObject npcObject;
    private bool facingRight = true; 

    void Start()
    {
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
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; 
        transform.localScale = scale;
    }

    // déclencher le dialogue en appelant DialogueManager
    public void TriggerDialogue() 
    {
        DialogueManager.instance.StartDialogue(dialogue);
    } 
    private IEnumerator IsComing() 
    {
        float endPosition = 3f; // position cible

        // déplacement du NPC jusqu'à la position cible
        while(npcObject.transform.position.x < endPosition) {
            npcObject.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            yield return null;
        }

        // quand le NPC atteint la position cible
        TriggerDialogue(); // lancer le dialogue
        Flip(); // changer l'orientation du NPC, faire demi-tour

        if (ncpObject.position = 0)
        {
            Destroy(gameObject);
            // lancer le prochain NPC
        } 
    
    } 
    
}


