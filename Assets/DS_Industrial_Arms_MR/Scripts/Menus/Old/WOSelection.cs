using UnityEngine;

public class WOSelection : MonoBehaviour
{
    [SerializeField] public GameObject textvisible;
    [SerializeField] public GameObject texthidden;
    [SerializeField] public GameObject WOImage;


    public int currentTool;
    public int index;
    private bool visible = true;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Next()
    {
        index++;
        if (index == transform.childCount)
        {
            index = 0;
        }
        ChangeTool();
    }
    public void Previous()
    {
        index--;
        if (index == -1)
        {
            index = transform.childCount-1;
        }
        ChangeTool();
    }

    private void SelectTool(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
            
            switch (_index) // ??
            {
                case 0:
                    WOImage.transform.GetChild(1).gameObject.SetActive(false);
                    WOImage.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case 1:
                    WOImage.transform.GetChild(0).gameObject.SetActive(false);
                    WOImage.transform.GetChild(1).gameObject.SetActive(true);
                    break;
            }
        }
    }

    public void ChangeTool()
    {
        currentTool = index;
        SelectTool(currentTool);
    }

    public void Visibility()
    {
        if (visible==true)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            visible = false;
            textvisible.SetActive(false);
            texthidden.SetActive(true);

        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            visible = true;
            texthidden.SetActive(false);
            textvisible.SetActive(true);
        }
        
    }
}
