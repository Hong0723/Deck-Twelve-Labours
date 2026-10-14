# Deck: Twelve Labours
> 헤라클레스의 12과업에서 영감을 받은, 탐험과 턴 기반 카드 전투를 결합한 Unity 2D 게임.

**[게임 다운로드 · itch.io](https://hong0723.itch.io/deck-twelve-labours)** · **[게임 소스 · develop](https://github.com/Hong0723/Deck-Twelve-Labours/tree/develop)**

## 프로젝트 소개
필드를 탐험하며 적과 만나 카드 전투를 진행하고, 전투 보상과 아이템으로 다음 전투를 준비합니다. 이동·대화·퀘스트가 있는 탐험 구간과 카드·마나·적 행동을 판단하는 전투 구간을 하나의 플레이 흐름으로 연결했습니다.

| 항목 | 내용 |
| --- | --- |
| 장르 | 2D 탐험 · 카드 전투 |
| 개발 환경 | Unity 6000.2.6f2, C# |
| 주요 기술 | Unity 2D, 코루틴, DOTween, 액션·반응 기반 전투 처리 |
| 프로젝트 형태 | 팀 프로젝트 |
| 담당 역할 | PM, 맵·레벨 제작 |

## 담당 역할
PM으로 개발 범위와 일정을 관리하고 맵·레벨 제작을 담당했습니다. 팀의 작업을 취합하고 Unity 씬 충돌을 해결하며 배포까지 프로젝트를 진행했습니다. 아래 전투·시스템 설명은 팀 프로젝트의 전체 구현을 소개합니다.

## 플레이 흐름
**필드 탐험 → 적과 조우 → 카드 전투 → 보상 선택 → 탐험 복귀**

- 필드에서 이동하고 대화·상호작용을 통해 과업을 진행합니다.
- 전투에서는 손패와 마나를 확인해 카드를 사용하고, 적의 행동에 대응합니다.
- 전투 종료 후 보상을 선택하고 인벤토리를 관리합니다.
- 탐험과 전투 사이에서 플레이어 상태와 진행 정보를 연결합니다.

## 주요 구현

### 탐험과 씬 이동
필드 이동과 애니메이션을 연결하고, 포털을 통한 씬 전환에 페이드 효과와 입력 잠금을 적용합니다. 목적지의 스폰 지점을 지정해 이동 후 위치를 결정하며, 게임 세션과 월드 상태를 통해 씬 사이의 진행 정보를 관리합니다.

**관련 코드:** [player_set.cs](https://github.com/Hong0723/Deck-Twelve-Labours/blob/develop/Assets/Script/Player/player_set.cs) · [PortalTransition.cs](https://github.com/Hong0723/Deck-Twelve-Labours/blob/develop/Assets/Script/Portal/PortalTransition.cs)

### 카드 전투와 액션 처리
- 드로우 더미·손패·버림 더미를 관리하고 드로우, 카드 사용, 버리기를 처리합니다.
- 플레이어 턴·적 턴·승리·패배 상태에 따라 전투를 진행합니다.
- 액션의 전·실행·후 단계와 반응을 분리해 연쇄 효과를 순서대로 처리합니다.
- 마나, 대상 선택, 카드 효과를 연결하고 DOTween으로 카드 이동을 표현합니다.

**관련 코드:** [CardSystem.cs](https://github.com/Hong0723/Deck-Twelve-Labours/blob/develop/Assets/Script/Battle%20Scene/CardSystem.cs) · [BattleManager.cs](https://github.com/Hong0723/Deck-Twelve-Labours/blob/develop/Assets/Script/Battle%20Scene/BattleManager.cs) · [ActionSystem.cs](https://github.com/Hong0723/Deck-Twelve-Labours/blob/develop/Assets/Script/ACTION-REACTION%20System/ActionSystem.cs)

### 보상과 재시작
전투 보상은 아이템의 가중치를 이용해 선택지를 생성하고 인벤토리로 전달합니다. 새 게임을 시작할 때는 플레이어·월드 상태와 남아 있는 트윈을 정리해 이전 세션의 상태가 다음 플레이에 영향을 주지 않도록 구성했습니다.

## 조작 방법
| 입력 | 기능 |
| --- | --- |
| W / A / S / D | 이동 |
| Space | 대화·UI 진행 |
| I | 인벤토리 |
| 카드 드래그 | 카드 사용 |
| Esc | 메뉴 |

## 실행 방법
**플레이:** itch.io에서 배포 파일을 내려받아 실행합니다.

**소스 확인:** 게임 프로젝트는 **`develop` 브랜치**에 있습니다. `main`은 프로젝트 소개용 README를 제공합니다.

```bash
git clone --branch develop https://github.com/Hong0723/Deck-Twelve-Labours.git
```

Unity Hub에 저장소 루트를 추가하고 **Unity 6000.2.6f2**로 엽니다. 패키지를 복원한 뒤 시작 씬을 실행합니다.

## 코드 탐색
모든 링크는 실제 게임 코드가 있는 `develop` 브랜치를 가리킵니다.

- [전투 시스템](https://github.com/Hong0723/Deck-Twelve-Labours/tree/develop/Assets/Script/Battle%20Scene)
- [액션·반응 시스템](https://github.com/Hong0723/Deck-Twelve-Labours/tree/develop/Assets/Script/ACTION-REACTION%20System)
- [플레이어](https://github.com/Hong0723/Deck-Twelve-Labours/tree/develop/Assets/Script/Player)
- [포털·씬 이동](https://github.com/Hong0723/Deck-Twelve-Labours/tree/develop/Assets/Script/Portal)
- [전체 스크립트](https://github.com/Hong0723/Deck-Twelve-Labours/tree/develop/Assets/Script)

## 개발에서 다룬 문제
팀 작업 과정에서 Unity 씬 충돌을 병합하고 작업을 통합했습니다. 맵·레벨 제작과 전투 시스템이 따로 완성되는 데 그치지 않도록 탐험에서 전투로 진입하고 다시 돌아오는 흐름을 중심으로 통합 작업을 진행했습니다.

프로젝트에 포함된 외부 에셋 및 라이브러리의 권리는 각 제작자에게 있습니다.

