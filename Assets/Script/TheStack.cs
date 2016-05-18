using UnityEngine;
using System.Collections;

public class TheStack : MonoBehaviour {

    private const float BOUND_SIZE = 3.5f;
    private const float STACK_MOVING_SPEED = 5.0f;

    private GameObject[] theStack;
    private int scoreCount = 0;
    private int stackIndex;
    private int combo = 0;
    private Vector2 stackBounds = new Vector2(BOUND_SIZE, BOUND_SIZE);
    private float tileTransition = 0.0f;
    private float tileSpeed = 2.0f;
    private float secondaryPosition;
    private Vector3 desirePossition;
    private Vector3 lastTilePosition;
    private const float ERROR_MARGIN = 0.1f;

    private bool isMovingOnX = true;
    private bool isDead = false;

    // Use this for initialization
    void Start () {
        theStack = new GameObject[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            theStack[i] = transform.GetChild(i). gameObject;
        }

        stackIndex = transform.childCount - 1;
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                SpawTile();
                scoreCount++;
            }
            else
            {
                EndGame ();
            }
        }
        MoveTile();

        transform.position = Vector3.Lerp(transform.position, desirePossition, STACK_MOVING_SPEED * Time.deltaTime);

    }
    private void MoveTile()
    {
        if (isDead)
            return;

        tileTransition += Time.deltaTime * tileSpeed;
        if(isMovingOnX)
            theStack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTransition) * BOUND_SIZE, scoreCount, secondaryPosition);
        else
            theStack[stackIndex].transform.localPosition = new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTransition) * BOUND_SIZE);


    }

    private void SpawTile()
    {
        lastTilePosition = theStack[stackIndex].transform.localPosition;
        stackIndex--;
        if (stackIndex < 0)
        {
            stackIndex = transform.childCount - 1;
        }
        desirePossition = (Vector3.down) * scoreCount;
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);

    }
    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;

        if (isMovingOnX)
        {
            float deltaX = lastTilePosition.x - t.position.x;
            if (Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                //CUT
                combo = 0;
                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0)
                {
                    return false;
                }
                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);

            }

        }
        else
        {
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {
                //CUT
                combo = 0;
                stackBounds.y -= Mathf.Abs(deltaZ);
                if (stackBounds.y <= 0)
                {
                    return false;
                }
                float middle = lastTilePosition.z + t.localPosition.z / 2;
                t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
            }
        }
        if(isMovingOnX)
        {
            secondaryPosition = t.localPosition.x;
        }
        else
        {
            secondaryPosition = t.localPosition.z;

        }
        isMovingOnX =! isMovingOnX;
        return true;

    }
    private void EndGame()
    {
        isDead = true;
        Debug.Log("Looser!");
        theStack[stackIndex].AddComponent<Rigidbody>();

    }
}
