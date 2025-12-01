using AdventOfCodeTools;

namespace AdventOfCode2024.Day3WithoutRegex;
/**
* This class is used to create a state machine that can be used to parse the input string.
* { @includeControls } is used to include the control characters in the state machine.
*/
public class AdventDay3Parser(ConsoleWriter logger, bool includeControls = false)
{
    private int? Left = null;
    private bool LeftCompleted = false;
    private int? Right = null;
    private char? Previous = null;
    private bool Do = true;
    private string Control = "";
    public bool Valid
    {
        get
        {
            return Left != null && Right != null && Previous == ')';
        }
    }
    private static readonly char[] Numbers = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];
    private static readonly Dictionary<char, char[]> ValidRegularTransitions = new()
    {
        { 'm', ['u']},
        { 'u', ['l'] },
        { 'l', ['('] },
        { '1', [..Numbers, ')', ','] },
        { '2', [..Numbers, ')', ','] },
        { '3', [..Numbers, ')', ','] },
        { '4', [..Numbers, ')', ','] },
        { '5', [..Numbers, ')', ','] },
        { '6', [..Numbers, ')', ','] },
        { '7', [..Numbers, ')', ','] },
        { '8', [..Numbers, ')', ','] },
        { '9', [..Numbers, ')', ','] },
        { '0', [..Numbers, ')', ','] },
        { '(', [..Numbers]},
        { ',', [..Numbers]}
    };

    private static readonly Dictionary<char, char[]> ValidTransitionsWithControls = new()
    {
        { 'm', ['u']},
        { 'u', ['l'] },
        { 'l', ['('] },
        { '1', [..Numbers, ')', ','] },
        { '2', [..Numbers, ')', ','] },
        { '3', [..Numbers, ')', ','] },
        { '4', [..Numbers, ')', ','] },
        { '5', [..Numbers, ')', ','] },
        { '6', [..Numbers, ')', ','] },
        { '7', [..Numbers, ')', ','] },
        { '8', [..Numbers, ')', ','] },
        { '9', [..Numbers, ')', ','] },
        { '0', [..Numbers, ')', ','] },
        { ',', [..Numbers]},
        { 'd', ['o'] },
        { 'o', ['(', 'n'] },
        { 'n', ['\''] },
        { '\'', ['t'] },
        { 't', ['('] },
        { '(', [ ..Numbers, ')'] }
    };

    private static readonly char[] ValidRegularStarters = ['m'];
    private static readonly char[] ValidStartersWithControls = [ 'm', 'd'];
    private char[] ValidStarters => includeControls ? ValidStartersWithControls : ValidRegularStarters;

    public Dictionary<char, char[]> ValidTransitions
    {
        get
        {
            if (includeControls)
            {
                return ValidTransitionsWithControls;
            }
            return ValidRegularTransitions;
        }
    }

    private void Reset()
    {
        Left = null;
        Right = null;
        Previous = null;
        LeftCompleted = false;
        Control = "";
    }

    public void CheckControl()
    {
        if (Control == "do()")
        {
            Do = true;
        }
        else if (Control == "don't()")
        {
            Do = false;
        }
    }

    public void Read(char c)
    {
        CheckControl();
        if (Previous.HasValue && Previous.Value == ')')
        {
            logger.Info($"Clearing up parser before continuing");
            Reset();
        }
        if (c == ')')
        {
            Control += c;
            Previous = c;
        }
        else
        {
            if (!Previous.HasValue)
            {
                if (ValidStarters.Contains(c))
                {
                    if (includeControls && ValidStartersWithControls.Contains(c))
                    {
                        Control += c;
                    }
                    Previous = c;
                }
                else
                {
                    logger.Debug($"Ignoring invalid starter {c}");
                }
            }
            else
            {
                if (!ValidTransitions[Previous.Value].Contains(c))
                {
                    logger.Info($"Invalid transition from {Previous.Value} to {c}");
                    Reset();
                }
                else
                {
                    if (c == ',' && Left.HasValue)
                    {
                        LeftCompleted = true;
                    }
                    else if (Numbers.Contains(c))
                    {
                        if (LeftCompleted)
                        {
                            if (!Right.HasValue)
                            {
                                Right = int.Parse(c.ToString());
                            }
                            else
                            {
                                Right = Right * 10 + int.Parse(c.ToString());
                            }
                            logger.Debug($"Updated Right value: {Right}");
                        }
                        else
                        {
                            if (!Left.HasValue)
                            {
                                Left = int.Parse(c.ToString());
                            }
                            else
                            {
                                Left = Left * 10 + int.Parse(c.ToString());
                            }
                            logger.Debug($"Updated Left value: {Left}");
                        }
                    }
                    Control += c;
                    Previous = c;
                }
            }
        }
    }

    public int GetMultiplication()
    {
        if (!Left.HasValue) throw new ArgumentNullException("Left value is not set");
        if (!Right.HasValue) throw new ArgumentNullException("Right value is not set");
        var result = Do ? Left.Value * Right.Value : 0;
        Reset();
        logger.Debug($"Clearing up parser before returning result");
        return result;
    }
}