using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private float _localScale;
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _frontWall;
    [SerializeField] private GameObject _backWall;
    [SerializeField] private GameObject _unvisitedBlock;

    private bool _isVisited;

    public bool IsVisited => _isVisited;
    public float LocalScale => _localScale;

    public GameObject FrontWall => _frontWall;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * _localScale;  
    }

    public void Visit()
    {
        _isVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall() => _leftWall.SetActive(false);

    public void ClearRightWall() => _rightWall.SetActive(false);

    public void ClearFrontWall() => _frontWall.SetActive(false);

    public void ClearBackWall() => _backWall.SetActive(false);
}
