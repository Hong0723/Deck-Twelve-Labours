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

        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        questRoot = root.Q<VisualElement>("quest-root");
        closeBtn = root.Q<Button>("quest-close-btn");
        questList = root.Q<ScrollView>("quest-list");

        // Detail side: quest-right에 name을 부여했기 때문에 name으로 안전하게 찾음
        questRight = root.Q<VisualElement>("quest-right");
        questTitle = root.Q<Label>("quest-title");
        questDesc = root.Q<Label>("quest-desc");
        objectivesTitle = root.Q<Label>("objectives-title");
        objectivesRoot = root.Q<VisualElement>("objectives-root");

        // ScrollView 설정
        questList.verticalScrollerVisibility = ScrollerVisibility.Hidden;
        questList.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        questList.contentContainer.style.flexDirection = FlexDirection.Column;
        questList.RegisterCallback<WheelEvent>(OnQuestListWheel, TrickleDown.TrickleDown);

        closeBtn.clicked += Hide;

        // 초기: 상세 영역 숨김 + 섹션 타이틀 문구 설정
        if (objectivesTitle != null) objectivesTitle.text = "퀘스트 목표";
        HideDetail();

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

        // ✅ 더블클릭 이슈 해결: Root에 포커스
        questRoot.Focus();

        // 선택이 없으면 상세 숨김 유지
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

        bool anyChanged = false;

        foreach (var q in quests)
        {
            if (q.completed) continue;

            foreach (var obj in q.objectives)
            {
                if (string.IsNullOrEmpty(obj.targetEnemyId)) continue;

                if (obj.targetEnemyId == enemyId)
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

        for (int i = 1; i <= 12; i++)
        {
            quests.Add(new QuestModel
            {
                id = $"chapter{i}",
                title = $"Chapter{i}.",
                description = $"스테이지 {i}를 클리어하기 위해 적을 처치하자.",
                completed = false,
                objectives = new List<QuestObjective>
                {
                    new QuestObjective
                    {
                        text = $"Test{i} 처치",
                        targetEnemyId = $"Test{i}",
                        current = 0,
                        required = 1
                    }
                }
            });
        }
    }

}
