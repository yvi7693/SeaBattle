<div align="center">

<img src="docs/banner.png" width="100%" alt="SeaBattle">

<br>
<br>

<img src="https://img.shields.io/badge/Unity-6000.4.11f1-000000?style=flat-square&logo=unity&logoColor=white" alt="Unity">
<img src="https://img.shields.io/badge/C%23-.NET%20Standard%202.1-512BD4?style=flat-square&logo=csharp&logoColor=white" alt="C#">
<img src="https://img.shields.io/badge/render-URP%2017.4-2496ED?style=flat-square" alt="URP">
<img src="https://img.shields.io/badge/tests-234%20passing-2ea043?style=flat-square" alt="Tests">
<img src="https://img.shields.io/badge/coverage-93.7%25%20lines-2ea043?style=flat-square" alt="Coverage">
<img src="https://img.shields.io/badge/license-MIT-blue?style=flat-square" alt="License">
<img src="https://img.shields.io/badge/status-%D0%B7%D0%B0%D0%B2%D0%B5%D1%80%D1%88%D1%91%D0%BD-2ea043?style=flat-square" alt="Status">

<br>
<br>

**Классический «Морской бой» в 2.5D — на Unity 6, с чистой игровой моделью и ИИ-противником.**

[**Скачать и играть**](#-скачать-и-играть) · [Архитектура](#-архитектура) · [ИИ противника](#-ии-противника) · [Тесты](#-тестирование) · [Сборка](#-сборка-из-исходников)

</div>

---

## О проекте

Два флота на наклонных 3D-сетках, снаряды прилетают из-за горизонта, попадания взрываются огнём и дымом, промахи тонут с всплеском. После каждого выстрела на поле появляется персонаж стреляющей стороны и комментирует результат. Играть можно вдвоём за одним экраном или против корабельного ИИ.

Вся боевая логика — расстановка, валидация, выстрелы, добивание, определение победителя — живёт в обычных C#-классах без единой ссылки на UnityEngine и покрыта 234 модульными тестами. Unity-слой отвечает только за отрисовку и ввод.

<div align="center">

| | |
|:---|:---|
| **Игровая логика** | ~40 C#-классов, ядро отделено от Unity |
| **Модульных тестов** | 234 (NUnit, без Unity-зависимостей) |
| **Покрытие ядра** | 93.7% строк, 97.8% методов |
| **Unity-зависимостей в модели** | 0 |
| **Сцен** | 6, полный игровой цикл с рестартом |

</div>

<br>

## 📥 Скачать и играть

<div align="center">

[![Скачать билд](https://img.shields.io/github/v/release/yvi7693/SeaBattle?style=for-the-badge&label=%D1%81%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C%20%D0%B1%D0%B8%D0%BB%D0%B4&color=2ea043&logo=github)](https://github.com/yvi7693/SeaBattle/releases/latest)

Готовая сборка для **Windows** — Unity ставить не нужно.

</div>

<br>

## ⚓ Что умеет игра

| | |
|---|---|
| 🎮 **Режим «С другом»** | Хот-сит на одном устройстве: игроки поочерёдно расставляют флот и делают ходы |
| 🤖 **Режим «С ИИ»** | Игрок расставляет корабли сам, ИИ разворачивает флот и наводит огонь автоматически |
| 🧩 **Расстановка** | Перетаскивание и вращение кораблей мышью или мгновенная авторасстановка одной кнопкой |
| ✅ **Валидация флота** | Корабли не могут пересекаться и касаться друг друга — правила проверяются в модели, а не в UI |
| 🎯 **Подсветка секторов** | Клетка под курсором подсвечивается — видно, куда уйдёт снаряд |
| 🚀 **Огонь** | Снаряд анимированно летит из-за экрана к выбранной клетке |
| 💥 **Попадание** | Взрыв, огонь и дым на подбитом отсеке; ход остаётся у стрелявшего |
| 🌊 **Промах** | Всплеск воды и передача хода противнику |
| 🚢 **Добивание** | При уничтожении корабля вся акватория вокруг него автоматически помечается как пустая |
| 📊 **Учёт флота** | Счётчики уцелевших 1/2/3/4-палубных кораблей в реальном времени для обеих сторон |
| 💬 **Реплики персонажей** | После выстрела персонаж появляется с портретом и случайной фразой — своя на попадание, своя на промах |
| 🏆 **Победа** | Отдельная сцена победителя с анимацией и кнопкой мгновенного реванша |

<br>

<details>
<summary><b>📸 Скриншоты</b></summary>

<br>

<table>
<tr>
<td width="50%"><b>Главное меню</b><br>Выбор противника — второй игрок или ИИ<br><br><img src="docs/screenshots/01_main_menu.png" width="100%"></td>
<td width="50%"><b>Расстановка флота</b><br>Корабли ставятся мышью или кнопкой авторасстановки<br><br><img src="docs/screenshots/02_deploy_ships.png" width="100%"></td>
</tr>
<tr>
<td width="50%"><b>Бой</b><br>Слева поле игрока, справа поле противника. Горящий корабль — попадание, светлая клетка — промах<br><br><img src="docs/screenshots/03_battle_hit.png" width="100%"></td>
<td width="50%"><b>Реплика персонажа</b><br>Капитан комментирует промах; красная рамка — клетка под курсором<br><br><img src="docs/screenshots/04_battle_commentary.png" width="100%"></td>
</tr>
<tr>
<td width="50%"><b>Концовка партии</b><br>Флоты почти уничтожены, снаряд в полёте оставляет след<br><br><img src="docs/screenshots/05_battle_endgame.png" width="100%"></td>
<td width="50%"><b>Экран победителя</b><br>Итог партии и кнопка мгновенного реванша<br><br><img src="docs/screenshots/06_winner.png" width="100%"></td>
</tr>
</table>

</details>

<br>

## 🗺️ Архитектура

Граница в проекте одна — между **Unity-слоем** и **игровым ядром**. Ядро (`Assets/Scripts/Model`) не зависит от Unity, поэтому правила боя запускаются и тестируются без Editor и Play Mode.

```
┌──────────────────────────────────────────────────────────┐
│  UNITY-СЛОЙ                                              │
│                                                          │
│  View        MonoBehaviour: клики, 3D-сетки, анимации    │
│              BoardView · SectorView · DeployBoard        │
│                            ↕                             │
│  Presenter   мост Unity ⇄ ядро                           │
│              BattlePresenter · AiBattlePresenter         │
│              DeployPresenter · SwitchPresenter           │
└────────────────────────┬─────────────────────────────────┘
                         │  TacticalDirective(x, y) → MissionResult
                         │  единственный вход в ядро
┌────────────────────────┴─────────────────────────────────┐
│  ЯДРО                                                    │
│                                                          │
│  Controllers правила боя и наведение ИИ                  │
│              Staff → Commander → PlansOfficer            │
│                    → Assignee → TurnRecon                │
│              BattleController · HomingWeapon             │
│                            ↕                             │
│  Models      данные и их инварианты                      │
│              Sea · Sector · Ship · Fleet                 │
└──────────────────────────────────────────────────────────┘
```

### Как проходит один выстрел

`Staff` — единственная точка входа в модель. Presenter вызывает `TacticalDirective(x, y)` и получает обратно `MissionResult`, по которому решает, что проиграть на сцене:

```csharp
MissionResult result = staff.TacticalDirective(x, y);

// Hit            → взрыв, ход остаётся у стрелявшего
// Miss           → всплеск, TurnRecon уже переключил цель на другое море
// HaveWinner     → переход на WinnerScene
// UnsucessfulShot→ клетка уже обстреляна, ход не засчитан
```

Внутри цепочка ответственностей выстроена как штаб корабля — каждый класс делает ровно одно:

| Класс | Зона ответственности |
|---|---|
| `Staff` | Фасад модели: собирает граф зависимостей и отдаёт наружу две команды — расстановка и выстрел |
| `Commander` | Оркестрация хода: разрешение → атака → проверка победы → передача хода |
| `PlansOfficer` | Валидация приказа: существует ли клетка, не обстреляна ли уже, корректна ли расстановка |
| `Assignee` | Исполнение: меняет статус сектора, наносит урон кораблю, топит его |
| `TurnRecon` | Чьё море сейчас атакующее, а чьё — целевое |
| `BattleController` | Состояние партии: два флота, поиск корабля по палубности, объявление победителя |
| `DeploymentOfficer` | Геометрия расстановки: пересечения, касания, нормализация координат |
| `AttackResolver` / `Sinker` | Разрешение попадания и затопление корабля с обводкой соседних клеток |

<details>
<summary><b>Развернуть структуру <code>Assets/Scripts</code></b></summary>

```
Assets/Scripts
├── GameAssembly.asmdef      отдельная сборка игровой логики
│
├── Model/
│   ├── Models/              Sea, Sector, Ship, Fleet
│   │                        — чистые игровые данные
│   └── Controllers/         Staff, Commander, PlansOfficer, Assignee,
│                            TurnRecon, BattleController, HomingWeapon,
│                            Services (DeploymentOfficer, AttackResolver, Sinker)
│                            — правила боя и наведение ИИ
│
├── Presenter/               BattlePresenter, AiBattlePresenter,
│                            DeployPresenter, SwitchPresenter
│
└── View/
    ├── BattleView/          BoardView, SectorView, ShipBattle
    ├── DeployView/          DeployBoard, DeploySector, DeployShip, MessagePanel
    ├── Animation/           DOTween: интро, кнопки, снаряды, загрузка, победа
    │                        + CommentaryView — выезд портрета с репликой
    ├── Person.cs            ScriptableObject: иконка + списки фраз на хит/промах
    └── GameSession, GameMode, GameUI, MusicPlayer, Restart, WinnerView
```

</details>

### Курс похода (сцены)

```mermaid
flowchart LR
    A[Intro] --> B[StartScene]
    B -->|С другом / С ИИ| C[DeployScene]
    C --> D[LoadScene]
    D -->|async| E[BattleScene]
    E --> F[WinnerScene]
    F -->|Restart| B
```

`LoadScene` подгружает боевую сцену через `LoadSceneAsync`, чтобы переход не подвисал на слабых машинах.

<br>

## 🤖 ИИ противника

ИИ живёт в классе `HomingWeapon` и работает как система наведения, а не как случайный генератор. Состояние — три сектора: последний выстрел, последнее попадание и предыдущее попадание.

1. **Поиск.** Пока попаданий нет — случайный выстрел по необстрелянной клетке.
2. **Захват цели.** После попадания ИИ переключается на соседние клетки вокруг подбитого отсека.
3. **Добивание по линии.** Второе попадание задаёт ось корабля — дальше ИИ бьёт вдоль неё, а упёршись в промах, возвращается к первому попаданию и идёт в обратную сторону. Это то, что позволяет корректно топить 3- и 4-палубные корабли.
4. **Сброс.** Корабль потоплен → `Sinker` помечает воду вокруг него, ИИ теряет цель и возвращается к поиску.

Расстановку своего флота ИИ выполняет тем же валидатором `DeploymentOfficer`, что и игрок, — никаких привилегий у него нет.

<br>

## 🧪 Тестирование

**234 теста** на Unity Test Framework (NUnit), Edit Mode — покрыто игровое ядро `Assets/Scripts/Model`.

<div align="center">

| | |
|:---|:---|
| **Тестов** | 234 · все проходят |
| **Покрытие строк** | **93.7%** (402 из 429) |
| **Покрытие методов** | **97.8%** (89 из 91) |
| **Время прогона** | 0.14 с |

</div>

Цифры — из отчёта Unity Code Coverage, а не из ручного подсчёта. Полный отчёт по классам и разбор непокрытых строк: [`docs/coverage/Summary.md`](docs/coverage/Summary.md).

**100% строк и методов** — `Sector`, `Ship`, `Fleet`, `BattleController`, `TurnRecon`, `Staff`, `Commander`, `Assignee`, `AttackResolver`, `Sinker` (10 из 14 классов).

Остальные четыре:

| Класс | Строки | Методы |
|---|---:|---:|
| `DeploymentOfficer` | 98.1% | 100% |
| `Sea` | 97.6% | 91.6% |
| `PlansOfficer` | 86.7% | 100% |
| `HomingWeapon` | 77.5% | 91.6% |

Непокрытыми остались 27 строк — редкие ветки ИИ-наведения в `HomingWeapon` и откат неудачной расстановки в `PlansOfficer`. Причина в том, что `HomingWeapon` использует несидированный `System.Random`: детерминированно загнать алгоритм в конкретную ветку добивания из теста нельзя. Это ограничение дизайна, а не забытые тесты.

**Прогнать самому**

```bash
# Через Unity Editor
Window → General → Test Runner → EditMode → Run All
```

```bash
# Из командной строки (macOS) — тесты + отчёт о покрытии
/Applications/Unity/Hub/Editor/6000.4.11f1/Unity.app/Contents/MacOS/Unity \
  -batchmode -nographics -projectPath . \
  -runTests -testPlatform EditMode -testResults ./TestResults.xml \
  -enableCodeCoverage -coverageResultsPath ./CodeCoverage \
  -coverageOptions "generateHtmlReport;assemblyFilters:+GameAssembly;pathFilters:+**/Assets/Scripts/Model/**"
```

> HTML-отчёт появится в `CodeCoverage/Report/index.html`. Флаг `-quit` добавлять нельзя — Unity выйдет до старта тестов.

| Файл | Что проверяет |
|---|---|
| `SeaTests` · `SectorTests` | Сетка 10×10, статусы клеток, поиск соседей |
| `ShipTests` · `FleetTests` | Палубность, урон, затопление, состав флота (4×1, 3×2, 2×3, 1×4) |
| `ServicesTests` | Валидация расстановки: пересечения, касания, выход за границы |
| `StaffTests` · `CommanderTests` | Полный сценарий хода и переключение сторон |
| `PlansOfficerTests` · `AssigneeTests` | Разрешение и исполнение приказа |
| `TurnReconTests` · `BattleControllerTests` | Смена хода, объявление победителя |
| `HomingWeaponTests` | Алгоритм наведения ИИ |

<br>

## 🔧 Технологии

| Компонент | Роль |
|---|---|
| **Unity 6** `6000.4.11f1` | Движок, Universal Render Pipeline 17.4 |
| **C#** / .NET Standard 2.1 | Игровая логика в отдельной сборке `GameAssembly.asmdef` |
| **Input System** `1.19` | Обработка ввода |
| **DOTween** | Анимации интерфейса, снарядов, персонажей, переходов |
| **TextMesh Pro** | Весь текст интерфейса |
| **Unity Test Framework** `1.6` | NUnit-тесты игровой логики |
| **Unity Code Coverage** `1.2` | Замер покрытия ядра тестами |
| Кастомный шейдер моря | Анимированный океан на фоне (`Assets/Shader`) |
| Particle System | Огонь, дым, искры, всплески |
| **Trail Renderer** | Дымный след снаряда в полёте (`Assets/Prefabs/missile.prefab`) |

<br>

## 🚀 Сборка из исходников

> Просто поиграть — берите [готовый билд](https://github.com/yvi7693/SeaBattle/releases/latest). Этот раздел для тех, кто хочет открыть проект в редакторе.

**Требуется:** [Unity Hub](https://unity.com/download) и Unity **6000.4.11f1** (или совместимая Unity 6) с модулем сборки под вашу платформу.

```bash
git clone https://github.com/yvi7693/SeaBattle.git
cd SeaBattle
```

1. Unity Hub → **Add** → указать папку проекта.
2. Дождаться импорта ассетов (первый раз — несколько минут).
3. Открыть `Assets/Scenes/Intro.unity` и нажать **▶ Play**.

Сцены уже расставлены в `Build Settings` в боевом порядке, так что **File → Build Settings → Build** собирает игру без дополнительной настройки.

<br>

## 🕹️ Управление

| Действие | Как |
|---|---|
| Выбрать клетку / кнопку интерфейса | Клик мышью |
| Повернуть корабль при расстановке | Клик по кораблю |
| Автоматическая расстановка флота | Кнопка **⤫** на экране расстановки |
| Вернуться на предыдущий экран | Кнопка **Back** |
| Начать заново | Кнопка **Restart** на экране победителя |

<br>

## 📁 Структура репозитория

```
SeaBattle/
├── Assets/
│   ├── Scripts/         игровая логика (см. раздел «Архитектура»)
│   ├── Tests/           234 NUnit-теста модели
│   ├── Scenes/          Intro, StartScene, DeployScene, LoadScene,
│   │                    BattleScene, WinnerScene
│   ├── Models/          3D-модели кораблей
│   ├── Prefabs/         префабы полей, секторов, снарядов, UI
│   ├── Sprites/         иконки, кнопки, портреты персонажей
│   ├── Shader/ E_Water/ шейдер и материалы моря
│   ├── FireAssets/      частицы огня, дыма, всплесков
│   ├── Audio/           музыка и звуковой микшер
│   └── Plugins/         DOTween
├── docs/screenshots/    скриншоты для README
├── docs/coverage/       отчёт о покрытии тестами
├── Packages/            манифест пакетов Unity
└── ProjectSettings/     настройки проекта и Build Settings
```

<br>

## 📄 Лицензия

[MIT](LICENSE) — можно свободно использовать, изменять и распространять.

Музыка в `Assets/Audio` используется в учебных целях и права на неё принадлежат их авторам.

<br>

## 👨‍💻 Авторы

<p align="center">
  <b>Product by</b><br>
  <a href="https://github.com/yvi7693">Yaroslav Volkov</a>
</p>

<p align="center">
  <b>Under the leadership of</b><br>
  <a href="https://github.com/dante-pol">Dmitry Rak</a>
</p>

<br>

<div align="center">

**≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈**

*Занять позиции. Открыть огонь. Потопить врага.*

**≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈**

</div>
