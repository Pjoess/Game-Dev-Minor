using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenScript : MonoBehaviour
{
    private RawImage panel;
    [SerializeField] private float blackScreenSpeed;

    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<RawImage>();
    }

    public void EnableBlackScreen()
    {
        StartCoroutine(DoBlackScreen());
    }

    public void DisableBlackScreen()
    {
        StartCoroutine(RemoveBlackScreen());
    }

    private IEnumerator DoBlackScreen()
    {
        while(panel.color.a < 1)
        {
            var clr = panel.color;
            clr.a += blackScreenSpeed;
            panel.color = clr;
            yield return null;
        }
    }

    private IEnumerator RemoveBlackScreen()
    {
        while (panel.color.a > 0)
        {
            var clr = panel.color;
            clr.a -= blackScreenSpeed;
            panel.color = clr;
            yield return null;
        }
    }

}
