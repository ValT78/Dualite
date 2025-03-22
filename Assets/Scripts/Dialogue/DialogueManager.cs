using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private Queue<string> sentences; // file d'attente pour stocker les phrases du dialogue
    public static DialogueManager instance; // instance statique pour accéder au DialogueManager depuis d'autres scripts

    // afficher un avertissement si il y a déjà une instance de DialogueManager sur la scène
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("il n'y a plus de DialogueManager dans la scène");
            return;
        }
        instance = this; 
        sentences = new Queue<string>();
    }

    // méthode qui démarre un dialogue
    public void StartDialogue(Dialogue dialogue) 
    {
         panel.SetActive(true);
         nameText.text = dialogue.name; // insérer le nom dans la case nom
         sentences.Clear(); // effacer les anciennes phrases de la liste d'attente
         //foreach (string sentence in dialogue.sentences) 
         //{
            sentences.Enqueue(dialogue.sentences); // Enqueue envoie des elt dans la file d'attente
         //}
         DisplayNextSentence();
    }

    void DisplayNextSentence() 
    {
        if(sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue(); // Dequeue récupère le prochaine elt de la liste d'attente
        dialogueText.text = sentence;
    }

    void EndDialogue() 
    {
        Debug.Log("Fin du dialogue"); 
        panel.SetActive(false);
        StartCoroutine(DialogueScripter.Instance.IsReturning());
    }

    public void LeftDialogAction(InputAction.CallbackContext context)
    {
        DisplayNextSentence();
    }

    // Afficher et choisir les 3 propositions (boutons QSD)
    public void RightDialogAction(InputAction.CallbackContext context)
    {
        DisplayNextSentence();
    }
    public void MiddleDialogAction(InputAction.CallbackContext context)
    {
        DisplayNextSentence(); 
    }


}