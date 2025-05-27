using UnityEngine;

public class SetStartPos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(60, 0f, 0);
        transform.rotation = Quaternion.Euler(0f, 270f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
