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
        Vector3 previousPos = this.transform.position;
        foreach (WalkPath_Dot dot in path)
        {
            float distance = (dot.transform.position - previousPos).magnitude;
            //maybe a speed = move.x and move.y depending on the direction of movement ?
            s.Append(transform.DOMove(dot.transform.position, distance / speedMove.x))
                .SetEase(Ease.Linear);
            previousPos = dot.transform.position;
            s.Join(transform.DOScale(dot.scale, distance / speedMove.x));
            //s.Join();
            //add some info for the animation here
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
