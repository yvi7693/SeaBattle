# Покрытие кода тестами — SeaBattle

> Сгенерировано автоматически: Unity Code Coverage 1.2.6 (ReportGenerator).
> Воспроизвести: см. раздел «Тестирование» в [README](../../README.md).

## Итог
|||
|:---|:---|
| Generated on: | 8/1/2026 - 12:28:33 AM |
| Parser: | MultiReportParser (2x OpenCoverParser) |
| Assemblies: | 1 |
| Classes: | 14 |
| Files: | 12 |
| Covered lines: | 402 |
| Uncovered lines: | 27 |
| Coverable lines: | 429 |
| Total lines: | 1191 |
| Line coverage: | 93.7% (402 of 429) |
| Covered branches: | 0 |
| Total branches: | 0 |
| Covered methods: | 89 |
| Total methods: | 91 |
| Method coverage: | 97.8% (89 of 91) |

|**Name**|**Covered**|**Uncovered**|**Coverable**|**Total**|**Line coverage**|**Covered**|**Total**|**Branch coverage**|**Covered**|**Total**|**Method coverage**|
|:---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
|**GameAssembly**|**402**|**27**|**429**|**1563**|**93.7%**|**0**|**0**|****|**89**|**91**|**97.8%**|
|Assignee|18|0|18|43|100%|0|0||3|3|100%|
|AttackResolver|6|0|6|186|100%|0|0||1|1|100%|
|BattleController|21|0|21|67|100%|0|0||7|7|100%|
|Commander|16|0|16|45|100%|0|0||2|2|100%|
|DeploymentOfficer|53|1|54|186|98.1%|0|0||6|6|100%|
|Fleet|31|0|31|84|100%|0|0||7|7|100%|
|HomingWeapon|62|18|80|206|77.5%|0|0||11|12|91.6%|
|PlansOfficer|46|7|53|120|86.7%|0|0||4|4|100%|
|Sea|41|1|42|129|97.6%|0|0||11|12|91.6%|
|Sector|23|0|23|77|100%|0|0||11|11|100%|
|Ship|32|0|32|92|100%|0|0||9|9|100%|
|Sinker|10|0|10|186|100%|0|0||2|2|100%|
|Staff|23|0|23|78|100%|0|0||8|8|100%|
|TurnRecon|20|0|20|64|100%|0|0||7|7|100%|

## Что осталось непокрытым (27 строк)

| Файл | Строки | Что это |
|---|---|---|
| `Model/Controllers/HomingWeapon.cs` | 42, 66–67, 72–73, 133–134, 138–147, 172–175, 189 | Редкие ветки ИИ-наведения: добивание корабля с другого конца (`LinearAttack(step: 2)`), сброс памяти после потери цели, вертикальная линия добивания, `throw` при неколлинеарных координатах |
| `Model/Controllers/PlansOfficer.cs` | 94–95, 104–107, 117 | Откат расстановки при неудачном размещении и `throw` по исчерпании лимита перезапусков |
| `Model/Controllers/Services.cs` | 97 | Ранний `return false` в `ValidateEqualSectors` для вертикальной линии |
| `Model/Models/Sea.cs` | 45 | `GetSize()` — геттер, не вызывается тестами напрямую |

Непокрытые методы (2 из 91): `HomingWeapon.DropMemory()`, `Sea.GetSize()`.

Общая причина: `HomingWeapon` использует несидированный `System.Random`, поэтому загнать
алгоритм наведения в конкретную редкую ветку детерминированно нельзя. Это известное
ограничение текущего дизайна, а не пропуск в тестах.
