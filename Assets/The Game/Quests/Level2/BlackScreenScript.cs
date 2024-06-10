using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenScript : MonoBehaviour
{
    private Image panel;
    [SerializeField] private float blackScreenSpeed;

    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<Image>();
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
            clr.a += 0.2f;
            panel.color = clr;
            yield return null;
        }
    }

    private IEnumerator RemoveBlackScreen()
    {
        while (panel.color.a > 0)
        {
            var clr = panel.color;
            clr.a -= 0.2f;
            panel.color = clr;
            yield return null;
        }
    }

}
