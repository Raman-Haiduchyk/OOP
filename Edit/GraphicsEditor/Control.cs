﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Reflection;

namespace Edit
{
    static class Control           // класс организации взаимодействия GUI с классом хранения данных 
    {

        private static List<PointF> renderPoints = new List<PointF>();

        public static int Sides { get; set; } = 5;

        public static int BorderWidth { get; set; } = 3;

        //Программная инициализация списка (для 1 задания)
        public static void ListInitialization(Model model)
        {
            model.Figures = new List<Figure>();
            Rectangle rect = new Rectangle("Rect", Color.FromKnownColor(KnownColor.White), BorderWidth, 
                                            new PointF(20, 20), new PointF(40, 40));
            model.Figures.Add(rect);
            Parallelogram par = new Parallelogram("par", Color.FromKnownColor(KnownColor.White), BorderWidth, 
                                                   new PointF(6, 100), new PointF(100, 150), new PointF(120, 200));
            model.Figures.Add(par);
        }

        //Отрисовка списка фигур
        public static void DrawFigureList(Model model, Graphics graphics)
        {
            foreach (Figure figure in model.Figures)
            {
                figure.Draw(graphics);
            }
        }

        public static bool PreRenderFigure(Graphics graphics, int index, Model data)
        {
            bool result = false;
            if (renderPoints.Count() > 1)
            {
                MethodInfo draw = data.Types[index].GetMethod("PreRender");
                result = true;
                object value;
                if (data.Types[index].Name == "RegularPolygon")
                {
                    value = draw.Invoke(null, new object[] { graphics, Sides, renderPoints.ToArray() });
                }
                else
                {
                    value = draw.Invoke(null, new object[] { graphics, renderPoints.ToArray() });
                }
                if ((bool)value)
                {
                    FinishDrawingFigure(index, data);
                    result = false;
                }
            }
            return result;
        }

        public static void FinishDrawingFigure(int index, Model data)
        {
            renderPoints.RemoveAt(renderPoints.Count() - 1);
            try
            {
                if (data.Types[index].Name == "RegularPolygon")
                {
                    Figure shape = (Figure)Activator.CreateInstance(data.Types[index], new object[] 
                    { "pol", Color.FromKnownColor(KnownColor.White), BorderWidth, Sides, renderPoints.ToArray() });
                    data.Figures.Add(shape);
                }
                else
                {
                    Figure shape = (Figure)Activator.CreateInstance(data.Types[index], new object[] 
                    { "pol", Color.FromKnownColor(KnownColor.White), BorderWidth, renderPoints.ToArray() });
                    data.Figures.Add(shape);
                }
            }
            finally { }
            renderPoints.Clear();
        }

        public static void OnMouseMoveEvent(bool renderingFlag, PointF point)
        {
            if (renderingFlag)
            {
                renderPoints.RemoveAt(renderPoints.Count() - 1);
                renderPoints.Add(point);
            }
        }

        public static void OnMouseLeftClickEvent(bool renderingFlag, PointF point)
        {
            if (!renderingFlag)
            {
                renderPoints.Add(point);   //добавление еще одной точки, если отрисовка только началась
            }
            renderPoints.Add(point);
        }


        // отмена отрисовки по нажатию пкм
        public static void OnMouseRightClickEvent(bool renderingFlag)
        {
            if (renderingFlag)
            {
                renderPoints.Clear();
            }
        }


        //завершение отрисовки многоугольника по двойному нажатию
        public static void OnMouseDoubleClickEvent(bool renderingFlag, PointF point, int index, Model data)
        {
            if (renderingFlag && data.Types[index].Name == "Polygon")
            {
                renderPoints.RemoveAt(renderPoints.Count() - 1);
                renderPoints.Add(point);
                FinishDrawingFigure(index, data);
            }
        }





    }
}