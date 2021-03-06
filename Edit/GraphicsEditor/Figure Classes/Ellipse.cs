﻿using System;
using System.Linq;
using System.Drawing;
using FigureClass;

namespace GraphicsEditor
{
    [Serializable]
    class Ellipse : Figure
    {
        public float MajorSemiaxis { get; set; }
        public float MinorSemiaxis { get; set; }    // полуоси (для будущего редактирования фигур)

        protected PointF Center;               


        // В конструктор передаются левая верхняя и
        // правая нижняя точки, ограничивающего прямоугольника
        public Ellipse(string str, Color brushCol, Color penColor, int borderWidth, params PointF[] points) : base(str, brushCol, penColor, borderWidth)
        { 
            PointF A = points[0];
            PointF B = points[1];
            Center = new PointF(A.X + (B.X - A.X) / 2, A.Y + (B.Y - A.Y) / 2);
            MajorSemiaxis = Math.Abs(B.X - A.X) / 2;
            MinorSemiaxis = Math.Abs(B.Y - A.Y) / 2;
        }

        protected Ellipse(string str, Color brushCol, Color penColor, int borderWidth) : base(str, brushCol, penColor, borderWidth)
        {
        }

        public override void Draw(Graphics graphics)
        {
            Pen FigurePen = new Pen(PenColor, BorderWidth);
            SolidBrush FigureBrush = new SolidBrush(BrushColor);
            graphics.FillEllipse(FigureBrush, Center.X - MajorSemiaxis, Center.Y - MinorSemiaxis, 
                2 * MajorSemiaxis, 2 * MinorSemiaxis);
            graphics.DrawEllipse(FigurePen, Center.X - MajorSemiaxis, Center.Y - MinorSemiaxis, 
                2 * MajorSemiaxis, 2 * MinorSemiaxis);
        }


        public static bool Preview(Graphics graphics, params PointF[] renderPoints)
        {
            if (renderPoints.Count() == 2)
            {
                PointF A = renderPoints[0];
                PointF B = renderPoints[1];
                float a = (B.X - A.X);
                float b = (B.Y - A.Y);
                graphics.DrawEllipse(new Pen(prevCol), A.X, A.Y, a, b);
                return false;
            }
            else if (renderPoints.Count() > 2)
            {
                return true;
            }
            else return false;
        }
    }
}
