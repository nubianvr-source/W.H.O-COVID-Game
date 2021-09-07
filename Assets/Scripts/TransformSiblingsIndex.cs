using UnityEngine;
using Random = UnityEngine.Random;

public class TransformSiblingsIndex : MonoBehaviour
{
    private int m_IndexNumber;
    public GameObject TrueBtn;
    public GameObject FalseBtn;

    // Start is called before the first frame update
    void Start()
    {
        m_IndexNumber = Random.Range(0, 2);
        if (m_IndexNumber == 0)
        {
            TrueBtn.transform.SetSiblingIndex(m_IndexNumber);
            FalseBtn.transform.SetSiblingIndex(1);
        }
        else {
            FalseBtn.transform.SetSiblingIndex(0);
            TrueBtn.transform.SetSiblingIndex(m_IndexNumber);
        }
        
        Debug.Log("TrueBtn Index : " + TrueBtn.transform.GetSiblingIndex());
        Debug.Log("FalseBtn Index : " + FalseBtn.transform.GetSiblingIndex());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}

