using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class QuestUIController : MonoBehaviour
{
    [Header("UXML Templates")]
    [SerializeField] private VisualTreeAsset questListItemTemplate;
    [SerializeField] private VisualTreeAsset objectiveRowTemplate;

    [Header("Input (Optional)")]
    [SerializeField] private bool useHotkeyToggle = false;
    [SerializeField] private KeyCode toggleKey = KeyCode.J;

    [SerializeField] private Sprite questIconIdle;
    [SerializeField] private Sprite questIconHover;

    public static QuestUIController Instance { get; private set; }

    private UIDocument doc;

    // NEW: Open button (outside quest-root recommended)
    private Button openBtn;

    private VisualElement questRoot;
    private Button closeBtn;
    private ScrollView questList;

    // Right panel (detail)
    private VisualElement questRight;
    private Label questTitle;
    private Label questDesc;
    private Label objectivesTitle;
    private VisualElement objectivesRoot;

    private readonly List<QuestModel> quests = new();
    private QuestModel selected;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        // ✅ 화면 클릭으로 열기 버튼
        openBtn = root.Q<Button>("quest-open-btn");
        if (openBtn != null)
        {
            openBtn.clicked += () =>
            {
                if (IsVisible()) Hide();
                else Show();
            };
        }
        else
        {
            Debug.LogWarning("[QuestUIController] UXML에서 name='quest-open-btn' 버튼을 찾지 못했습니다. (UXML에 버튼 추가 필요)");
        }
        var icon = root.Q<Image>("quest-icon");

        if (icon != null)
        {
            icon.sprite = questIconIdle;

            openBtn.RegisterCallback<MouseEnterEvent>(_ =>
            {
                icon.sprite = questIconHover;
            });

            openBtn.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                icon.sprite = questIconIdle;
            });
        }
        questRoot = root.Q<VisualElement>("quest-root");
        closeBtn = root.Q<Button>("quest-close-btn");
        questList = root.Q<ScrollView>("quest-list");

        questRight = root.Q<VisualElement>("quest-right");
        questTitle = root.Q<Label>("quest-title");
        questDesc = root.Q<Label>("quest-desc");
        objectivesTitle = root.Q<Label>("objectives-title");
        objectivesRoot = root.Q<VisualElement>("objectives-root");

        questList.verticalScrollerVisibility = ScrollerVisibility.Hidden;
        questList.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        questList.contentContainer.style.flexDirection = FlexDirection.Column;
        questList.RegisterCallback<WheelEvent>(OnQuestListWheel, TrickleDown.TrickleDown);

        closeBtn.clicked += Hide;

        if (objectivesTitle != null) objectivesTitle.text = "퀘스트 목표";
        HideDetail();

        Hide();
        SeedDemo();
        RefreshList();
    }

    private void Update()
    {
        // ✅ J키 토글은 옵션
        if (useHotkeyToggle && Input.GetKeyDown(toggleKey))
        {
            if (IsVisible()) Hide();
            else Show();
        }

        // ✅ 퀘스트 창이 열려있을 때 ESC 처리: 설정창 호출 방지(ESC 소비)
        if (IsVisible() && Input.GetKeyDown(KeyCode.Escape))
        {
            // 원하면 ESC로 퀘스트창 닫기
            Hide();

            EscapeGuard.ConsumeEsc(); // ★ 이 프레임 ESC는 퀘스트창이 먹음
            return;
        }
        // 테스트용
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("테스트: Test1 처치 호출");
            NotifyEnemyDefeated("Test1", 1);
        }
    }

    public bool IsOpen => IsVisible();

    private bool IsVisible()
        => questRoot != null && questRoot.style.display.value == DisplayStyle.Flex;

    private void Show()
    {
        questRoot.style.display = DisplayStyle.Flex;

        // 더블클릭 이슈 해결: Root에 포커스
        questRoot.Focus();

        if (selected == null) HideDetail();
        Time.timeScale = 0f;
    }

    private void Hide()
    {
        questRoot.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
    }

    private void HideDetail()
    {
        if (questTitle != null) questTitle.style.display = DisplayStyle.None;
        if (questDesc != null) questDesc.style.display = DisplayStyle.None;
        if (objectivesTitle != null) objectivesTitle.style.display = DisplayStyle.None;
        if (objectivesRoot != null) objectivesRoot.style.display = DisplayStyle.None;
    }

    private void ShowDetailUI()
    {
        if (questTitle != null) questTitle.style.display = DisplayStyle.Flex;
        if (questDesc != null) questDesc.style.display = DisplayStyle.Flex;
        if (objectivesTitle != null) objectivesTitle.style.display = DisplayStyle.Flex;
        if (objectivesRoot != null) objectivesRoot.style.display = DisplayStyle.Flex;
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

            state.style.display = q.completed ? DisplayStyle.Flex : DisplayStyle.None;
            state.pickingMode = PickingMode.Ignore;

            if (selected != null && selected.id == q.id)
                btn.AddToClassList("selected");
            else
                btn.RemoveFromClassList("selected");

            btn.clicked += () =>
            {
                selected = q;
                ShowDetail(q);
                RefreshList();
            };

            questList.Add(item);
        }
    }

    private void OnQuestListWheel(WheelEvent e)
    {
        const float speed = 35f;
        var offset = questList.scrollOffset;
        offset.y += e.delta.y * speed;
        questList.scrollOffset = offset;

        e.PreventDefault();
    }

    private void ShowDetail(QuestModel q)
    {
        if (q == null)
        {
            HideDetail();
            return;
        }

        ShowDetailUI();

        if (questTitle != null) questTitle.text = q.title;
        if (questDesc != null) questDesc.text = q.description;
        if (objectivesTitle != null) objectivesTitle.text = "퀘스트 목표";

        objectivesRoot?.Clear();

        foreach (var obj in q.objectives)
        {
            var row = objectiveRowTemplate.Instantiate();
            var label = row.Q<Label>("obj-text");
            label.text = $"{obj.text} ({obj.current}/{obj.required})";
            objectivesRoot.Add(row);
        }
    }

    public void NotifyEnemyDefeated(string enemyId, int amount = 1)
    {
        if (string.IsNullOrEmpty(enemyId)) return;
        Debug.Log($"[Quest] NotifyEnemyDefeated 호출됨: {enemyId}");
        bool anyChanged = false;

        foreach (var q in quests)
        {
            if (q.completed) continue;

            foreach (var obj in q.objectives)
            {
                if (string.IsNullOrEmpty(obj.targetEnemyId)) continue;

                if (string.Equals(
                        obj.targetEnemyId?.Trim(),
                        enemyId?.Trim(),
                        System.StringComparison.Ordinal))
                {
                    obj.current = Mathf.Min(obj.current + amount, obj.required);
                    anyChanged = true;
                }
            }

            bool allDone = true;
            foreach (var obj in q.objectives)
            {
                if (obj.current < obj.required)
                {
                    allDone = false;
                    break;
                }
            }

            if (allDone)
            {
                q.completed = true;
                anyChanged = true;
            }
        }

        if (!anyChanged) return;

        RefreshList();
        if (selected != null)
            ShowDetail(selected);
    }

    private void SeedDemo()
    {
        quests.Clear();

        quests.Add(new QuestModel
        {
            id = "labour1",
            title = "제1과업",
            description = "무기가 통하지 않는 사자를 동굴로 몰아넣고 질식시켜 처치하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "네메아의 사자 처치(질식)", targetEnemyId = "Nemean Lion", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour2",
            title = "제2과업",
            description = "머리를 자르면 재생한다. 불로 지져 재생을 막아라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "히드라 처치", targetEnemyId = "Lernaean Hydra", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour3",
            title = "제3과업",
            description = "죽이지 말고 지치게 만들어 생포하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "사슴 포획", targetEnemyId = "Ceryneian Hind", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour4",
            title = "제4과업",
            description = "눈 덮인 산으로 몰아 지치게 만든 뒤 포획하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "멧돼지 생포", targetEnemyId = "Erymanthian Boar", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour5",
            title = "제5과업",
            description = "강의 흐름을 바꿔 하루 만에 외양간을 씻어내라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "외양간 과업 완료", targetEnemyId = "Gorgon", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour6",
            title = "제6과업",
            description = "청동 방울로 새를 날려보낸 뒤 격추하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "괴조 처치", targetEnemyId = "Stymphalian Birds", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour7",
            title = "제7과업",
            description = "광폭해진 황소를 죽이지 않고 제압하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "크레타의 황소 제압", targetEnemyId = "Cretan Bull", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour8",
            title = "제8과업",
            description = "폭군을 쓰러뜨리고 식인 말을 제압하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "디오메데스 처치", targetEnemyId = "Mares of Diomedes", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour9",
            title = "제9과업",
            description = "전투에서 승리해 허리띠를 획득하라.(퀘스트 달성 불가로 인한 처리)",
            completed = true,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "히폴리테 전투 승리", targetEnemyId = "Hippolyta", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour10",
            title = "제10과업",
            description = "게리온을 쓰러뜨리고 소 떼를 확보하라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "게리온 처치", targetEnemyId = "Geryon and  Orthrus", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour11",
            title = "제11과업",
            description = "시련을 통과해 황금 사과를 얻어라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "황금 사과 획득", targetEnemyId = "Atlas", current = 0, required = 1 }
            }
        });

        quests.Add(new QuestModel
        {
            id = "labour12",
            title = "제12과업",
            description = "무기 없이 케르베로스를 제압하고 돌아오라.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective { text = "케르베로스 제압", targetEnemyId = "Cerberus", current = 0, required = 1 }
            }
        });
    }
    // ✅ 1~11 과업(=labour1~labour11) 모두 완료했는지
    public bool AreLabours1To11Completed()
    {
        for (int i = 1; i <= 11; i++)
        {
            string id = $"labour{i}";
            var q = quests.Find(x => x.id == id);
            if (q == null || !q.completed)
                return false;
        }
        return true;
    }

    // ✅ (선택) 현재 1~11 완료 개수
    public int GetCompletedLabours1To11Count()
    {
        int count = 0;
        for (int i = 1; i <= 11; i++)
        {
            string id = $"labour{i}";
            var q = quests.Find(x => x.id == id);
            if (q != null && q.completed) count++;
        }
        return count;
    }
}
