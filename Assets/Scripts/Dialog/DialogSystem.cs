using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private List<Speaker> speakerList = new List<Speaker>();        // 대화에 참여하는 캐릭터들의 리스트
    [SerializeField] private List<DialogDate> dialogList = new List<DialogDate>();   // 현재 분기의 대사 목록 리스트
    [SerializeField] private bool isAutoStart = true;       // 자동 시작 여부
    private bool isFirst = true;                            // 최초 1회만 호출하기 위한 변수
    private int currentDialogIndex = -1;                    // 현재 대사 순번
    private int currentSpeakerIndex = 0;                    // 현재 말을 하는 Speaker의 리스트 순번

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
        // 모든 대화 관련 오브젝트 비활성화
        for (int i = 0; i < speakerList.Count; i++)
        {
            SetActiveObjects(speakerList[i], false);

            // 캐릭터 이미지는 보이도록 설정
            speakerList[i].spriteRenderer.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        // 대사 분기가 시작될 때 1회만 호출
        if (isFirst)
        {
            // 초기화. 캐릭터 이미지는 활성화하고, 대사 관련 UI는 모두 비활성화
            SetUp();

            // 자동 재생(isAutoStart=true)으로 설정되어 있으면 첫 번째 대사 재생
            if (isAutoStart) SetNextDialog();

            isFirst = false;
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            // 대사가 남아있을 경우 다음 대사 진행
            if (dialogList.Count > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            // 대사가 더 이상 없을 경우 모든 오브젝트를 비활성화하고 true 반환
            else
            {
                // 현재 대화에 참여했던 모든 캐릭터, 대화 관련 UI를 보이지 않게 비활성화
                for (int i = 0; i < dialogList.Count; i++)
                {
                    SetActiveObjects(speakerList[i], false);

                    // SetActiveObjects()에 캐릭터 이미지를 보이지 않게 하는 부분이 없기 때문에 별도로 호출
                    speakerList[i].spriteRenderer.gameObject.SetActive(false);
                }

                return true;
            }
        }

        return false;
    }

    private void SetNextDialog()
    {
        // 이전 화자의 대화 관련 오브젝트 비활성화
        SetActiveObjects(speakerList[currentSpeakerIndex], false);

        // 다음 대사를 진행하도록
        currentDialogIndex++;

        // 현재 화자 순번 설정
        currentSpeakerIndex = dialogList[currentDialogIndex].speakerIndex;

        // 현재 화자의 대화 관련 오브젝트 활성화
        SetActiveObjects(speakerList[currentSpeakerIndex], true);

        // 현재 화자 이름 텍스트 설정
        speakerList[currentSpeakerIndex].textName.text = dialogList[currentDialogIndex].name;

        // 현재 화자의 대사 텍스트 설정
        speakerList[currentSpeakerIndex].textDialogue.text = dialogList[currentDialogIndex].dialogue;
    }


    private void SetActiveObjects(Speaker speaker, bool isActive)
    { 
        speaker.imageDialog.gameObject.SetActive(isActive);
        speaker.textName.gameObject.SetActive(isActive);
        speaker.textDialogue.gameObject.SetActive(isActive);

        // 대화 종료 시 나오는 UI는 종료시에만 나오기 때문에 항상 false
        speaker.objectArrow.SetActive(false );

        // 캐릭터 알파 값 변경
        Color color = speaker.spriteRenderer.color;
        color.a = isActive ? 1 : 0.2f;
        speaker.spriteRenderer.color = color;   
    }
}

[System.Serializable]
public struct Speaker
{ 
    public Image spriteRenderer;                // 캐릭터 이미지
    public Image imageDialog;                   // 대화창 Image UI
    public TMP_Text textName;                   // 현재 대사중인 캐릭터 이름
    public TMP_Text textDialogue;               // 현재 대사 출력
    public GameObject objectArrow;              // 대사가 완료될 시 출력되는 오브젝트
}

[System.Serializable]
public struct DialogDate
{
    public int speakerIndex;                    // 이름과 대사를 출력할 Speaker의 배열 Index 값
    public string name;                         // 캐릭터 이름
    [TextArea(3, 5)]
    public string dialogue;                     // 대사내용
}