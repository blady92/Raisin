﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Cyber2O
{
    public class StageParser
    {
        Dictionary<Color, IGenerable> dict;
        public Stage ParseBitmap(string path)
        {
            Stage stage = new Stage();

            Bitmap bitmap = new Bitmap(path);
            stage.Height = bitmap.Height;
            stage.Width = bitmap.Width;
            /*
            Dictionary<Color, IGenerable> dict = new Dictionary<Color, IGenerable>();
            dict.Add(Color.Black, new Corridor());
            dict.Add(Color.Gray, new Room());
            dict.Add(Color.Yellow, new StageObject());
            dict.Add(Color.Red, new StageNPC());
            */

            dict = new Dictionary<Color, IGenerable>();
            dict.Add(Color.FromArgb(0, 0, 0), new Corridor(bitmap.Height, bitmap.Width));
            dict.Add(Color.FromArgb(128, 128, 128), new Room(bitmap.Height, bitmap.Width));
            dict.Add(Color.FromArgb(255, 255, 0), new StageObject(bitmap.Height, bitmap.Width));
            dict.Add(Color.FromArgb(224, 32, 64), new StageNPC(bitmap.Height, bitmap.Width));

            bool[,] hashTable = new bool[bitmap.Height, bitmap.Width];
            /*
            for (int row = 0; row < bitmap.Height; row++)
            {
                for (int col = 0; col < bitmap.Width; col++)
                {
                    hashTable[row, col] = false;
                }
            } 
            */
            for (int row = 0; row < bitmap.Height; row++)
            {
                for (int col = 0; col < bitmap.Width; col++) // przeszukujemy całą bitmapę
                {
                    if (hashTable[row,col] == true) // jak pole już dodane
                    {
                        continue; // lecimy dalej
                    }

                    Color color = bitmap.GetPixel(col, row);
                    //color = Color.FromArgb(color.A, color.R, color.G, color.B);
                    if (dict.ContainsKey(color)) // jeśli znaczący kolor
                    {
                        IGenerable generable = RecursiveSearch(bitmap, col, row); // pobieramy cały złożony obiekt
                        InsertInto(stage, generable); // wrzucamy obiekt do poziomu
                        for (int i = 0; i < bitmap.Height; i++)
                        {
                            for (int j = 0; j < bitmap.Width; j++) // oznaczamy pola tej przestrzeni, którą żeśmy ogarnęli, jako sprawdzone
                            {
                                if (generable.Structure[j, i] == true)
                                {
                                    Color newColor = bitmap.GetPixel(j, i);
                                    if (!(newColor.Equals(Color.FromArgb(224, 32, 64)) || newColor.Equals(Color.FromArgb(255, 255, 0)))) // patrz TODO niżej, zrobię to ładniej, ale późno już, a ja rano w pole jadę zboże siać :v
                                    {
                                        hashTable[i, j] |= generable.Structure[j, i];
                                    }
                                }
                            }
                        }
                    }
                }
            } 

            return stage;
        }

        private void InsertInto(Stage stage, IGenerable generable)
        {
            if (generable is Corridor)
            {
                stage.Corridors.Add(generable as Corridor);
            }
            else if (generable is Room)
            {
                stage.Rooms.Add(generable as Room);
            }
            else if (generable is StageNPC)
            {
                stage.NPCs.Add(generable as StageNPC);
            }
            else if (generable is StageObject)
            {
                stage.Objects.Add(generable as StageObject);
            }
        }

        private IGenerable RecursiveSearch(Bitmap bitmap, int x, int y)
        {


            Color color = bitmap.GetPixel(x, y);
            IGenerable representative = dict[color].clone();
            if (representative.IsSingleBlock())
            {
                representative.Structure[x, y] = true;
                return representative;
            }
            

            Queue<Pair<int, int> > queue = new Queue<Pair<int, int> >(); // Tu nakurwiam przeszukiwanie BFS
            queue.Enqueue(new Pair<int, int>(x, y));
            while (queue.Count > 0)
            {
                Pair<int, int> coords = queue.Dequeue(); // Wyciąganie z kolejki
                Color newColor = bitmap.GetPixel(coords.X, coords.Y);
                if (representative.Structure[coords.X, coords.Y] == true) // jak już to pole było ogarniane
                {
                    continue; // lecimy dalej
                }
                if (newColor.Equals(color) ||  // jeżeli pole ma kolor taki jak ten co go uzupełniamy, albo to jest pole wewnątrz pokoju/korytarza
                    newColor.Equals(Color.FromArgb(224, 32, 64)) || newColor.Equals(Color.FromArgb(255, 255, 0))) // TODO: Janek popraw to później kurwa bo wstyd - Janek
                {
                    representative.Structure[coords.X, coords.Y] = true;
                    if (coords.X > 1)
                    {
                        queue.Enqueue(new Pair<int, int>(coords.X - 1, coords.Y));
                    }
                    if (coords.X < bitmap.Width - 1)
                    {
                        queue.Enqueue(new Pair<int, int>(coords.X + 1, coords.Y));
                    }
                    if (coords.Y > 1)
                    {
                        queue.Enqueue(new Pair<int, int>(coords.X, coords.Y - 1));
                    }
                    if (coords.Y < bitmap.Height - 1)
                    {
                        queue.Enqueue(new Pair<int, int>(coords.X, coords.Y + 1));
                    }
                }
            }
            return representative;
        }
    }

    public class Pair<T, K>
    {
        public Pair(T x, K y)
        {
            X = x;
            Y = y;
        }
        public T X { set; get; }
        public K Y { set; get; }
    }
}
