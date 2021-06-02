using Assignment.Characters;
using Assignment.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Manager
{
    enum CamDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    class CameraManager
    {
        public static Vector2 MapMovement(GraphicsDevice graphicsDevice, Vector2 worldSize)
        {
            Vector2 worldOrigin = worldSize;
            return worldOrigin;
        }
        //Get Unit Viewport Position
        public static Vector2 ViewportPos(Vector2 _mapPos, ref Level _level)
        {
            return _mapPos + _level.worldOrigin;
        }
        public static void Movement(Unit _unit, Level _level, GraphicsDevice _graphicsDevice)
        {
            Vector2 center = new Vector2(
                _graphicsDevice.Viewport.Width / 2,
                _graphicsDevice.Viewport.Height / 2);
            Vector2 distance = center - _unit.centerPos;
            if (_unit.centerPos.X > center.X)//Map Moves to Right
            {
                CameraViewport(CamDirection.Right, distance.X, ref _level, _graphicsDevice);
            }
            else if (_unit.centerPos.X < center.X)//Map Moves to Left
            {
                CameraViewport(CamDirection.Left, distance.X, ref _level, _graphicsDevice);
            }
            if (_unit.centerPos.Y > center.Y)//Map Moves to Down
            {
                CameraViewport(CamDirection.Down, distance.Y, ref _level, _graphicsDevice);
            }
            else if (_unit.centerPos.Y < center.Y)//Map Moves to Up
            {
                CameraViewport(CamDirection.Up, distance.Y, ref _level, _graphicsDevice);
            }
            _level.camOrigin = _level.worldSize + _level.worldOrigin;

        }
        public static void CameraViewport(CamDirection _camDirection, float _distance, ref Level _level, GraphicsDevice _graphicsDevice)
        {
            Vector2 nextWorldOrigin = _level.worldOrigin;
            switch (_camDirection)
            {
                case CamDirection.Up:
                    if (!(_level.worldOrigin.Y >= 0))
                        nextWorldOrigin.Y += _distance;
                    break;
                case CamDirection.Down:
                    if (!(_level.camOrigin.Y <= _graphicsDevice.Viewport.Height))
                        nextWorldOrigin.Y += _distance;
                    break;
                case CamDirection.Left:
                    if (!(_level.worldOrigin.X >= 0))
                        nextWorldOrigin.X += _distance;
                    break;
                case CamDirection.Right:
                    if (!(_level.camOrigin.X <= _graphicsDevice.Viewport.Width))
                        nextWorldOrigin.X += _distance;
                    break;
            }
            _level.worldOrigin = Vector2.Lerp(_level.worldOrigin, nextWorldOrigin, 0.05f);
            ResetWorldOrigin(ref _level, _graphicsDevice);
        }
        public static void Drag(MouseState _ms, MouseState _lastMs, Level _level, GraphicsDevice _graphicsDevice)
        {
            if (_ms.LeftButton == ButtonState.Pressed)
            {
                if (_lastMs.LeftButton == ButtonState.Released)
                {
                    _level.lastMousePos = _ms.Position.ToVector2();
                    _level.lastWorldOrign = _level.worldOrigin;
                }
                Vector2 dragDistance = _ms.Position.ToVector2() - _level.lastMousePos;
                _level.worldOrigin = _level.lastWorldOrign + dragDistance;
                CameraManager.ResetWorldOrigin(ref _level, _graphicsDevice);
            }
        }
        private static void ResetWorldOrigin(ref Level _level, GraphicsDevice _graphicsDevice)
        {
            if (_level.worldOrigin.Y >= 0)
                _level.worldOrigin.Y = 0;
            else if (_level.worldSize.Y + _level.worldOrigin.Y <= _graphicsDevice.Viewport.Height)
                _level.worldOrigin.Y = -(_level.worldSize.Y - _graphicsDevice.Viewport.Height);
            if (_level.worldOrigin.X >= 0)
                _level.worldOrigin.X = 0;
            else if (_level.worldSize.X + _level.worldOrigin.X <= _graphicsDevice.Viewport.Width)
                _level.worldOrigin.X = -(_level.worldSize.X - _graphicsDevice.Viewport.Width);
        }
    }
}
