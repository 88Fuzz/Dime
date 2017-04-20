/*
 * Any action that should be supported by the console should implement this class and add it to the list of static commands.
 */
public abstract class Command
{
    public static Command[] commands =
    {
        new StatsCommand()
    };

    /*
     * Return the string that needs to be entered for the command to be envoked. If for example the command "dance" should trigger all enemies to dance,
     * the returned string shoule be "dance".
     */
    public abstract string GetCommandString();

    /*
     * Called when the string returned form GetCommandString was matched. The input is the full command entered by the user parsed on ' '
     */
    public abstract void ProcessCommand(string[] command);
}