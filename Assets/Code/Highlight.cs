using UnityEngine;

public class Highlight : MonoBehaviour
{
    public Renderer rend;
    Material baseMat;
    public Material highlightMat;

    void Start()
    {
        baseMat = rend.material;
    }
    public void Selected(bool select)
    {
        if (select)
        {
            rend.material = highlightMat;
        }
        else
        {
            rend.material = baseMat;
        }
    }
}