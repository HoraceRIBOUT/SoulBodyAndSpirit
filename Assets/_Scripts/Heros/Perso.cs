using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
