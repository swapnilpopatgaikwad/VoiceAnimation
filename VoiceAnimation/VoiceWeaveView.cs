namespace VoiceAnimation
{
    public class VoiceWeaveView : GraphicsView
    {
        private double _currentAmplitude = 0;
        private double _phaseShift = 0;

        public double Amplitude { get; set; } = 50; // Max height of the wave
        public double Frequency { get; set; } = 0.2; // Density of waves
        public double Speed { get; set; } = 0.05; // Speed of animation

        public VoiceWeaveView()
        {
            Drawable = new VoiceWeaveDrawable(this);

            // Start animation
            Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
            {
                _phaseShift += Speed;
                Invalidate(); // Redraw
                return true; // Continue timer
            });
        }

        public void UpdateAmplitude(double amplitude)
        {
            _currentAmplitude = amplitude;
            Invalidate();
        }

        private class VoiceWeaveDrawable : IDrawable
        {
            private readonly VoiceWeaveView _parent;

            public VoiceWeaveDrawable(VoiceWeaveView parent)
            {
                _parent = parent;
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                canvas.StrokeSize = 2;
                canvas.StrokeColor = Colors.Blue;

                var width = dirtyRect.Width;
                var height = dirtyRect.Height / 2;

                for (double x = 0; x < width; x += 1)
                {
                    var y = height + _parent._currentAmplitude * Math.Sin(2 * Math.PI * _parent.Frequency * (x / width) + _parent._phaseShift);
                    canvas.DrawLine((float)x, (float)height, (float)x, (float)y);
                }
            }
        }
    }
}
