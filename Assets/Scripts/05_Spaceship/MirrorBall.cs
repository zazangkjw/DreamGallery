using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBall : MonoBehaviour
{
    public bool isOn = true;
    public Light mirrorBallLight;
    public Material mirrorBallmaterial;

    int r, g, b;
    Color newColor;
    WaitForSeconds delay = new WaitForSeconds(5f);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ColorChangeCoroutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOn)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 0.5f, transform.localEulerAngles.z + 0.25f);

            newColor = Color.Lerp(mirrorBallLight.color, new Color(r / 255f, g / 255f, b / 255f), 0.01f);
            mirrorBallLight.color = newColor;
            mirrorBallmaterial.SetColor("_EmissionColor", newColor * Mathf.Pow(2, 4));
        }
        else
        {
            newColor = new Color(100f / 255f, 100f / 255f, 100f / 255f);
            mirrorBallLight.color = newColor;
            mirrorBallmaterial.SetColor("_EmissionColor", newColor * Mathf.Pow(2, 4));
        }
    }

    IEnumerator ColorChangeCoroutine()
    {
        while (true)
        {
            if (isOn)
            {
                r = Random.Range(0, 192);
                g = Random.Range(0, 192);
                b = Random.Range(0, 192);
            }

            yield return delay;
        }
    }
}
