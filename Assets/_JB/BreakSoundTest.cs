using UnityEngine;

public class BreakSoundTest : MonoBehaviour
{
    public GameObject cube;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            cube.SetActive(false);
        }
    }
}
