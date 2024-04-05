using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CountDown countDown;
    [SerializeField]
    private MoleSpawner moleSpawner;
    private int score;
    private int combo;
    private float currentTime;

    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }
    public int Combo
    {
        set
        {
            combo = Mathf.Max(0, value);
            //70 �̻��� �� MaxSpawnMole�� 5�� �����Ǳ� ������ 70������ üũ
            if(combo <= 70)
            {
                //�޺��� ���� �����Ǵ� �ִ� �δ��� ����
                moleSpawner.MaxSpawnMole = 1 + (combo + 10) / 20;
            }
            //�ִ� �޺� ����
            if(combo > MaxCombo)
            {
                MaxCombo = combo;
            }
        }
        get => combo;
    }
    public int MaxCombo { private set; get; }
    public int NormalMoleHitCount { set; get;}
    public int RedMoleHitCount { set; get; }
    public int BlueMoleHitCount { set; get; }

    [field: SerializeField]
    public float MaxTime { private set; get; }
    public float CurrentTime
    {
        set => currentTime = Mathf.Clamp(value, 0, MaxTime);
        get => currentTime;
    }

    private void Start()
    {
        countDown.StartCountDown(GameStart);
    }

    private void GameStart()
    {
        moleSpawner.Setup();

        StartCoroutine("OnTimeCount");
    }

    private IEnumerator OnTimeCount()
    {
        CurrentTime = MaxTime;

        while( CurrentTime > 0 )
        {
            CurrentTime -= Time.deltaTime;

            yield return null;
        }

        //CurrentTime�� 0�� �Ǹ� GameOver() �޼ҵ带 ȣ���� ���ӿ��� ó��
        GameOver();
    }

    private void GameOver()
    {
        //���� ������������ ȹ���� ���� ���� ����
        PlayerPrefs.SetInt("CurrentScore", Score);
        PlayerPrefs.SetInt("CurrentMaxCombo", MaxCombo);
        PlayerPrefs.SetInt("CurrentNormalMoleHitCount", NormalMoleHitCount);
        PlayerPrefs.SetInt("CurrentRedMoleHitCount", RedMoleHitCount);
        PlayerPrefs.SetInt("CurrentBlueMoleHitCout", BlueMoleHitCount);

        //GameOver ������ �̵�
        SceneManager.LoadScene("GameOver");
    }
}
