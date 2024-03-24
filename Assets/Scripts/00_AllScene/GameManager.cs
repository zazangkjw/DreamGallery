using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // �̱��� //
    // instance��� ������ static���� ������ �Ͽ� �ٸ� ������Ʈ ���� ��ũ��Ʈ������ instance�� �ҷ��� �� �ְ� �մϴ� 

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null) //instance�� null. ��, �ý��ۻ� �����ϰ� ���� ������
        {
            instance = this; // ���ڽ��� instance�� �־��ݴϴ�.
            DontDestroyOnLoad(gameObject); // OnLoad(���� �ε� �Ǿ�����) �ڽ��� �ı����� �ʰ� ����
        }
        else
        {
            if (instance != this) // instance�� ���� �ƴ϶�� �̹� instance�� �ϳ� �����ϰ� �ִٴ� �ǹ�
            {
                Destroy(this.gameObject); // �� �̻� �����ϸ� �ȵǴ� ��ü�̴� ��� AWake�� �ڽ��� ����
            }
        }
    }
    // �̱��� //

    // GamaManager ������Ʈ�� �ִ� �ٸ� ��ũ��Ʈ��
    public FPS_Limit fps_Limit;
    public SaveManager saveManager;
    public TextFileManager textFileManager;

    // GamaManager ������Ʈ�� �ִ� ��ü��
    public UniversalRendererData urpRenderer; // SSAO ��Ʈ�ѿ� ������
}