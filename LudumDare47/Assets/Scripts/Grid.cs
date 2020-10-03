// using UnityEngine;
// using Utils;
//
// public class Grid
// {
//     public int Width => _gridArray.GetLength(dimension: 0);
//     public int Height => _gridArray.GetLength(dimension: 1);
//
//     private int _width;
//     private int _height;
//
//     private readonly int[,] _gridArray;
//
//     public Grid(int width, int height)
//     {
//         _width = width;
//         _height = height;
//
//         _gridArray = new int[width, height];
//
//         for (int x = 0; x < _gridArray.GetLength(dimension: 0); x++)
//         {
//             for (int y = 0; y < _gridArray.GetLength(dimension: 1); y++)
//             {
//                 UtilsClass.CreateWorldText(text: _gridArray[x, y].ToString(),
//                                            parent: null,
//                                            localPosition: new Vector3(x: x, y: y),
//                                            fontSize: 85,
//                                            characterSize: 0.05f,
//                                            color: Color.white,
//                                            textAnchor: TextAnchor.MiddleCenter);
//                 Debug.DrawLine(start: new Vector3(x: x, y: y),
//                                end: new Vector3(x: x, y: y + 1),
//                                color: Color.gray,
//                                duration: 100f);
//                 Debug.DrawLine(start: new Vector3(x: x, y: y),
//                                end: new Vector3(x: x + 1, y: y),
//                                color: Color.gray,
//                                duration: 100f);
//             }
//         }
//
//         Debug.DrawLine(start: new Vector3(x: 0, y: height),
//                        end: new Vector3(x: width, y: height),
//                        color: Color.gray,
//                        duration: 100f);
//         Debug.DrawLine(start: new Vector3(x: width, y: height),
//                        end: new Vector3(x: width, y: 0),
//                        color: Color.gray,
//                        duration: 100f);
//     }
// }