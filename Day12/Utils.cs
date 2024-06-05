namespace Utils;

public record ConditionRecord(string Springs, int[] DamagedGroups);

readonly record struct State(int position, int lastPassedPosition, int numCheckedTokens, string uncheckedTokenSoFar);

class InterpretationGenerator(List<char> Chars, List<int> ReqNums)
{
    bool IsValidSoFar (string currToken, int whichNum)
    {
        bool finishedToken = currToken.EndsWith(".");
        if (finishedToken)
        {
            return ReqNums.Count > whichNum && ReqNums[whichNum] == currToken.Trim('.').Length;
        }
        else
        {
            return ReqNums.Count > whichNum && ReqNums[whichNum] >= currToken.Length;
        }
    }


    Dictionary<State, long> DynamicProgrammingCache = new();

    public long CountValidInterpretations(State state)
    {
        if (DynamicProgrammingCache.TryGetValue(state, out long cachedValue)) { return cachedValue; }

        var (position, lastPassedPosition, numCheckedTokens, uncheckedTokenSoFar) = state;
        uncheckedTokenSoFar = uncheckedTokenSoFar.TrimStart('.');

        if (position == Chars.Count && uncheckedTokenSoFar.Length > 0) { uncheckedTokenSoFar += "."; }

        if (uncheckedTokenSoFar.Length > 0)
        {
            if (IsValidSoFar(uncheckedTokenSoFar, numCheckedTokens))
            {
                if (uncheckedTokenSoFar.EndsWith('.'))
                {
                    numCheckedTokens++;
                    uncheckedTokenSoFar = "";
                    lastPassedPosition = position;
                }
            }
            else 
            { 
                return 0;
            }
        }

        if (position == Chars.Count) { return numCheckedTokens == ReqNums.Count ? 1 : 0; }


        if (Chars[position] == '?')
        {
            long validForDot = CountValidInterpretations(new State(position + 1, lastPassedPosition, numCheckedTokens, uncheckedTokenSoFar + "."));
            long validForHash = CountValidInterpretations(new State(position + 1, lastPassedPosition, numCheckedTokens, uncheckedTokenSoFar + "#"));
            long res = validForDot + validForHash;
            DynamicProgrammingCache[state] = res;
            return res;
        }
        else
        {
            long res = CountValidInterpretations(new State(position + 1, lastPassedPosition, numCheckedTokens, uncheckedTokenSoFar + Chars[position]));
            DynamicProgrammingCache[state] = res;
            return res;
        }
    }
}