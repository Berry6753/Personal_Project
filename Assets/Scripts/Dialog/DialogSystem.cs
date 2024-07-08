using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private List<Speaker> speakerList = new List<Speaker>();        // ��ȭ�� �����ϴ� ĳ���͵��� ����Ʈ
    [SerializeField] private List<DialogDate> dialogList = new List<DialogDate>();   // ���� �б��� ��� ��� ����Ʈ
    [SerializeField] private bool isAutoStart = true;       // �ڵ� ���� ����
    private bool isFirst = true;                            // ���� 1ȸ�� ȣ���ϱ� ���� ����
    private int currentDialogIndex = -1;                    // ���� ��� ����
    private int currentSpeakerIndex = 0;                    // ���� ���� �ϴ� Speaker�� ����Ʈ ����

    //private void Awake()
    //{
    //    speakerList = new List<Speaker>();
    //    dialogList = new List<DialogDate>();
    //}
    private void OnEnable()
    {
        SetUp();
        StartCoroutine(CoDialog());
    }
    private IEnumerator CoDialog()
    {
        yield return new WaitUntil(() => UpdateDialog());
    }
    private void SetUp()
    {
        // ��� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
        for (int i = 0; i < speakerList.Count; i++)
        {
            SetActiveObjects(speakerList[i], false);

            // ĳ���� �̹����� ���̵��� ����
            speakerList[i].spriteRenderer.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        // ��� �бⰡ ���۵� �� 1ȸ�� ȣ��
        if (isFirst)
        {
            // �ʱ�ȭ. ĳ���� �̹����� Ȱ��ȭ�ϰ�, ��� ���� UI�� ��� ��Ȱ��ȭ
            SetUp();

            // �ڵ� ���(isAutoStart=true)���� �����Ǿ� ������ ù ��° ��� ���
            if (isAutoStart) SetNextDialog();

            isFirst = false;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            // ��簡 �������� ��� ���� ��� ����
            if (dialogList.Count > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            // ��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� true ��ȯ
            else
            {
                // ���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
                for (int i = 0; i < dialogList.Count; i++)
                {
                    SetActiveObjects(speakerList[i], false);

                    // SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
                    speakerList[i].spriteRenderer.gameObject.SetActive(false);
                }

                return true;
            }
        }

        return false;
    }

    private void SetNextDialog()
    {
        // ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
        SetActiveObjects(speakerList[currentSpeakerIndex], false);

        // ���� ��縦 �����ϵ���
        currentDialogIndex++;

        // ���� ȭ�� ���� ����
        currentSpeakerIndex = dialogList[currentDialogIndex].speakerIndex;

        // ���� ȭ���� ��ȭ ���� ������Ʈ Ȱ��ȭ
        SetActiveObjects(speakerList[currentSpeakerIndex], true);

        // ���� ȭ�� �̸� �ؽ�Ʈ ����
        speakerList[currentSpeakerIndex].textName.text = dialogList[currentDialogIndex].name;

        // ���� ȭ���� ��� �ؽ�Ʈ ����
        speakerList[currentSpeakerIndex].textDialogue.text = dialogList[currentDialogIndex].dialogue;
    }


    private void SetActiveObjects(Speaker speaker, bool isActive)
    { 
        speaker.imageDialog.gameObject.SetActive(isActive);
        speaker.textName.gameObject.SetActive(isActive);
        speaker.textDialogue.gameObject.SetActive(isActive);

        // ��ȭ ���� �� ������ UI�� ����ÿ��� ������ ������ �׻� false
        speaker.objectArrow.SetActive(false );

        // ĳ���� ���� �� ����
        Color color = speaker.spriteRenderer.color;
        color.a = isActive ? 1 : 0.2f;
        speaker.spriteRenderer.color = color;   
    }
}

[System.Serializable]
public struct Speaker
{ 
    public Image spriteRenderer;                // ĳ���� �̹���
    public Image imageDialog;                   // ��ȭâ Image UI
    public TMP_Text textName;                   // ���� ������� ĳ���� �̸�
    public TMP_Text textDialogue;               // ���� ��� ���
    public GameObject objectArrow;              // ��簡 �Ϸ�� �� ��µǴ� ������Ʈ
}

[System.Serializable]
public struct DialogDate
{
    public int speakerIndex;                    // �̸��� ��縦 ����� Speaker�� �迭 Index ��
    public string name;                         // ĳ���� �̸�
    [TextArea(3, 5)]
    public string dialogue;                     // ��系��
}