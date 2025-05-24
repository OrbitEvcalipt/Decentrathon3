using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [Header("FPS Display Settings")]
    [SerializeField] private bool showFPS = true;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private int fontSize = 24;
    [SerializeField] private Vector2 position = new Vector2(10, 10);
    
    private float deltaTime = 0.0f;
    private GUIStyle style;
    private Rect rect;
    private string fpsText = "";
    
    void Start()
    {
        // Убираем ограничение на FPS
        Application.targetFrameRate = -1;
        QualitySettings.vSyncCount = 0;
        
        // Инициализация стиля для текста
        style = new GUIStyle();
        
        // Устанавливаем позицию и размер прямоугольника для текста
        rect = new Rect(position.x, position.y, 200, 50);
        
        // Делаем объект неуничтожимым при загрузке новых сцен
        DontDestroyOnLoad(gameObject);
    }
    
    void Update()
    {
        if (!showFPS) return;
        
        // Вычисляем deltaTime для расчета FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }
    
    void OnGUI()
    {
        if (!showFPS) return;
        
        // Настройка стиля текста
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = fontSize;
        style.normal.textColor = textColor;
        
        // Вычисляем FPS
        float fps = 1.0f / deltaTime;
        
        // Форматируем текст с FPS
        fpsText = string.Format("FPS: {0:0.}", fps);
        
        // Добавляем тень для лучшей читаемости
        GUI.color = Color.black;
        GUI.Label(new Rect(rect.x + 1, rect.y + 1, rect.width, rect.height), fpsText, style);
        
        // Основной текст
        GUI.color = textColor;
        GUI.Label(rect, fpsText, style);
        
        // Сбрасываем цвет GUI
        GUI.color = Color.white;
    }
    
    // Методы для управления отображением FPS из кода
    public void SetFPSVisibility(bool visible)
    {
        showFPS = visible;
    }
    
    public void ToggleFPS()
    {
        showFPS = !showFPS;
    }
    
    public void SetTextColor(Color color)
    {
        textColor = color;
    }
    
    public void SetFontSize(int size)
    {
        fontSize = size;
    }
    
    public void SetPosition(Vector2 newPosition)
    {
        position = newPosition;
        rect = new Rect(position.x, position.y, 200, 50);
    }
}
