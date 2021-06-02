using Assignment.Characters;
using Assignment.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Manager
{
    class Collider
    {
        public Unit unit;
        public Rectangle colMove, colBody;
        Texture2D texture;
        public Collider(Unit _thisObject)
        {
            unit = _thisObject;
            texture = _thisObject.LoadTextureContent("Images//White");
        }
        public void Update()
        {
            colBody.Size = unit.currRect.Size;
            colBody.X = (int)unit.viewportPos.X;
            colBody.Y = (int)unit.viewportPos.Y;
            colMove = colBody;
            colMove.X += 25;
            colMove.Y += unit.currRect.Height/2;
            colMove.Width -= 50;
            colMove.Height /= 2;
        }
        public void CollisionBoundary(ref Wall _target)
        {

            if (IsTouchingRight(ref _target))
            {
                unit.mapPos.X = unit.lastMapPos.X;
                //unit.mapPos.X += 0.1f;
            }
            if (IsTouchingLeft(ref _target))
            {
                unit.mapPos.X = unit.lastMapPos.X;
                //unit.mapPos.X -= 0.1f;
            }
            if (IsTouchingBottom(ref _target))
            {
                unit.mapPos.Y = unit.lastMapPos.Y;
                //unit.mapPos.Y += 0.1f;
            }
            if (IsTouchingTop(ref _target))
            {
                unit.mapPos.Y = unit.lastMapPos.Y;
                //unit.mapPos.Y -= 0.1f;
            }

        }
        protected bool IsTouchingLeft(ref Wall wall)
        {
            return colMove.Right + unit.currSpeed.X > wall.rectangle.Left &&
              colMove.Left < wall.rectangle.Left &&
              colMove.Bottom > wall.rectangle.Top &&
              colMove.Top < wall.rectangle.Bottom;
        }

        protected bool IsTouchingRight(ref Wall wall)
        {
            return colMove.Left + unit.currSpeed.X < wall.rectangle.Right &&
              colMove.Right > wall.rectangle.Right &&
              colMove.Bottom > wall.rectangle.Top &&
              colMove.Top < wall.rectangle.Bottom;
        }

        protected bool IsTouchingTop(ref Wall wall)
        {
            return colMove.Bottom + unit.currSpeed.Y > wall.rectangle.Top &&
              colMove.Top < wall.rectangle.Top &&
              colMove.Right > wall.rectangle.Left &&
              colMove.Left < wall.rectangle.Right;
        }

        protected bool IsTouchingBottom(ref Wall wall)
        {
            return colMove.Top + unit.currSpeed.Y < wall.rectangle.Bottom &&
              colMove.Bottom > wall.rectangle.Bottom &&
              colMove.Right > wall.rectangle.Left &&
              colMove.Left < wall.rectangle.Right;
        }
        
        public void DrawRect(SpriteBatch _spriteBatch, Color? _color = null)
        {
            Color color = _color ?? Color.Black;
            
            _spriteBatch.Draw(texture, colBody, color*0.5f);
        }
        public void DrawRectBord(SpriteBatch _spriteBatch, int bw = 2, Color? _color = null)
        {
            Color color = _color ?? Color.Black;
            // Top
            _spriteBatch.Draw(texture, 
                new Rectangle(colMove.Left, colMove.Top, colMove.Width + bw, bw), color);
            // Bottom
            _spriteBatch.Draw(texture, 
                new Rectangle(colMove.Left, colMove.Bottom, colMove.Width + bw, bw), color);
            // Left
            _spriteBatch.Draw(texture, 
                new Rectangle(colMove.Left, colMove.Top, bw, colMove.Height + bw), color);
            // Right
            _spriteBatch.Draw(texture, 
                new Rectangle(colMove.Right, colMove.Top, bw, colMove.Height + bw), color);
        }
        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {

            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }
        public bool CheckCollision(Vector2 _pos, Rectangle _rect, Point _textureSize, ref Color[] _data, Vector2? _rectCenter, float _rotAngle = 0)
        {
            Matrix transform =
                Matrix.CreateTranslation(new Vector3(-_rectCenter ?? Vector2.Zero, 0)) *
                Matrix.CreateRotationZ(_rotAngle) *
                Matrix.CreateTranslation(new Vector3(_pos, 0));
            //_rect = CalculateBoundingRectangle(_rect, transform);
            if (CalculateBoundingRectangle(_rect, transform).Intersects(colBody))
            {
                if (PixelCollision(
                    Matrix.CreateTranslation(new Vector3(unit.viewportPos, 0)),
                    unit.currRect, unit.currTexture.Width, ref unit.currData,
                    transform, _rect, _textureSize.X, ref _data))
                    return true;
            }
            return false;
        }
        private bool PixelCollision(
           Matrix transformA, Rectangle rectA, int widthA, ref Color[] dataA,
           Matrix transformB, Rectangle rectB, int widthB, ref Color[] dataB)
        {
            Matrix AToB = transformA * Matrix.Invert(transformB);
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, AToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, AToB);
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, AToB);
            for (int yA = rectA.Top; yA < rectA.Bottom; yA++)
            { 	// For each row in A
                Vector2 posInB = yPosInB; // At the beginning of the row
                for (int xA = rectA.Left; xA < rectA.Right; xA++)
                { // For each pixel in the row
                    int xB = (int)Math.Round(posInB.X)+rectB.Left;
                    int yB = (int)Math.Round(posInB.Y);
                    if (0 <= xB && xB < widthB && 0 <= yB && yB < rectB.Height && xB >= rectB.Left && xB < rectB.Right)
                    {
                        try
                        {
                            Color colorA = dataA[xA + yA * widthA];
                            Color colorB = dataB[xB + yB * widthB];
                            if (colorA.A != 0 && colorB.A != 0)
                                return true;
                        }
                        catch(Exception e)
                        {
                            return false;
                        }
                        
                    }
                    posInB += stepX; // Move to next pixel in the row
                }
                yPosInB += stepY; // Move to the next row
            }
            return false; // No intersection found
        }
    }
}
