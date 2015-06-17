using System;
using UIKit;
using SpeechAuth.Views;
using CorePlot;
using CoreGraphics;
using Foundation;

namespace SpechAuth.iOS.Views.GraphsPage
{
    public partial class GraphsViewController
    {
        private UIButton  _startRecord;
        private UIButton  _stopRecord;

        private static CPTXYGraph _sourceGraph;
        private static CPTXYGraph _transformGraph;

        private UIView _loading;

        public override void LoadView ()
        {
            base.LoadView ();
            InitializeControls ();
        }

        private void InitializeControls ()
        {
            View.BackgroundColor = Theme.Color.MainBackgroundColor;

            SetupGraphs ();

            UIScrollView scrollView;
            StackPanel stackPanel;

            View.AddSubviews (
                scrollView = new UIScrollView ()
                .WithFrame (0, 0, View.Frame.Width, View.Frame.Height)
                .WithTune (tune => {
                    tune.ContentInset = new UIEdgeInsets(0, 0, 20, 0);
                    tune.ScrollEnabled = true;
                    tune.ShowsVerticalScrollIndicator = false;
                })
                .WithSubviews (
                    stackPanel = new StackPanel (StackPanelOrientation.Vertical)
                    .WithFrame (0, 0, View.Frame.Width, 300)
                    .WithSubviews (
                        Theme.Label.InfoLabel (Theme.Font.HelveticaNeue_Regular(20), UIColor.White, "Исходный сигнал")
                        .WithFrame(20, 10, 200, 30),
                        new CPTGraphHostingView {
                            Frame = new CGRect (0, 5, View.Frame.Width, 220),
                            HostedGraph = _sourceGraph
                        },
                        Theme.Label.InfoLabel (Theme.Font.HelveticaNeue_Regular(20), UIColor.White, "Преобразованный сигнал")
                        .WithFrame(10, 10, 300, 30),
                        new CPTGraphHostingView {
                            Frame = new CGRect (0, 5, View.Frame.Width, 220),
                            HostedGraph = _transformGraph
                        },
                        _startRecord = Theme.Button.ImageResizableButton ("Images/BlueButtonBackground.png", "Images/BlueButtonBackground_Pressed.png", new UIEdgeInsets (8, 8, 8, 8))
                        .WithFrame (25, 60, View.Frame.Width - 50, 60)
                        .WithTune (tune => {
                            tune.SetTitleColor (UIColor.White, UIControlState.Normal);
                            tune.SetTitle ("Начать запись", UIControlState.Normal);
                        }),
                        _stopRecord = Theme.Button.ImageResizableButton ("Images/BlueButtonBackground.png", "Images/BlueButtonBackground_Pressed.png", new UIEdgeInsets (8, 8, 8, 8))
                        .WithFrame (25, 30, View.Frame.Width - 50, 60)
                        .WithTune (tune => {
                            tune.SetTitleColor (UIColor.White, UIControlState.Normal);
                            tune.SetTitle ("Завершить запись", UIControlState.Normal);
                        })
                    )
                ),
                _loading = new UIView()
                .WithFrame (0, 64, View.Frame.Width, View.Frame.Height)
                .WithBackground (UIColor.FromRGBA (0, 0, 0, (nfloat)0.7))
                .WithSubviews (
                    new UIActivityIndicatorView()
                    .WithFrame ((View.Frame.Width - 30)/2, (View.Frame.Height - 30)/2, 30, 30)
                    .WithTune (tune => {
                        tune.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
                        tune.StartAnimating();
                    })
                )
            );

            stackPanel.LayoutSubviews ();

            scrollView.ContentSize = new CGSize (stackPanel.Frame.Width, stackPanel.Frame.Height + 20);
        }

        private void SetupGraphs ()
        {
            SetupGraph (ref _sourceGraph);
            SetupGraph (ref _transformGraph);

            _sourceGraph.Title = "";
            _transformGraph.Title = "";

            _lineColor = CPTColor.WhiteColor;
        }

        void SetupGraph (ref CPTXYGraph graph)
        {
            graph = new CPTXYGraph (new CGRect (0, 0, 200, 200), CPTScaleType.Linear, CPTScaleType.Linear) 
            {
                BackgroundColor = Theme.Color.MainBackgroundColor.CGColor,
                TitleTextStyle = new CPTTextStyle() {
                    FontName = "HelveticaNeue",
                    FontSize = (nfloat)20.0,
                    Color = CPTColor.WhiteColor
                }
            };
            graph.PaddingBottom = 20;
            graph.PaddingTop = 20;
            graph.PaddingLeft = 40;

            var axisSet = graph.AxisSet;
            foreach (var axe in axisSet.Axes) 
            {
                axe.LabelingPolicy = CPTAxisLabelingPolicy.Automatic;
                axe.AxisLineStyle = new CPTLineStyle () {
                    LineColor = CPTColor.WhiteColor,
                    LineWidth = (nfloat)1.0
                };
                axe.TitleTextStyle = new CPTTextStyle () {
                    FontName = "HelveticaNeue",
                    FontSize = (nfloat)12.0,
                    Color = CPTColor.RedColor
                };
                axe.ContentsScale = (nfloat)2.0;

                if (axe.Coordinate == CPTCoordinate.X)
                    axe.Title = "";
                if (axe.Coordinate == CPTCoordinate.Y) {
                    axe.Title = "";

                    axe.MajorTickLength = (nfloat)1000.0;
                    axe.MinorTickLength = (nfloat)100.0;
                }
            }
        }
    }
}

