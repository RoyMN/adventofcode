namespace Utils;

public interface ICard : IComparable<ICard>
{
    public int Value { get; }
}
public class Card : ICard
{
    public CardValue CardValue { get; set; }
    public int Value
    {
        get
        {
            return (int)CardValue;
        }
    }
    public int CompareTo(ICard? other)
    {
        if (other == null) return 1;
        return Value.CompareTo(other.Value);
    }
    public override string ToString()
    {
        return $"{CardValue.ToString()}";
    }
}
public class JCard : ICard
{
    public JokeredCardValue CardValue { get; set; }
    public int Value
    {
        get
        {
            return (int)CardValue;
        }
    }
    public int CompareTo(ICard? other)
    {
        if (other == null) return 1;
        return Value.CompareTo(other.Value);
    }
}
public record Player<T>(Hand<T> Hand, int Bid) where T : ICard
{
    public override string ToString()
    {
        return $"{Hand.ToString()}, Bid: {Bid}";
    }
}
public class Hand<T>(T[] cards) : IComparable<Hand<T>> where T : ICard
{
    public T[] Cards { get; set; } = cards;
    public HandType Type
    {
        get
        {
            if (typeof(T) == typeof(Card))
            {
                var groups = Cards.GroupBy(c => c.Value);
                var pairs = groups.Where(g => g.Count() == 2).ToArray();
                var threeOfAKind = groups.Where(g => g.Count() == 3).ToArray();
                var fourOfAKind = groups.Where(g => g.Count() == 4).ToArray();
                var fiveOfAKind = groups.Where(g => g.Count() == 5).ToArray();
                if (Cards.Length != 5) throw new Exception("Hand must contain 5 cards");
                if (fiveOfAKind.Length == 1) return HandType.FiveOfAKind;
                if (fourOfAKind.Length == 1) return HandType.FourOfAKind;
                if (pairs.Length == 1 && threeOfAKind.Length == 1) return HandType.FullHouse;
                if (threeOfAKind.Length == 1) return HandType.ThreeOfAKind;
                if (pairs.Length == 2) return HandType.TwoPair;
                if (pairs.Length == 1) return HandType.OnePair;
                return HandType.HighCard;
            }
            else if (typeof(T) == typeof(JCard))
            {
                if (Cards.Length != 5) throw new Exception("Hand must contain 5 cards");
                var jokers = Cards.Where(c => c.Value == (int)JokeredCardValue.Joker).ToArray();
                if (jokers.Length == 0)
                {
                    var groups = Cards.GroupBy(c => c.Value);
                    var pairs = groups.Where(g => g.Count() == 2).ToArray();
                    var threeOfAKind = groups.Where(g => g.Count() == 3).ToArray();
                    var fourOfAKind = groups.Where(g => g.Count() == 4).ToArray();
                    var fiveOfAKind = groups.Where(g => g.Count() == 5).ToArray();
                    // Reular hand.
                    if (fiveOfAKind.Length == 1) return HandType.FiveOfAKind;
                    if (fourOfAKind.Length == 1) return HandType.FourOfAKind;
                    if (pairs.Length == 1 && threeOfAKind.Length == 1) return HandType.FullHouse;
                    if (threeOfAKind.Length == 1) return HandType.ThreeOfAKind;
                    if (pairs.Length == 2) return HandType.TwoPair;
                    if (pairs.Length == 1) return HandType.OnePair;
                    return HandType.HighCard;
                }
                else 
                {
                    var groups = Cards.Where(c => c.Value != (int)JokeredCardValue.Joker).GroupBy(c => c.Value);
                    var pairs = groups.Where(g => g.Count() == 2).ToArray();
                    var threeOfAKind = groups.Where(g => g.Count() == 3).ToArray();
                    var fourOfAKind = groups.Where(g => g.Count() == 4).ToArray();
                    var fiveOfAKind = groups.Where(g => g.Count() == 5).ToArray();
                    if (jokers.Length == 5 || jokers.Length == 4) return HandType.FiveOfAKind;
                    if (jokers.Length == 3)
                    {
                        if (groups.Any(g => g.Count() > 1)) return HandType.FiveOfAKind;
                        return HandType.FourOfAKind; // Worst case.
                    }
                    if (jokers.Length == 2)
                    {
                        if (groups.Any(g => g.Count() > 2)) return HandType.FiveOfAKind;
                        if (pairs.Length != 0) return HandType.FourOfAKind;
                        return HandType.ThreeOfAKind; // Worst case.
                    }
                    if (jokers.Length == 1)
                    {
                        if (fiveOfAKind.Length == 1) return HandType.FiveOfAKind;
                        if (fourOfAKind.Length == 1) return HandType.FiveOfAKind;
                        if (threeOfAKind.Length == 1) return HandType.FourOfAKind;
                        if (pairs.Length == 2) return HandType.FullHouse;
                        if (pairs.Length == 1) return HandType.ThreeOfAKind;
                        return HandType.OnePair; // Worst case.
                    }
                    return HandType.OnePair; // Absolute worst case.
                }
            }
            else
            {
                throw new Exception("Unknown card type");
            }
        }
    }
    public int CompareTo(Hand<T>? other)
    {
        if (other == null) return 1;
        if (Type != other.Type) return Type.CompareTo(other.Type);
        for (int i = 0; i < Cards.Length; i++)
        {
            if (Cards[i].Value != other.Cards[i].Value) return Cards[i].Value.CompareTo(other.Cards[i].Value);
        }
        return 0;
    }
    public override string ToString()
    {
        return $"Type: {Type.ToString()}, Cards: {string.Join(", ", Cards.Select(c => c.Value.ToString()))}";
    }
}
public class Hand(Card[] cards) : IComparable<Hand>
{
    public Card[] Cards { get; set; } = cards;

