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
    public Vector2 runSpeedMove = new Vector2(5f, 2.5f);
    public Vector2 speedScale = new Vector2(0, 0.2f);
    public bool speedDependOnScale = true;

    [Header("Movement point to point")]
    public bool walking = false;
    public WalkPath.closestPointResult currentGoal;
    public WalkPath_Dot currentGoal_Dot;
    public List<Tween> currentTween = new List<Tween>();

    private bool alreadyOnPoint = false;
    private Vector3 lastPos;
    private Vector3 lastScale;

    [System.Serializable]
    public struct Statistique
    {
        public float pv;
        public float strenght;
        public float defense;
        public float agility;
        public float precision;
        public float luck;
        public void resetStat() { pv = 1; strenght = 0; defense = 0; agility = 0; precision = 0; luck = 0; }
    }
    public Statistique currentStat;
    [Header("Equipment")]
    public List<Limb> listLimb = new List<Limb>();
    public List<Limb> listLimbEquipped = new List<Limb>();

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

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Hellooooooooo");
            Debug.Break();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale *= 2;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Time.timeScale /= 2;
        }
    }

    public void MoveManagement()
    {
        
    }

    [MyBox.ButtonMethod()]
    public void UpdateStatistique()
    {
        currentStat.resetStat();
        foreach (Limb l in listLimbEquipped)
        {
            currentStat.pv        += l.statMove.pv;
            currentStat.strenght  += l.statMove.strenght;
            currentStat.defense   += l.statMove.defense;
            currentStat.agility   += l.statMove.agility;
            currentStat.precision += l.statMove.precision;
            currentStat.luck      += l.statMove.luck;
        }
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
        walking = true;
        currentGoal_Dot = toThatDot;
        WalkPath wp = GameManager.Instance.scenario.currentRoom.walkPath;
        List<WalkPath_Dot> path = wp.StartPath(fromThisDot, toThatDot);
        
        //s.SetEase(Ease.Linear);
        MakeNextStep(path);
        //s.AppendCallback(() => ChangeAnimDirection(Vector3.zero));
    }

    void MakeNextStep(List<WalkPath_Dot> path)
    {
        if(path.Count == 0)
        {
            Debug.Log("Finish !");
            EndMovement();
            return;
        }
        currentTween.Clear();

        WalkPath_Dot dot = path[0];
        //prepare next step
        path.RemoveAt(0);

        //Fill sequence
        Vector3 direction = (dot.transform.position - this.transform.position);
        float distance = direction.magnitude;
        float duration = distance / speedMove.x;
        duration /= (speedDependOnScale ? dot.scale : 1f);
        //maybe a speed = move.x and move.y depending on the direction of movement ?
        ChangeAnimDirection(direction);
        currentTween.Add(transform.DOMove(dot.transform.position, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => MakeNextStep(path)));
        currentTween.Add(transform.DOScale(dot.scale, duration)
            .SetEase(Ease.Linear));
    }

    void EndMovement()
    {
        walking = false;
        currentTween.Clear();
        ChangeAnimDirection(Vector3.zero);
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
    
    public void SavePosAndTP()
    {
        if (!alreadyOnPoint)
        {
            alreadyOnPoint = true;
            lastPos = this.transform.position;
            lastScale = this.transform.localScale;
        }
    }

    public void GoesToClosestDot(Vector3 positionInSpace)
    {
        if (walking)
        {
            foreach(Tween tw in currentTween)
                tw.Kill();
            currentTween.Clear();
            /////If : already seeking this point or a close point AND path is like more than 3 long, then : runSpeedMove and not speedMove
        }

        WalkPath wp = GameManager.Instance.scenario.currentRoom.walkPath;
        //probably will need to  watch distance of seeking
        WalkPath.closestPointResult first = wp.ClosestDot(this.transform.position);
        WalkPath.closestPointResult last = wp.ClosestDot(positionInSpace);
        GoesFromThisPointToThatPoint(first.firstPoint, last.firstPoint);
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
