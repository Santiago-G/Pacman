﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Pacman;
using System.Net.NetworkInformation;
using System.ComponentModel.Design;
using MonoGame.Extended.Timers;
using MonoGame.Extended.Collections;

namespace LePacman.Screens.MainGame
{
    public abstract class Entity : SpriteBase
    {
        #region Textures
        private static protected Dictionary<EntityStates, Point> Textures = new Dictionary<EntityStates, Point>()
        {
            [EntityStates.ClosedPacman] = new Point(1, 23),
            [EntityStates.Pacman] = new Point(15, 23),
            [EntityStates.OpenPacman] = new Point(29, 23),

            [EntityStates.GhostChamber] = new Point(1, 97),

            [EntityStates.BlinkyRight] = new Point(1, 37),
            [EntityStates.BlinkyRightShifty] = new Point(16, 37),
            [EntityStates.BlinkyLeft] = new Point(31, 37),
            [EntityStates.BlinkyLeftShifty] = new Point(46, 37),
            [EntityStates.BlinkyUp] = new Point(61, 37),
            [EntityStates.BlinkyUpShifty] = new Point(76, 37),
            [EntityStates.BlinkyDown] = new Point(91, 37),
            [EntityStates.BlinkyDownShifty] = new Point(106, 37),

            [EntityStates.ClydeRight] = new Point(1, 52),
            [EntityStates.ClydeRightShifty] = new Point(16, 52),
            [EntityStates.ClydeLeft] = new Point(31, 52),
            [EntityStates.ClydeLeftShifty] = new Point(46, 52),
            [EntityStates.ClydeUp] = new Point(61, 52),
            [EntityStates.ClydeUpShifty] = new Point(76, 52),
            [EntityStates.ClydeDown] = new Point(91, 52),
            [EntityStates.ClydeDownShifty] = new Point(106, 52),

            [EntityStates.InkyRight] = new Point(1, 67),
            [EntityStates.InkyRightShifty] = new Point(16, 67),
            [EntityStates.InkyLeft] = new Point(31, 67),
            [EntityStates.InkyLeftShifty] = new Point(46, 67),
            [EntityStates.InkyUp] = new Point(61, 67),
            [EntityStates.InkyUpShifty] = new Point(76, 67),
            [EntityStates.InkyDown] = new Point(91, 67),
            [EntityStates.InkyDownShifty] = new Point(106, 67),

            [EntityStates.PinkyRight] = new Point(1, 82),
            [EntityStates.PinkyRightShifty] = new Point(16, 82),
            [EntityStates.PinkyLeft] = new Point(31, 82),
            [EntityStates.PinkyLeftShifty] = new Point(46, 82),
            [EntityStates.PinkyUp] = new Point(61, 82),
            [EntityStates.PinkyUpShifty] = new Point(76, 82),
            [EntityStates.PinkyDown] = new Point(91, 82),
            [EntityStates.PinkyDownShifty] = new Point(106, 82),

            [EntityStates.BlueWah] = new Point(76, 97),
            [EntityStates.BlueWahShifty] = new Point(91, 97),

            [EntityStates.WhiteWah] = new Point(76, 112),
            [EntityStates.WhiteWahShifty] = new Point(91, 112),

        };

        protected virtual EntityStates EntityState { get; set; }
        #endregion

        #region Grid Positions
        protected Point prevGridPos;
        protected Point gridPos;
        public Point localPos;

        public Point GridPosition
        {
            get => gridPos;
            set
            {
                prevGridPos = gridPos;
                gridPos = value;
            }
        }
        #endregion

        #region Movement and Timers

        public TimeSpan maxSpeed;
        public bool canMove = true;

        protected Vector2 startingPostion;
        protected Vector2 endPosition;

        public EntityStates currDirection;
        public static Dictionary<EntityStates, Point> directions = new Dictionary<EntityStates, Point>
        {
            [EntityStates.Up] = new Point(0, -1),
            [EntityStates.Right] = new Point(1, 0),
            [EntityStates.Down] = new Point(0, 1),
            [EntityStates.Left] = new Point(-1, 0),
        };

        public TimeSpan ļSpeed;
        protected TimeSpan timer;

        protected Vector2 diagonalOffset = new Vector2(0);
        #endregion

        #region Animation
        protected TimeSpan animationLimit;
        protected TimeSpan animationTimer;

        protected int animationDirection = -1;
        protected int animationMin = 0;
        protected int animationMax = 10;

        protected bool animate = true;
        #endregion

