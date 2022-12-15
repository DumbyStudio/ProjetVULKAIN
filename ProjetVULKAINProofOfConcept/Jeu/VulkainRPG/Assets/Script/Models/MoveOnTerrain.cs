using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int> {}

public class MoveOnTerrain : MonoBehaviour
{
    [SerializeField] private Transform myTransform;
    [SerializeField] private Terrain t;
    [SerializeField] private bool autoCalculate = true;
    private int posX;
    private int posZ;

    private Vector3 lastPos;
    [SerializeField] private IntEvent onTerrainEffect = new();
    private int lastTerrain;
    [SerializeField] private IntEvent onChangeTerrain = new();
    private bool isMoving;
    [SerializeField] private UnityEvent onStopTerrainEffect = new();

    void Start()
    {
        if(t == null)
            t = Terrain.activeTerrain;
        if(myTransform == null)
            myTransform = gameObject.transform;
        if (!autoCalculate)
            this.enabled = false;
        lastTerrain = -1;
    }

    void ConvertPosition(Vector3 playerPosition)
    {
        Vector3 terrainPosition = playerPosition - t.transform.position;
        Vector3 mapPosition = new Vector3
        (terrainPosition.x / t.terrainData.size.x, 0,
        terrainPosition.z / t.terrainData.size.z);
        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;
        posX = (int)xCoord;
        posZ = (int)zCoord;
    }

    void CheckTexture()
    {
        float[,,] aMap = t.terrainData.GetAlphamaps(posX, posZ, 1, 1);


        int textureIndex = 0;
        for(int i = 0; i < aMap.Length; i ++)
        {
            if (aMap[0, 0, i] > aMap[0, 0, textureIndex])
                textureIndex = i;
        }
        onTerrainEffect.Invoke(textureIndex);
        if (lastTerrain != textureIndex)
        {
            onChangeTerrain.Invoke(textureIndex);
            lastTerrain = textureIndex;
        }
    }

    void Update()
    {
        // For better performance, move this out of update 
        // and only call it when you need a footstep.
        if (lastPos != myTransform.position)
        {
            OnMove();
            lastPos = myTransform.position;
            isMoving = true;
        }
        else if(isMoving)
        {
            onStopTerrainEffect.Invoke();
            isMoving = false;
        }
    }
    public void OnMove()
    {
        ConvertPosition(myTransform.position);
        CheckTexture();
    }
}
