namespace VoiceAnimation
{
    public class DynamicBarAnimationView : GraphicsView
    {
        private double _bar1Height = 20;
        private double _bar2Height = 40;
        private double _bar3Height = 20;
        private double _bar4Height = 40;

        private bool _toggle = false; // Used to alternate between height states
        private bool _isAnimating = false; // Tracks animation state
        private CancellationTokenSource _animationCancellationToken;

        // Bindable Property for Animation Speed
        public static readonly BindableProperty AnimationSpeedProperty =
            BindableProperty.Create(
                nameof(AnimationSpeed),
                typeof(double),
                typeof(DynamicBarAnimationView),
                300.0,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((DynamicBarAnimationView)bindable).RestartAnimation();
                });

        public double AnimationSpeed
        {
            get => (double)GetValue(AnimationSpeedProperty);
            set => SetValue(AnimationSpeedProperty, value);
        }

        // Bindable Property for Corner Radius
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(double),
                typeof(DynamicBarAnimationView),
                8.0);

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // Bindable Property for Spacing Factor
        public static readonly BindableProperty SpacingFactorProperty =
            BindableProperty.Create(
                nameof(SpacingFactor),
                typeof(double),
                typeof(DynamicBarAnimationView),
                1.5);

        public double SpacingFactor
        {
            get => (double)GetValue(SpacingFactorProperty);
            set => SetValue(SpacingFactorProperty, value);
        }

        // Bindable Property for MaxHeight
        public static readonly BindableProperty MaxHeightProperty =
            BindableProperty.Create(
                nameof(MaxHeight),
                typeof(double),
                typeof(DynamicBarAnimationView),
                40.0); // Default value

        public double MaxHeight
        {
            get => (double)GetValue(MaxHeightProperty);
            set => SetValue(MaxHeightProperty, value);
        }

        // Bindable Property for MinHeight
        public static readonly BindableProperty MinHeightProperty =
            BindableProperty.Create(
                nameof(MinHeight),
                typeof(double),
                typeof(DynamicBarAnimationView),
                20.0); // Default value

        public double MinHeight
        {
            get => (double)GetValue(MinHeightProperty);
            set => SetValue(MinHeightProperty, value);
        }
        
        // Bindable Property for Bar Color
        public static readonly BindableProperty BarColorProperty =
            BindableProperty.Create(
                nameof(BarColor),
                typeof(Color),
                typeof(DynamicBarAnimationView),
                Colors.Blue, // Default color
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((DynamicBarAnimationView)bindable).Invalidate(); // Redraw on color change
                });

        public Color BarColor
        {
            get => (Color)GetValue(BarColorProperty);
            set => SetValue(BarColorProperty, value);
        }
        public DynamicBarAnimationView()
        {
            Drawable = new BarDrawable(this);
        }

        public void StartAnimation()
        {
            if (_isAnimating) return;

            _isAnimating = true;
            _animationCancellationToken = new CancellationTokenSource();

            Dispatcher.StartTimer(TimeSpan.FromMilliseconds(AnimationSpeed), () =>
            {
                if (_animationCancellationToken.IsCancellationRequested)
                {
                    _isAnimating = false;
                    return false; // Stop timer
                }

                AnimateBars();
                Invalidate(); // Redraw the view
                return true; // Continue timer
            });
        }

        public void StopAnimation()
        {
            _animationCancellationToken?.Cancel();
            _isAnimating = false;
        }

        private void RestartAnimation()
        {
            StopAnimation();
            StartAnimation();
        }

        private void AnimateBars()
        {
            if (_toggle)
            {
                _bar1Height = MaxHeight;
                _bar2Height = MinHeight;
                _bar3Height = MaxHeight;
                _bar4Height = MinHeight;
            }
            else
            {
                _bar1Height = MinHeight;
                _bar2Height = MaxHeight;
                _bar3Height = MinHeight;
                _bar4Height = MaxHeight;
            }

            _toggle = !_toggle;
        }

        private class BarDrawable : IDrawable
        {
            private readonly DynamicBarAnimationView _parent;

            public BarDrawable(DynamicBarAnimationView parent)
            {
                _parent = parent;
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                // Use the parent's BarColor property
                canvas.FillColor = _parent.BarColor;

                float width = (float)(dirtyRect.Width / (8 * _parent.SpacingFactor)); // Adjusted width
                float spacing = width; // Reduced spacing
                float centerY = dirtyRect.Height / 2;

                // Draw bar 1 with rounded corners
                canvas.FillRoundedRectangle(spacing, (float)(centerY - _parent._bar1Height / 2), width, (float)_parent._bar1Height, (float)_parent.CornerRadius);

                // Draw bar 2 with rounded corners
                canvas.FillRoundedRectangle(spacing * 3, (float)(centerY - _parent._bar2Height / 2), width, (float)_parent._bar2Height, (float)_parent.CornerRadius);

                // Draw bar 3 with rounded corners
                canvas.FillRoundedRectangle(spacing * 5, (float)(centerY - _parent._bar3Height / 2), width, (float)_parent._bar3Height, (float)_parent.CornerRadius);

                // Draw bar 4 with rounded corners
                canvas.FillRoundedRectangle(spacing * 7, (float)(centerY - _parent._bar4Height / 2), width, (float)_parent._bar4Height, (float)_parent.CornerRadius);
            }
        }
    }
}
