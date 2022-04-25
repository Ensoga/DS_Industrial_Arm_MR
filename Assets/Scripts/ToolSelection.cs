using UnityEngine;

public class ToolSelection : MonoBehaviour
{
    [SerializeField] public GameObject textvisible;
    [SerializeField] public GameObject texthidden;

    public int currentTool;
    public int index;
    public bool visible = true;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Next()
    {
        index++;
        if (index == 2)
        {
            index = 0;
        }
        ChangeTool(index);
    }
    public void Previous()
    {
        index--;
        if (index == -1)
        {
            index = 1;
        }
        ChangeTool(index);
    }

    private void SelectTool(int _index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }

    public void ChangeTool(int _change)
    {
        currentTool = index;
        SelectTool(currentTool);
    }

    public void Visibility()
    {
        if (visible==true)
        {
            transform.GetChild(index).gameObject.SetActive(false);
            visible = false;
            textvisible.SetActive(false);
            texthidden.SetActive(true);
        }
        else
        {
            transform.GetChild(index).gameObject.SetActive(true);
            visible = true;
            texthidden.SetActive(false);
            textvisible.SetActive(true);
        }
        
    }
}
