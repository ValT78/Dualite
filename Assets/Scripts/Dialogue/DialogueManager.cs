using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
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
    public void StartDialogue(Dialogue dialogue) {
         nameText.text = dialogue.name; // insérer le nom dans la case nom
         sentences.Clear(); // effacer les anciennes phrases de la liste d'attente
         foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence); // Enqueue envoie des elt dans la file d'attente
         }
         DisplayNextSentence();
    }

    void DisplayNextSentence() {
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue(); // Dequeue récupère le prochaine elt de la liste d'attente
        dialogueText.text = sentence;
    }

    void EndDialogue() {
        Debug.Log("Fin du dialogue");
    }
}