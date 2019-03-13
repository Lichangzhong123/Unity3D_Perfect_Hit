using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPropListPos : MonoBehaviour
{
    private List<Vector3> propPosList =new List<Vector3>();

    public List<Vector3> GetpropPosList()
    {
        propPosList.Add(transform.localPosition);
        for (int j = 0; j < 40; j++)
        {
            for (int i = 0; i < 7; i++)
            {
                propPosList.Add(new Vector3(i, 0, j * -1));
            }
        }
        return propPosList;
    }
}
