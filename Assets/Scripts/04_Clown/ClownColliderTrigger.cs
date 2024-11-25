using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClownColliderTrigger : MonoBehaviour
{
    [SerializeField]
    Collider trigger_Chase; // ���� �߰� ���� Ʈ���� �ݶ��̴�

    [SerializeField]
    Collider trigger_SafeZone; // ���� ���� Ʈ���� �ݶ��̴�

    [SerializeField]
    Clown_Chase clown_Chase;
    public Collider clown_chase_col;

    [SerializeField]
    Collider[] triggers_CircusSuccess;

    [SerializeField]
    CircusFlash circusFlash;

    [SerializeField]
    Collider Ladder_Trigger;

    Rigidbody myRigid;

    [SerializeField]
    ClownRaycast clownRaycast;

    bool is_falling;
    public GameObject bed;
    public AudioSource bounce_sound;
    public AudioSource neck_crack_sound;
    public AudioSource wind_sound;

    bool is_chasing_music_on;
    bool pre_is_chasing_music_on;
    public AudioSource chasing_music;
    Coroutine audio_coroutine;

    public bool[] isSuccess = new bool[1];

    public ClownWorm clownWorm;

    public Collider fallTrigger;
    public AudioSource scream;

    public RawImage fadeInOutImage; // ���̵�-��, �ƿ� �̹���
    public FadeInOutScript fadeInOutScript; // ���̵�-��, �ƿ� ��ũ��Ʈ




    void Start()
    {
        wind_sound.Stop();
        is_falling = true;
        myRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (is_falling)
        {
            if (collision.gameObject == bed)
            {
                bounce_sound.Play();
            }
            else
            {
                neck_crack_sound.Play();
                StartCoroutine(DieCoroutine());
            }
            is_falling = false;
            wind_sound.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ���� Ʈ����
        if (other == trigger_SafeZone)
        {
            ChasingMusicControl(false, 0f);
            clown_Chase.isSafe = true;
        }

        // �߰� ���� Ʈ���ſ� ���� �÷��� OFF
        if (other == trigger_Chase && clown_Chase.isSafe == false)
        {
            ChasingMusicControl(true, 0.7f);
            circusFlash.isFlashOn = false;
        }

        // ��Ŀ�� ���� ���� Ʈ����
        for(int i = 0; i < triggers_CircusSuccess.Length; i++)
        {
            if (!isSuccess[i] && other == triggers_CircusSuccess[i] && clownRaycast.life > 0)
            {
                isSuccess[i] = true;
                circusFlash.isFlashOn = true;
                StartCoroutine(AudioOnOffScript.VolumeCoroutine(clownRaycast.applause, true, 2f, 0.5f));
                clownRaycast.yay.Play();
                // clownRaycast.circusSong.Stop();
                clownRaycast.elevatorAnims[3].Play("Open");
            }
        }

        // �����ϸ� �����
        if(other.gameObject == clownWorm.deadTrigger || other == fallTrigger || other == clown_chase_col)
        {
            if(other == clown_chase_col)
            {
                clown_Chase.sound_and_collider.transform.parent.gameObject.SetActive(false);
                neck_crack_sound.Play();
            }
            is_falling = false;
            wind_sound.Stop();
            clownWorm.deadTrigger.SetActive(false);
            fallTrigger.enabled = false;
            StartCoroutine(DieCoroutine());
        }
    }

    private void OnTriggerStay(Collider other)

    {
        // �߰� ���� Ʈ���ſ� ���� ���� �߰� ����
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = true;
        }

        // ��ٸ� Ʈ����
        if (other == Ladder_Trigger)
        {
            UpLadder();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �߰� ���� Ʈ���ſ��� ������ ���� �߰� ����
        if (other == trigger_Chase)
        {
            ChasingMusicControl(false, 0f);
            clown_Chase.isChasing = false;
        }
        if (other == trigger_SafeZone)
        {
            //clown_Chase.isSafe = false;
        }
    }

    // ���� �߰� ���� ���
    void ChasingMusicControl(bool turnOnOff, float volume)
    {
        if (turnOnOff != pre_is_chasing_music_on)
        {
            pre_is_chasing_music_on = turnOnOff;
            if (audio_coroutine != null)
            {
                StopCoroutine(audio_coroutine);
            }
            audio_coroutine = StartCoroutine(AudioOnOffScript.VolumeCoroutine(chasing_music, turnOnOff, 1f, volume));
        }
    }

    // ��ٸ� ���
    private void UpLadder()
    {
        if (Input.GetKey(KeyCode.W))
        {
            myRigid.velocity = new Vector3(myRigid.velocity.x, 2.5f, myRigid.velocity.z);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                myRigid.velocity = new Vector3(myRigid.velocity.x, 5f, myRigid.velocity.z);
            }
        }
        else
        {
            myRigid.velocity = new Vector3(myRigid.velocity.x, -2.5f, myRigid.velocity.z);
        }
    }

    // �װ� �ٽý���
    IEnumerator DieCoroutine()
    {
        fadeInOutImage.color = Color.black;
        scream.Play();
        yield return new WaitForSeconds(2f);
        LoadSceneScript.FailLoadScene("04_Clown");
    }
}
