using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Chaotx.Mgx.Controls.Menus;
using Chaotx.Mgx.Layout;
using Chaotx.Mgx.Views;

using System.Collections.Generic;
using System.Linq;
using System;

namespace Chaotx.Mgx.Demo {
    public class DemoView : FadingView {
        private int gridWidth = 10;
        private int gridHeight = 10;

        public DemoView(LayoutPane root) : base(root) {}

        protected override void Init() {
            // fill grids with random colored images
            var grid00 = GetItem<GridPane>("slp00.gdp");
            var grid10 = GetItem<GridPane>("slp01.gdp");
            var grid01 = GetItem<GridPane>("slp10.gdp");
            var grid11 = GetItem<GridPane>("slp11.gdp");
            var grids = new GridPane[]{grid00, grid10, grid01, grid11};

            foreach(var grid in grids) {
                for(int x, y = 0; y < gridHeight; ++y) {
                    for(x = 0; x < gridWidth; ++x) {
                        var img = Content.Load<Texture2D>("textures/blank");
                        var mni = new MenuItem(img);
                        mni.ImageItem.KeepAspectRatio = true;
                        mni.ImageItem.Color = RandomColor();
                        mni.HGrow = mni.VGrow = 1;
                        InitMenuItemEvents(mni);
                        grid.Set(x, y, mni);
                    }
                }
            }

            // define slideshow behaviour
            var slp00 = GetItem<SlidingPane>("slp00");
            var slp10 = GetItem<SlidingPane>("slp10");
            var slp01 = GetItem<SlidingPane>("slp01");
            var slp11 = GetItem<SlidingPane>("slp11");

            List<SlidingPane> textSlider =
                GetItem<StackPane>("stpStory")
                .Children.Cast<SlidingPane>().ToList();

            slp00.SlidedIn += (s, a) => slp10.SlideIn();
            slp10.SlidedIn += (s, a) => slp01.SlideIn();
            slp01.SlidedIn += (s, a) => slp11.SlideIn();
            slp11.SlidedIn += (s, a) => {
                slp00.SlideOut();
                slp10.SlideOut();
                slp01.SlideOut();
                slp11.SlideOut();
            };

            var fdpTroll = GetItem<FadingPane>("fdpTroll");
            fdpTroll.FadedOut += (s, a) => slp00.SlideIn(); // restarts loop

            var slpQuit = textSlider[0];
            slpQuit.SlidedIn += (s, a) => {
                // does nothing but prevents further drawing
                // TODO fix bug: lock position when slided in?
                slpQuit.SlideOut(slpQuit.GenericTarget);
                fdpTroll.FadeIn();
            };

            var mniQuit = GetItem<MenuItem>("mniQuit");
            var oldColor = mniQuit.TextItem.Color;
            mniQuit.FocusGain += (s, a) => mniQuit.TextItem.Color = Color.Yellow;
            mniQuit.FocusLoss += (s, a) => mniQuit.TextItem.Color = oldColor;
            mniQuit.Action += (s, a) => Close();

            if(textSlider.Count == 1) return;
            slp11.SlidedIn += (s, a) =>
                textSlider[1].SlideIn(GenericPosition.Bottom);

            var rng = new Random();
            var dirs = new  GenericPosition[]{
                GenericPosition.Left,
                GenericPosition.Top,
                GenericPosition.Right,
                GenericPosition.Bottom
            };

            for(int i = 1; i < textSlider.Count; ++i) {
                var curr = textSlider[i];

                if(i == textSlider.Count-1) curr.SlidedOut += (s, a) => {
                    int r = rng.Next(dirs.Length);
                    var quitIn = dirs[r];
                    var quitOut = dirs[(r + dirs.Length/2)%dirs.Length];
                    slpQuit.SlideIn(quitIn, quitOut);
                }; else {
                    var succ = textSlider[i+1];

                    curr.SlidedIn += (s, a) =>
                        succ.SlideIn(GenericPosition.Bottom);
                }

                curr.SlidedIn += (s, a) =>
                    curr.SlideOut(dirs[rng.Next(dirs.Length-1)]);
            }
        }

        private void InitMenuItemEvents(MenuItem mni) {
            var imgTroll = Content.Load<Texture2D>("textures/trollface");
            var oldColor = mni.ImageItem.Color;

            mni.FocusGain += (s, a) =>  {
                mni.ImageItem.Color = Color.White;
                mni.Alpha = 0.75f;
            };

            mni.FocusLoss += (s, a) => {
                mni.ImageItem.Color = oldColor;
                mni.Alpha = 1;
            };

            mni.Action += (s, a) => mni.ImageItem.Image = imgTroll;
        }

        private static Random colorRng;
        private static Color RandomColor() {
            if(colorRng == null) colorRng = new Random();

            return Color.FromNonPremultiplied(
                1 + colorRng.Next(255),
                1 + colorRng.Next(255),
                1 + colorRng.Next(255), 255);
        }
    }
}