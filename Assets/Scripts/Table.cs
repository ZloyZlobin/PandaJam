using System.Collections.Generic;
using System.Linq;
using PanndaJamTest.Behaviours;
using PanndaJamTest.Bricks;
using PanndaJamTest.State;
using UnityEngine;

namespace PanndaJamTest
{
	public class Table: MonoBehaviour
	{
        /// <summary>
        /// Bricks factory
        /// </summary>
        [SerializeField]
        private BricksFactory bricksFactory;
        /// <summary>
        /// Table width (count bricks by X)
        /// </summary>
        [SerializeField]
        private int Width;
        /// <summary>
        /// Table height (count bricks by Y)
        /// </summary>
        [SerializeField]
        private int Height;
        /// <summary>
        /// Left bottom offset
        /// </summary>
        [SerializeField]
        private Vector3 ZeroPosition;
        /// <summary>
        /// Size of brick
        /// </summary>
        [SerializeField]
        private Vector2 BrickSize;
        /// <summary>
        /// Speed falling annimation
        /// </summary>
        [SerializeField]
        private float fallingSpeed;
        /// <summary>
        /// Speed setup new line
        /// </summary>
        [SerializeField]
        private float upSpeed;
        /// <summary>
        /// Count neighbour bricks to trigger
        /// </summary>
        [SerializeField]
        private int countToTrigger;
        /// <summary>
        /// Count of empty top lines on start game
        /// </summary>
        [SerializeField]
        private int freeTop;
        /// <summary>
        /// Time in sec to add new line
        /// </summary>
        [SerializeField]
        private int newLineInterval;
        /// <summary>
        /// Table of briks
        /// </summary>
        private Brick[][] bricks;
        /// <summary>
        /// New line bricks
        /// </summary>
        private Brick[] newLine;
        /// <summary>
        /// Current moving bricks
        /// </summary>
        private List<Brick> movingBricks;
        /// <summary>
        /// Speed of current animmation
        /// </summary>
        private float currentSpeed;
        /// <summary>
        /// Remain time to new line
        /// </summary>
        private float timeToNewLine;
        /// <summary>
        /// Delta to stop moving
        /// </summary>
        private const float MOVE_DELTA = 0.001f;

        private void Start()
        {
            GameStateController.OnGameStateChanged += OnGameStateChanged;
            if(GameStateController.GameState == GameState.StartGame)
                Init();
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.StartGame)
                Init();
        }

