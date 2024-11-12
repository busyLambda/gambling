using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Text;

class Program
{
    public static void Main(String[] args)
    {
        Blackjack blackjack = new Blackjack();
        blackjack.Menu();
    }
}

class Card
{
    public int Value;
    public String kind;
    public String icon = "";

    public Card()
    {
        Random rand = new Random();
        int value_rand = rand.Next(1, 11);
        int kind_rand = rand.Next(1, 58);

        Value = value_rand;

        if (kind_rand <= 13)
        {
            kind = "Hearts";
            icon = "♥";
        }
        else if (kind_rand <= 26)
        {
            kind = "Diamonds";
            icon = "♦";
        }
        else if (kind_rand <= 39)
        {
            kind = "Clubs";
            icon = "♣";
        }
        else if (kind_rand <= 52)
        {
            kind = "Spades";
            icon = "♠";
        }
        else if (kind_rand <= 54)
        {
            kind = "Joker";
            icon = "🃏";
            Value = 10;
        }
        else if (kind_rand <= 56)
        {
            kind = "Queen";
            icon = "♕";
            Value = 10;
        }
        else
        {
            kind = "King";
            icon = "♔";
            Value = 10;
        }
    }

    public String[] GetGraphicMap()
    {
        String[] map = new String[5];
        map[0] = "┌───────┐";
        map[1] = "│   " + icon + "   │";
        map[2] = "│       │";
        if (Value >= 10)
        {
            map[3] = "│  " + Value + "   │";
        }
        else
        {
            map[3] = "│   " + Value + "   │";
        }
        map[4] = "└───────┘";
        return map;
    }
}

class Blackjack
{
    List<Card> house_deck = new List<Card> { new Card(), new Card() };
    List<Card> player_deck = new List<Card> { new Card(), new Card() };

    int turn = 0;
    int money = 1000;
    int current_score = 0;
    int prev_score = 0;
    int house_score;

    public Blackjack()
    {
        house_score = EvalScore(house_deck, 0);

        foreach (Card card in player_deck)
        {
            if (card.Value == 11)
            {
                if (current_score + card.Value > 21)
                {
                    card.Value = 1;
                }
            }
            else
            {
                current_score += card.Value;
            }
        }
    }

    private int EvalScore(List<Card> cards, int p_s)
    {
        int score = 0;
        foreach (Card card in cards)
        {
            if (card.Value == 11)
            {
                if (p_s + cards.Last().Value > 21)
                {
                    score += 1;
                }
                else
                {
                    score += card.Value;
                }
            }
            else
            {
                score += card.Value;
            }
        }
        return score;
    }

    private void PrintStringMap(String[] map)
    {
        foreach (String line in map)
        {
            Console.WriteLine(line);
        }
    }

    private void PrintDeck(List<Card> deck)
    {
        List<String[]> listOfPlayerGraphicMaps = new List<String[]>();

        foreach (Card card in deck)
        {
            listOfPlayerGraphicMaps.Add(card.GetGraphicMap());
        }

        List<String> unifiedPlayerGraphicMapLines = new List<String>();

        for (int i = 0; i < 5; i++)
        {
            String line = "";
            for (int j = 0; j < listOfPlayerGraphicMaps.Count; j++)
            {
                line += listOfPlayerGraphicMaps[j][i];
            }
            unifiedPlayerGraphicMapLines.Add(line);
        }

        foreach (String line in unifiedPlayerGraphicMapLines)
        {
            Console.WriteLine(line);
        }
    }

    public void Menu()
    {
        while (true)
        {
            int bet = 0;

            if (money <= 0)
            {
                Console.WriteLine("You are now homeless, get ready to enjoy blaha lujza aluljaro!");
                Environment.Exit(0);
            }

            Console.WriteLine("Money: " + money);
            Console.Write("Please bet an amount: ");
            bet = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("bet: " + bet);

            if (bet <= money)
            {
                money -= bet;
            }
            else
            {
                Console.WriteLine("You do not have that kind of money!");
                continue;
            }

            bool run = true;

            while (run)
            {
                Console.WriteLine("Money: " + money);
                Console.WriteLine("Bet: " + bet);

                Console.WriteLine("Dealer score: " + house_score);
                Console.WriteLine("Your score: " + current_score);
                Console.WriteLine("Dealer Deck:");
                PrintDeck(house_deck);

                Console.WriteLine("Your Deck:");
                PrintDeck(player_deck);

                if (current_score > 21)
                {
                    Console.Clear();

                    Console.WriteLine("You busted!");
                    bet = 0;
                    break;
                }
                else if (current_score == 21)
                {
                    Console.Clear();

                    Console.WriteLine("You won!");
                    money = money + bet * 2;
                    bet = 0;
                    break;
                }

                Console.WriteLine("hit | stand | exit");

                String? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Invalid input! <null>");
                    continue;
                }

                switch (input)
                {
                    case "hit":
                        Console.Clear();
                        player_deck.Add(new Card());
                        prev_score = current_score;
                        current_score = EvalScore(player_deck, current_score);
                        break;
                    case "stand":
                        house_deck.Add(new Card());
                        house_score = EvalScore(house_deck, house_score);
                        if (house_score > 21 || house_score < current_score || current_score == 21)
                        {
                            Console.Clear();
                            Console.WriteLine("You win!");
                            money = money + bet * 2;
                            bet = 0;
                            run = false;
                            break;
                        }
                        else if (house_score > current_score)
                        {
                            Console.Clear();

                            Console.WriteLine("You lose!");
                            bet = 0;
                            run = false;
                            break;
                        }
                        else
                        {
                            Console.Clear();

                            Console.WriteLine("Even!");
                            money = money + bet;
                            bet = 0;
                            run = false;
                            break;
                        }
                    case "exit":
                        Console.WriteLine("Exiting!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
            }

            prev_score = 0;
            house_score = 0;
            current_score = 0;
            player_deck = new List<Card> { new Card(), new Card() };
            house_deck = new List<Card> { new Card(), new Card() };
            house_score = EvalScore(house_deck, 0);
            current_score = EvalScore(player_deck, 0);
        }
    }
}