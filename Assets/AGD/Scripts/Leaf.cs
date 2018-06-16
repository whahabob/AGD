using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Leaf : MonoBehaviour
{

    
     private int MIN_LEAF_SIZE = 6;
    
  	private GameObject _wall;
     private GameObject _floor;
    
    private float _wallOffset;
    private float _floorOffset;
	private GameObject EntitiesParent { get; set; }
    
    private float _wallPercentage = 0.26f;
     private int _roomSpacePercentage = -2;
   
    public int x,y,width,height;
    public Leaf leftChild,rightChild; 
    public int[,] Map { get; set; }

    public Leaf(int x, int y, int width, int height,GameObject wall, GameObject floor)
    {
		//EntitiesParent = new GameObject("_Entities");
        this.x = x;
		_wall = wall;
		_floor = floor;
        this.y = y;
        this.width = width;
        this.height = height;
        Map = new int[width, height];
        int middle = height / 2;
        _wallPercentage = 0.28f;
		
        for (int xx = 0; xx < width; xx++)
        {
			
            for (int yy = 0; yy < height; yy++)
            {
			
                if (xx == 0) { Map[xx, yy] = 1;    }
                else if (yy == 0) Map[xx, yy] = 1;
                else if (xx == width - 1) Map[xx, yy] = 1;
                else if (yy == height - 1) Map[xx, yy] = 1;
                else 
				{
					//Map[xx,yy] = 0;
					Map[xx, yy] = yy == middle ? 0 : Random.value < _wallPercentage ? 1 : 0;
					
				}
                
            }
           
        }
    }

    public bool split(int xx, int yy)
    {
        this.x = xx;
        this.y = yy;
        if(leftChild != null || rightChild != null)
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
        if(height != 0)
        {
            if (width > height && width / height >= 1.25)
                splitH = false;
            else if (height > width && height / width >= 1.25)
                splitH = true;

            int max = (splitH ? height : width) - MIN_LEAF_SIZE;
            if(max <= MIN_LEAF_SIZE)
                return false;

            int split2 = (Random.Range(MIN_LEAF_SIZE, max))/3;
            int r = Random.Range(1,3);
            int split = split2*r;

            if(splitH)
            {
                leftChild = new Leaf(xx+_roomSpacePercentage+ split, yy+_roomSpacePercentage+ split, 
                                    width, height+ split,_wall, _floor);
                    split2 = (Random.Range(MIN_LEAF_SIZE, max))/6;
                   r = Random.Range(1,3);
                   split = split2*r;
                rightChild = new Leaf(xx+_roomSpacePercentage+ split, yy+_roomSpacePercentage  + split, 
                                    width, height - split,_wall, _floor);
            }
            else
            {
                leftChild = new Leaf(xx+_roomSpacePercentage+ split,  yy+_roomSpacePercentage+ split,  
                                    width+split, height,_wall, _floor);
                    split2 = (Random.Range(MIN_LEAF_SIZE, max))/6;
                    r = Random.Range(1,3);
                    split = split2*r;
                rightChild = new Leaf(xx+_roomSpacePercentage + split, yy+_roomSpacePercentage,  
                                    width - split, height,_wall, _floor);
            }
           
            return true;
        }
        else
        {
            return false;
        }
 

        
    }


}

