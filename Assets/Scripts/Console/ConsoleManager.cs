using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class ConsoleManager : MonoBehaviour
{
    private static readonly int DEFAULT_STRING_SIZE = 50;
    public Canvas canvas;
    public Text textField;

    private StringBuilder stringBuilder;
    private MyString myStupidAssString;
    private bool consoleEnabled = false;
    private IDictionary<string, Command> commands;


    public void Awake()
    {
        commands = new Dictionary<string, Command>();
        foreach(Command command in Command.commands)
        {
            commands[command.GetCommandString()] = command;
        }

        stringBuilder = new StringBuilder(DEFAULT_STRING_SIZE);
        myStupidAssString = new MyString();
        textField.text = "";
        canvas.gameObject.SetActive(false);
        ActionManager actionManager = Singleton<ActionManager>.Instance;
        actionManager.registerStartButtonListener(InputButton.CONSOLE, Enableconsole);
    }

    public void Enableconsole(InputButton inputButton)
    {
        consoleEnabled = !consoleEnabled;
        canvas.gameObject.SetActive(consoleEnabled);
    }

    public void OnGUI()
    {
        if (!consoleEnabled)
            return;

        Event currentEvent = Event.current;
        if (currentEvent.type != EventType.KeyDown)
            return;


        switch (currentEvent.character)
        {
            case '\n':
                NewLineEntered();
                return;
            case '\0':
                if (currentEvent.keyCode != KeyCode.Backspace)
                    return;
                BackspaceEntered();
                return;
            case '`':
                return;
            default:
                NewCharacterEntered(currentEvent.character);
                return;
        }
    }

    private void NewLineEntered()
    {
        string commandString = stringBuilder.ToString();
        stringBuilder.Length = 0;
        ProcessString(commandString);
        SetDisplayString();
    }

    private void BackspaceEntered()
    {
        if (stringBuilder.Length == 0)
            return;
        stringBuilder.Remove(stringBuilder.Length-1, 1);
        SetDisplayString();
    }

    private void NewCharacterEntered(char character)
    {
        stringBuilder.Append(character);
        SetDisplayString();
    }

    private void SetDisplayString()
    {
        textField.text = stringBuilder.ToString();
    }

    private void ProcessString(string commandString)
    {
        string[] split = commandString.Split(' ');

        Command command;
        if (commands.TryGetValue(split[0], out command))
        {
            command.ProcessCommand(split);
        }
        else
        {
            Debug.Log("Could not process command: " + commandString);
        }
    }
}