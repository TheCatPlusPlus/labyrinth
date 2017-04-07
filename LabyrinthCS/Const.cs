using System;

namespace Labyrinth
{
    public static class Const
    {
        public const int Width = 120;
        public const int Height = 35;
        public const int WidthSidebar = 30;
        public const int WidthSidebarGauge = 10;
        public const int WidthPlayerName = WidthSidebar - 6;
        public const int WidthViewport = Width - WidthSidebar - 3;
        public const int HeightMessages = 10;
        public const int HeightBossMeter = 3;
        public const int HeightViewport = Height - HeightMessages - HeightBossMeter - 4;

        public const int SpeedBase = 100;
        public const int MoveCostBase = SpeedBase;

        public const int FovRadius = 4;

        public static readonly TimeSpan AnimFrameTime = TimeSpan.FromMilliseconds(25);
    }
}
