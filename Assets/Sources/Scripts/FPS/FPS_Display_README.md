# FPS Display для Unity Android

Этот скрипт отображает FPS (кадры в секунду) в верхнем левом углу экрана в билдах Unity для Android.

## Установка и использование

### 1. Добавление скрипта в проект
1. Скопируйте файл `FPSDisplay.cs` в папку `Assets/Scripts/` вашего Unity проекта
2. Если папки `Scripts` нет, создайте её

### 2. Настройка в сцене
1. Создайте пустой GameObject в сцене (GameObject → Create Empty)
2. Переименуйте его в "FPS Display"
3. Добавьте компонент FPSDisplay к этому объекту:
   - Выберите GameObject
   - В Inspector нажмите "Add Component"
   - Найдите и выберите "FPS Display"

### 3. Настройка параметров
В Inspector вы можете настроить:
- **Show FPS**: Включить/выключить отображение FPS
- **Text Color**: Цвет текста (по умолчанию белый)
- **Font Size**: Размер шрифта (по умолчанию 24)
- **Position**: Позиция на экране (по умолчанию верхний левый угол)

### 4. Сборка для Android
1. Откройте Build Settings (File → Build Settings)
2. Выберите платформу Android
3. Добавьте текущую сцену в Build
4. Нажмите "Build" или "Build and Run"

## Особенности

- **Работает в билдах**: Скрипт оптимизирован для работы в финальных билдах на Android
- **Снимает ограничения FPS**: Автоматически устанавливает максимальную частоту кадров (targetFrameRate = -1) и отключает VSync
- **Сохраняется между сценами**: Объект не уничтожается при переходе между сценами
- **Тень текста**: Добавлена тень для лучшей читаемости на любом фоне
- **Настраиваемый**: Все параметры можно изменить через Inspector или код

## Управление из кода

Вы можете управлять FPS Display из других скриптов:

```csharp
// Найти компонент FPS Display
FPSDisplay fpsDisplay = FindObjectOfType<FPSDisplay>();

// Показать/скрыть FPS
fpsDisplay.SetFPSVisibility(true);

// Переключить видимость
fpsDisplay.ToggleFPS();

// Изменить цвет текста
fpsDisplay.SetTextColor(Color.red);

// Изменить размер шрифта
fpsDisplay.SetFontSize(30);

// Изменить позицию
fpsDisplay.SetPosition(new Vector2(100, 50));
```

## Производительность

Скрипт использует минимальные ресурсы:
- Обновление FPS происходит с сглаживанием
- OnGUI вызывается только при необходимости
- Нет создания новых объектов в Update()

## Совместимость

- Unity 2019.4 и выше
- Android API Level 21+
- Работает на всех разрешениях экрана
