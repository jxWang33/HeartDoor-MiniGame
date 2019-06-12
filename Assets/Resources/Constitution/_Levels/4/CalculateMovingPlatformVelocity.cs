using UnityEngine;

// 移动跳台速度计算 https://www.cnblogs.com/wantnon/p/4457879.html
public class CalculateMovingPlatformVelocity : MonoBehaviour
{
    private Vector2 m_pos;
    private Vector2 m_posF;
    public Vector2 m_velocity;

    void FixedUpdate()
    {
        m_posF = m_pos;
        m_pos = gameObject.transform.position;
        m_velocity = (m_pos - m_posF) / Time.fixedDeltaTime;
    }
}
