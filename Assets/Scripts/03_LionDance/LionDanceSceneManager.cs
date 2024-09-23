using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class LionDanceSceneManager : DefaultSceneManager
{
    public Material fogMat; // �Ȱ� ���׸���
    public LionDanceDirector lionDanceDirector; // �� ���� �ƾ��� ����ִ� ��ũ��Ʈ
    public PlayableDirector openingDirector;



    void Start()
    {
        WhenStart();

        //GameManager.instance.urpRenderer.rendererFeatures[0].SetActive(false); // SSAO ����. �ƽ� ī�޶� �������Ұ� ������� �̻��ϰ� ������ ������ �ƽ� ���� ��Ȱ��ȭ
        fogMat.color = new Color(1f, 1f, 1f, (255f / 255f)); // ���� �ʱ�ȭ

        // ������ ����
        openingDirector.Play();
    }

    void Update()
    {
        WhenUpdate();
    }




    // ��� ����� �ؽ�Ʈ ���ΰ�ħ
    public override void ReloadText()
    {
        uiTexts[0].text = GameManager.instance.textFileManager.ui[4];
        uiTexts[1].text = GameManager.instance.textFileManager.ui[5];
        uiTexts[2].text = GameManager.instance.textFileManager.ui[6];
        uiTexts[3].text = GameManager.instance.textFileManager.ui[7];
        uiTexts[4].text = GameManager.instance.textFileManager.ui[8];
        uiTexts[5].text = GameManager.instance.textFileManager.ui[9];
        uiTexts[6].text = GameManager.instance.textFileManager.ui[10];
        uiTexts[7].text = GameManager.instance.textFileManager.ui[11];
        uiTexts[8].text = GameManager.instance.textFileManager.ui[12];
        uiTexts[9].text = GameManager.instance.textFileManager.ui[13];
        uiTexts[10].text = GameManager.instance.textFileManager.ui[18];
        uiTexts[11].text = GameManager.instance.textFileManager.ui[19];
        uiTexts[12].text = GameManager.instance.textFileManager.ui[20];
        uiTexts[13].text = GameManager.instance.textFileManager.ui[23];
        uiTexts[14].text = GameManager.instance.textFileManager.ui[25];
    }




    // ���� ������� �ڷ�ƾ
    public void FogOut()
    {
        StartCoroutine(FogOutCoroutine());
    }

    IEnumerator FogOutCoroutine()
    {
        while (fogMat.color.a > 0f)
        {
            fogMat.color = new Color(1f, 1f, 1f, fogMat.color.a - ((1f / 10f) * Time.deltaTime));
            yield return null;
        }
    }
}
