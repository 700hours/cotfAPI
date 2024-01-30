using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using cotf.Base;
using System.Threading;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using System.Numerics;
using System.ComponentModel;

namespace cotf
{
    public sealed class LightPass
    {
        static List<Tile> NearbyTile(Lamp lamp, Tile[,] tile)
        {
            List<Tile> brush = new List<Tile>();
            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {
                    if (tile[i, j] != null && tile[i, j].active)
                    {
                        if (Helper.Distance(tile[i, j].Center, lamp.position) < lamp.range)
                        {
                            brush.Add(tile[i, j]);
                        }
                    }
                }
            }
            return brush;
        }
        static List<Wall> NearbyFloor(Lamp lamp, Wall[,] wall)
        {
            List<Wall> brush = new List<Wall>();
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                for (int j = 0; j < wall.GetLength(1); j++)
                {
                    if (wall[i, j] != null && wall[i, j].active)
                    {
                        if (Helper.Distance(wall[i, j].Center, lamp.position) < lamp.range)
                        {
                            brush.Add(wall[i, j]);
                        }
                    }
                }
            }
            return brush;
        }
        public static void PreProcessing(Lamp[] lamp, Wall[,] wall, Tile[,] tile)
        {
            for (int n = 0; n < lamp.Length; n++)
            {
                Lamp _lamp = lamp[n];
                if (_lamp == null || !_lamp.active || _lamp.owner != 255)
                    continue;

                List<Tile> brush = NearbyTile(_lamp, tile);
                
                int i = wall.GetLength(0) - 1;
                int j = wall.GetLength(1) - 1;
                int num = i + j;

                //  DEBUG
                _lamp.range = 600f;
                while (num-- > 0)
                {
                    new Thread(() => 
                    {
                        BEGIN:
                        i--;
                        i = Math.Min(Math.Max(0, i), wall.GetLength(0) - 1);
                        j = Math.Min(Math.Max(0, j), wall.GetLength(0) - 1);
                        if (wall[i, j] == null || !wall[i, j].active)
                        {
                        }
                        else
                        {
                            if (Helper.Distance(wall[i, j].Center, _lamp.Center) <= _lamp.range)
                            {
                                try
                                { 
                                    lock (wall[i, j].texture)
                                    {
                                        wall[i, j].texture = Drawing.Lightpass0(brush, wall[i, j].texture, wall[i, j].position, _lamp, _lamp.range);
                                    }
                                }
                                catch 
                                {
                                    if (i == 0)
                                    {
                                        i = wall.GetLength(0) - 1;
                                        --j;
                                        goto NEXT; 
                                    }
                                    else goto BEGIN;
                                }
                            }
                        }
                        NEXT:
                        i = Math.Min(Math.Max(0, i), wall.GetLength(0) - 1);
                        j = Math.Min(Math.Max(0, j), wall.GetLength(0) - 1);
                        if (wall[i, j] == null || !wall[i, j].active)
                        {
                        }
                        else 
                        {
                            if (Helper.Distance(wall[i, j].Center, _lamp.Center) <= _lamp.range)
                            {
                                try
                                {
                                    lock (wall[i, j].texture)
                                    {
                                        wall[i, j].texture = Drawing.Lightpass0(brush, wall[i, j].texture, wall[i, j].position, _lamp, _lamp.range);
                                    }
                                }
                                catch 
                                { 
                                }
                            }
                        }
                    }).Start();
                }
            }
        }
    }
}
