using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // The variable which we need for 'moving cube animation'
    private Vector3 lerpPosition;
    
    //Integer Variables
    private static int currentPosition;
    private static int childCount;
    private static int Score;
    private static int ReturnValue;

    //Constant variables
    private const int lerpConstant = 4;
    private const int wheelBrake = 60;

    //Bool Triggers
    private static bool letsRotate;
    private static bool Lerp;
    private static bool isStarted;
    private static bool findNextSquare;
    
    //Gameobjects
    public GameObject wheelGameobject;
    public GameObject playerGameobject;
    public GameObject GameBoard;

    //Float variables for wheel rotating
    private static float Speed;
    private static float wheelDegree;
    
    // UI Elements
    public Button spinButton;
    public Text scoreText;
    void Start()
    {
        InitializeValue();
    }
    void Update()
    {
        RotatingWheel(letsRotate);
        WheelStopped(Speed, isStarted);
        FindNextSquare(findNextSquare);
        LerpingCube(Lerp);     
    }
    public void InitializeValue() // Setting first value to some variables 
    {
        Lerp = false;
        isStarted = false;
        findNextSquare = false;
        letsRotate = false;
        currentPosition = 0;
        Score = 0;
        childCount = GameBoard.transform.childCount;
    } 
    public void ButtonClick() // Starts wheel spinning if button clicked
    {
        spinButton.enabled = false;
        Speed = Random.Range(360, 720);
        letsRotate = true;
        isStarted = true;
    } 
    public void RotatingWheel(bool rotatingStatus) // Rotating wheel by decreasing speed
    {
        if (rotatingStatus)
        {
            if (Speed > 0)
            {
                Speed -= wheelBrake * Time.deltaTime;
            }
            wheelGameobject.transform.Rotate(Vector3.up * Time.deltaTime * Speed);
        }
    } 
    public void WheelStopped(float spd, bool str) // Find degree if wheel stopped 
    {
        if (spd <= 0 && str && !spinButton.enabled)
        {
            letsRotate = false;
            
            wheelDegree = UnityEditor.TransformUtils.GetInspectorRotation(wheelGameobject.transform).x;
            if (wheelDegree < 0)
            {
                wheelDegree = wheelDegree + 360;
            }
            spinButton.enabled = true;
            ValueFinder((int)wheelDegree);
            findNextSquare = true;
        }
    } 
    public int ValueFinder(int degree) // Find value according to degree
    {
        if (degree >= 0 && degree < 60)
        {
            ReturnValue = 1;
        }
        if (degree >= 60 && degree < 120)
        {
            ReturnValue = 2;
        }
        if (degree >= 120 && degree < 180)
        {
            ReturnValue = 3;
        }
        if (degree >= 180 && degree <= 240)
        {
            ReturnValue = 4;
        }
        if (degree >= 240 && degree < 300)
        {
            ReturnValue = 5;
        }
        if (degree >= 300 && degree < 360)
        {
            ReturnValue = 6;
        }
        return ReturnValue;
    } 
    public void FindNextSquare(bool FindingStatus) // Find next square depending current position
    {
        if (FindingStatus)
        {
            lerpPosition = new Vector3(GameBoard.transform.GetChild(currentPosition + 1).transform.position.x, 1.2f, GameBoard.transform.GetChild(currentPosition + 1).transform.position.z);
            Lerp = true;
            findNextSquare = false;
            Scoring(1);
        }
    } 
    public void Scoring(int number) // Get 10 points from each square 
    {
        Score += number * 10;
        scoreText.text = "Score: " + " " + Score;
    }
    public void LerpingCube(bool lerpingStatus) // Moving cube step by step
    {
        if (lerpingStatus)
        {
            float newX = Mathf.Lerp(playerGameobject.transform.position.x, lerpPosition.x, Time.deltaTime * lerpConstant);
            float newY = Mathf.Lerp(playerGameobject.transform.position.y, lerpPosition.y, Time.deltaTime * lerpConstant);
            float newZ = Mathf.Lerp(playerGameobject.transform.position.z, lerpPosition.z, Time.deltaTime * lerpConstant);
            playerGameobject.transform.position = new Vector3(newX, newY, newZ);
            if (Vector3.Distance(playerGameobject.transform.position, lerpPosition) < 0.05)
            {
                Lerp = false;
                ReturnValue--;
                currentPosition++;
                if (currentPosition >= 19)
                {
                    currentPosition = -1;
                }
                if (ReturnValue > 0)
                {
                    findNextSquare = true;
                }
            }
        }
    }
}