using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject[] items;
    Transform camTrans;

    void Start()
    {
        camTrans = Camera.main.transform;
    }
   
    public void MakeItem(int itemNum)
    {
        Vector3 position = camTrans.position + camTrans.forward;
        Instantiate(items[itemNum], position, Quaternion.identity);
    }

}
