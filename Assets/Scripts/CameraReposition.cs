using UnityEngine;
using UnityEngine.UI;

public class CameraReposition : MonoBehaviour
{
    public Sprite rightBtn;
    public Sprite leftBtn;
    private readonly Vector3 _firstMapPos = Vector3.zero;
    private readonly Vector3 _secondMapPos = new Vector3(80f, 0f, 0f);
    private bool _isFirstMap = true;
    private Vector3 _curCameraPos;
    
    // Start is called before the first frame update
    void Start()
    {
        // 초기 위치 설정
        _curCameraPos = _isFirstMap ? _firstMapPos : _secondMapPos;
        Camera.main.transform.position = _curCameraPos;
    }
    public void MoveButton()
    {
        if (_isFirstMap)
        {
            Camera.main.transform.position = _secondMapPos;
            gameObject.GetComponent<Image>().sprite = leftBtn;
            _isFirstMap = false;
        }
        else 
        {
            Camera.main.transform.position = _firstMapPos;
            gameObject.GetComponent<Image>().sprite = rightBtn;
            _isFirstMap = true;
        }
    }

}