        PelletGrid mapGrid = PelletGrid.Instance;

        public float Scalar => (float)(timer.TotalMilliseconds / ļSpeed.TotalMilliseconds);
        public override Vector2 Origin { get => SourceRectangle.Size.ToVector2() / 2; }

        protected Point defaultSize;
        public Rectangle SourceRectangle => new Rectangle(Textures[EntityState], defaultSize);
        public Rectangle DestinationRectangle => new Rectangle(Position.ToPoint(), new Point((int)(defaultSize.X * Scale.X), (int)(defaultSize.Y * Scale.Y)));

        public Entity(Vector2 Position, Color Tint, Vector2 Scale, EntityStates EntityState, Point Coord) : base(MainGame.spriteSheet, Position, Tint)
        {
            this.Scale = Scale;
            this.EntityState = EntityState;
            Rotation = 0;
            GridPosition = Coord;
            GridPosition = Coord;
        }

        public Entity(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(MainGame.spriteSheet, Position, Tint)
        {
            this.Scale = Scale;
            Rotation = 0;
            GridPosition = Coord;
            GridPosition = Coord;
        }


        protected virtual void LaSillaDivertida()
        {
            return;
        }

        protected virtual bool NextPositionValid() => true;
        protected virtual void AnimationLogic()
        {
            if (animationTimer > animationLimit)
            {
                if ((int)EntityState >= animationMax || (int)EntityState <= animationMin)
                {
                    animationDirection *= -1;
                }

                EntityState += animationDirection;
                animationTimer = TimeSpan.Zero;
            }
        }

        public void EddenUpdate(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;

            //MainGame.pelletGrid[prevGridPos.X, prevGridPos.Y].currentState = States.Empty;
            localPos = Scalar >= .5f ? gridPos : prevGridPos;
            //MainGame.pelletGrid[localPos.X, localPos.Y].currentState = States.Debug2;

            if (timer >= ļSpeed)
            {
                LaSillaDivertida();

                if (NextPositionValid())
                {
                    GridPosition += directions[currDirection];
                    animate = true;

                    //

                    //29, 32
                    if (GridPosition.X == 0)
                    {
                        GridPosition = new Point(MainGame.pelletGrid.GetLength(0) - 1, GridPosition.Y);
                        prevGridPos = GridPosition;
                    }
                    else if (GridPosition.X == MainGame.pelletGrid.GetLength(0) - 1)
                    {
                        GridPosition = new Point(0, GridPosition.Y);
                        prevGridPos = GridPosition; 
                    }
                    else if (GridPosition.Y < 0)
                    {
                        GridPosition = new Point(GridPosition.X, MainGame.pelletGrid.GetLength(1) - 1);
                        prevGridPos = GridPosition;
                    }
                    else if (GridPosition.Y >= MainGame.pelletGrid.GetLength(1) - 1)
                    {
                        GridPosition = new Point(GridPosition.X, 0);
                        prevGridPos = GridPosition;
                    }

                    startingPostion = mapGrid.CoordToPostion(prevGridPos) + new Vector2(mapGrid.tileSize / 2);
                    endPosition = mapGrid.CoordToPostion(gridPos) + new Vector2(mapGrid.tileSize / 2);


                    diagonalOffset = new Vector2(0);
                }
                else
                {
                    GridPosition = GridPosition;
                    animate = false;
                }

                timer = TimeSpan.Zero;
            }
            
            Position = Vector2.Lerp(startingPostion, endPosition, 
                (float)(timer.TotalMilliseconds / ļSpeed.TotalMilliseconds));

            if (animate)
            {
                animationTimer += gameTime.ElapsedGameTime;

                AnimationLogic();
            }
        }

        public override void Update(GameTime gameTime)
        {
            //timer += gameTime.ElapsedGameTime;
            //Position = Vector2.Lerp(MainGame.CoordToPostion(prevGridPos), MainGame.CoordToPostion(gridPos), (float)(timer.TotalMilliseconds  / ļSpeed.TotalMilliseconds));

            if (animate)
            {
                animationTimer += gameTime.ElapsedGameTime;

                if (animationTimer > animationLimit)
                {
                    if ((int)EntityState >= animationMax || (int)EntityState <= animationMin)
                    {
                        animationDirection *= -1;
                    }

                    EntityState += animationDirection;
                    animationTimer = TimeSpan.Zero;
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, DestinationRectangle, SourceRectangle, Tint, Rotation, Origin, SpriteEffects.None, 0);
        }
    }
}
