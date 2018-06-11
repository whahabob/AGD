using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Leaf : MonoBehaviour
{

    [Header("Initial Map Values")]
    [SerializeField] private int MIN_LEAF_SIZE = 6;
    
  	 [Header("Level Creation - Prefabs")]
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _floor;
    [Header("Level Creation - Offsets")]
    [SerializeField] private float _wallOffset;
    [SerializeField] private float _floorOffset;
	private GameObject EntitiesParent { get; set; }
    
    [SerializeField][Range(0, 1)] private float _wallPercentage = 0.26f;
   
    public int x,y,width,height;
    public Leaf leftChild,rightChild; 
    public int[,] Map { get; set; }

    public Leaf(int x, int y, int width, int height,GameObject wall, GameObject floor)
    {
		EntitiesParent = new GameObject("_Entities");
        this.x = x;
		_wall = wall;
		_floor = floor;
        this.y = y;
        this.width = width;
        this.height = height;
        Map = new int[width, height];
        int middle = height / 2;
        _wallPercentage = 0.28f;
		Debug.Log(_wallPercentage);
		Debug.Log(width);
        for (int xx = 0; xx < width; xx++)
        {
			
            for (int yy = 0; yy < height; yy++)
            {
			
                if (x == 0) { Map[xx, yy] = 1;}
                else if (yy == 0) Map[xx, yy] = 1;
                else if (xx == width - 1) Map[xx, yy] = 1;
                else if (yy == height - 1) Map[xx, yy] = 1;
                else 
				{
					Map[xx,yy] = 0;
					//Map[xx, yy] = y == middle ? 0 : Random.value < _wallPercentage ? 1 : 0;
					Debug.Log(Map[xx,yy]);
				}
            }
        }
        for (int xx = 0; xx < width; xx++)
        {
            for (int yy = 0; yy < height; yy++)
            {
				
				GameObject obj;
				//Debug.Log(Map[xx, yy]);
				
				if(Map[xx, yy] == 1)
					obj = floor;
				else
					obj = wall;
                GameObject go = Instantiate(obj);

                go.transform.position = new Vector3(
                    xx * go.transform.localScale.x, 
                    Map[xx, yy] == 1 ? _wallOffset : _floorOffset, 
                    yy * go.transform.localScale.z);
                //go.transform.parent = transform;
            }
        }
    }

    public bool split()
    {
        if(!leftChild || !rightChild)
        {
            return false;
        }

        bool splitH;
        if(Random.Range(0,1)== 0)
        {
            splitH = true;
        }
        else
        {
            splitH = false;
        }

        if (width > height && width / height >= 1.25)
            splitH = false;
        else if (height > width && height / width >= 1.25)
            splitH = true;

        int max = (splitH ? height : width) - MIN_LEAF_SIZE;
        if(max <= MIN_LEAF_SIZE)
            return false;

        int split = Random.Range(MIN_LEAF_SIZE, max);

        if(splitH)
        {
            leftChild = new Leaf(x, y, width, split,_wall, _floor);
            rightChild = new Leaf(x, y + split, width, height - split,_wall, _floor);
        }
        else
        {
            leftChild = new Leaf(x, y, split, height,_wall, _floor);
            rightChild = new Leaf(x + split, y, width - split, height,_wall, _floor);
        }
 

        return true;;
    }


}

