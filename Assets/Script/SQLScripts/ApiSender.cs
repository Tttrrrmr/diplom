
using UnityEngine;

public static class ApiSender
{
    public static void SendResult(int score, int total)
    {
        Debug.Log($"Отправляем результат: {score}/{total}");
        // Здесь можно подключить реальный API POST запрос
    }
}
