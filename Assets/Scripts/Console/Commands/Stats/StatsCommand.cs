
/*
 * Command used to change the player's stats.
 */
public class StatsCommand : Command
{
    private static readonly string COMMAND = "stats";
    public override string GetCommandString()
    {
        return COMMAND;
    }

    public override void ProcessCommand(string[] command)
    {
        if (command.Length < 2)
            return;

        /*
         * Yes, this should have a map processor thing just like the Command class does in the ConsoleManager. If I'm not lazy in the future I'll fix it.
         * TODO, don't be lazy
         */
        switch(command[1])
        {
            case "speed":
                ProcessSpeed(command);
                break;
        }
    }

    private void ProcessSpeed(string[] command)
    {
        if (command.Length < 3)
            return;

        PlayerStats.movementSpeed = int.Parse(command[2]);
    }
}