using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController: MonoBehaviour
{
    private static PlayerController _currenPlayerController;
    public static PlayerController Instance
    {
        get
        {
            if (_currenPlayerController == null)
                _currenPlayerController = new PlayerController();
            return _currenPlayerController;
        }
        set
        {
            _currenPlayerController = value;
        }
    }
    private float screenWidth, screenHeight;
    private Vector2 midPoint;
    private bool canMove;
    MobileController controls;
    private void Awake()
    {
        if (_currenPlayerController == null)
            Instance = this;
        canMove = true;
        controls = new MobileController();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        midPoint = new Vector2((int)screenWidth / 2, (int)screenHeight / 2);
        //EnhancedTouchSupport.Enable();
        controls.Enable();
        //controls.OneHand.Move.started += _ => Move();//Move(_.ReadValue<Vector2>());
        //controls.OneHand.Move.started += _ => 
    }

    private void Start()
    {
        controls.OneHand.Move.started += _ => Move();
    }

    private void OnEnable()
    {
        canMove = true;
        StartCoroutine(_dontMove(2.0f));
        controls.Enable();
    }

    public void dontMove(float timer)
    {
        StartCoroutine(_dontMove(timer));
    }

    IEnumerator _dontMove(float timer)
    {
        yield return new WaitForSeconds(timer);
    }

    private void OnDisable()
    {
        canMove = false;
        controls.Disable();
    }

    public void PlayerWait()
    {
        Maze.Instance.UpdatePlayerPos(0, 0);
    }

    void Move()
    {
        if (!canMove) return;
        var pointerPos = Pointer.current.position.ReadValue();

        Vector3 pos = pointerPos;
        //Vector2 Rpos = Camera.main.ScreenToWorldPoint(pos);
        //RaycastHit2D hit = Physics2D.Raycast(Rpos, Vector2.zero);
        //Debug.Log("Ray hit " + hit.collider.gameObject.name);
        if (pos.y / screenHeight < 0.3f) return;
        Vector2 dir = pointerPos - midPoint;
        float angle = Vector2.SignedAngle(dir, Vector2.up);
        Debug.Log(pos + " " + angle);
        if (pos.x > midPoint.x)
        {
            if(pos.y > midPoint.y)
            {
                if(angle >= 22.5f)
                {
                    if(angle >= 67.5f)
                    {
                        Maze.Instance.UpdatePlayerPos(1, 0);//Right
                        Debug.Log(">");
                    }
                    else
                    {
                        Maze.Instance.UpdatePlayerPos(1, 1);
                        Debug.Log("//");
                    }                    
                }
                else
                {
                    Maze.Instance.UpdatePlayerPos(0, 1);//Front
                    Debug.Log("^");
                }
            }
            else
            {
                if (angle >= 112.5f)
                {
                    if (angle >= 157.5f)
                    { 
                        Maze.Instance.UpdatePlayerPos(0, -1);//Down
                        Debug.Log("V");
                    }
                    else
                    {
                        Maze.Instance.UpdatePlayerPos(1, -1);
                        Debug.Log(" \\ ");
                    }
                }
                else
                {
                    Maze.Instance.UpdatePlayerPos(1, 0);//Right
                    Debug.Log(">");
                }
            }
        }
        else
        {
            if (pos.y > midPoint.y)
            {
                if (angle <= -22.5f)
                {
                    if (angle <= -67.5f)
                    {
                        Maze.Instance.UpdatePlayerPos(-1, 0);//left
                        Debug.Log("<");
                    }
                    else
                    {
                        Maze.Instance.UpdatePlayerPos(-1, 1);
                        Debug.Log("\\");
                    }                    
                }
                else
                {
                    Maze.Instance.UpdatePlayerPos(0, 1);//Front
                    Debug.Log("^");
                }
            }
            else
            {
                if (angle <= -112.5f)
                {
                    if(angle <= -157.5f)
                    {
                        Maze.Instance.UpdatePlayerPos(0, -1);//Down
                        Debug.Log("V");
                    }
                    else
                    {
                        Maze.Instance.UpdatePlayerPos(-1, -1);
                        Debug.Log("//");
                    }
                }
                else
                {
                    Maze.Instance.UpdatePlayerPos(-1, 0);//Left
                    Debug.Log("<");
                }
            }
        }
        StartCoroutine(MoveAgain());
    }

    IEnumerator MoveAgain()
    {
        canMove = false;
        yield return new WaitForSeconds(0.3f);
        canMove = true;
    }
}