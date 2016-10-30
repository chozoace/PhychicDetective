using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject _playerGameObject;
    [SerializeField] float _xCameraThreshold = 2.5f;
    [SerializeField] float _yCameraThreshold = 2.5f;
    Vector2 _cameraPosition = new Vector2(0,0);
    public Vector2 CameraScreenPosition { get { return _cameraPosition; } }
    float _viewportHeight;
    float _viewportWidth;

    void Start ()
    {
        _viewportHeight = 2 * Camera.main.orthographicSize * 100;
        _viewportWidth = _viewportHeight * Camera.main.aspect;
        //Debug.Log("Height: " + _viewportHeight + "\nWidth: " + _viewportWidth);
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCameraMovement();
    }

    void UpdateCameraMovement()
    {
        float playerXPos = _playerGameObject.transform.position.x;
        float playerYPos = _playerGameObject.transform.position.y;
        _cameraPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        Vector3 cameraPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        if (cameraPosition.x - playerXPos < _xCameraThreshold)
        {
            Vector3 v = this.transform.position;
            v.x = v.x + (_xCameraThreshold - (cameraPosition.x - playerXPos));
            this.transform.position = v;
        }
        if (cameraPosition.y - playerYPos < _yCameraThreshold)
        {
            Vector3 v = this.transform.position;
            v.y = v.y + (_yCameraThreshold - (cameraPosition.y - playerYPos));
            this.transform.position = v;
        }
        cameraPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        if (playerXPos - cameraPosition.x < _xCameraThreshold)
        {
            Vector3 v = this.transform.position;
            v.x = v.x - (_xCameraThreshold - (playerXPos - cameraPosition.x));
            this.transform.position = v;
        }
        if (playerYPos - cameraPosition.y < _yCameraThreshold)
        {
            Vector3 v = this.transform.position;
            v.y = v.y - (_yCameraThreshold - (playerYPos - cameraPosition.y));
            this.transform.position = v;
        }
    }
}
