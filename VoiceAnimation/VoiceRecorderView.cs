namespace VoiceAnimation
{
    public class VoiceRecorderView : GraphicsView
    {
        private readonly List<Circle> _circles = new();
        private double _maxAmplitude = 0;

        public double Amplitude { get; set; } = 100; // Max radius
        public double AnimationSpeed { get; set; } = 5; // Speed of expansion

        public VoiceRecorderView()
        {
            Drawable = new VoiceRecorderDrawable(this);

            // Start animation
            Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
            {
                UpdateCircles();
                Invalidate(); // Redraw
                return true; // Continue timer
            });
        }

        public void UpdateAmplitude(double amplitude)
        {
            _maxAmplitude = amplitude;

            // Add a new circle on significant amplitude change
            if (_circles.Count == 0 || _circles[^1].Radius > Amplitude / 3)
            {
                _circles.Add(new Circle { Radius = 0, Opacity = 1 });
            }
        }

        private void UpdateCircles()
        {
            foreach (var circle in _circles)
            {
                circle.Radius += AnimationSpeed;
                circle.Opacity -= 0.02; // Fade effect
            }

            // Remove circles that are no longer visible
            _circles.RemoveAll(c => c.Opacity <= 0);
        }

        private class VoiceRecorderDrawable : IDrawable
        {
            private readonly VoiceRecorderView _parent;

            public VoiceRecorderDrawable(VoiceRecorderView parent)
            {
                _parent = parent;
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                canvas.FillColor = Colors.Transparent;
                canvas.StrokeColor = Colors.Blue;

                var centerX = dirtyRect.Width / 2;
                var centerY = dirtyRect.Height / 2;

                // Draw all active circles
                foreach (var circle in _parent._circles)
                {
                    canvas.StrokeSize = 2;
                    canvas.StrokeColor = Colors.Blue.WithAlpha((float)circle.Opacity);
                    canvas.DrawCircle((float)centerX, (float)centerY, (float)circle.Radius);
                }

                // Draw the central dot
                canvas.FillColor = Colors.Red;
                canvas.FillCircle((float)centerX, (float)centerY, 10);
            }
        }

        private class Circle
        {
            public double Radius { get; set; }
            public double Opacity { get; set; }
        }
    }
}
