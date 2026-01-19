using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class QuestUIController : MonoBehaviour
{
    [Header("UXML Templates")]
    [SerializeField] private VisualTreeAsset questListItemTemplate;
    [SerializeField] private VisualTreeAsset objectiveRowTemplate;

    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.J;

    private UIDocument doc;

    private VisualElement questRoot;
    private Button closeBtn;
    private ScrollView questList;
    private Label questTitle;
    private Label questDesc;
    private VisualElement objectivesRoot;

    private readonly List<QuestModel> quests = new();
    private QuestModel selected;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        questRoot = root.Q<VisualElement>("quest-root");
        closeBtn = root.Q<Button>("quest-close-btn");
        questList = root.Q<ScrollView>("quest-list");
        questTitle = root.Q<Label>("quest-title");
        questDesc = root.Q<Label>("quest-desc");
        objectivesRoot = root.Q<VisualElement>("objectives-root");

        questList.verticalScrollerVisibility = ScrollerVisibility.Hidden;
        questList.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        questList.contentContainer.style.flexDirection = FlexDirection.Column;
        questList.RegisterCallback<WheelEvent>(OnQuestListWheel, TrickleDown.TrickleDown);

        closeBtn.clicked += Hide;

        Hide();
        SeedDemo();
        RefreshList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (IsVisible()) Hide();
            else Show();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsVisible()) Hide();
        }
    }


    public bool IsOpen => IsVisible();

    private bool IsVisible()
        => questRoot != null && questRoot.style.display.value == DisplayStyle.Flex;

    private void Show()
    {
        questRoot.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
        questList?.Focus();
    }


    private void Hide()
    {
        questRoot.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
    }

    private void RefreshList()
    {
        questList.Clear();

        foreach (var q in quests)
        {
            var item = questListItemTemplate.Instantiate();

            var btn = item.Q<Button>("item-btn");
            var title = item.Q<Label>("item-title");
            var state = item.Q<Label>("item-state");

            title.text = q.title;

            // 완료 표시: 버튼 크기 그대로, (완료)만 보이기
            state.style.display = q.completed ? DisplayStyle.Flex : DisplayStyle.None;

            // 선택 강조
            if (selected != null && selected.id == q.id)
                btn.AddToClassList("selected");

            btn.clicked += () =>
            {
                selected = q;
                ShowDetail(q);
                RefreshList(); // 선택 강조 갱신
            };

            questList.Add(item);
        }
    }

    private void OnQuestListWheel(WheelEvent e)
    {
        const float speed = 35f; // 취향
        var offset = questList.scrollOffset;
        offset.y += e.delta.y * speed;
        questList.scrollOffset = offset;

        e.StopPropagation();
    }

    private void ShowDetail(QuestModel q)
    {
        questTitle.text = q.title;
        questDesc.text = q.description;

        objectivesRoot.Clear();

        foreach (var obj in q.objectives)
        {
            var row = objectiveRowTemplate.Instantiate();
            var label = row.Q<Label>("obj-text");
            label.text = $"{obj.text} ({obj.current}/{obj.required})";
            objectivesRoot.Add(row);
        }
    }

    private void SeedDemo()
    {
        quests.Clear();

        // q1~q12: 예시 12개
        quests.Add(new QuestModel {
            id = "q1",
            title = "초원 정찰",
            description = "마을 주변을 정찰하고 위협 요소를 보고하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="주변 지역 방문", current=1, required=3 },
                new QuestObjective { text="보고서 제출", current=0, required=1 }
            }
        });

        quests.Add(new QuestModel {
            id = "q2",
            title = "슬라임 퇴치",
            description = "슬라임을 처치해 마을을 안전하게 만들어라.",
            completed = true,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="슬라임 처치", current=5, required=5 }
            }
        });

        quests.Add(new QuestModel {
            id = "q3",
            title = "부서진 울타리 수리",
            description = "마을 외곽의 부서진 울타리를 수리해야 한다.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="나무 판자 수집", current=2, required=5 },
                new QuestObjective { text="울타리 수리", current=0, required=1 }
            }
        });

        quests.Add(new QuestModel {
            id = "q4",
            title = "잃어버린 반지",
            description = "마을 주민이 잃어버린 반지를 찾아 돌려주자.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="우물 주변 조사", current=0, required=1 },
                new QuestObjective { text="반지 회수", current=0, required=1 }
            }
        });

        quests.Add(new QuestModel {
            id = "q5",
            title = "고블린 흔적 추적",
            description = "숲에서 발견된 고블린의 흔적을 추적하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="발자국 조사", current=1, required=3 },
                new QuestObjective { text="고블린 처치", current=0, required=2 }
            }
        });

        quests.Add(new QuestModel {
            id = "q6",
            title = "약초 채집",
            description = "치료용 약초를 채집해 연금술사에게 가져가자.",
            completed = true,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="약초 채집", current=5, required=5 }
            }
        });

        quests.Add(new QuestModel {
            id = "q7",
            title = "버려진 창고 조사",
            description = "마을 북쪽의 버려진 창고를 조사하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="창고 내부 진입", current=0, required=1 },
                new QuestObjective { text="의심스러운 물건 확인", current=0, required=3 }
            }
        });

        quests.Add(new QuestModel {
            id = "q8",
            title = "야간 경비",
            description = "밤 동안 마을을 순찰하며 이상 징후를 확인하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="동쪽 구역 순찰", current=0, required=1 },
                new QuestObjective { text="서쪽 구역 순찰", current=0, required=1 },
                new QuestObjective { text="보고 완료", current=0, required=1 }
            }
        });

        quests.Add(new QuestModel {
            id = "q9",
            title = "도적의 은신처",
            description = "도적들이 숨어 있는 은신처를 찾아내 제거하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="은신처 발견", current=0, required=1 },
                new QuestObjective { text="도적 처치", current=1, required=4 }
            }
        });

        quests.Add(new QuestModel {
            id = "q10",
            title = "부상자 치료",
            description = "전투로 부상당한 주민을 치료하라.",
            completed = true,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="붕대 사용", current=1, required=1 },
                new QuestObjective { text="치료 확인", current=1, required=1 }
            }
        });

        quests.Add(new QuestModel {
            id = "q11",
            title = "고대 석판 해독",
            description = "유적에서 발견된 고대 석판을 해독하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="석판 조사", current=0, required=1 },
                new QuestObjective { text="문자 해독", current=0, required=2 }
            }
        });

        quests.Add(new QuestModel {
            id = "q12",
            title = "마을 회의 참석",
            description = "중요한 안건이 논의되는 마을 회의에 참석하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text="회의장 도착", current=0, required=1 },
                new QuestObjective { text="회의 종료까지 대기", current=0, required=1 }
            }
        });
    }
}
