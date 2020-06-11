using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Perso : MonoBehaviour
{
    public Animator _animBody = new Animator();
    public Animator _animFace = new Animator();
    public SpriteRenderer _faceTMP;

    public List<Sprite> faceSprite = new List<Sprite>();
    private int indexForFace = 0;

    public Vector2 speedMove = new Vector2(5f, 2.5f);
    public Vector2 speedScale = new Vector2(0, 0.2f);
    public bool speedDependOnScale = true;

    [Header("Movement point to point")]
    public Sequence s;
    
    public void Update()
    {
        MoveManagement();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            indexForFace++;
            if (indexForFace == faceSprite.Count)
                indexForFace = 0;
            _faceTMP.sprite = faceSprite[indexForFace];
        }
    }

    public void MoveManagement()
    {
        
    }






    public AnimationCurve curveCurve;




    /// <summary>
    /// //Test
    /// </summary>

    [MyBox.ButtonMethod()]
    public void GoesFromHereToLast()
    {
        WalkPath wp = GameManager.Instance.scenario.currentRoom.walkPath;
        WalkPath.closestPointResult first = wp.ClosestDot(this.transform.position);
        WalkPath_Dot last = wp.wholePath_Dots[wp.wholePath_Dots.Count - 1];
        GoesFromThisPointToThatPoint(first.firstPoint, last);///TOD OOOOO
    }
    [MyBox.ButtonMethod()]
    public void GoesFromFirstToLast()
    {
        WalkPath wp = GameManager.Instance.scenario.currentRoom.walkPath;
        WalkPath_Dot first = wp.wholePath_Dots[0];
        WalkPath_Dot last = wp.wholePath_Dots[wp.wholePath_Dots.Count - 1];
        GoesFromThisPointToThatPoint(first, last);
    }
    [MyBox.ButtonMethod()]
    public void GoesFromLastToFirst()
    {
        WalkPath wp = GameManager.Instance.scenario.currentRoom.walkPath;
        WalkPath_Dot first = wp.wholePath_Dots[wp.wholePath_Dots.Count - 1];
        WalkPath_Dot last = wp.wholePath_Dots[0];
        GoesFromThisPointToThatPoint(first, last);
    }

    public void GoesFromThisPointToThatPoint(WalkPath_Dot fromThisDot, WalkPath_Dot toThatDot)
    {
        WalkPath wp = GameManager.Instance.scenario.currentRoom.walkPath;
        List<WalkPath_Dot> path = wp.StartPath(fromThisDot, toThatDot);

        Sequence s = DOTween.Sequence();
        //s.SetEase(Ease.Linear);
        Vector3 previousPos = this.transform.position;
        foreach (WalkPath_Dot dot in path)
        {
            Vector3 direction = (dot.transform.position - previousPos);
            float distance = direction.magnitude;
            float duration = distance / speedMove.x;
            duration /= (speedDependOnScale ? dot.scale : 1f);
            //maybe a speed = move.x and move.y depending on the direction of movement ?
            s.AppendCallback(() => ChangeAnimDirection(direction));
            s.Append(transform.DOMove(dot.transform.position, duration).SetEase(Ease.Linear));
            s.Join(transform.DOScale(dot.scale, duration).SetEase(Ease.Linear));
            previousPos = dot.transform.position;
        }
        //s.AppendCallback(() => ChangeAnimDirection(Vector3.zero));
    }

    void MakeNextStep(List<WalkPath_Dot> path)
    {
        WalkPath_Dot dot = path[0];
        Vector3 direction = (dot.transform.position - this.transform.position);
        float distance = direction.magnitude;
        float duration = distance / speedMove.x;
        duration /= (speedDependOnScale ? dot.scale : 1f);
        //maybe a speed = move.x and move.y depending on the direction of movement ?
        s.AppendCallback(() => ChangeAnimDirection(direction));
        s.Append(transform.DOMove(dot.transform.position, duration).SetEase(Ease.Linear));
        s.Join(transform.DOScale(dot.scale, duration).SetEase(Ease.Linear));


    }
    
    void ChangeAnimDirection(Vector3 direction)
    {
        Debug.Log("Waypoint index changed to " + direction);
        _animBody.SetFloat("Horizontal", direction.x);
        _animBody.SetFloat("Vertical", direction.y * 2);
        if (direction.x > 0)
            _animBody.transform.parent.localScale = Vector3.one * 0.1f;
        else
            _animBody.transform.parent.localScale = new Vector3(-1, 1, 1) * 0.1f;
        //_animBody.SetFloat("GoHaut", direction.y);
        //_animBody.SetBool("GoGauche", direction.x > 0);
        //_animBody.SetBool("GoDroite", direction.x < 0);
        //_animBody.SetBool("GoHaut", direction.y > 0);
        //_animBody.SetBool("GoBas", direction.y < 0);

        //if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        //{
        //    _animBody.SetBool("GoHaut", false);
        //    _animBody.SetBool("GoBas", false);
        //}
        //else
        //{
        //    _animBody.SetBool("GoGauche", false);
        //    _animBody.SetBool("GoDroite", false);
        //}

        //Climb ?
    }

    private bool alreadyOnPoint = false;
    public Vector3 lastPos;
    public Vector3 lastScale;

    public void SavePosAndTP()
    {
        if (!alreadyOnPoint)
        {
            alreadyOnPoint = true;
            lastPos = this.transform.position;
            lastScale = this.transform.localScale;
        }
    }
    [MyBox.ButtonMethod()]
    public void GoesBackToLastPos()
    {
        if (alreadyOnPoint)
        {
            alreadyOnPoint = false;
            this.transform.position = lastPos;
            this.transform.localScale = lastScale;
        }
    }


    ////
    /// fin test
    ////







    public void _obsoleteMoveManagement()
    {
        bool goUp = Input.GetKey(KeyCode.UpArrow);
        bool goDown = Input.GetKey(KeyCode.DownArrow);
        bool goLeft = Input.GetKey(KeyCode.LeftArrow);
        bool goRight = Input.GetKey(KeyCode.RightArrow);

        if (goUp)
        {
            this.transform.position += speedMove.y * Time.deltaTime * Vector3.up;
            this.transform.localScale -= speedScale.y * Time.deltaTime * Vector3.one;
        }
        _animBody.SetBool("GoHaut", goUp);

        if (goDown)
        {
            this.transform.position += speedMove.y * Time.deltaTime * Vector3.down;
            this.transform.localScale += speedScale.y * Time.deltaTime * Vector3.one;
        }
        _animBody.SetBool("GoBas", goDown);

        if (goLeft)
        {
            this.transform.position += speedMove.x * Time.deltaTime * Vector3.left;
        }
        _animBody.SetBool("GoGauche", goLeft);

        if (goRight)
        {
            this.transform.position += speedMove.x * Time.deltaTime * Vector3.right;
        }
        _animBody.SetBool("GoDroite", goRight);
    }
}
