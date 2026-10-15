using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

public partial class MainWindow : Window
{
    private CController controller;
    private DispatcherTimer timer;
    private CPlayer player;          // игрок (Задание №2)
    private CooldownTimer cooldown;  // отдельный класс перезарядки

    public MainWindow()
    {
        InitializeComponent();

        timer = new DispatcherTimer();
        timer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100 мс
        timer.Tick += Timer_Tick;
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        GameCanvas.Children.Clear();

        player = new CPlayer();
        cooldown = new CooldownTimer();
        controller = new CController(
            spawnRate: 0.5,
            startTime: 30,                   // игра длится 30 секунд
            sceneSize: new Size(GameCanvas.ActualWidth, GameCanvas.ActualHeight)
        );

        // Передаём Canvas, чтобы контроллер сам добавлял/удалял сферы
        controller.setCanvas(GameCanvas);

        timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // 1. Обновляем контроллер (спавн, время жизни, удаление)
        controller.update(0.1);

        // 2. Обновляем перезарядку
        cooldown.update(0.1);

        // 3. Обновляем UI
        ScoreText.Text = controller.getPoints().ToString("0");
        TimeText.Text = controller.getRemainingTime().ToString("0.0");

        // 4. Конец игры
        if (controller.isFinished())
        {
            timer.Stop();
            MessageBox.Show($"Игра окончена! Очки: {controller.getPoints():0}");
        }
    }

    private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (controller == null) return;

        Point mousePos = e.GetPosition(GameCanvas);

        // Перезарядка: если ещё не готова — игнорируем клик
        if (!cooldown.isReady())
            return;

        // Полиморфный вызов: контроллер сам перебирает List<CCollectable>
        bool hit = controller.handleClick(player, mousePos);

        if (hit)
            cooldown.reset(); // сброс перезарядки после успешного клика
    }
}