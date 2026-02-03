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
    public static QuestUIController Instance { get; private set; }
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

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

    private CursorLockMode _prevLockMode;
    private bool _prevCursorVisible;

    private void Show()
    {
        questRoot.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;

        _prevLockMode = UnityEngine.Cursor.lockState;
        _prevCursorVisible = UnityEngine.Cursor.visible;

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        questRoot?.Focus();
    }

    private void Hide()
    {
        questRoot.style.display = DisplayStyle.None;
        Time.timeScale = 1f;

        UnityEngine.Cursor.lockState = _prevLockMode;
        UnityEngine.Cursor.visible = _prevCursorVisible;
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

            // ✅ (완료) 표시는 클릭을 먹지 않도록 하는 게 안전
            state.style.display = q.completed ? DisplayStyle.Flex : DisplayStyle.None;
            state.pickingMode = PickingMode.Ignore;

            // ✅ 선택 강조: 추가/제거를 항상 명시
            if (selected != null && selected.id == q.id)
                btn.AddToClassList("selected");
            else
                btn.RemoveFromClassList("selected");

            // ✅ 중복 콜백 방지용: Refresh로 재생성되긴 하지만 습관적으로 안전장치
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

        // ✅ StopPropagation 대신 기본 동작만 막기(입력 꼬임 방지)
        e.PreventDefault();
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
    public void NotifyEnemyDefeated(string enemyId, int amount = 1)
    {
        if (string.IsNullOrEmpty(enemyId)) return;

        bool anyChanged = false;

        foreach (var q in quests)
        {
            if (q.completed) continue;

            foreach (var obj in q.objectives)
            {
                // targetEnemyId가 비어있으면 “특정 적” 목표가 아닌 것으로 취급 (원하면 반대로 바꿔도 됨)
                if (string.IsNullOrEmpty(obj.targetEnemyId)) continue;

                if (obj.targetEnemyId == enemyId)
                {
                    obj.current = Mathf.Min(obj.current + amount, obj.required);
                    anyChanged = true;
                }
            }

            // 퀘스트 완료 판정: 모든 objective가 required 충족이면 완료
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
        quests.Add(new QuestModel {
            id = "q1",
            title = "기초 훈련",
            description = "앞에 있는 몬스터를 처치하고 보상을 획득하자.",
            completed = false,
            objectives = new List<QuestObjective>
            {
                new QuestObjective
                {
                    text = "Lernaean Hydra 처치하기",
                    targetEnemyId = "Lernaean Hydra",
                    current = 0,
                    required = 1
                }
            }
        });
    }
}
