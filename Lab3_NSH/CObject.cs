using System.Security.Policy;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

public CObject(Point position, double size, double lifetime)
{
    this.position = position;
    this.size = new Size(size, size);
    this.lifetime = lifetime;
    sprite = new Ellipse();
    sprite.Fill = Brushes.BlueViolet;
    sprite.StrokeThickness = 2;
    sprite.Stroke = Brushes.Black;
    sprite.HorizontalAlignment = HorizontalAlignment.Center;
    sprite.VerticalAlignment = VerticalAlignment.Center;
    sprite.Width = this.size.Width;
    sprite.Height = this.size.Height;
    sprite.RenderTransform = new TranslateTransform(position.X, position.Y);
    pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
}