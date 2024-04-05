using UnityEngine;
using TMPro;

public class RankSystem : MonoBehaviour
{
    [SerializeField]
    private int maxRankCount = 10; //최대 랭크 표시 개수
    [SerializeField]
    private GameObject textPrefab; //랭크 정보를 출력하는 Text UI 프리팹
    [SerializeField]
    private Transform panelRankInfo; //Text가 배치되는 부모 Panel Transform

    private RankData[] rankDataArray; //랭크 정보를 저장하는 RankData 타입의 배열
    private int currentIndex = 0;

    private void Awake()
    {
        //github 파일변경 테스트

        rankDataArray = new RankData[maxRankCount];

        //기존의 랭크 정보 불러오기
        LoadRankData();
        //1등으로부터 차례대로 현재 스테이지에서 획득한 점수와 비교
        CompareRank();
        //랭크 정보 출력
        PrintRankData();
        //새로운 랭크 정보 저장
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
        //Instantiate()로 textPrefab 복사체 생성하고,clone변수에 저장
        GameObject clone = Instantiate(textPrefab);
        //clone의 TextMeshProUGUI 컴포넌트 정보를 얻어와 text변수에 저장
        TextMeshProUGUI text = clone.GetComponent<TextMeshProUGUI>();

        //생성한 text ui 오브젝트의 부모를 panelRankInfo 오브젝트로 설정
        clone.transform.SetParent(panelRankInfo);
        //자식으로 등록되면서 크기가 변환될수 있기에 크기를 1로 설정
        clone.transform.localScale = Vector3.one;
        //text ui에 출력할 내용과 폰트 색상 설정
        text.text = print;
        text.color = color;
    }
    private void CompareRank()
    {
        //현재 스테이지에서 달성한 정보
        RankData currentData = new RankData();
        currentData.score = PlayerPrefs.GetInt("CurrentScore");
        currentData.maxCombo = PlayerPrefs.GetInt("CurrentMaxCombo");
        currentData.normalMoleHitCount = PlayerPrefs.GetInt("CurrentNormalMoleHitCount");
        currentData.redMoleHitCount = PlayerPrefs.GetInt("CurrentRedMoleHitCount");
        currentData.blueMoleHitCount = PlayerPrefs.GetInt("CurrentBlueMoleHitCount");

        //1~10등의 점수와 현재 스테이지에서 달성한 점수 비교
        for(int i=0; i<maxRankCount; ++i)
        {
            if(currentData.score > rankDataArray[i].score)
            {
                //랭크에 들어갈 수 있는 점수를 달성했으면 반복문 중지
                currentIndex = i;
                break;
            }
        }

        //currentData의 등수 아래로 점수를 한칸씩 밀어서 저장
        for(int i=maxRankCount-1; i>0; --i)
        {
            rankDataArray[i] = rankDataArray[i - 1];

            if(currentIndex == i - 1)
            {
                break;
            }
        }

        //새로운 점수를 랭크에 집어넣기
        rankDataArray[currentIndex] = currentData;
    }

    private void PrintRankData()
    {
        Color color = Color.white;

        for(int i=0; i<maxRankCount; ++i)
        {
            //방금 플레이어이 점수가 랭크에 등록되면 색상을 노란색으로 표시
            color = currentIndex != i ? Color.white : Color.yellow;

            //Text-TextMeshPro 생성 및 원하는 데이터 출력
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
