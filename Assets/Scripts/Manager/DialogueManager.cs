using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public Text nameText;
    public Text dialogueText;
    public string lastSentence;
    public Animator animator;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue){
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            lastSentence = sentence;
        }

        DisplayNextSentence();
    }

    public int DisplayNextSentence(){
        if(sentences.Count == 0){
            EndDialogue();
            return 1;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        return 0;
    }

    IEnumerator TypeSentence (string sentence){
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void DisplayLastSentence(){
        animator.SetBool("isOpen", true);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(lastSentence));
    }

    public void EndDialogue(){
        animator.SetBool("isOpen", false);
    }
    
}
