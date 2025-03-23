using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;
using System.Threading;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private Story story;
    public static DialogueManager instance; // instance statique pour accéder au DialogueManager depuis d'autres scripts
    private float timer;
    [SerializeField] private float timerMax;
    [SerializeField] private Material material;
    private Coroutine coroutineTimer;
    List<int> indexs;

    // afficher un avertissement si il y a déjà une instance de DialogueManager sur la scène
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("il n'y a plus de DialogueManager dans la scène");
            return;
        }
        instance = this;
    }

    // méthode qui démarre un dialogue
    public void StartDialogue(Dialogue dialogue, List<int> shuffledList) 
    {
         panel.SetActive(true);
         indexs = shuffledList;
         nameText.text = dialogue.name;
         story = dialogue.story; 
         DisplayNextSentence();
         coroutineTimer = StartCoroutine(startTimer());

    }

    private IEnumerator startTimer()
    {
        timer = 0;
        while (timer < timerMax) 
        {
            timer += Time.deltaTime;
            material.SetFloat("_t",timer/timerMax);
            yield return new WaitForFixedUpdate();
        }
        coroutineTimer = null;
        GameManager.Instance.anger += 0.17f;
        EndDialogue();
    }

    void DisplayNextSentence()
    {
        if(story.GetCurrentNode() == null) return;
        dialogueText.text = story.GetCurrentNode().getText(); 
        if (story.GetCurrentNode().GetNextNodes().Count == 0) 
        {
            EndDialogue();
            return;
        }
    }

    void EndDialogue() 
    {
        Debug.Log("Fin du dialogue"); 
        if(coroutineTimer != null) StopCoroutine(coroutineTimer);
        StartCoroutine(DialogueScripter.Instance.IsReturning());
    }


    public string GetChoiceSuffix(int index)
    {
        switch (index) {
            case 1: return "-good";
            case 2:
                GameManager.Instance.anger += 0.08f;
                return "-bad";
            case 3:
                GameManager.Instance.anger += 0.17f;
                return "-awful";
        }
        return "";
    }

    // Afficher et choisir les 3 propositions (boutons QSD)
    public void LeftDialogAction(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled) return;
        story.SetNextNode(story.GetCurrentNode().GetTitle()+ GetChoiceSuffix(indexs[0]));
        SoundManager.PlaySound(SoundType.Button, 0.4f);
        DisplayNextSentence();
    }

    public void RightDialogAction(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled) return;
        story.SetNextNode(story.GetCurrentNode().GetTitle() + GetChoiceSuffix(indexs[1]));
        SoundManager.PlaySound(SoundType.Button, 0.4f);
        DisplayNextSentence();
    }
    public void MiddleDialogAction(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled) return;
        story.SetNextNode(story.GetCurrentNode().GetTitle()+ GetChoiceSuffix(indexs[2]));
        SoundManager.PlaySound(SoundType.Button, 0.4f);
        DisplayNextSentence(); 
    }


}