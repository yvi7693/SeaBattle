<div align="center">

<img src="docs/screenshots/01_main_menu.png" width="100%" alt="SeaBattle — главное меню">

<br>

<img src="https://img.shields.io/badge/Unity-6000.4.11f1-000000?style=for-the-badge&logo=unity&logoColor=white" alt="Unity">
<img src="https://img.shields.io/badge/Render%20Pipeline-URP-2496ED?style=for-the-badge" alt="URP">
<img src="https://img.shields.io/badge/платформа-macOS%20|%20Windows-6c757d?style=for-the-badge" alt="Platform">
<img src="https://img.shields.io/badge/статус-в%20разработке-orange?style=for-the-badge" alt="Status">

**≈≈≈ Морской бой в 2.5D. Наведи орудия. Потопи флот. ≈≈≈**

</div>

<br>

> **БОЕВОЕ ДОНЕСЕНИЕ**
> Классический «Морской бой», переизданный в 2.5D: два флота на наклонных сетках, снаряды прилетают из-за горизонта, попадания взрываются огнём и дымом, а промахи тонут с тихим всплеском. На каждый удачный или неудачный выстрел живо реагируют персонажи — с репликами и эмоциями прямо на поле боя. Играй с другом за одним экраном — или бросай вызов корабельному ИИ.

<br>

<div align="center">

### 📥 Скачать и играть

[![Скачать билд](https://img.shields.io/github/v/release/yvi7693/SeaBattle?style=for-the-badge&label=%D1%81%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C%20%D0%B1%D0%B8%D0%BB%D0%B4&color=2ea043&logo=github)](https://github.com/yvi7693/SeaBattle/releases/latest)

Готовая сборка для macOS / Windows — без Unity, установки и танцев с бубном.

</div>

<br>

## 📡 Разведданные (скриншоты)

<table>
<tr>
<td width="50%">

**Главное меню**
<img src="docs/screenshots/01_main_menu.png" width="100%">

</td>
<td width="50%">

**Расстановка флота**
<img src="docs/screenshots/02_deploy_ships.png" width="100%">

</td>
</tr>
<tr>
<td width="50%">

**Морской бой**
<img src="docs/screenshots/03_battle_scene.png" width="100%">

</td>
<td width="50%">

**Прямое попадание**
<img src="docs/screenshots/04_battle_hit.png" width="100%">

</td>
</tr>
</table>

<div align="center">
<img src="docs/screenshots/05_battle_miss.png" width="60%" alt="Промах и полёт снаряда">
<br><sub>Промах — снаряд уходит в воду</sub>
</div>

<br>

<div align="center">
<img src="docs/screenshots/06_battle_commentary.png" width="80%" alt="Реплика персонажа после попадания">
<br><sub>💬 Комендант ИИ комментирует прямое попадание — новая система диалоговых реплик</sub>
</div>

<br>

## ⚓ Тактико-технические характеристики

| | |
|---|---|
| 🎮 **Режим «С другом»** | Хот-сит на одном устройстве: игроки поочерёдно расставляют флот и делают ходы |
| 🤖 **Режим «С ИИ»** | Игрок расставляет корабли сам, ИИ разворачивает и наводит огонь автоматически |
| 🧩 **Расстановка** | Ручное перетаскивание/вращение кораблей или мгновенная авторасстановка одной кнопкой |
| 🚀 **Огонь** | Снаряд анимированно летит из-за экрана к выбранной клетке |
| 💥 **Попадание** | Взрыв, огонь и дым на подбитом отсеке корабля |
| 🌊 **Промах** | Всплеск воды в точке падения снаряда |
| 📊 **Учёт флота** | Счётчики уцелевших кораблей (1×/2×/3×/4×-палубные) в реальном времени для обеих сторон |
| 💬 **Реплики персонажей** | После каждого выстрела персонаж стреляющей стороны появляется с портретом и случайной репликой — своя фраза на попадание, своя на промах |
| 🏆 **Победа** | Экран победителя с анимацией и кнопкой мгновенного реванша (**Restart**) |

<br>

## 🗺️ Карта операции (архитектура)

Игровая логика отделена от Unity-слоя по схеме **Model → Controller → Presenter → View** — правила боя тестируемы и не зависят от `MonoBehaviour`.

<details>
<summary><b>Развернуть структуру <code>Assets/Scripts</code></b></summary>

```
Assets/Scripts
├── Model/
│   ├── Models/          Sea, Sector, Ship, Fleet
│   │                     — чистые игровые данные, без зависимостей от Unity
│   └── Controllers/     Staff, Commander, PlansOfficer, Assignee,
│                         TurnRecon, BattleController, HomingWeapon
│                         — игровые правила и наведение ИИ
│
├── Presenter/           BattlePresenter, AiBattlePresenter,
│                         DeployPresenter, SwitchPresenter
│                         — мост между моделью и сценой
│
└── View/
    ├── BattleView/       BoardView, SectorView, ShipBattle
    │                      — 3D-представление поля боя
    ├── DeployView/        DeployBoard, DeploySector, DeployShip, MessagePanel
    ├── Animation/         DOTween: интро, кнопки, снаряды, победа
    ├── CommentaryView, Person
    │                      — портреты персонажей и их реплики на попадание/промах
    │                        (Person — ScriptableObject с иконкой и списком фраз)
    └── GameSession, GameMode, GameUI, MusicPlayer, Restart, WinnerView
```

</details>

**Курс похода (сцены):**

`Intro` → `StartScene` → `DeployScene` → `BattleScene 1` → `WinnerScene`

<br>

## 🔧 Боекомплект (технологии)

| Компонент | Назначение |
|---|---|
| **Unity 6** `6000.4.11f1` | Движок, Universal Render Pipeline (URP) |
| **Input System** | Обработка ввода нового поколения |
| **DOTween** | Анимации интерфейса, снарядов, переходов между экранами |
| **TextMesh Pro** | Весь текст интерфейса |
| Кастомный шейдер моря (`Assets/E_Water`, `Assets/Shader`) | Анимированный океан на фоне |
| Particle System | Огонь, дым, искры, всплески |

<br>

## 🚀 Развёртывание

> Просто поиграть? Скачайте готовый билд по кнопке выше — этот раздел для тех, кто хочет собрать проект из исходников.

```bash
# 1. Клонировать репозиторий
git clone https://github.com/yvi7693/SeaBattle.git

# 2. Открыть Unity Hub → Add → указать папку проекта
```

1. Установите [Unity Hub](https://unity.com/download) и Unity **6000.4.11f1** (или совместимую Unity 6).
2. Откройте проект через Unity Hub.
3. Дождитесь импорта ассетов и откройте сцену `Assets/Scenes/Intro.unity`.
4. Нажмите **▶ Play** — начнётся интро и главное меню.

Для сборки билда сцены уже выставлены в `Build Settings` в боевом порядке (`Intro → StartScene → DeployScene → BattleScene 1 → WinnerScene`).

<br>

## 🕹️ Управление огнём

| Действие | Клавиша/жест |
|---|---|
| Выбрать клетку / кнопку интерфейса | Клик мышью |
| Автоматическая расстановка флота | Кнопка **⤫** на экране расстановки |
| Вернуться на предыдущий экран | Кнопка **Back** |
| Начать бой заново | Кнопка **Restart** на экране победителя |

<br>

<div align="center">

**≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈**

*Занять позиции. Открыть огонь. Потопить врага.*

**≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈**

</div>
