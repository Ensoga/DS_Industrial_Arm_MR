using UnityEngine;

public class ToolSelection : MonoBehaviour
{
    [SerializeField] public GameObject textvisible;
    [SerializeField] public GameObject texthidden;
    [SerializeField] public GameObject toolImage;
    public GameObject tool1;


    [SerializeField] public Vector3 _rotation;
    [SerializeField] public float _speed;
    [SerializeField] public float _limit;
    [SerializeField] public float _speedUD;




    public int currentTool;
    public int index;
    public bool visible = true;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void Update()
    {
        toolImage.transform.Rotate(_rotation * _speed * Time.deltaTime);
        tool1.transform.Rotate(_rotation * _speed * Time.deltaTime);

        float y = Mathf.PingPong(Time.time * _speedUD, 1) * 6 - 3; //Code for hovering
        toolImage.transform.position = new Vector3(0, y, 0);

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
            
            switch (_index)
            {
                case 0:
                    toolImage.transform.GetChild(1).gameObject.SetActive(false);
                    toolImage.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case 1:
                    toolImage.transform.GetChild(0).gameObject.SetActive(false);
                    toolImage.transform.GetChild(1).gameObject.SetActive(true);
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
