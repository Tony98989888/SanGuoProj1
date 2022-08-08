using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> m_presetPositions;

    [SerializeField] private float m_minRadius;
    [SerializeField] private float m_maxRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Vector2> GenerateRandomPositionsBasedOnPresetPositions()
    {
        List<Vector2> randomPos = new List<Vector2>();
        
        if (m_presetPositions.Count == 0)
        {
            return randomPos;
        }

        foreach (var trans in m_presetPositions)
        {
            var randOffset = Random.insideUnitCircle * Random.Range(m_minRadius, m_maxRadius);
            var randPos = trans.position + new Vector3(randOffset.x, 0.0f, randOffset.y);
            randomPos.Add(randPos);
        }

        return randomPos;
    }
}
