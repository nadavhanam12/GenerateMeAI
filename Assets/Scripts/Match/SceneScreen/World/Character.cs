using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float m_speed = 0.1f;
    PlayerDetails m_playerDetails;
    Transform m_transform;
    float[] m_positionsArray;
    int m_curPosition;

    internal void Init(PlayerDetails playerDetails, float[] positionsArray)
    {
        m_playerDetails = playerDetails;
        m_curPosition = 1;
        m_positionsArray = positionsArray;
        m_transform = GetComponent<Transform>();
        Vector3 curPos = m_transform.localPosition;
        curPos.z = positionsArray[1];
        m_transform.localPosition = curPos;
    }

    internal void SetPos(int posIndex)
    {
        m_curPosition = posIndex;
    }
    void FixedUpdate()
    {
        Vector3 curPos = m_transform.localPosition;
        float posZ = m_positionsArray[m_curPosition];
        if (Math.Abs(curPos.z - posZ) > m_speed)
        {
            if (curPos.z < posZ)
            {
                curPos.z += m_speed;
            }
            else
            {
                curPos.z -= m_speed;
            }
            m_transform.localPosition = curPos;
        }
    }
}