    public HandType HandType
    {
        get
        {
            var groups = Cards.GroupBy(c => c.Value);
            var pairs = groups.Where(g => g.Count() == 2).ToArray();
            var threeOfAKind = groups.Where(g => g.Count() == 3).ToArray();
            var fourOfAKind = groups.Where(g => g.Count() == 4).ToArray();
            var fiveOfAKind = groups.Where(g => g.Count() == 5).ToArray();
            if (Cards.Length != 5) throw new Exception("Hand must contain 5 cards");
            if (fiveOfAKind.Length == 1) return HandType.FiveOfAKind;
            if (fourOfAKind.Length == 1) return HandType.FourOfAKind;
            if (pairs.Length == 1 && threeOfAKind.Length == 1) return HandType.FullHouse;
            if (threeOfAKind.Length == 1) return HandType.ThreeOfAKind;
            if (pairs.Length == 2) return HandType.TwoPair;
            if (pairs.Length == 1) return HandType.OnePair;
            return HandType.HighCard;
        }
    }

    public int CompareTo(Hand? other)
    {
        if (other == null) return 1;
        if (HandType != other.HandType) return HandType.CompareTo(other.HandType);
        for (int i = 0; i < Cards.Length; i++)
        {
            if (Cards[i].Value != other.Cards[i].Value) return Cards[i].Value.CompareTo(other.Cards[i].Value);
        }
        return 0;
    }

    public override string ToString()
    {
        return $"Type: {HandType.ToString()}, Cards: {string.Join(", ", Cards.Select(c => c.Value.ToString()))}";
    }
}
public enum CardValue
{
    Ace = 1,
    King = 2,
    Queen = 3,
    Jack = 4,
    Ten = 5,
    Nine = 6,
    Eight = 7,
    Seven = 8,
    Six = 9,
    Five = 10,
    Four = 11,
    Three = 12,
    Two = 13
}
public enum JokeredCardValue
{
    Ace = 1,
    King = 2,
    Queen = 3,
    Ten = 4,
    Nine = 5,
    Eight = 6,
    Seven = 7,
    Six = 8,
    Five = 9,
    Four = 10,
    Three = 11,
    Two = 12,
    Joker = 13
}
public enum HandType
{
    FiveOfAKind,
    FourOfAKind,
    FullHouse,
    ThreeOfAKind,
    TwoPair,
    OnePair,
    HighCard
}
public static class ExtensionMethods
{
    public static Player<T> ParsePlayer<T>(this string input) where T : ICard
    {
        // Input string looks like this: 32T3K 765
        var split = input.Split(" ");
        var hand = ParseHand<T>(split[0]);
        var bid = int.Parse(split[1]);
        return new Player<T>(hand, bid);
    }
    public static Hand<T> ParseHand<T>(string input) where T : ICard
    {
        // Input string looks like this: 32T3K
        return new Hand<T>(input.Select(c => ParseCardValue<T>(c.ToString())).ToArray());
    }
    public static T ParseCardValue<T>(string value) where T : ICard
    {
        if (typeof(T) == typeof(Card))
        {
            return (T)(object)new Card { CardValue = ParseCardValue(value) };
        }
        else if (typeof(T) == typeof(JCard))
        {
            return (T)(object)new JCard { CardValue = ParseJokeredCardValue(value) };
        }
        else
        {
            throw new Exception("Unknown card type");
        }
    }
    public static CardValue ParseCardValue(string value)
    {
        return value switch
        {
            "2" => CardValue.Two,
            "3" => CardValue.Three,
            "4" => CardValue.Four,
            "5" => CardValue.Five,
            "6" => CardValue.Six,
            "7" => CardValue.Seven,
            "8" => CardValue.Eight,
            "9" => CardValue.Nine,
            "T" => CardValue.Ten,
            "J" => CardValue.Jack,
            "Q" => CardValue.Queen,
            "K" => CardValue.King,
            "A" => CardValue.Ace,
            _ => throw new Exception("Unknown card value")
        };
    }
    public static JokeredCardValue ParseJokeredCardValue(string value)
    {
        return value switch
        {
            "2" => JokeredCardValue.Two,
            "3" => JokeredCardValue.Three,
            "4" => JokeredCardValue.Four,
            "5" => JokeredCardValue.Five,
            "6" => JokeredCardValue.Six,
            "7" => JokeredCardValue.Seven,
            "8" => JokeredCardValue.Eight,
            "9" => JokeredCardValue.Nine,
            "T" => JokeredCardValue.Ten,
            "J" => JokeredCardValue.Joker,
            "Q" => JokeredCardValue.Queen,
            "K" => JokeredCardValue.King,
            "A" => JokeredCardValue.Ace,
            _ => throw new Exception("Unknown card value")
        };
    }
    public static int Solve<T>(this string[] input, bool debug = false) where T : ICard
    {
        var players = new List<Player<T>>();
        for (int i = 0; i < input.Length; i++)
        {
            players.Add(input[i].ParsePlayer<T>());
        }

        // Sort in reverse so that worst hand is first.
        players.Sort((a, b) => b.Hand.CompareTo(a.Hand));

        var sum = 0;
        for (int i = 0; i < players.Count; i++)
        {
            sum += players[i].Bid * (i + 1);
            if (debug)
            {
                Console.WriteLine($"{players[i].ToString()}");
            }
        }
        return sum;
    }
}
