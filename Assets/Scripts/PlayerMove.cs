using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private IEnumerator Start()
    {
        // 保留你在場景中調整好的起始高度
        float groundY = transform.position.y;
        float z = transform.position.z;

        // Vector3 陣列：依序存放人物要抵達的位置
        Vector3[] points =
        {
            new Vector3(-3f, groundY, z),       // 走到起跳位置
            new Vector3(-0.6f, groundY + 2f, z), // 跳上高台
            new Vector3(0.6f, groundY + 2f, z),  // 在高台往前走
            new Vector3(3f, groundY, z),        // 跳下高台
            new Vector3(6f, groundY, z)         // 走到終點
        };

        // 每段移動的跳躍高度；0 表示走路
        float[] jumpHeights = { 0f, 2f, 0f, 1f, 0f };

        // 每段移動花費的秒數
        float[] durations = { 1.5f, 1.2f, 0.8f, 1.2f, 1.5f };

        // 開始前停留一秒
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < points.Length; i++)
        {
            yield return MoveTo(
                points[i],
                durations[i],
                jumpHeights[i]
            );
        }

        Debug.Log("已到達終點！");
    }

    private IEnumerator MoveTo(
        Vector3 target,
        float duration,
        float jumpHeight)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 計算起點到終點之間的位置
            Vector3 position = Vector3.Lerp(start, target, t);

            // 加上弧形高度：起點與終點為 0，中間最高
            position.y += 4f * jumpHeight * t * (1f - t);

            transform.position = position;

            // 等到下一個影格再繼續
            yield return null;
        }

        // 確保最後準確抵達目標
        transform.position = target;
    }
}