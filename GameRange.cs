using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRange : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerView>().OnCone();
        }
    }
}
