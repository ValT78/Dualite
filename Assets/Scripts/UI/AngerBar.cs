using UnityEngine;
using UnityEngine.UI;

public class AngerBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Material blood;
    [SerializeField] Material speed;

    [SerializeField, Range(0f, 1f)] float bloodThreashold;
    [SerializeField, Range(0f, 1f)] float SpeedThreashold;

    // Update is called once per frame
    void Update()
    {
        slider.value = GameManager.Instance.anger;
        if (GameManager.Instance.anger > bloodThreashold) blood.SetFloat("_Alpha", map(GameManager.Instance.anger, bloodThreashold, 1f, 0f, 1f));
        else blood.SetFloat("_Alpha", 0f);
        if (GameManager.Instance.anger > SpeedThreashold) speed.SetFloat("_Alpha", map(GameManager.Instance.anger, SpeedThreashold, 1f, 0f, 1f));
        else speed.SetFloat("_Alpha",0f);
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}
