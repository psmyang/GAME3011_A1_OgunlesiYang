using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public Color color;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(TileGame.instance.playerMode == PlayerMode.scanMode 
            && TileGame.instance.currentScanCount<TileGame.instance.MaxScanCount)
        {
            //print(this.gameObject.name);
            TileGame.instance.ShowMessage("("+this.gameObject.name +")" + " Scan");
            TileGame.instance.AddScanCount();
            string tileindex = this.gameObject.name;
            char[] spearator = { ',' };
            Int32 count = 2;
            string[] index = tileindex.Split(spearator);
            int x = int.Parse(index[0]);
            int y = int.Parse(index[1]);
            TileGame.instance.ScanTiles(x, y);


        }
        else if(TileGame.instance.playerMode == PlayerMode.scanMode)
        {
            TileGame.instance.ShowMessage("Used All Scan");

        }

        if (TileGame.instance.playerMode == PlayerMode.extractMode
            && TileGame.instance.currentExtractCount < TileGame.instance.MaxExtractCount)
        {
            //print(this.gameObject.name);
            TileGame.instance.ShowMessage("(" + this.gameObject.name + ")" + " Extract");
            TileGame.instance.AddExtractCount();
            string tileindex = this.gameObject.name;
            char[] spearator = { ',' };
            Int32 count = 2;
            string[] index = tileindex.Split(spearator);
            int x = int.Parse(index[0]);
            int y = int.Parse(index[1]);
            TileGame.instance.ExtractTiles(x, y);


        }
        else if (TileGame.instance.playerMode == PlayerMode.extractMode)
        {
            TileGame.instance.ShowMessage("Extract All resources GameOver");

        }
    }
}
