using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum PlayerMode
{
    scanMode,
    extractMode
}
public class TileGame : MonoBehaviour
{
    public PlayerMode playerMode;
    [Header("Grid Setting")]
    public int numofColums;
    public int numofRows;
    public float ColSpacing;
    public float RowSpacing;
    public int randomMinResourceAmount;
    public int randomMaxResourceAmount;
    public int numofMaxResourceTile;

    [Header("Tile Setting")]
    public GameObject tile;
    public Color unscanTileColor;
    public Color zeroTileColor;
    public Color QuaterTileColor;
    public Color hakfTileColor;
    public Color MaxTileColor;
    [Header("User Settings")]
    public int MaxScanCount;
    public int MaxExtractCount;
    public TextMeshProUGUI scanCountText;
    public TextMeshProUGUI ExtractCountText;
    public TextMeshProUGUI ResourceCountText;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI messageText;
    public Image tileInfoZeroImage;
    public Image tileQuaterZeroImage;
    public Image tileHalfZeroImage;
    public Image tileMaxZeroImage;
    public Image tileunScanZeroImage;
    private GridLayoutGroup gridLayoutGroup;
    public static TileGame instance;
    [HideInInspector]
    public int currentScanCount = 0;
    public int currentExtractCount = 0;
    public int resourcesCount = 0;
    public int totalResourceCount = 0;
    public Tile[,] grid;
    public int valid = 16;
    public int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1, -2, 2, 0, 0, -2, -2, 2, 2 ,0, 0, 4, 4};
    public int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1, 0, 0, -2, 2, -2, 2, -2, 2 ,-1, 3, 1, 3};
    bool[,] visited;
    public bool debug = false;
    private void Awake()
    {
        instance = this;
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        
    }

    bool Valid(int x, int y)
    {
        return (x >=0 && x<numofRows && y>=0 && y< numofColums );
    }
    void GenerateGrid()
    {
        
        float cellWidth = Screen.width / numofColums;
        float cellHeight = Screen.height / numofRows;
        grid = new Tile[numofRows, numofColums];
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
        for (int x = 0; x < numofRows; x++)
        {
            for (int y = 0; y < numofColums; y++)
            {
                var _tile = Instantiate(tile, new Vector3(x,y), Quaternion.identity);
                _tile.GetComponent<Image>().color = unscanTileColor;
                _tile.transform.parent = this.transform;
                _tile.name = $"{x},{y}";

                grid[x, y] = _tile.GetComponent<Tile>();
                
                //num of max resources

               
            }
        }
    }
    void Start()
    {

        GenerateGrid();
        SetInitialStats();
        for (int i = 0; i < numofMaxResourceTile; i++)
        {
            GenerateValidResources(UnityEngine.Random.Range(0, numofRows), UnityEngine.Random.Range(0, numofColums));
        }
           // GenerateValidResources(7, 7);
        
    }

    public void GenerateValidResources(int x, int y)
    {
       // print(x + y);
        if (x < 0 || x >= valid) return;
        if (y < 0 || y >= valid) return;


        if(debug)
        {
            grid[x, y].GetComponent<Image>().color = MaxTileColor;
        }else
        {
             grid[x, y].color = MaxTileColor;
        }
        for (int i = 0; i < 8; i++)
        {
            int next_x = x + dx[i];
            int next_y = y + dy[i];

            if(Valid(next_x,next_y))
            {
                if(debug)
                {
                    grid[next_x, next_y].GetComponent<Image>().color = hakfTileColor;
                }
                else
                {
                    grid[next_x, next_y].color = hakfTileColor;
                }
                
            }
                
        }
        for (int i = 8; i < 24; i++)
        {
            int next_x = x + dx[i];
            int next_y = y + dy[i];

            if (Valid(next_x, next_y))
            {
                if (debug)
                {
                    grid[next_x, next_y].GetComponent<Image>().color = QuaterTileColor;
                }
                else
                {
                    grid[next_x, next_y].color = QuaterTileColor;
                }
            }

        }
        //for (int i = 16; i < dx.Length; i++)
        //{
        //    int next_x = x + dx[i];
        //    int next_y = y + dy[i];

        //    if (Valid(next_x, next_y))
        //    {
        //        grid[next_x, next_y].GetComponent<Image>().color = QuaterTileColor;
        //    }

        //}

    }
    public void GenerateResources(int i, int j)
    {
        grid[i, j].color = MaxTileColor;
        if((i-1)>=0)
            grid[i - 1, j].color = hakfTileColor;   //left
        if(j-1 >=0)
            grid[i, j - 1].color = hakfTileColor;    //up
        if (i + 1 >= 0)
            grid[i + 1,j].color = hakfTileColor;   //right       //error line
        if (i + 1 >= 0 && j+1 >=0)
            grid[i + 1,j + 1].color = hakfTileColor; //right down     //error line
        if((i-1)>=0)
            grid[i - 1, j + 1].color = hakfTileColor; //left down
        if (j + 1 >= 0)
            grid[i, j+1].color = hakfTileColor;    // down
        if (i + 1 >= 0 && j - 1 >=0)
            grid[i+1, j - 1].color = hakfTileColor;    //up  right
        if ((i - 1) >= 0 && j-1 >= 0)
            grid[i-1, j - 1].color = hakfTileColor;    //up  left
        if (j - 2 >= 0)
            grid[i , j - 2].color = QuaterTileColor; // up
        if (i + 1 >= 0 && j-2 >=0)
            grid[i+1 , j - 2].color = QuaterTileColor;
        if (i - 1 >= 0 && j - 2 >= 0)
            grid[i-1 , j - 2].color = QuaterTileColor;
        if (i + 2 >= 0 && j - 2 >= 0)
            grid[i+2 , j - 2].color = QuaterTileColor;     //error line
        if (i - 2 >= 0 && j - 2 >= 0)
            grid[i-2 , j - 2].color = QuaterTileColor;
        if (j + 2 >= 0)
            grid[i, j + 2].color = QuaterTileColor; //down         //error line
        if (i - 1 >= 0 && j + 2 >= 0)
            grid[i-1, j + 2].color = QuaterTileColor;
        if (i + 1 >= 0 && j + 2 >= 0)
            grid[i+1, j + 2].color = QuaterTileColor;
        if (i - 2 >= 0 && j + 2 >= 0)
            grid[i-2 , j + 2].color = QuaterTileColor;
        if (i + 2 >= 0 && j + 2 >= 0)
            grid[i+2 , j + 2].color = QuaterTileColor;
        if (i + 2 >= 0)
            grid[i + 2, j].color = QuaterTileColor;   //right
        if (i + 2 >= 0 && j + 1 >= 0)
            grid[i + 2, j+1].color = QuaterTileColor;   //right
        if (i - 2 >= 0)
            grid[i - 2, j].color = QuaterTileColor;   //right
        if (i - 2 >= 0 && j - 1 >= 0)
            grid[i - 2, j - 1].color = QuaterTileColor;   //right
        if (i + 2 >= 0 && j - 1 >= 0)
            grid[i + 2, j - 1].color = QuaterTileColor;   //right
        if (i - 2 >= 0 && j + 1 >= 0)
            grid[i - 2, j + 1].color = QuaterTileColor;   //right

        // need to check that i-1 is not negative, that j-1 is not negative, 
        //and that i+1 and j+1 are not larger than the array.
    }

    public void ScanTiles(int i, int j)
    {
        if(grid[i,j].color == grid[i,j].color)
        {
            grid[i, j].transform.gameObject.GetComponent<Image>().color = grid[i, j].color;
        }
        if (grid[i - 1, j].color == grid[i - 1, j].color)
        {
            grid[i - 1, j].transform.gameObject.GetComponent<Image>().color = grid[i - 1, j].color;
        }
        if (grid[i, j - 1].color == grid[i, j - 1].color)
        {
            grid[i, j - 1].transform.gameObject.GetComponent<Image>().color = grid[i, j - 1].color;
        }
        if (grid[i + 1, j].color == grid[i + 1, j].color)
        {
            grid[i + 1, j].transform.gameObject.GetComponent<Image>().color = grid[i + 1, j].color;
        }
        if (grid[i + 1, j + 1].color == grid[i + 1, j + 1].color)
        {
            grid[i + 1, j + 1].transform.gameObject.GetComponent<Image>().color = grid[i + 1, j + 1].color;
        }
        if (grid[i - 1, j + 1].color == grid[i - 1, j + 1].color)
        {
            grid[i - 1, j + 1].transform.gameObject.GetComponent<Image>().color = grid[i - 1, j + 1].color;
        }
        if (grid[i, j + 1].color == grid[i, j + 1].color)
        {
            grid[i, j + 1].transform.gameObject.GetComponent<Image>().color = grid[i, j + 1].color;
        }
        if (grid[i + 1, j - 1].color == grid[i + 1, j - 1].color)
        {
            grid[i + 1, j - 1].transform.gameObject.GetComponent<Image>().color = grid[i + 1, j - 1].color;
        }
        if (grid[i - 1, j - 1].color == grid[i - 1, j - 1].color)
        {
            grid[i - 1, j - 1].transform.gameObject.GetComponent<Image>().color = grid[i - 1, j - 1].color;
        }
        //if (grid[i, j - 2].color == grid[i, j - 2].color)
        //{
        //    grid[i, j - 2].transform.gameObject.GetComponent<Image>().color = grid[i, j - 2].color;
        //}

        
    }

    void AddResources()
    {
        ResourceCountText.text = totalResourceCount.ToString();
    }
    public void ExtractTiles(int i, int j)
    {
        if (grid[i, j].color == MaxTileColor)
        {
            
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i, j].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if(grid[i, j].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2 ;
            AddResources();
            grid[i, j].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if(grid[i, j].color == QuaterTileColor)
        {
            totalResourceCount += resourcesCount / 4;
            AddResources();
            grid[i, j].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }

        if (grid[i - 1, j].color == MaxTileColor)
        {
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i - 1, j].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if (grid[i - 1, j].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2;
            AddResources();
            grid[i - 1, j].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if (grid[i, j].color == QuaterTileColor)
        {
            totalResourceCount += resourcesCount / 4;
            AddResources();
            grid[i - 1, j].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }

        if (grid[i, j - 1].color == MaxTileColor)
        {
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i, j - 1].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if (grid[i, j - 1].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2;
            AddResources();
            grid[i, j - 1].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if (grid[i, j - 1].color == QuaterTileColor)
        {
            totalResourceCount += resourcesCount / 4;
            AddResources();
            grid[i, j - 1].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }

        if (grid[i + 1, j].color == MaxTileColor)
        {
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i + 1, j].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if (grid[i, j - 1].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2;
            AddResources();
            grid[i + 1, j].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if (grid[i, j - 1].color == QuaterTileColor)
        {
            totalResourceCount += resourcesCount / 4;
            AddResources();
            grid[i + 1, j].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }

        if (grid[i + 1, j + 1].color == MaxTileColor)
        {
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i + 1, j + 1].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if (grid[i + 1, j + 1].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2;
            AddResources();
            grid[i + 1, j + 1].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if (grid[i + 1, j + 1].color == QuaterTileColor)
        {
            totalResourceCount += resourcesCount / 4;
            AddResources();
            grid[i + 1, j + 1].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }

        if (grid[i - 1, j + 1].color == MaxTileColor)
        {
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i - 1, j + 1].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if (grid[i - 1, j + 1].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2;
            AddResources();
            grid[i - 1, j + 1].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if (grid[i - 1, j + 1].color == QuaterTileColor)
        {
            totalResourceCount += resourcesCount / 4;
            AddResources();
            grid[i - 1, j + 1].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }

        if (grid[i, j + 1].color == MaxTileColor)
        {
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i, j + 1].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if (grid[i, j + 1].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2;
            AddResources();
            grid[i, j + 1].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if (grid[i, j + 1].color == QuaterTileColor)
        {
            grid[i, j + 1].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }

        if (grid[i + 1, j - 1].color == MaxTileColor)
        {
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i + 1, j - 1].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if (grid[i + 1, j - 1].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2;
            AddResources();
            grid[i + 1, j - 1].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if (grid[i + 1, j - 1].color == QuaterTileColor)
        {
            grid[i + 1, j - 1].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }

        if (grid[i - 1, j - 1].color == MaxTileColor)
        {
            totalResourceCount += resourcesCount;
            AddResources();
            grid[i - 1, j - 1].transform.gameObject.GetComponent<Image>().color = hakfTileColor;
        }
        if (grid[i - 1, j - 1].color == hakfTileColor)
        {
            totalResourceCount += resourcesCount / 2;
            AddResources();
            grid[i - 1, j - 1].transform.gameObject.GetComponent<Image>().color = QuaterTileColor;
        }
        if (grid[i - 1, j - 1].color == QuaterTileColor)
        {
            grid[i - 1, j - 1].transform.gameObject.GetComponent<Image>().color = zeroTileColor;

        }
        

    }
    void SetInitialStats()
    {
        //set colors of tile images
        tileInfoZeroImage.color = zeroTileColor;
        tileQuaterZeroImage.color = QuaterTileColor;
        tileHalfZeroImage.color = hakfTileColor;
        tileMaxZeroImage.color = MaxTileColor;
        tileunScanZeroImage.color = unscanTileColor;

        //set all text
        scanCountText.text = "Scan Count" +currentScanCount +"/" + MaxScanCount;
        ExtractCountText.text = "Extract Count" + currentExtractCount + "/" + MaxExtractCount;
        messageText.text = "Wellcome!";
    }
    

    public void ToggleButton()
    {
        if(playerMode == PlayerMode.scanMode)
        {
            playerMode = PlayerMode.extractMode;
            messageText.text = "Toggle Mode to Extract Mode";
            modeText.text = "Extract Mode";
        }
        else
        {
            playerMode = PlayerMode.scanMode;
            messageText.text = "Toggle Mode to Scan Mode";
            modeText.text = "Scan Mode";
        }
        resourcesCount = UnityEngine.Random.Range(randomMinResourceAmount, randomMaxResourceAmount);
    }

    public void ShowMessage(string msg)
    {
        messageText.text = msg;
    }

    public void AddScanCount()
    {
        currentScanCount++;
        scanCountText.text = "Scan Count" + currentScanCount + "/" + MaxScanCount;
    }

    public void AddExtractCount()
    {
        currentExtractCount++;
        ExtractCountText.text = "Extract Count" + currentExtractCount + "/" + MaxExtractCount;
    }
}
