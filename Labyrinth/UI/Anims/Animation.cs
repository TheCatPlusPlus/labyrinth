using System;
using System.Collections.Generic;
using System.Threading;

using BearLib;

using Labyrinth.UI.Input;

namespace Labyrinth.UI.Anims
{
    public abstract class Animation
    {
        public void Run()
        {
            var frames = MakeFrames();
            var scene = State.Scene;

            while (true)
            {
                Terminal.Clear();
                scene.Draw();
                if (!frames.MoveNext())
                {
                    return;
                }

                Terminal.Refresh();

                var frameTime = frames.Current ?? Const.AnimFrameTime;

                if (!Event.HasNext())
                {
                    Thread.Sleep(frameTime);
                }

                if (Event.HasNext())
                {
                    return;
                }
            }
        }

        protected abstract IEnumerator<TimeSpan?> MakeFrames();
    }
}
