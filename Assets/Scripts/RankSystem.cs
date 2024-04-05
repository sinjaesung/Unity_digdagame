using UnityEngine;
using TMPro;

public class RankSystem : MonoBehaviour
{
    [SerializeField]
    private int maxRankCount = 10; //�ִ� ��ũ ǥ�� ����
    [SerializeField]
    private GameObject textPrefab; //��ũ ������ ����ϴ� Text UI ������
    [SerializeField]
    private Transform panelRankInfo; //Text�� ��ġ�Ǵ� �θ� Panel Transform

    private RankData[] rankDataArray; //��ũ ������ �����ϴ� RankData Ÿ���� �迭
    private int currentIndex = 0;

    private void Awake()
    {
        //github ���Ϻ��� �׽�Ʈ

        rankDataArray = new RankData[maxRankCount];

        //������ ��ũ ���� �ҷ�����
        LoadRankData();
        //1�����κ��� ���ʴ�� ���� ������������ ȹ���� ������ ��
        CompareRank();
        //��ũ ���� ���
        PrintRankData();
        //���ο� ��ũ ���� ����
        SaveRankData();
    }

    private void LoadRankData()
    {
        for(int i =0; i<maxRankCount; ++i)
        {
            rankDataArray[i].score = PlayerPrefs.GetInt("RankScore" + i);
            rankDataArray[i].maxCombo = PlayerPrefs.GetInt("RankMaxCombo" + i);
            rankDataArray[i].normalMoleHitCount = PlayerPrefs.GetInt("RankNormalMoleHitCount" + i);
            rankDataArray[i].redMoleHitCount = PlayerPrefs.GetInt("RankRedMoleHitCout" + i);
            rankDataArray[i].blueMoleHitCount = PlayerPrefs.GetInt("RankBlueMoleHitCount" + i);
        }
    }
    
    private void SpawnText(string print,Color color)
    {
        //Instantiate()�� textPrefab ����ü �����ϰ�,clone������ ����
        GameObject clone = Instantiate(textPrefab);
        //clone�� TextMeshProUGUI ������Ʈ ������ ���� text������ ����
        TextMeshProUGUI text = clone.GetComponent<TextMeshProUGUI>();

        //������ text ui ������Ʈ�� �θ� panelRankInfo ������Ʈ�� ����
        clone.transform.SetParent(panelRankInfo);
        //�ڽ����� ��ϵǸ鼭 ũ�Ⱑ ��ȯ�ɼ� �ֱ⿡ ũ�⸦ 1�� ����
        clone.transform.localScale = Vector3.one;
        //text ui�� ����� ����� ��Ʈ ���� ����
        text.text = print;
        text.color = color;
    }
    private void CompareRank()
    {
        //���� ������������ �޼��� ����
        RankData currentData = new RankData();
        currentData.score = PlayerPrefs.GetInt("CurrentScore");
        currentData.maxCombo = PlayerPrefs.GetInt("CurrentMaxCombo");
        currentData.normalMoleHitCount = PlayerPrefs.GetInt("CurrentNormalMoleHitCount");
        currentData.redMoleHitCount = PlayerPrefs.GetInt("CurrentRedMoleHitCount");
        currentData.blueMoleHitCount = PlayerPrefs.GetInt("CurrentBlueMoleHitCount");

        //1~10���� ������ ���� ������������ �޼��� ���� ��
        for(int i=0; i<maxRankCount; ++i)
        {
            if(currentData.score > rankDataArray[i].score)
            {
                //��ũ�� �� �� �ִ� ������ �޼������� �ݺ��� ����
                currentIndex = i;
                break;
            }
        }

        //currentData�� ��� �Ʒ��� ������ ��ĭ�� �о ����
        for(int i=maxRankCount-1; i>0; --i)
        {
            rankDataArray[i] = rankDataArray[i - 1];

            if(currentIndex == i - 1)
            {
                break;
            }
        }

        //���ο� ������ ��ũ�� ����ֱ�
        rankDataArray[currentIndex] = currentData;
    }

    private void PrintRankData()
    {
        Color color = Color.white;

        for(int i=0; i<maxRankCount; ++i)
        {
            //��� �÷��̾��� ������ ��ũ�� ��ϵǸ� ������ ��������� ǥ��
            color = currentIndex != i ? Color.white : Color.yellow;

            //Text-TextMeshPro ���� �� ���ϴ� ������ ���
            SpawnText((i + 1).ToString(), color);
            SpawnText(rankDataArray[i].score.ToString(), color);
            SpawnText(rankDataArray[i].maxCombo.ToString(), color);
            SpawnText(rankDataArray[i].normalMoleHitCount.ToString(), color);
            SpawnText(rankDataArray[i].redMoleHitCount.ToString(), color);
            SpawnText(rankDataArray[i].blueMoleHitCount.ToString(), color);
        }
    }

    private void SaveRankData()
    {
        for(int i = 0; i<maxRankCount; ++i)
        {
            PlayerPrefs.SetInt("RankScore" + i, rankDataArray[i].score);
            PlayerPrefs.SetInt("RankMaxCombo" + i, rankDataArray[i].maxCombo);
            PlayerPrefs.SetInt("RankNormalMoleHitCount" + i, rankDataArray[i].normalMoleHitCount);
            PlayerPrefs.SetInt("RankRedMoleHitCount" + i, rankDataArray[i].redMoleHitCount);
            PlayerPrefs.SetInt("RankBlueMoleHitCount" + i, rankDataArray[i].blueMoleHitCount);
        }
    }

    [System.Serializable]
    public struct RankData
    {
        public int score;
        public int maxCombo;
        public int normalMoleHitCount;
        public int redMoleHitCount;
        public int blueMoleHitCount;
    }
}