        private void Update()
        {
            if (movingBricks != null && movingBricks.Count > 0)//play mmoving animation
            {
                MoveBricks();
            }
            else//finish animate
            {
                if (GameStateController.GameState == GameState.AnimateFall)//was triggered -> check for complete
                {
                    if (CheckToComplete())
                        GameStateController.SetState(GameState.Win);
                    else
                        GameStateController.SetState(GameState.Play);
                }
                else if(GameStateController.GameState == GameState.AnimateUp)//added new line
                {
                    GameStateController.SetState(GameState.Play);
                }

                if (GameStateController.GameState == GameState.Play)
                {
                    timeToNewLine -= Time.deltaTime;
                    if (timeToNewLine <= 0)
                    {
                        AddNewLine();
                    }
                    else
                    {
                        int index = (int)(Width * (1f - (timeToNewLine / newLineInterval)));//index of filled new line bricks
                        FillNewLine(index);
                    }
                }
            }
        }
        /// <summary>
        /// Force add new line
        /// </summary>
        public void ForceAddLine()
        {
            AddNewLine();
        }
        /// <summary>
        /// Fill elements of new line
        /// </summary>
        /// <param name="index">Right bound</param>
        private void FillNewLine(int index)
        {
            while (index >= 0 && newLine[index] == null)
            {
                var brick = bricksFactory.GetRandomBrick();
                newLine[index] = brick;
                brick.transform.position = GetPosition(index, -2);
                brick.PosX = index;
                brick.PosY = -1;
                index--;
            }
        }
        /// <summary>
        /// Setup new line to table
        /// </summary>
        private void AddNewLine()
        {
            FillNewLine(Width - 1);//fill all line
            if (!CheckToFail())//if no empty space -> fail
            {
                GameStateController.SetState(GameState.AnimateUp);
                //offset lines
                for (int i = Height - 1; i > 0; i--)
                    bricks[i] = bricks[i - 1];
                bricks[0] = newLine;
                foreach (var brick in bricks[0])
                    brick.OnClick += OnBrickClick;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (bricks[i][j] != null)
                            bricks[i][j].PosY++;
                    }
                }
                newLine = new Brick[Width];
                timeToNewLine = newLineInterval;//reset timer to new line
                RecalculatePositions();
                currentSpeed = upSpeed;
            }
            else
                GameStateController.SetState(GameState.Lose);
        }
        /// <summary>
        /// Moving animation
        /// </summary>
        private void MoveBricks()
        {
            for (int i = movingBricks.Count - 1; i >= 0; i--)
            {
                var brick = movingBricks[i];
                var targetPos = GetPosition(brick.PosX, brick.PosY);
                var maxMagnitude = (brick.transform.position - targetPos).magnitude;
                var moveVector = (targetPos - brick.transform.position).normalized;
                var speed = Time.deltaTime * currentSpeed;
                brick.transform.position += Vector3.ClampMagnitude( moveVector * speed , maxMagnitude);
                if ((brick.transform.position - targetPos).magnitude < MOVE_DELTA)//stop brick moving
                {
                    brick.transform.position = targetPos;
                    movingBricks.Remove(brick);
                }
            }
        }

        public void Init()
        {
            Clear();
            //Fill table
            bricks = new Brick[Height][];
            for (int i = 0; i < Height; i++)
            {
                bricks[i] = new Brick[Width];
                if (i < Height - freeTop)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        var brick = bricksFactory.GetRandomBrick();
                        SetupBrick(brick, j, i);
                        brick.OnClick += OnBrickClick;
                    }
                }
            }
            newLine = new Brick[Width];
            timeToNewLine = newLineInterval;
            //setup target
            var posY = Mathf.Min(Height - 1, Height - freeTop);
            var posX = Random.Range(0, Width - 1);
            if (bricks[posY][posX] != null)
                GameObject.DestroyImmediate(bricks[posY][posX].gameObject);
            SetupBrick(bricksFactory.GetTarget(), posX, posY);

            GameStateController.SetState(GameState.Play);
        }

        /// <summary>
        /// Setup brick parameters
        /// </summary>
        private void SetupBrick(Brick brick, int posX, int posY)
        {
            bricks[posY][posX] = brick;
            brick.transform.position = GetPosition(posX, posY);
            brick.PosX = posX;
            brick.PosY = posY;
        }
        /// <summary>
        /// Get target world position of brick
        /// </summary>
        /// <param name="x">X table position</param>
        /// <param name="y">Y table position</param>
        /// <returns>W=Target world position</returns>
        private Vector3 GetPosition(int x, int y)
        {
            return ZeroPosition + new Vector3(BrickSize.x * x, BrickSize.y * y);
        }

        private void OnBrickClick(Brick brick)
        {
            if (GameStateController.GameState != GameState.Play)
                return;

            var triggered = new HashSet<Brick>();
            var bricksToCheck = new Queue<Brick>();
            var checkedBricks = new HashSet<Brick>();
            bricksToCheck.Enqueue(brick);
            while(bricksToCheck.Count > 0)//check all neighbours to trigger by type
            {
                var targetBrick = bricksToCheck.Dequeue();
                if(checkedBricks.Contains(targetBrick))
                    continue;
                triggered.Add(targetBrick);
                //check left
                if(targetBrick.PosX > 0)
                {
                    var leftBrick = bricks[targetBrick.PosY][targetBrick.PosX - 1];
                    if(leftBrick != null && targetBrick.IsTriggerWith(leftBrick))
                            bricksToCheck.Enqueue(leftBrick);
                }
                //check right
                if(targetBrick.PosX < Width - 1)
                {
                    var rightBrick = bricks[ targetBrick.PosY][targetBrick.PosX + 1];
                    if(rightBrick != null && targetBrick.IsTriggerWith(rightBrick))
                            bricksToCheck.Enqueue(rightBrick);
                }
                //check top
                if(targetBrick.PosY < Height - 1)
                {
                    var topBrick = bricks[targetBrick.PosY + 1][targetBrick.PosX];
                    if(topBrick != null && targetBrick.IsTriggerWith(topBrick))
                        bricksToCheck.Enqueue(topBrick);
                }
                //check bottom
                if(targetBrick.PosY > 0)
                {
                    var bottomBrick = bricks[targetBrick.PosY - 1][targetBrick.PosX];
                    if(bottomBrick != null && targetBrick.IsTriggerWith(bottomBrick))
                        bricksToCheck.Enqueue(bottomBrick);
                }
                checkedBricks.Add(targetBrick);
            }
            if (triggered.Count < countToTrigger)
                return;

            GameStateController.SetState(GameState.AnimateFall);

            var activatedBricks = new HashSet<Brick>();
            var collectedBricks = new HashSet<Brick>();
            //Execute triggered bricks behaviours
            foreach (var triggeredBrick in triggered)
                Activate(triggeredBrick, activatedBricks, collectedBricks);

            //Collect triggered bricks
            foreach (var collectedBrick in collectedBricks)
            {
                bricks[collectedBrick.PosY][collectedBrick.PosX] = null;
                collectedBrick.OnClick -= OnBrickClick;
                GameStateController.CollectResources(collectedBrick.Resources);
                GameObject.DestroyImmediate(collectedBrick.gameObject);
            }
            currentSpeed = fallingSpeed;
            RecalculatePositions();
        }
        /// <summary>
        /// Execute bricks behaviours
        /// </summary>
        /// <param name="brick">Bricks to actiate</param>
        /// <param name="activatedBricks">Already activated bricks</param>
        /// <param name="collectedBricks">Bricks to collect</param>
        private void Activate(Brick brick, HashSet<Brick> activatedBricks, HashSet<Brick> collectedBricks)
        {
            if (activatedBricks.Contains(brick))
                return;
            activatedBricks.Add(brick);
            if (!brick.IsLocked)
                collectedBricks.Add(brick);
            foreach (var behaviour in brick.GetComponents<BrickBehaviour>())
            {
                foreach (var triggeredPos in behaviour.GetTriggered(Width, Height))
                {
                    var triggeredBrick = bricks[triggeredPos.Y][triggeredPos.X];
                    if (triggeredBrick != null)
                        Activate(triggeredBrick, activatedBricks, collectedBricks);
                }
                behaviour.ExecuteBehaviour(bricks);
            }
        }
        /// <summary>
        /// Check to fail game (not enough free space for new line)
        /// </summary>
        /// <returns>Is fail</returns>
        private bool CheckToFail()
        {
            return bricks[Height - 1].Any(brick => brick != null);
        }

        /// <summary>
        /// Check to comolete game (target brick on the bottom line)
        /// </summary>
        /// <returns>Is complete</returns>
        private bool CheckToComplete()
        {
            return bricks[0].Any(brick => brick != null && brick.Type == BrickType.Target);
        }
        /// <summary>
        /// Update bricks positions
        /// </summary>
        private void RecalculatePositions()
        {
            movingBricks = new List<Brick>();
            var widthOffset = 0;
            for (int i = 0; i < Width; i++)
            {
                var heightOffset = 0;
                for (int j = 0; j < Height; j++)
                {
                    if (bricks[j][i] == null)//hole
                        heightOffset++;
                    else if (heightOffset > 0)//fall
                    {
                        bricks[j][i].PosY -= heightOffset;
                        bricks[j][i].PosX -= widthOffset;
                        movingBricks.Add(bricks[j][i]);
                        bricks[j - heightOffset][i - widthOffset] = bricks[j][i];
                        bricks[j][i] = null;
                    }
                    else if (widthOffset > 0)//emmpty column
                    {
                        bricks[j][i].PosX -= widthOffset;
                        bricks[j][i - widthOffset] = bricks[j][i];
                        movingBricks.Add(bricks[j][i]);
                        bricks[j][i] = null;
                    }
                    else//check for already moved
                    {
                        var currentPos = bricks[j][i].transform.position;
                        var targetPos = GetPosition(j, i);
                        if ((targetPos - currentPos).magnitude > MOVE_DELTA)
                            movingBricks.Add(bricks[j][i]);
                    }
                }
                if (heightOffset == Height)//empty column
                    widthOffset++;
            }
        }
        /// <summary>
        /// Clear table
        /// </summary>
        public void Clear()
        {
            if (bricks == null)
                return;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (bricks[j][i] != null)
                    {
                        bricks[j][i].OnClick -= OnBrickClick;
                        GameObject.DestroyImmediate(bricks[j][i].gameObject);
                    }
                }
            }
            bricks = null;
            foreach (var brick in newLine)
            {
                if(brick != null)
                    GameObject.DestroyImmediate(brick.gameObject);
            }
            newLine = null;
        }
	}
}