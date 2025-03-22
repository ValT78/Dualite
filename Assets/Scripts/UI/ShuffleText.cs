using System.Linq;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]

public class ShuffleText : MonoBehaviour
{
    private bool[] textMask;
    [SerializeField] TextMeshProUGUI text;
    private string charList = "={#{)?+@}$#;;=/:}([%}.?#.)@[,&#!}{&}-,:=_:@#:#!.&%[$*,=%:,?.#{&:,*)%}*(%}{[/{+=!=)_[!.{.:/*];._{+@*[)&-]{]#&$!;.&[/_@&/(%:&;;@%+!$}$}{(,{@@%]_,=*_)&!+}$$$@##,@.=}=;!.*&[=!={*$+*!!,!&.-[@.%?-(=@?-/*(+/?.-??,*}?{;";

    [SerializeField] float delay;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMask = new bool[text.text.Length];
        for (int i = 0; i < textMask.Length; i++) textMask[i] = (Random.value <= GameManager.Instance.anger && text.text[i] != ' ');
        foreach(bool b in textMask) Debug.Log(b);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            text.text = shuffledText(text.text);
            timer = 0f;
        }
    }

    private string shuffledText(string text)
    {
        string retour = "";
        for(int i = 0; i < textMask.Length; i++)
        {
            if (textMask[i] == false || Random.Range(0f, 1f) > 0.8) retour += text[i];
            else retour += GetRandomChar();
        }

        return retour;
    }

    private char GetRandomChar()
    {
        return charList[Random.Range(0, charList.Length)];
    }
}
