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


    // Start is called before the first frame update
    void Start()
    {
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += speedMove.y * Time.deltaTime * Vector3.up;
            this.transform.localScale -= speedScale.y * Time.deltaTime * Vector3.one;
            _animBody.SetBool("GoHaut", true);
        }
        else
            _animBody.SetBool("GoHaut", false);
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position += speedMove.y * Time.deltaTime * Vector3.down;
            this.transform.localScale += speedScale.y * Time.deltaTime * Vector3.one;
            _animBody.SetBool("GoBas", true);
        }
        else
            _animBody.SetBool("GoBas", false);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += speedMove.x * Time.deltaTime * Vector3.left;
            _animBody.SetBool("GoGauche", true);
        }
        else
            _animBody.SetBool("GoGauche", false);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += speedMove.x * Time.deltaTime * Vector3.right;
            _animBody.SetBool("GoDroite", true);
        }
        else
            _animBody.SetBool("GoDroite", false);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            indexForFace++;
            if (indexForFace == faceSprite.Count)
                indexForFace = 0;
            _faceTMP.sprite = faceSprite[indexForFace];
        }
    }
}
