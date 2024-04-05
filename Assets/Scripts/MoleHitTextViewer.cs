using System.Collections;
using UnityEngine;
using TMPro;

public class MoleHitTextViewer : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 30.0f; //�̵� �ӵ�
    private Vector2 defaultPosition; //�̵� �ִϸ��̼ǿ� �־ �ʱ� ��ġ ����
    private TextMeshProUGUI textHit;
    private RectTransform rectHit;

    private void Awake()
    {
        textHit = GetComponent<TextMeshProUGUI>();
        rectHit = GetComponent<RectTransform>();
        defaultPosition = rectHit.anchoredPosition;

        gameObject.SetActive(false);
    }

    public void OnHit(string hitData, Color color)
    {
        //������Ʈ�� ȭ�鿡 ���̵��� ����
        gameObject.SetActive(true);
        //Score ++xx, score -300, Time +3�� ���� ����� ���� ����
        textHit.text = hitData;

        //�ؽ�Ʈ�� ���� �̵��ϸ� ���� ������� OnAnimation() �ڷ�ƾ ����
        StopCoroutine("OnAnimation");
        StartCoroutine("OnAnimation", color);
    }

    private IEnumerator OnAnimation(Color color)
    {
        //������Ʈ�� on,off �ؼ� ����ϰ�, �̵� �ִϸ��̼��� �߱� ������ ��ġ ����
        rectHit.anchoredPosition = defaultPosition;

        while(color.a > 0)
        {
            //Vector2.up �������� �̵�
            rectHit.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
            //���� 1 -> 0���� ����
            color.a -= Time.deltaTime;
            textHit.color = color;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
