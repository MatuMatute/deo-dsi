using Godot;
using System.Linq;

public partial class ControlBox : PanelContainer
{
    enum Mode {sign, dialog, item, skill};
    enum State {ready, writing, finished}
    [Export]
    private Label dialogLabel;
    [Export]
    private Label finishLabel;
    private const float tweenWritingSpeed = 0.05f;
    private const string battleStartMessage = "La vida es como una caja de bombones, nunca sabés que bombones te pueden salir.";
    private Tween mainTween;
    private Mode currentMode;
    private State currentState;
    private string[] dialogQueue = [];

    public override void _Ready()
    {
        AddDialog(battleStartMessage);
        Set(PropertyName.Scale, new Vector2(0.0f, 0.0f));
        dialogLabel.Set("visible_characters", 0);
    }

    public override void _Process(double delta)
    {
        switch (currentState)
        {
            case State.ready:
                if (!dialogQueue.IsEmpty())
                {
                    finishLabel.Hide();
                    ShowDialog();
                }
                else
                {
                    
                }
                break;
            case State.writing:
                if (Input.IsActionJustPressed("ui_accept"))
                {
                    mainTween.CustomStep(tweenWritingSpeed * dialogLabel.Text.Length);
                    mainTween.Kill();
                }
                break;
            case State.finished:
                if (Input.IsActionJustPressed("ui_accept"))
                {
                    currentState = State.ready;
                }
                break;
        }
    }

    public void Grow()
    {
        mainTween = CreateTween();
        mainTween.TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), 1.0f);
        mainTween.Connect("finished", new Callable(mainTween, "kill"), 4);
        mainTween.Connect("finished", Callable.From(() => SetMode(Mode.dialog)), 4);
        mainTween.Play();
    }

    public void AddDialog(string dialog)
    {
        dialogQueue = dialogQueue.Append(dialog).ToArray();
    }

    public void ShowDialog()
    {
        currentState = State.writing;
        dialogLabel.Text = dialogQueue.First();
        dialogQueue = dialogQueue.Skip(0).ToArray();
        mainTween = CreateTween();
        mainTween.TweenProperty(dialogLabel, "visible_characters", dialogLabel.Text.Length, tweenWritingSpeed * dialogLabel.Text.Length).From(0);
        mainTween.Connect("finished", new Callable(this, "FinishedDialog"), 4);
        mainTween.Play();
    }

    private void FinishedDialog()
    {
        finishLabel.Show();
        currentState = State.finished;
    }

    private void SetMode(Mode mode)
    {
        Mode previousMode = currentMode;
        currentMode = mode;

        if (currentMode == Mode.dialog)
        {
            currentState = State.ready;
        }
    }
}
