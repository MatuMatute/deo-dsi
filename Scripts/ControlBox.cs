using Godot;
using System.Linq;

public partial class ControlBox : PanelContainer
{
    [Signal]
    public delegate void DialogBoxFinishedEventHandler();
    enum Mode {none, sign, dialog, item, skill};
    enum State {ready, writing, finished}
    [Export]
    private Label dialogLabel;
    [Export]
    private Label finishLabel;
    private const float tweenWritingSpeed = 0.05f;
    private const string battleStartMessage = "la batalla comienza";
    private Tween mainTween;
    private Mode currentMode = Mode.none;
    private State currentState;
    private string[] dialogQueue = [];

    public override void _Ready()
    {
        AddDialog(battleStartMessage, false);
        Set(PropertyName.Scale, new Vector2(0.0f, 0.0f));
        dialogLabel.Set("visible_characters", 0);
    }

    public override void _Process(double delta)
    {
        switch (currentMode)
        {
            case Mode.sign:
                if (!dialogQueue.IsEmpty())
                {
                    ShowDialog();
                }
                else
                {
                    dialogLabel.Hide();
                }
                break;
            case Mode.dialog:
                switch (currentState)
                {
                    case State.ready:
                        finishLabel.Hide();
                        if (!dialogQueue.IsEmpty())
                        {
                            ShowDialog();
                            mainTween.Connect("finished", new Callable(this, "FinishedDialog"), 4);
                        }
                        else
                        {
                            dialogLabel.Hide();
                            EmitSignal(SignalName.DialogBoxFinished);
                            SetMode(Mode.none);
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

    public void AddDialog(string dialog, bool isSign)
    {
        dialogQueue = dialogQueue.Append(dialog).ToArray();

        if (isSign)
        {
            SetMode(Mode.sign);
        }
        else
        {
            SetMode(Mode.dialog);
        }
    }

    public void ShowDialog()
    {
        currentState = State.writing;
        dialogLabel.Text = dialogQueue.First();
        dialogQueue = dialogQueue.Skip(1).ToArray();
        mainTween = CreateTween();
        mainTween.TweenProperty(dialogLabel, "visible_characters", dialogLabel.Text.Length, tweenWritingSpeed * dialogLabel.Text.Length).From(0);
        mainTween.Play();
        dialogLabel.Show();
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
