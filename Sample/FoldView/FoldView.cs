using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoldView
{
    /// <summary>
    /// Tadeo Sauceda
    /// Abril 2 2018
    /// </summary>
    class FoldView : RelativeLayout
    {
        #region Properties
        public int ExpandHeight { get; private set; }

        public static readonly BindableProperty AnimationVelocityProperty =
           BindableProperty.Create("AnimationVelocity", typeof(int), typeof(FoldView), 350, BindingMode.Default, null, OnAnimationVelocityPropertyChanged);

        public static readonly BindableProperty View1HeightProperty =
           BindableProperty.Create("View1Height", typeof(int), typeof(FoldView), 80, BindingMode.Default, null, OnView1HeightPropertyChanged);

        public static readonly BindableProperty View2HeightProperty =
           BindableProperty.Create("View2Height", typeof(int), typeof(FoldView), 80, BindingMode.Default, null, OnView2HeightPropertyChanged);

        public static readonly BindableProperty View3HeightProperty =
          BindableProperty.Create("View3Height", typeof(int), typeof(FoldView), 50, BindingMode.Default, null, OnView3HeightPropertyChanged);

        public static readonly BindableProperty IsBackGroundClickedProperty =
         BindableProperty.Create("IsBackGroundClicked", typeof(bool), typeof(FoldView), false, BindingMode.Default, null, OnIsBackGroundClickedPropertyChanged);

        public static readonly BindableProperty ImageExpandProperty =
        BindableProperty.Create("ImageExpand", typeof(string), typeof(FoldView), "", BindingMode.Default, null, OnImageExpandPropertyPropertyChanged);

        public int AnimationVelocity
        {
            get { return (int)base.GetValue(AnimationVelocityProperty); }
            set { base.SetValue(AnimationVelocityProperty, value); }
        }
        public int View1Height
        {
            get { return (int)base.GetValue(View1HeightProperty); }
            set { base.SetValue(View1HeightProperty, value); }
        }
        public int View2Height
        {
            get { return (int)base.GetValue(View2HeightProperty); }
            set { base.SetValue(View2HeightProperty, value); }
        }
        public int View3Height
        {
            get { return (int)base.GetValue(View3HeightProperty); }
            set { base.SetValue(View3HeightProperty, value); }
        }
        public bool IsBackGroundClicked
        {
            get { return (bool)base.GetValue(IsBackGroundClickedProperty); }
            set { base.SetValue(IsBackGroundClickedProperty, value); }
        }
        public string ImageExpand
        {
            get { return (string)base.GetValue(ImageExpandProperty); }
            set { base.SetValue(ImageExpandProperty, value); }
        }

        private static void OnAnimationVelocityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var foldView = (FoldView)bindable;
            foldView.AnimationVelocity = (int)newValue;
        }

        private static void OnView1HeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var foldView = (FoldView)bindable;
            foldView.View1Height = (int)newValue;
        }

        private static void OnView2HeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var foldView = (FoldView)bindable;
            foldView.View2Height = (int)newValue;
        }

        private static void OnView3HeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var foldView = (FoldView)bindable;
            foldView.View3Height = (int)newValue;
        }

        private static void OnIsBackGroundClickedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var foldView = (FoldView)bindable;
            foldView.IsBackGroundClicked = (bool)newValue;
        }
        private static void OnImageExpandPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var foldView = (FoldView)bindable;
            foldView.ImageExpand = (string)newValue;
        }
        #endregion

        private ContentView s1, s2, s3;
        private bool toogle = true;
        private string defaultImage = "iVBORw0KGgoAAAANSUhEUgAAAB4AAAAeCAYAAAA7MK6iAAAABmJLR0QA/wD/AP+gvaeTAAABNElEQVRIDe2Tu04CQRSGdyx4AIQGayoSYwihMLRqZW3C26C+h7GC3stLiDEaKh9ArHwBEobvnDCbM2TZWFEd8n97LvMPk5zZLQr/+QR8Aj4Bn8A/JxCsL8bYpB5DF+YwCyGsiJnwDWlcQ4QnPG/ETHgaNG5gAN8wxfdHzIVxAL9g9UXRsk7qe1hDkuS3O542iwuwWlL0rU9zmrtGWqoHNfCgGoEcRMgkvXMsKlYeoUqfauBxBAWOE2IPqnRlmhfk2fVQi6R3KckWm29bGk45qyOZHkwSYZ/q1vbtqevr/+nBXPoPzg+o0rNpvpLrRqLVmuIFkuye1JP4zllLSUoYQR+qXq7j0kSC5w7kTgkqyScslaLbggVYyct1lkwhJRJxNYlj6MJhPicOcvkEfAI+AZ9A7QQ2KIkhefMAOd8AAAAASUVORK5CYII=";
        private const int EXPANDERHEIGHT = 35;
        private Image expander = new Image();
        private int angle = -180;

        public FoldView()
        {
            this.ChildAdded += (sender, e) =>
            {
                if (this.Children.Count >= 2)
                {
                    //On 2 or more Childs, Go!
                    Create();
                }
            };

            //Create Expander
            expander.HeightRequest = EXPANDERHEIGHT;
            expander.IsVisible = true;
            expander.HorizontalOptions = LayoutOptions.CenterAndExpand;

            if (!string.IsNullOrEmpty(ImageExpand))
                expander.Source = ImageExpand;
            else
                expander.Source = CreateIcon(defaultImage);

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (s, ev) =>
            {
                Toogle();
            };
            expander.GestureRecognizers.Add(tap);

            //Create Listener Tap
            TapGestureRecognizer tap2 = new TapGestureRecognizer();
            tap2.Tapped += (sender, e) =>
            {
                if (IsBackGroundClicked)
                {
                    Toogle();
                }
            };
            this.GestureRecognizers.Add(tap2);
        }

        private ImageSource CreateIcon(string imgB64)
        {
            ImageSource imgb64 = Xamarin.Forms.ImageSource.FromStream(
         () => new MemoryStream(Convert.FromBase64String(imgB64)));
            return imgb64;
        }
        public void Create()
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.Margin = 5;

            s1 = (ContentView)this.Children[0];
            s1.Margin = new Thickness(10, 10, 10, 0);
            s1.HeightRequest = View1Height;
            s1.IsVisible = true;

            s2 = (ContentView)this.Children[1];
            s2.Margin = (this.Children.Count == 3) ? new Thickness(10, 0, 10, 10) : new Thickness(10, 0, 10, 0);
            s2.HeightRequest = View2Height;
            s2.IsVisible = false;
            s2.AnchorY = 0;

            this.Children.Add(s1,
                               xConstraint: Constraint.Constant(0),
                               yConstraint: Constraint.Constant(0),
                               widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                               heightConstraint: Constraint.Constant(View1Height));

            this.Children.Add(s2,
                               xConstraint: Constraint.Constant(0),
                               yConstraint: Constraint.Constant(View1Height),
                               widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                               heightConstraint: Constraint.Constant(View2Height));

            s2.RotationX =angle;

            if (this.Children.Count == 4)
            {
                s3 = (ContentView)this.Children[2];
                s3.Margin = new Thickness(10, 0, 10, 10);
                s3.HeightRequest = View3Height;
                s3.IsVisible = false;
                s3.AnchorY = 0;

                this.Children.Add(s3,
                             xConstraint: Constraint.Constant(0),
                             yConstraint: Constraint.Constant(View1Height + View2Height),
                             widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                             heightConstraint: Constraint.Constant(View3Height));

                s3.RotationX = angle;

            }

            CreateExpander();

            this.HeightRequest = View1Height;
            ExpandHeight = (this.Children.Count == 4) ? View1Height + View2Height + View3Height : View1Height + View2Height;

        }
        private void CreateExpander()
        {
            if (!IsBackGroundClicked)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.Children.Add(expander,
                            xConstraint: Constraint.Constant(0),
                            yConstraint: Constraint.Constant(View1Height - EXPANDERHEIGHT),
                            widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                            heightConstraint: Constraint.Constant(EXPANDERHEIGHT));

                });
            }
        }
        public async void Toogle()
        {
            if (toogle)
            {
                if (IsBackGroundClicked) this.IsEnabled = false; else expander.IsEnabled = false; expander.IsVisible = false;

                //Open
                if (this.Children.Count == 2)
                {
                    s2.IsVisible = true;
                    ShowChildrens(s2);
                    s2.Opacity = 0;
                    this.Animate("Anima", new Animation((d) =>
                    {
                        this.HeightRequest = d;
                        this.MinimumHeightRequest = d;
                        this.Layout(new Rectangle());

                    }, View1Height, ExpandHeight, Easing.Linear, () => { }), 16, (uint)AnimationVelocity);
                    s2.FadeTo(1, (uint)AnimationVelocity);//RunSynchronously
                    s2.RotateXTo(0, (uint)AnimationVelocity);//RunSynchronously

                    await Task.Delay((int)AnimationVelocity);
                    if (IsBackGroundClicked) this.IsEnabled = true; else expander.IsEnabled = true; expander.IsVisible = true;
                    toogle = !toogle;
                }
                else if (this.Children.Count == 4)
                {
                    s2.IsVisible = true;
                    ShowChildrens(s2);
                    s2.Opacity = 0;
                    this.Animate("Anima", new Animation((d) =>
                    {
                        this.HeightRequest = d;
                        this.MinimumHeightRequest = d;
                        this.Layout(new Rectangle());

                    }, View1Height, View1Height + View2Height, Easing.Linear, () => { }), 16, (uint)AnimationVelocity);
                    s2.FadeTo(1, (uint)AnimationVelocity);//RunSynchronously
                    s2.RotateXTo(0, (uint)AnimationVelocity);//RunSynchronously

                    await Task.Delay((int)AnimationVelocity);

                    s3.IsVisible = true;
                    ShowChildrens(s3);
                    s3.Opacity = 0;
                    this.Animate("Anima", new Animation((d) =>
                    {
                        this.HeightRequest = d;
                        this.MinimumHeightRequest = d;
                        this.Layout(new Rectangle());

                    }, View1Height + View2Height, ExpandHeight, Easing.Linear, () => { }), 16, (uint)AnimationVelocity);
                    s3.FadeTo(1, (uint)AnimationVelocity);//RunSynchronously
                    s3.RotateXTo(0, (uint)AnimationVelocity);//RunSynchronously

                    await Task.Delay((int)AnimationVelocity);
                    if (IsBackGroundClicked) this.IsEnabled = true; else expander.IsEnabled = true; expander.IsVisible = true;
                    toogle = !toogle;
                }


            }
            else
            {
                if (IsBackGroundClicked) this.IsEnabled = false; else expander.IsEnabled = false; expander.IsVisible = false;

                //Close
                if (this.Children.Count == 2)
                {
                    s2.IsVisible = true;
                    HideChildrens(s2);
                    s2.RotateXTo(angle, (uint)AnimationVelocity);//RunSynchronously
                    s2.FadeTo(0, (uint)AnimationVelocity);//RunSynchronously
                    this.Animate("Anima", new Animation((d) =>
                    {
                        this.HeightRequest = d;
                        this.MinimumHeightRequest = d;
                        this.Layout(new Rectangle());

                    }, ExpandHeight, View1Height, Easing.Linear, () => { }), 16, (uint)AnimationVelocity, Easing.Linear, async (d, b) => { await Task.Delay(50); s2.IsVisible = false; if (IsBackGroundClicked) this.IsEnabled = true; else expander.IsEnabled = true; expander.IsVisible = true; toogle = !toogle; });
                }
                else if (this.Children.Count == 4)
                {
                    s3.IsVisible = true;
                    HideChildrens(s3);
                    s3.RotateXTo(angle, (uint)AnimationVelocity);//RunSynchronously
                    s3.FadeTo(0, (uint)AnimationVelocity);//RunSynchronously
                    this.Animate("Anima", new Animation((d) =>
                    {
                        this.HeightRequest = d;
                        this.MinimumHeightRequest = d;
                        this.Layout(new Rectangle());

                    }, ExpandHeight, View1Height + View2Height, Easing.Linear, () => { }), 16, (uint)AnimationVelocity, Easing.Linear, async (d, b) => { await Task.Delay(AnimationVelocity); s3.IsVisible = false; });

                    await Task.Delay((int)AnimationVelocity);

                    s2.IsVisible = true;
                    HideChildrens(s2);
                    s2.RotateXTo(angle, (uint)AnimationVelocity);//RunSynchronously
                    s2.FadeTo(0, (uint)AnimationVelocity);//RunSynchronously
                    this.Animate("Anima", new Animation((d) =>
                    {
                        this.HeightRequest = d;
                        this.MinimumHeightRequest = d;
                        this.Layout(new Rectangle());

                    }, View1Height + View2Height, View1Height, Easing.Linear, () => { }), 16, (uint)AnimationVelocity, Easing.Linear, async (d, b) => { await Task.Delay(50); s2.IsVisible = false; if (IsBackGroundClicked) this.IsEnabled = true; else expander.IsEnabled = true; expander.IsVisible = true; toogle = !toogle; });

                }

            }

        }
        public void HideChildrens(ContentView view)
        {
            if (view.Content != null)
                view.Content.IsVisible = false;
        }
        public void ShowChildrens(ContentView view)
        {
            if (view.Content != null)
                view.Content.IsVisible = true;
        }
    }
}
